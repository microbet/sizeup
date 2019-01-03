var sizeup = require("sizeup-api")({ key:process.env.SIZEUP_KEY });
function setSizeup(_sizeup) {
  sizeup = _sizeup;
}

const pdf = require("./app.js");

var advertising = {};

// Functions to run on the advertising query

advertising.validateQuery = function(advertisingQuery) {
  throw Error("Not implemented");
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
  var pReport = advertising.runQuery(advertisingQuery);
  pReport.then((report) => {
    advertising.renderPDF(report, customerKey, stream, title);
  })
  .catch(console.error);
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
    return "Best places to advertise near " + advertisingReport.place.City.LongName;
  } catch (_nullPointer) {
    return "Best places to advertise";
  }
};

advertising.renderPDF = function(advertisingReport, customerKey, stream, title) {
  if (typeof(title) == "undefined") {
    title = advertising.getShortTitle(advertisingReport);
  }
  pdf.startPdf(
    advertisingReport,
    sizeup.customer.getReportGraphics(customerKey),
   // "Best Places to Advertise", // TODO Jay: this looks like your
    // attempt to provide a default title, but I can't see where
    // this "msg" argument is used. In previous version, when
    // I specified no title there was simply no title on the PDF.
    // elminated, J - was not used for anything anymore
    stream, title);
};

// TODO this belongs in advertising.validateQuery, above, but it can't mutate
// the object it claims to be validating. The input object is either valid
// or invalid.
// J - I eliminated this function, didn't do the validation we need
// I'll redo it 

module.exports = {
  advertising: advertising,
  setSizeup: setSizeup
};
