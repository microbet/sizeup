// Config. TODO(twilson) relocate this section to config files.
var config = {
  editorRootUrl: "https://application.sizeup.com/widget",
  googlemap_key: process.env.GOOGLEMAP_KEY,
  sizeup_key: process.env.SIZEUP_KEY,
  sizeup: require("sizeup-api")({ key: process.env.SIZEUP_KEY })
}
function setConfig(_config) {
  for (var key in _config) { config[key] = _config[key]; }
}
function setSizeup(_sizeup) { // for backwards compatibility
  setConfig({ sizeup: _sizeup });
}

// We may have a circular dependency with this module. Consider carefully
// and rearrange modules.exports if needed. See
// https://stackoverflow.com/questions/10869276/how-to-deal-with-cyclic-dependencies-in-node-js
const pdf = require("./app.js");

var advertising = {};

// Functions to run on the advertising query

advertising.validateQuery = function(advertisingQuery) {
  throw Error("Not implemented");
}

advertising.getEditorUrl = function(query) {
  // For now, only implemented for Sizeup 1 editor, and will break customer frame.
  // TODO either upgrade the embedding tech in Sizeup 1, or convert this
  // function to Sizeup 2 editor.
  var url = util.format("%s/advertising/%s/%s/%s/%s#attribute=%s&distance=%s&sort=%s",
    config.editorRootUrl,
    query.area.place.state, query.area.place.county, query.area.place.city,
    query.ranking_metric.industry,
    query.ranking_metric.kpi,
    query.area.distance,
    query.ranking_metric.order
  );
  for (var key in query.filter) {
    url = util.format("%s&%s=%s", url, key, query.filter[key].min || 0);
    if (pdf.searchFilterTypes[key] != "percent-or-higher") {
      url = util.format("%s&%s=%s", url, key, query.filter[key].max || 0);
    }
  }
  return url;
}

advertising.getDescription = function(advertisingQuery) {
  throw Error("Not implemented");
}

advertising.getUserAdvice = function(advertisingQuery) {
  var userAdvice = {
    "totalRevenue": "You should consider using this list if you are selling to businesses or consumers and want to know where the most money is being made in your industry."
  };
  return userAdvice[advertisingQuery.ranking_metric.kpi];
}

/**
 * Accepts query (structure documented TODO) and runs functions in the
 * Sizeup API to generate a report that answers the query. The report
 * is returned in a Promise. The report contains (tentative):
 * - query (original query that was run)
 * - place (complete Place object referred to in the query)
 * - industry (complete Industry object referred to in the query)
 * - bestPlaces (ordered list of BestPlaces (Items) with count (Total) (a Sizeup API structure))
 * - bands (list of Band objects (a Sizeup API structure) that quantile the bestPlaces)
 * 
 * TODO: Not included in report but should be eventually: date
 * (effective date of report - currently is when the report was run,
 * but could be backdated to data)
 */
advertising.runQuery = function(advertisingQuery) {

  return Promise.all([
    sizeup.data.getPlaceBySeokey(
      `${advertisingQuery.area.place.state}/${advertisingQuery.area.place.county}/${advertisingQuery.area.place.city}`),
    sizeup.data.getIndustryBySeokey(advertisingQuery.ranking_metric.industry)
  ])

  .then(([place, industry]) => {
    var argument_list = {
      bands: 5, 
      page: 1,
      industryId: industry[0].Id,
      geographicLocationId: place[0].Id,
      sortAttribute: advertisingQuery.ranking_metric.kpi,
      attribute: advertisingQuery.ranking_metric.kpi, 
    }
    for ( let key in advertisingQuery.filter) {
      if (advertisingQuery.filter.hasOwnProperty(key)) {
        if (!advertisingQuery.filter.hasOwnProperty('max'))
          argument_list[key] = [ advertisingQuery.filter[key]['min'], null ];
        else {
          argument_list[key] = [ advertisingQuery.filter[key]['min'], advertisingQuery.filter[key]['max']];
        }
      }
    }
    for ( let key in advertisingQuery.ranking_metric ) {
      if (advertisingQuery.ranking_metric.hasOwnProperty(key)) {
        argument_list[key] = advertisingQuery.ranking_metric[key];
      }
    }
    for ( let key in advertisingQuery.area) {
      if (advertisingQuery.area.hasOwnProperty(key)) {
        argument_list[key] = advertisingQuery.area[key];
      }
    }
    if (advertisingQuery.ranking_metric.hasOwnProperty('order')) {
      argument_list['sort'] = advertisingQuery.ranking_metric['order'];
    }
    return Promise.all([
      Promise.resolve(place),
      Promise.resolve(industry),
      sizeup.data.getBestPlacesToAdvertise(argument_list),
      sizeup.data.getBestPlacesToAdvertiseBands(argument_list)
    ]);
  })

  .then(([place, industry, bestPlaces, bestPlacesBands]) => {
    return {
      query: advertisingQuery,
      place: place[0],
      industry: industry[0],
      bestPlaces: bestPlaces,
      bands: bestPlacesBands
    };
  });
};

advertising.generatePDF = function(advertisingQuery, customerKey, stream, title) {
  return advertising.runQuery(advertisingQuery)
  .then((report) => {
    return advertising.renderPDF(report, customerKey, stream, title);
  });
};

// Functions to run on the report

advertising.getShortTitle = function(advertisingReport, locale) {
  // We don't really use locale at the moment, but I want consumers to pass
  // it in if they feel like it, since this function outputs natural language.
  // TODO(twilson) internationalize properly, someday.
  if (locale && locale != "en-US") {
    throw Error("Unsupported locale " + locale);
  }
  try {
    if (advertisingReport.query.area.distance) {
      return "Best places to advertise near " + advertisingReport.place.City.LongName;
    } else {
      return "Best places to advertise nationwide";
    }
  } catch (_nullPointer) {
    return "Best places to advertise";
  }
};

advertising.renderPDF = function(advertisingReport, customerKey, stream, title) {
  if (typeof(title) == "undefined") {
    title = advertising.getShortTitle(advertisingReport);
  }
  // n.b: getReportGraphics is expected to remain synchronous, but not sure
  var graphics = sizeup.customer.getReportGraphics(customerKey);
  
  return pdf.getGoogleMap(advertisingReport)
  .then(map => {
    return pdf.buildPdf(advertisingReport, graphics, map, stream, title);
  });
};

module.exports = {
  advertising: advertising,
  setConfig: setConfig,
  setSizeup: setSizeup
};
