const fs = require('fs');
const PDFDocument = require('pdfkit');
const https = require('https');
require('dotenv').config();
const request = require('request');
const staticMap = require('./mapGenerator.js');

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

var scalarFilters = ["medianAge", "whiteCollarWorkers", "bachelorsDegreeOrHigher", "highSchoolOrHigher"];
var searchFilterTypes = { 
                    'population' : 'scalar',
                    'averageRevenue' : 'money-range',
                    'totalRevenue' : 'money-range',
                    'totalEmployees' : 'scalar-range',
                    'revenuePerCapita' : 'money-range',
                    'householdIncome' : 'money-range',
                    'householdExpenditures' : 'money-range',
                    'medianAge' : 'scalar-range',
                    'bachelorsDegreeOrHigher' : 'percent-or-higher',
                    'highSchoolOrHigher' : 'percent-or-higher',
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
                    'highSchoolOrHigher' : 'percent',
                    'whiteCollarWorkers' : 'percent',
                    'householdIncome' : 'money-average',
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

function getBand(kpi, bands, Item) {
  if (kpi === 'underservedMarkets') { kpi = 'revenuePerCapita'; }
  let point;
  if (itemFilterTypes[kpi].includes('range')) {
    point = Item[filterToItemFilter(kpi)].Min;
  } else {
    point = Item[filterToItemFilter(kpi)];
  }
  for(let i=0; i<bands.length; i++) {
    if (point >= bands[i].Min && point <= bands[i].Max) {
      // console.log("the band should be ", i);
      return i;
    }
  }
}
  
/******
 * startPdf gets the map and after getting it calls the 
 * buildPdf function to create the pdf
 */

function startPdf(
  report, customerGraphics, stream, title
) {
  // var numBands = 5; // TODO this function is mutating objects
  // that don't belong to it. Please find another way to do this.
  // done, J  I wasn't really using it anyway, but the display
  // of bands is going to be very sensitive to not being 5
  // there's not much room to play with in that little area

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
  let centroidArr = getCentroids(report.bestPlaces.Items);
  for (let i=0; i<centroidArr.length; i++) {
    let markerLabel = String.fromCharCode(65 + i);
    // I need to know what band it's in to get the color
    whichBand = getBand(report.query.ranking_metric.kpi, report.bands, report.bestPlaces.Items[i]);
    markerStr += "markers=color:" + pdfColors[whichBand].replace("#", "0x") + "%7C" + "label:" + markerLabel + "%7C" + centroidArr[i]['latitude'] + ',' + centroidArr[i]['longitude'] + '&';
  }
  const url = 'https://maps.googleapis.com/maps/api/staticmap?size=400x300&maptype=roadmap&' + markerStr + 'key=' + process.env.GOOGLEMAP_KEY; 
  var request = require('request').defaults({ encoding: null });
  request.get(url, function (error, response, body) {
    if (!error && response.statusCode == 200) {
      data = "data:" + response.headers["content-type"] + ";base64," + new Buffer(body).toString('base64');
      buildPdf( report.query, report.place.City.LongName, report.industry.Name, 
        customerGraphics, report.bands, report.bestPlaces.Items, msg="success", 
        pdfColors, body, stream, title);
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

function displaySrch(realFiltersArr, doc, filter, pdfColors, distance) {

  doc.rect( 25, 180, 158, 300 );
  doc.fillAndStroke('#f3f3f3');
  doc.fillColor('black');
  let i = 0;
  let startX = 30;
  doc.y = 173;
  doc.text(' ');
  doc.text('The list has been filtered to include only areas that have:', startX, doc.y, { width: 146 })
  .text(' ')
  .fontSize(8);
  if (realFiltersArr.length === 0) {
    doc.text('Distance: ' + distance + ' miles', startX, doc.y, { width: 146 });
    return;
  }
  realFiltersArr.forEach(function(element) {
    i++;
    if (searchFilterTypes[element] === 'money-range') {
      doc.text(filterToDisplay(element) + ' between ' + formatDollars(filter[element].min) + ' and ' + formatDollars(filter[element].max), startX, doc.y, { width: 146 });
    }
    else if (searchFilterTypes[element] === 'scalar') {
      doc.text(filterToDisplay(element) + ' ' + filter[element] + ' or greater', startX, doc.y, { width: 146 }); 
    }
    else if (searchFilterTypes[element] === 'scalar-range') {
      doc.text(filterToDisplay(element) + ' between ' + numberWithCommas(filter[element].min) + ' and ' + numberWithCommas(filter[element].max), startX, doc.y, { width: 146 });
    }
    else if (searchFilterTypes[element] === 'percent-or-higher') {
      doc.text(filterToDisplay(element) + ' greater than ' + filter[element].min + '%', startX, doc.y, { width: 146 }); 
    }
    if (i < realFiltersArr.length) {
     doc.text(' ');
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

function sortIndicator(sortAttribute, order, pdfColors, doc) {
  doc.fillColor(pdfColors[3]);
  doc.fontSize(6);
  doc.text(sortAttribute, 515 - doc.widthOfString(sortAttribute), doc.y);
  // tiny asc or desc triangle
  if (order === 'desc') {
    doc.polygon( [519, doc.y - 2], [523, doc.y - 2], [521, doc.y - 6]);
  } else {
    doc.polygon( [519, doc.y - 6], [523, doc.y - 6], [521, doc.y - 2]);
  }
  doc.fillAndStroke(pdfColors[3]) 
}
  
/****
 * this function is building the pdf  
 */

function buildPdf( query, LongName, industryName, 
        customerGraphics, bands, Items, msg="success", 
        pdfColors, googleMap, stream, title) {
  
  let doc = new PDFDocument( { 'margins':  { 'top': 0, 'bottom': 0, 'left': 0, 'right': 20 } } );

  doc.pipe(stream);
  
  doc.image(googleMap, 183, 180, { width: 400 } );

  let theme = { text: { color: pdfColors[2] } };
  let realFiltersArr = getRealFilters(query.filter);
  
  let sortAttribute = '';
  if (query.ranking_metric.kpi === 'underservedMarkets') {
    sortAttribute = 'revenuePerCapita';
  } else {
    sortAttribute = query.ranking_metric.kpi;
  }

  // start writing text

  // header text
  customerGraphics.writeHeader(doc, theme);
  doc.font('Helvetica-Bold');

  doc.fontSize(15);
  doc.moveDown(2);
  doc.fillColor(pdfColors[4])
  doc.text(title, 25, doc.y);
  doc.fillColor(pdfColors[5]);
  doc.fontSize(10);
  doc.fontSize(10);
    doc.text("This is a list of postal codes with the highest ", 25, doc.y + 10, { continued: true } )
    .fillColor(pdfColors[3])
    .text(filterToDisplay(sortAttribute), { continued: true } )
    .fillColor(pdfColors[5])
    .text(" in the ", { continued: true } )
    .fillColor(pdfColors[3])
    .text(industryName, { continued: true } )
    .fillColor(pdfColors[5])
    .text(" industry.  You should consider using this list if you are selling to businesses or consumers and want to know where the ", { continued: true } )
    .fillColor(pdfColors[3])
    .text(filterToDisplay(sortAttribute), { continued: true } )
    .fillColor(pdfColors[5])
    .text(" is the highest. ", { continued: true } )
    .text("The analysis is based on locations ", { continued: true } )
   .fillColor(pdfColors[3])
    .text(query.area.distance, { continued: true } )
    .fillColor(pdfColors[5])
    .text(" miles from the centroid of ", { continued: true } )
   .fillColor(pdfColors[3])
    .text(LongName, { continued: true } );
   doc.fillColor(pdfColors[5]);
   displaySrch(realFiltersArr, doc, query.filter, pdfColors, query.area.distance);
  doc.fontSize(10);   
  doc.fillColor(pdfColors[4]);

  // the bands 
  doc.fontSize(8);
  doc.fillColor(pdfColors[4]);
  doc.moveDown(0.5);

  doc.rect( 25, 480, 558, 22 );
  doc.fillAndStroke('#f3f3f3');

  //  construct an array of x,y starting points for the bands
  let j = 0, n = 0; m = 0;
  let bandMinText, widthMinText, widthDash;
  let startArr = [];
  for (let k=0; k<bands.length; k++) {
    n = k % 3;  // n (remainder of k/3) is the column in the display of bands
    m = Math.floor(k/3);  // each row will have 3 bands listed
    startArr.push([200 + n*140, 484 + m*10]);
  }

  // then render the bands
  doc.fillColor(pdfColors[4]);
  i = 0;
  bands.forEach(function(element) {
    doc.fillColor(pdfColors[i]);
    if (itemFilterTypes[sortAttribute].includes('money')) {
      bandMinText = formatDollars(element.Min);
      bandMaxText = formatDollars(element.Max);
    } else {
      bandMinText = element.Min.toString();
      bandMaxText = element.Max.toString();
    }
    miniPin(startArr[j][0] - 4, startArr[j][1] + 5, pdfColors[i], doc);
    doc.text(bandMinText, startArr[j][0], startArr[j][1]);
    widthMinText = doc.widthOfString(bandMinText);
    doc.text(' - ', startArr[j][0] + widthMinText, startArr[j][1]);
    widthDash = doc.widthOfString(' - ');
    doc.text(bandMaxText, startArr[j][0] + widthMinText + widthDash, startArr[j][1]);
    i++;
    j++;
  });
  
  let xpos = 250;
  const listHeight = 210;  // pixels given to the bottom section
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
 // sortIndicator(sortAttribute, searchObj.ranking_metric.order, pdfColors, doc);
  sortIndicator(sortAttribute, query.ranking_metric.order, pdfColors, doc);
  let toggle = 1;
  for (let i=0; i<Items.length; i++) {
    if (doc.y > 620) {
      // footer text
      customerGraphics.writeFooter(doc, theme);
      doc.addPage();
      customerGraphics.writeHeader(doc, theme);
      doc.font('Helvetica-Bold');
      doc.text(' ');
      doc.y = 110;
      sortIndicator(sortAttribute, query.ranking_metric.order, pdfColors, doc);
    }
     doc.fillColor(pdfColors[getBand(query.ranking_metric.kpi, bands, Items[i])])
    .text(' ')
    .fontSize(10)
    .circle(75, doc.y + 7, 7);
    doc.fill()
    .fillColor('#ffffff')
    .text(String.fromCharCode(65 + i), 72, doc.y + 3, { continued: true } )
    .fillColor(pdfColors[getBand(query.ranking_metric.kpi, bands, Items[i])])
    .fontSize(15)
    .text("  ", { continued: true } )
    .text(Items[i].ZipCode.Name)
    .fillColor('black')
    .fontSize(13)
    .moveDown(-1);
    if (itemFilterTypes[sortAttribute] === 'money-range') {
      xpos = 535 - (doc.widthOfString(formatDollars(Items[i][filterToItemFilter(sortAttribute)].Min).toString()) + doc.widthOfString(" - ") + doc.widthOfString(formatDollars(Items[i][filterToItemFilter(sortAttribute)].Max).toString()));
      doc.text(formatDollars(Items[i][filterToItemFilter(sortAttribute)].Min).toString() + " - " + formatDollars(Items[i][filterToItemFilter(sortAttribute)].Max).toString(), xpos, doc.y, { continued: true } );
    }
    if (itemFilterTypes[sortAttribute] === 'money-average') {
      xpos = 535 - (doc.widthOfString(formatDollars(Items[i][filterToItemFilter(sortAttribute)]).toString()));
      doc.text(formatDollars(Items[i][filterToItemFilter(sortAttribute)]), xpos, doc.y, { continued: true } )
    }
    if (itemFilterTypes[sortAttribute] === 'scalar') {
      xpos = 535 - (doc.widthOfString(Items[i][filterToItemFilter(sortAttribute)].toString()));
      doc.text(Items[i][filterToItemFilter(sortAttribute)].toString(), xpos, doc.y, { continued: true } );
    }
    if (itemFilterTypes[sortAttribute] === 'scalar-range') {
      xpos = 535 - (doc.widthOfString(Items[i][filterToItemFilter(sortAttribute)].Min.toString()) + doc.widthOfString(" - ") + doc.widthOfString(Items[i][filterToItemFilter(sortAttribute)].Max.toString()));
      doc.text(Items[i][filterToItemFilter(sortAttribute)].Min.toString() + " - " + Items[i][filterToItemFilter(sortAttribute)].Max.toString(), xpos, doc.y, { continued: true } );
    }
    if (itemFilterTypes[sortAttribute] === 'percent') {
      xpos = 535 - (doc.widthOfString(Items[i][filterToItemFilter(sortAttribute + '%')].toString()));
      doc.text(Items[i][filterToItemFilter(sortAttribute + '%')].toString());
    }
     doc.text(' ');
     doc.fontSize(8);
     printBelowResultFilters(realFiltersArr, doc, Items[i]);
     doc.moveDown(1);
  }

  // footer text
  doc.text(' ');
  customerGraphics.writeFooter(doc, theme);


  // Finalize the pdf file
  doc.end();
}

module.exports = {
  startPdf: startPdf
}
