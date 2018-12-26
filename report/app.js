const fs = require('fs');
const PDFDocument = require('pdfkit');
const https = require('https');
require('dotenv').config();
const request = require('request');
const staticMap = require('./mapGenerator.js');

var sizeup = require("sizeup-api")({ key:process.env.SIZEUP_KEY });

// Translate the query filter name to the API return item filter name
function filterToItemFilter(filter) {
  return filter.charAt(0).toUpperCase() + filter.slice(1);
}

function filterToDisplay(filter) {
  filterArr = filter.split('');
  let i = 0;
  filterArr.forEach(function(element) {
    if (element == element.toUpperCase()) {
      filterArr[i] = ' ' + element;
    }
    i++;
  });
  return filterToItemFilter(filterArr.join(''))
}



var moneyRangeFilters = ["totalRevenue", "averageRevenue", "revenuePerCapita"];
var moneyFilters = ["householdIncome", "householdExpenditures"];
var scalarRangeFilters = ["totalEmployees"];
var scalarFilters = ["medianAge", "whiteCollarWorkers", "bachelorsDegreeOrHigher", "highSchoolOrHigher"];
var searchFilterTypes = { 
                    'population' : 'scalar',
                    'averageRevenue' : 'money-range',
                    'totalRevenue' : 'money-range',
                    'totalEmployees' : 'scalar-range',
                    'revenuePerCapita' : 'money-range',
                    'householdExpenditures' : 'money-range',
                    'medianAge' : 'scalar-range',
                    'bachelorsDegreeOrHigher' : 'percent-or-higher',
                    'highschoolOrHigher' : 'percent-or-higher',
                    'whiteCollarWorkers' : 'percent-or-higher',
}

var itemFilterTypes = { 
                    'population' : 'scalar',
                    'averageRevenue' : 'money-range',
                    'totalRevenue' : 'money-range',
                    'totalEmployees' : 'scalar-range',
                    'revenuePerCapita' : 'money-range',
                    'householdExpenditures' : 'money-average',
                    'medianAge' : 'scalar',
                    'bachelorsDegreeOrHigher' : 'percent',
                    'highschoolOrHigher' : 'percent',
                    'whiteCollarWorkers' : 'percent',
                    'householdIncome' : 'money-average',
}


/***
* Function that can be called from outside.  Starts the process.
*/

var generatePDF = function( searchObj, customerKey, customerObj, stream) {

    Promise.all([
      sizeup.data.getPlaceBySeokey(
        `${searchObj.area.place.state}/${searchObj.area.place.county}/${searchObj.area.place.city}`),
      sizeup.data.getIndustryBySeokey(searchObj.ranking_metric.industry)
    ])
    
    .then(([place, industry]) => {
    //  searchObj.ranking_metric.kpi = 'averageRevenue'; // works
    //  searchObj.ranking_metric.kpi = 'totalRevenue'; // doesn't - test directly against api
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
  //      console.log("place = ", place);
  //      successCallback(
          startPdf(
          searchObj,
          place[0].City.LongName, industry[0].Name,
          customerObj.getReportGraphics(customerKey),
          bestPlacesBands, bestPlaces.Items, "Best Places to Advertise",
          stream);
      })
    }).catch(console.error);
}

/*****
 * Functions needed for the map
 */

function getCentroids(objWithCentroids) {
  let centroidArr = [];
  for (element of objWithCentroids) {
    centroidArr.push( { 'latitude' : element.Centroid.Lat, 'longitude' : element.Centroid.Lng } );
  }
  return centroidArr;
}

function getMapOptionsArr(centroidArr, pdfColors) {
  let markerStr = '';
  let markerLabel = '';
  for (let i=0; i<centroidArr.length; i++) {
    markerLabel = String.fromCharCode(65 + i);
    markerStr += "markers=color:" + pdfColors[i].replace("#", "0x") + "%7C" + "label:" + markerLabel + "%7C" + centroidArr[i]['latitude'] + ',' + centroidArr[i]['longitude'] + '&';
  }
  let optionsObj = {
	  url: 'https://maps.googleapis.com/maps/api/staticmap',
	  size: '600x300',
	  maptype: 'roadmap',
	  markerStr: markerStr,
	  key: process.env.GOOGLEMAP_KEY, 
  }
  return optionsObj;
}

function getBand(kpi, bestPlacesBands, Item) {
  for(let i=0; i<bestPlacesBands.length; i++) {
    if (Item[filterToItemFilter(kpi)].Min >= bestPlacesBands[i].Min && Item[filterToItemFilter(kpi)].Min <= bestPlacesBands[i].Max) {
      return i;
    }
  }
}
  
/******
 * startPdf gets the map and after getting it calls the 
 * buildPdf function to create the pdf
 */

function startPdf(
  searchObj, displayLocation, displayIndustry, customerGraphics,
  bestPlacesBands, bestPlacesItems, msg="success", stream
) {
  bestPlacesItems.bands = 5;

  /****
  * These colors are from SizeUp design and are used in the pdf
  */ 

  let pdfColors = [   
    '#dc3545', // red
    '#28a745', // green
    '#007bff', // blue
    '#fd7e14', // orange
    '#343a40', // dark grey
    '#6c757d', // gray
    '#dc3545', // red
    '#28a745', // green
    '#007bff', // blue
    '#fd7e14', // orange
    '#343a40', // dark grey
    '#6c757d', // gray
    '#dc3545', // red
    '#28a745', // green
    '#007bff', // blue
    '#fd7e14', // orange
    '#343a40', // dark grey
    '#6c757d', // gray
    '#dc3545', // red
    '#28a745', // green
    '#007bff', // blue
    '#fd7e14', // orange
    '#343a40', // dark grey
    '#dc3545', // red
    '#28a745', // green
    '#007bff', // blue
  ]
  
  let markerStr = '';
  let whichBand = 0;
  let centroidArr = getCentroids(bestPlacesItems);
  for (let i=0; i<centroidArr.length; i++) {
    let markerLabel = String.fromCharCode(65 + i);
    // I need to know what band it's in to get the color
    whichBand = getBand(searchObj.ranking_metric.kpi, bestPlacesBands, bestPlacesItems[i]);
    markerStr += "markers=color:" + pdfColors[whichBand].replace("#", "0x") + "%7C" + "label:" + markerLabel + "%7C" + centroidArr[i]['latitude'] + ',' + centroidArr[i]['longitude'] + '&';
  }
  const url = 'https://maps.googleapis.com/maps/api/staticmap?size=600x300&maptype=roadmap&' + markerStr + 'key=' + process.env.GOOGLEMAP_KEY; 
  var request = require('request').defaults({ encoding: null });
  request.get(url, function (error, response, body) {
    if (!error && response.statusCode == 200) {
      data = "data:" + response.headers["content-type"] + ";base64," + new Buffer(body).toString('base64');
     // buildPdf(pdfMsgObj, pdfColors, body);
      buildPdf( searchObj, displayLocation, displayIndustry, 
        customerGraphics, bestPlacesBands, bestPlacesItems, msg="success", 
        pdfColors, body, stream);
    }
  })
}

function getElementDisplay(element, item) {
  if (itemFilterTypes[element] === 'scalar') {
    return filterToItemFilter(element) + ': ' + numberWithCommas(item[filterToItemFilter(element)]);
  }
  else if (itemFilterTypes[element] === 'money-range') {
    return filterToItemFilter(element) + ': ' + formatDollars(item[filterToItemFilter(element)].Min) + ' - ' + formatDollars(item[filterToItemFilter(element)].Max);
  }
  else if (itemFilterTypes[element] == 'scalar-range') {
    return filterToItemFilter(element) + ': ' + numberWithCommas(item[filterToItemFilter(element)].Min) + ' - ' + numberWithCommas(item[filterToItemFilter(element)].Max);
  }
  else if (itemFilterTypes[element] == 'money-average') {
    return filterToItemFilter(element) + ' (Average): ' + formatDollars(item[filterToItemFilter(element)]);
  }
  else if (itemFilterTypes[element] == 'percent') {
    return filterToItemFilter(element) + ': ' + (Math.round(item[filterToItemFilter(element)] * 1000)/10) + '%';
  }
  else {
    return element;
  }
}

function displaySearch(realFiltersArr, doc, filter) {
//  console.log("rfa = ", realFiltersArr);
//  console.log("so = ", searchObj);
  realFiltersArr.forEach(function(element) {
    if (searchFilterTypes[element] === 'money-range') {
      doc.text(filterToDisplay(element) + ' between ' + formatDollars(filter[element].min) + ' and ' + formatDollars(filter[element].max) + ', ', doc.x, doc.y, { continued: true });
    }
    else if (searchFilterTypes[element] === 'scalar') {
      doc.text(filterToDisplay(element) + ' ' + filter[element] + ' or greater, ', doc.x, doc.y, { continued: true }); 
    }
    else if (searchFilterTypes[element] === 'scalar-range') {
      doc.text(filterToDisplay(element) + ' between ' + filter[element].min + ' and ' + filter[element].max + ', ', doc.x, doc.y, { continued: true });
    }
    else if (searchFilterTypes[element] === 'percent-or-higher') {
      doc.text(filterToDisplay(element) + ' greater than ' + filter[element].min + '%, ', doc.x, doc.y, { continued: true }); 
    }
  }); 
}

function printBelowResultFilters(realFiltersArr, doc, item) {
  let startX = 100;
  let widthAdj = 0;
  let displayElement = '';
  realFiltersArr.unshift('population');
  realFiltersArr.forEach(function(element) {
    displayElement = getElementDisplay(element, item);
    if (startX === 350) {
       doc.text(displayElement, startX - widthAdj, doc.y);
       startX = 100;
    } else {
       doc.text(displayElement, startX, doc.y, { continued: true });
       startX = 350;
    }
    widthAdj = doc.widthOfString(displayElement);
  });
  realFiltersArr.shift();
}

function formatDollars(string) {
  return Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD', maximumFractionDigits: 0, minimumFractionDigits: 0 }).format(string);
}

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function miniPin(x, y, color, doc) {
  doc.moveTo(x, y)   
   .lineWidth(1)
   .lineTo(x-2, y-3) 
   .lineTo(x+2, y-3)
   .circle(x, y-4, 2)
   .fillAndStroke(color, color);
}

// get the params from the search obj that aren't 0 or 0,null

function getRealFilters(filter) {
  let realFiltersArr = [];
  for (let key in filter) {
    if (filter[key].min != '0') {
      if (typeof filter[key].max !== 'undefined') {
        if (filter[key].max != 'null') {
          realFiltersArr.push(key);
        }
      } else {
        realFiltersArr.push(key);
      }
    }
  }
  return realFiltersArr;
}
  
/****
 * this function is building the pdf  
 */

function buildPdf( searchObj, displayLocation, displayIndustry, 
        customerGraphics, bestPlacesBands, bestPlacesItems, msg="success", 
        pdfColors, googleMap, stream) {
  
  // Create a document
  let doc = new PDFDocument;
  doc.pipe(stream);
  
  doc.image(googleMap, 25, 246, { width: 562 } );

  let theme = { text: { color: pdfColors[2] } };
  let realFiltersArr = getRealFilters(searchObj.filter);
  
  // Draw a rectangle for the header 
  doc.save()
    .moveTo(25, 30)
    .lineTo(588, 30)
    .lineTo(588, 90)
    .lineTo(25, 90)
    .fill(pdfColors[2]);
  
  // start writing text

  // header text
  customerGraphics.writeHeader(doc, theme);

  doc.fontSize(15);
  doc.moveDown(2);
  doc.fillColor(pdfColors[4])
  doc.text(searchObj.title, 25, doc.y);
  doc.fillColor(pdfColors[5]);
  doc.fontSize(10);
  doc.text("This is a list of postal codes with the highest combined business revenue in the ", 35, doc.y + 10, { continued: true } )
    .fillColor(pdfColors[3])
    .text(displayIndustry, { continued: true } )
    .fillColor(pdfColors[5])
    .text("industry.  You should consider using this list if you are selling to businesses or consumers and want to", { continued: true } )
    .text("know where the most money is being made in your industry. ", { continued: true } )
    .text("The analysis is based on locations ", { continued: true } )
   .fillColor(pdfColors[3])
    .text(searchObj.distance, { continued: true } )
    .fillColor(pdfColors[5])
    .text(" miles from the centroid of ", { continued: true } )
   .fillColor(pdfColors[3])
    .text(displayLocation, { continued: true } )
    .fillColor(pdfColors[5])
    .text(". The list has been filtered to include only areas that have ", { continued: true } );
   doc.fillColor(pdfColors[3]);
  // need to just get filters that are not maxed out
   displaySearch(realFiltersArr, doc, searchObj.filter);
    doc.text("")
    .moveDown(1);
    
  doc.fontSize(10);   
  doc.fillColor(pdfColors[4]);

  // the bands 
  doc.fontSize(8);
  doc.fillColor(pdfColors[4]);
  doc.moveDown(0.5);

  doc.rect( 25, 528, 563, 22 );
  doc.fillAndStroke('#f3f3f3');

  //  construct an array of x,y starting points for the bands
  let j = 0, n = 0; m = 0;
  let bandMinText, widthMinText, widthDash;
  let startArr = [];
  for (let k=0; k<bestPlacesItems.bands; k++) {
    n = k % 3;  // n (remainder of k/3) is the column in the display of bands
    m = Math.floor(k/3);  // each row will have 3 bands listed
    startArr.push([40 + n*180, 532 + m*10]);
  }

  // then render the bands
  doc.fillColor(pdfColors[4]);
  i = 0;
  bestPlacesBands.forEach(function(element) {
    doc.fillColor(pdfColors[i]);
    i++;
    bandMinText = Intl.NumberFormat('en-US', { 
      style: 'currency',
      currency: 'USD',
      maximumFractionDigits: 0,
      minimumFractionDigits: 0 
    }).format(element.Min);
    miniPin(startArr[j][0] - 4, startArr[j][1] + 5, pdfColors[i], doc);
    doc.text(bandMinText, startArr[j][0], startArr[j][1]);
    widthMinText = doc.widthOfString(bandMinText);
    doc.text(' - ', startArr[j][0] + widthMinText, startArr[j][1]);
    widthDash = doc.widthOfString(' - ');
    doc.text(Intl.NumberFormat('en-US', { 
      style: 'currency', 
      currency: 'USD', 
      maximumFractionDigits: 0, 
      minimumFractionDigits: 0 
    }).format(element.Max), startArr[j][0] + widthMinText + widthDash, startArr[j][1]);
    j++;
  });
  
  let xpos = 250;
  const listHeight = 165;  // pixels given to the bottom section
  doc.y = 720 - listHeight;
  let secondMargin = 300;
  let skipX = 0;

  /**
   * note: Dynamic font sizing is a possibility if more flexibility in
   * what content gets displayed on one page is needed.  It makes
   * precise layout more difficult though. eg:
   *
   *  numFields = pdfMsgObj.zip.length;
   *  fieldHeight = listHeight/numFields;
   *  fieldFontHeight = Math.round(fieldHeight/6); 
   *  subFieldFontHeight = Math.round(fieldFontHeight/2);
   */
  
  let sortAttribute = '';
  if (searchObj.ranking_metric.kpi === 'underservedMarkets') {
    sortAttribute = revenuePerCapita;
  } else {
    sortAttribute = searchObj.ranking_metric.kpi;
  }
  doc.fillColor(pdfColors[3]);
  doc.fontSize(6);
  doc.text(sortAttribute, 515 - doc.widthOfString(sortAttribute), doc.y);
  // tiny asc or desc triangle
  if (searchObj.ranking_metric.order === 'desc') {
    doc.polygon( [519, doc.y - 2], [523, doc.y - 2], [521, doc.y - 6]);
  } else {
    doc.polygon( [519, doc.y - 6], [523, doc.y - 6], [521, doc.y - 2]);
  }
  doc.fillAndStroke(pdfColors[3]) 
  let toggle = 1;
  for (let i=0; i<bestPlacesItems.length; i++) {
    if (doc.y > 600) {
      // footer text
      doc.text(' ');
      doc.moveDown(3);
      customerGraphics.writeFooter(doc, theme);
    }
    doc.fillColor(pdfColors[getBand(searchObj.ranking_metric.kpi, bestPlacesBands, bestPlacesItems[i])])
    .moveDown(1)
    .text(' ')
    .fontSize(10)
    .circle(75, doc.y + 5, 7);
    doc.fill()
    .fillColor('#ffffff')
    .text(String.fromCharCode(65 + i), 72, doc.y + 1, { continued: true } )
    .fillColor(pdfColors[getBand(searchObj.ranking_metric.kpi, bestPlacesBands, bestPlacesItems[i])])
    .fontSize(15)
    .text("  ", { continued: true } )
    .text(bestPlacesItems[i].ZipCode.Name) // , { continued: true })
    .fillColor('black')
    .fontSize(13)
    .moveDown(-1);
    xpos = 535 - (doc.widthOfString(formatDollars(bestPlacesItems[i][filterToItemFilter(sortAttribute)].Min.toString())) + doc.widthOfString(" - ") + doc.widthOfString(formatDollars(bestPlacesItems[i][filterToItemFilter(sortAttribute)].Max.toString())));
    doc.text(formatDollars(bestPlacesItems[i][filterToItemFilter(sortAttribute)].Min), xpos, doc.y, { continued: true } )
    .text(" - ", { continued: true } )
    .text(formatDollars(bestPlacesItems[i][filterToItemFilter(sortAttribute)].Max))
    .fillColor(pdfColors[5])
    .fontSize(8);
     printBelowResultFilters(realFiltersArr, doc, bestPlacesItems[i]);
     doc.moveDown(1);
  }

  // footer text
  doc.text(' ');
  customerGraphics.writeFooter(doc, theme);


  // Finalize the pdf file
  doc.end();
//  return pdfMsgObj.stream;
  console.log("PDF output.pdf created");
}

module.exports = {
  generatePDF: generatePDF,
  // setSizeup: setSizeup
}
