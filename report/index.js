var sizeup = require("sizeup-api")({ key:process.env.SIZEUP_KEY });
function setSizeup(_sizeup) {
  sizeup = _sizeup;
}

const pdf = require("./app.js");

var advertising = {};

/**
 * Accepts query (structure documented TODO) and runs functions in the
 * Sizeup API to generate a report that answers the query. The report
 * contains (tentative):
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
advertising.runQuery = function(query) {
  // TODO implement this. Presently it's part of generatePDF, but it
  // should be its own function, since the report data itself is
  // useful even if you don't want a PDF.
};

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

advertising.generatePDF = function(searchObj, customerKey, stream, title) {

  // seems like validation of searchObj has to happen here
  // it starts off right away with failure if any filters are missing
  // and hence have no min or max values

 // searchObj = validate(searchObj);
  validate(searchObj);

  Promise.all([
    sizeup.data.getPlaceBySeokey(
      `${searchObj.area.place.state}/${searchObj.area.place.county}/${searchObj.area.place.city}`),
    sizeup.data.getIndustryBySeokey(searchObj.ranking_metric.industry)
  ])
  
  .then(([place, industry]) => {
    var argument_list = {
      totalEmployees: [searchObj.filter.totalEmployees.min, searchObj.filter.totalEmployees.max],
      highSchoolOrHigher: searchObj.filter.highSchoolOrHigher.min,
      householdExpenditures: [searchObj.filter.householdExpenditures.min, searchObj.filter.householdExpenditures.max],
      householdIncome: [searchObj.filter.householdIncome.min, searchObj.filter.householdIncome.max],
      medianAge: [searchObj.filter.medianAge.min, searchObj.filter.medianAge.max],
      revenuePerCapita: [searchObj.filter.revenuePerCapita.min, searchObj.filter.revenuePerCapita.max],
      whiteCollarWorkers: searchObj.filter.whiteCollarWorkers.min,
      totalRevenue: [searchObj.filter.totalRevenue.min, searchObj.filter.totalRevenue.max],
      bands: 5,  // bands wasn't part of the search obj, so I'm just setting it to 5
      industryId: industry[0].Id,
      order: 'highToLow',  // don't see this is search obj
      page: 1,  // not sure what page is
      sort: searchObj.ranking_metric.order,  // doesn't seem right, but maybe is
      sortAttribute: searchObj.ranking_metric.kpi,  // I think
      geographicLocationId: place[0].Id,
      distance: searchObj.area.distance,
      attribute: searchObj.ranking_metric.kpi,  // not sure
    }
    Promise.all([
      Promise.resolve(place),
      Promise.resolve(industry),
      sizeup.data.getBestPlacesToAdvertise(argument_list),
      sizeup.data.getBestPlacesToAdvertiseBands(argument_list)
    ])
    
    .then(([place, industry, bestPlaces, bestPlacesBands]) => {
        var report = {
          query: searchObj,
          place: place[0],
          industry: industry[0],
          bestPlaces: bestPlaces,
          bands: bestPlacesBands
        };
        if (typeof(title) == "undefined") {
          title = advertising.getShortTitle(report);
        }
        pdf.startPdf(
          report,
          sizeup.customer.getReportGraphics(customerKey),
          "Best Places to Advertise", // TODO Jay: this looks like your
          // attempt to provide a default title, but I can't see where
          // this "msg" argument is used. In previous version, when
          // I specified no title there was simply no title on the PDF.
          stream, title);
    })
  }).catch(console.error);
};

function validate(searchObj) {
 // console.log(typeof searchObj.totalEmployees);
  let fieldsArr = ['totalEmployees', 'householdIncome', 'highSchoolOrHigher', 
    'averageRevenue', 'revenuePerCapita', 'householdExpenditures', 
    'medianAge', 'whiteCollarWorkers', 'bachelorsDegreeOrHigher', 'totalRevenue'];
  fieldsArr.forEach(function(element) {
    if (typeof searchObj.filter[element] === 'undefined') {
     searchObj.filter[element] = { 'min' : '0', 'max' : 'null' };
    }
  });
    /*

  if (typeof searchObj.filter.totalEmployees === 'undefined') {
    searchObj.filter.totalEmployees = { 'min' : '0', 'max' : 'null' };
  }
  if (typeof searchObj.filter.householdIncome === 'undefined') {
    searchObj.filter.householdIncome = { 'min' : '0', 'max' : 'null' };
  }
  if (typeof searchObj.filter.highSchoolOrHigher === 'undefined') {
    searchObj.filter.highSchoolOrHigher = { 'min' : '0', 'max' : 'null' };
  }
  // this needs to be done for 
   "totalRevenue": {
    "averageRevenue": {
    "totalEmployees": {
    "revenuePerCapita": {
    "householdIncome": {
    "householdExpenditures": {
    "medianAge": {
    "highSchoolOrHigher": {
    "whiteCollarWorkers": {
    "bachelorsDegreeOrHigher": {
    */

}

module.exports = {
  advertising: advertising,
  setSizeup: setSizeup
};
