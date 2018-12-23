const fs = require('fs');
const PDFDocument = require('pdfkit');
const https = require('https');
require('dotenv').config();
const request = require('request');
const staticMap = require('./mapGenerator.js');

var sizeup = require("sizeup-api")({ key:process.env.SIZEUP_KEY });
/*
function setSizeup(sizeupObj) {
  sizeup = sizeupObj;
}
*/
  /***
  reference - colors from sizeup
  --blue:#007bff;   --indigo:#6610f2;   --purple:#6f42c1;   --pink:#e83e8c;
  --red:#dc3545;    --orange:#fd7e14;   --yellow:#ffc107;   --green:#28a745;
  --teal:#20c997;   --cyan:#17a2b8;     --white:#fff;     --gray:#6c757d;
  --gray-dark:#343a40; --primary:#007bff;   --secondary:#6c757d;  --success:#28a745;
  --info:#17a2b8;   --warning:#ffc107;    --danger:#dc3545;   --light:#f8f9fa;
  --dark:#343a40;
  */

// display the search criteria to the user as capitalized words
// instead of one camelcased word

function formatCamelToDisplay(input) {
  input_arr = input.split('');
  input_arr.forEach(function(element, index, input_arr) {
    if (element == element.toUpperCase()) {
      input_arr[index] = ' ' + element;
    }
  });
  input_arr[0] = input_arr[0].toUpperCase();
  return input_arr.join('');
}

// Translate the query filter name to the API return item filter name
function filterToItemFilter(filter) {
  return filter.charAt(0).toUpperCase() + filter.slice(1);
}

/*
* this is used for the temporary image file name.  I think there is a better
* way to do this with the temp directory, but I don't see it immediately and
* I'm afraid that might cause a bug when moving to a production env I don't 
* know at this point.
*/

function IDGenerator() { 
   
  let length = 15;
  let timestamp = +new Date;
     
  let _getRandomInt = function( min, max ) {
    return Math.floor( Math.random() * ( max - min + 1 ) ) + min;
  }
     
  let ts = timestamp.toString();
  let parts = ts.split( "" ).reverse();
  let id = "";
  let index = "";
  for( let i = 0; i < length; ++i ) {
      index = _getRandomInt( 0, parts.length - 1 );
      id += parts[index];  
  }
  return id;
}

/***
 * It's just easier later if we know all the filter terms are 
 * defined. Also, everything probably comes in as a string,
 * but we should get the correct typing here.
 */

function isNumeric(n) {
  return !isNaN(parseFloat(n)) && isFinite(n);
}

function fixParam(paramObj, range) {
  let err = '';
  if (paramObj == null) {
    if (range === 'min') {
      paramObj = { "min" : 0 }
    }
    if (range === 'both') {
      paramObj = { "min" : 0, "max" : null, }
    }
  } else {
    if (paramObj.min == null) {
      paramObj.min = 0;
    } else {
      paramObj.min = parseFloat(paramObj.min);
    }
    if (paramObj.max == null) {  // could be undefined
      paramObj.max = null;
    } else { 
      if (paramObj.max.toLowerCase() === 'null') {
        paramObj.max = null;
      } else {
        paramObj.max = parseFloat(paramObj.max);
      }
    }
  }
  if (isNaN(paramObj.min)) {
    paramObj.min = 0;
    err += " min parameter was not a number";
  }
  if (isNaN(paramObj.max)) {
    paramObj.max = null;
    err += " max parameter was not a number";
  }
  return err;
}

function filloutPdfFilter(pdfMsgObj) {
  if (err = fixParam(pdfMsgObj.filter.whiteCollarWorkers, 'min')) {
    pdfMsgObj.error += 'White Collar Worker ' + err;
  };
  if (err = fixParam(pdfMsgObj.filter.bachelorsDegreeOrHigher, 'min')) {
    pdfMsgObj.error += 'Bachelors degree or higher ' + err;
  };
  if (err = fixParam(pdfMsgObj.filter.highSchoolOrHigher, 'min')) {
    pdfMsgObj.error += 'High school or higher ' + err;
  };
  if (err = fixParam(pdfMsgObj.filter.medianAge, 'both')) {
    pdfMsgObj.error += 'Median Age ' + err;
  }
  if (err = fixParam(pdfMsgObj.filter.totalRevenue, 'both')) {
    pdfMsgObj.error += 'Total Revenue ' + err;
  }
  if (err = fixParam(pdfMsgObj.filter.averageRevenue, 'both')) {
    pdfMsgObj.error += 'Average Revenue ' + err;
  }
  if (err = fixParam(pdfMsgObj.filter.revenuePerCapita, 'both')) {
    pdfMsgObj.error += 'Revenue per capita ' + err;
  }
  if (err = fixParam(pdfMsgObj.filter.totalEmployees, 'both')) {
    pdfMsgObj.error += 'Total Employees ' + err;
  }
}
 
var moneyRangeFilters = ["totalRevenue", "averageRevenue", "revenuePerCapita"];
var moneyFilters = ["householdIncome", "householdExpenditures"];
var scalarRangeFilters = ["totalEmployees"];
var scalarFilters = ["medianAge", "whiteCollarWorkers", "bachelorsDegreeOrHigher", "highSchoolOrHigher"];

/***
* Function that can be called from outside.  Starts the process.
*/

var generatePDF = async function(
	 searchObj,
    customerKey,
	 customerObj,
    stream) {
  
    // pdfMsgObj holds most of the data to be used in the pdf
	// if the bestplacestoadvertise functions took the objects
	// a lot of this wouldn't be necessary
  
    let pdfMsgObj = {};
        pdfMsgObj.error = '';
        pdfMsgObj.filter = searchObj.filter;
        filloutPdfFilter(pdfMsgObj);
        pdfMsgObj.title = searchObj.title;
	 pdfMsgObj.bands = 5;
	 pdfMsgObj.sortAttribute = searchObj.ranking_metric.kpi;  // I think
         pdfMsgObj.distance = searchObj.area.distance;
	 pdfMsgObj.attribute = searchObj.ranking_metric.kpi;  // not sure 
         pdfMsgObj.displayAttribute = formatCamelToDisplay(pdfMsgObj.attribute);
         pdfMsgObj.customerGraphics = customerObj.getReportGraphics(customerKey);
         pdfMsgObj.stream = stream;
         pdfMsgObj.filterDisplay = { toggle: 1 };

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

    for (filter of moneyRangeFilters.concat(scalarRangeFilters)) {
      if (searchObj.filter[filter].min === 0 && searchObj.filter[filter].max === null) {
        pdfMsgObj.filterDisplay[filter] = true;
      }
    }
    // I can't quite figure this one. I made the "*Filters" arrays based on how they
    // were used in successCallback. But here it seems like more of them are range
    // filters. The UI is confusing. Really all of them are range filters, but some
    // of them don't let the user control the "max" part of the range. I'm leaving
    // these, will clean up later.
    if (searchObj.filter.householdIncome.min === 0 && searchObj.filter.householdIncome.max === null) { pdfMsgObj.filterDisplay.householdIncome = true; }
    if (searchObj.filter.highSchoolOrHigher.min != 0) { pdfMsgObj.filterDisplay.highSchoolOrHigher = true; }
    if (searchObj.filter.medianAge.min != 0) { pdfMsgObj.filterDisplay.medianAge = true; }
    if (pdfMsgObj.filter.whiteCollarWorkers.min != 0) { pdfMsgObj.filterDisplay.whiteCollarWorkers = true; }
    pdfMsgObj.filterDisplay.population = true;

    /***
    * Communication with sizeup api
    * Get the info from the sizeup api, then in successCallback put the return info into the pdfMsgObj
    * Then build the pdf with that info
    */	

    Promise.all([
      sizeup.data.getPlaceBySeokey(
        `${searchObj.area.place.state}/${searchObj.area.place.county}/${searchObj.area.place.city}`),
      sizeup.data.getIndustryBySeokey(searchObj.ranking_metric.industry)
    ]).then(([place, industry]) => {
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
        attribute: searchObj.ranking_metric.kpi  // not sure
        // I think bachelorsdegree or higher is left out
      }
      Promise.all([
        Promise.resolve(place),
        Promise.resolve(industry),
        sizeup.data.getBestPlacesToAdvertise(argument_list),
        sizeup.data.getBestPlacesToAdvertiseBands(argument_list)
      ]).then(([place, industry, bestPlaces, bestPlacesBands]) => {
        pdfMsgObj['displayLocation'] = place[0].City.LongName;
        pdfMsgObj['displayIndustry'] = industry[0].Name;
        pdfMsgObj['bandArr'] = bestPlacesBands;
        successCallback(pdfMsgObj, pdfColors, bestPlaces.Items, "Best Places to Advertise"); 
      })
    }).catch(console.error);
  console.log(pdfMsgObj.error);
};

/**
 *  successCallback puts the return info into the pdfMsgObj.  The result (Items) is an array
 *  so loop through to build the arrays which are members of the pdfMsgObj
 *  A lot of formatting in here.  Max output is 3.  It's hard to present more on one page.
 */

function successCallback(pdfMsgObj, pdfColors, result, msg="success") {
  let i=0;
  pdfMsgObj.zip = [];
  pdfMsgObj.centroidLng = [];
  pdfMsgObj.centroidLat = [];
  pdfMsgObj.totalRevenueMin = [];
  pdfMsgObj.totalRevenueMax = [];
  pdfMsgObj.population = [];
  pdfMsgObj.averageRevenueMin = [];
  pdfMsgObj.averageRevenueMax = [];
  pdfMsgObj.totalEmployeesMin = [];
  pdfMsgObj.totalEmployeesMax = [];
  pdfMsgObj.revenuePerCapitaMin = [];
  pdfMsgObj.revenuePerCapitaMax = [];
  pdfMsgObj.householdIncome = [];
  pdfMsgObj.medianAge = [];
  pdfMsgObj.householdExpenditures = [];
  pdfMsgObj.whiteCollarWorkers = [];
  pdfMsgObj.bachelorsDegreeOrHigher = [];
  pdfMsgObj.highSchoolOrHigher = [];
  var currencyFormat = {
    style: 'currency', currency: 'USD', maximumFractionDigits: 0, minimumFractionDigits: 0
  };
  for (element of result) {
    i++;
    pdfMsgObj['zip'].push(element.ZipCode.Name);
//    console.log(element);   // good for debugging
    pdfMsgObj['centroidLng'].push(element.Centroid.Lng);
    pdfMsgObj['centroidLat'].push(element.Centroid.Lat);
    pdfMsgObj['population'].push(element.Population);

    for (filter of moneyRangeFilters) {
      pdfMsgObj[filter+'Min'].push(
        Intl.NumberFormat('en-US', currencyFormat).format(element[filterToItemFilter(filter)].Min));
      // throws an error if maxFDig is set to 0, but not minFDig
      pdfMsgObj[filter+'Max'].push(
        Intl.NumberFormat('en-US', currencyFormat).format(element[filterToItemFilter(filter)].Max));
    }

    for (filter of moneyFilters) {
      pdfMsgObj[filter].push(
        Intl.NumberFormat('en-US', currencyFormat).format(element[filterToItemFilter(filter)]));
    }

    for (filter of scalarFilters) {
      pdfMsgObj[filter].push(element[filterToItemFilter(filter)]);
    }

    for (filter of scalarRangeFilters) {
      pdfMsgObj[filter+'Min'].push(element[filterToItemFilter(filter)].Min);
      pdfMsgObj[filter+'Max'].push(element[filterToItemFilter(filter)].Max);
    }

    if (i >= 26) { break; }
  }
  startPdf(pdfMsgObj, pdfColors);
}

function failureCallback(error) {
  console.log("failure: " + error);
}

/****
 * note: If more than one pdf style is necessary it would not be hard
 * to make a template and then some markup for inserting values of the
 * pdfMsgObj. 
 */

function getBand(n, pdfMsgObj) {
  for(let i=0; i<pdfMsgObj.bandArr.length; i++) {
    let pMin = parseInt(pdfMsgObj.averageRevenueMin[n].replace(/\$/g, "").replace(/,/g,""));
    let pMax = parseInt(pdfMsgObj.averageRevenueMax[n].replace(/\$/g, "").replace(/,/g,""));
    if (pMin >= pdfMsgObj.bandArr[i].Min && pMax <= pdfMsgObj.bandArr[i].Max) {
      return i;
    }
  }
}
  

/***
*  startPdf creates the static map image using static google map api.
*  Perhaps the image could be streamed into the pdf and supposedly 
*  that is possible with pdfkit, but it didn't work as shown in docs. 
*  The image is downloaded and then brought into the pdf.
*  It is given a virtually unique file name and deleted after it 
*  is brought into the pdf. 
*/
 
function startPdf(pdfMsgObj, pdfColors) {

  // building the markers string for the pins on the map, then download the static map

  let markerStr = '';
  let whichBand = 0;
  for (let i=0; i<pdfMsgObj.centroidLat.length; i++) {
    let markerLabel = String.fromCharCode(65 + i);
    // I need to know what band it's in to get the color
    whichBand = getBand(i, pdfMsgObj);
    markerStr += "markers=color:" + pdfColors[whichBand].replace("#", "0x") + "%7C" + "label:" + markerLabel + "%7C" + pdfMsgObj.centroidLat[i] + ',' + pdfMsgObj.centroidLng[i] + '&';
  }
  const url = 'https://maps.googleapis.com/maps/api/staticmap?size=600x300&maptype=roadmap&' + markerStr + 'key=' + process.env.GOOGLEMAP_KEY; 
  var download = function(uri, filename, callback){
    request.head(uri, function(err, res, body){
      request(uri).pipe(fs.createWriteStream(filename)).on('close', callback);
    });
  };

  let mapImgFile = IDGenerator();  // create the random/unique name
  mapImgFile = mapImgFile + '.png';
    pdfMsgObj.mapImgFile = mapImgFile;
  download(url,  mapImgFile, function(){
    pdfMsgObj.mapImgFile = mapImgFile;
    buildPdf(pdfMsgObj, pdfColors);
  });
}

// instead of calling startPdf and having that call buildPdf I should just call buildPdf and 
// from inside of there I could call getStaticMap and that could be in another file


// are there no max's showing?
function showFilter(pdfMsgObj, label, param, min, max, doc, suffix=' ') {
  let startX;
  if (pdfMsgObj.filterDisplay[param] && param !== pdfMsgObj.sortAttribute) {
    if (pdfMsgObj.filterDisplay.toggle > 0) {
      startX = 100;
    } else {
      startX = 350;
    }
    doc.text(label, startX,
      doc.y, { continued: true })
    .text(min, { continued: true } );
    if (max) {
      doc.text(" - ", { continued: true } )
      .text(max, { continued: true } );
    }
    doc.text(suffix);
    if (pdfMsgObj.filterDisplay.toggle > 0) {
      doc.moveDown(-1);
    }
    pdfMsgObj.filterDisplay.toggle = pdfMsgObj.filterDisplay.toggle * -1;
  }
}

function formatDollars(string) {
  return Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD', maximumFractionDigits: 0, minimumFractionDigits: 0 }).format(string);
}

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}


function formatFilter(filterObj) {
  let filterText = '';
  filterText += addFilterText(filterObj.totalRevenue, 'both', 'total revenue', 'USD');
  filterText += addFilterText(filterObj.averageRevenue, 'both', 'average revenue', 'USD');
  filterText += addFilterText(filterObj.totalEmployees, 'both', ' total employees', 'number');
  filterText += addFilterText(filterObj.revenuePerCapita, 'both', 'revenue per capita', 'USD');
  filterText += addFilterText(filterObj.medianAge, 'both', 'median age');
  if (filterObj.highSchoolOrHigher > 0) { 
     filterText += 'High School graduate rate of ' + filterObj.highSchoolOrHigher.min + '% or greater' + ', ';
  }
  if (filterObj.bachelorsDegreeOrHigher > 0) { 
     filterText += 'bachelors degree rate of ' + filterObj.highSchoolOrHigher.min + '% or greater, ';
  }
  if (filterObj.whiteCollarWorkers.min > 0) { 
     filterText += 'white collar worker rate of ' + filterObj.whiteCollarWorkers.min + '% or greater, ';
  }
  return filterText;
}

function addFilterText(paramObj, range, msg, format='') {
  let moreText = '';
  if (paramObj.min > 0 || paramObj.max != null) { 
    moreText += msg + ' ';
  }
  if (paramObj.min > 0) {
    moreText += 'greater than ';
    if (format === 'USD') {
      moreText += formatDollars(paramObj.min);
    } else if (format === 'number') {
      moreText += numberWithCommas(paramObj.min);
    }
    else {
      moreText += paramObj.min;
    }
  }
  if (paramObj.max != 'null') {
      moreText +=  ' and less than ';
    if (format === 'USD') {
      moreText += formatDollars(paramObj.max);
    } else if (format === 'number') {
      moreText += numberWithCommas(paramObj.max);
    } else {
      moreText += paramObj.max;
    }
    moreText + ', ';
  }
  return moreText;
}

function miniPin(x, y, color, doc) {
  doc.moveTo(x, y)   
   .lineWidth(1)
   .lineTo(x-2, y-3) 
   .lineTo(x+2, y-3)
   .circle(x, y-4, 2)
   .fillAndStroke(color, color);
}
  
/****
 * this function is building the pdf using the info from the pdfMsgObj and using 
 * pdfkit module
 */

async function buildPdf(pdfMsgObj, pdfColors) {
  
  // Create a document
  let doc = new PDFDocument;
  doc.pipe(pdfMsgObj.stream);
  
  let theme = { text: { color: pdfColors[2] } };
  
  // Draw a rectangle for the header 
  doc.save()
    .moveTo(25, 30)
    .lineTo(588, 30)
    .lineTo(588, 90)
    .lineTo(25, 90)
    .fill(pdfColors[2]);
  
  // start writing text

  // header text
  pdfMsgObj.customerGraphics.writeHeader(doc, theme);

  doc.fontSize(15);
  doc.moveDown(2);
  doc.fillColor(pdfColors[4])
  doc.text(pdfMsgObj.title, 25, doc.y);
  doc.fillColor(pdfColors[5]);
  doc.fontSize(10);
  doc.text("This is a list of postal codes with the highest combined business revenue in the ", 35, doc.y + 10, { continued: true } )
    .fillColor(pdfColors[3])
    .text(pdfMsgObj.displayIndustry, { continued: true } )
    .fillColor(pdfColors[5])
    .text("industry.  You should consider using this list if you are selling to businesses or consumers and want to", { continued: true } )
    .text("know where the most money is being made in your industry. ", { continued: true } )
    .text("The analysis is based on locations ", { continued: true } )
   .fillColor(pdfColors[3])
    .text(pdfMsgObj.distance, { continued: true } )
    .fillColor(pdfColors[5])
    .text(" miles from the centroid of ", { continued: true } )
   .fillColor(pdfColors[3])
    .text(pdfMsgObj.displayLocation, { continued: true } )
    .fillColor(pdfColors[5])
    .text(". The list has been filtered to include only areas that have ", { continued: true } );
   doc.fillColor(pdfColors[3]);
  doc.text(formatFilter(pdfMsgObj.filter));
  // get filters here in natural language
    doc.text("")
    .moveDown(1);
  /*
  doc.text("Best places to advertise in the ", 25, doc.y, { continued: true } )
   .fillColor(pdfColors[3])
    .text(pdfMsgObj.displayIndustry, { continued: true } )
    .fillColor(pdfColors[4])
    .text(" industry near ", { continued: true } )
      .fillColor(pdfColors[3]) 
    .text(pdfMsgObj.displayLocation, { continued: true } )
    .fillColor(pdfColors[4])
    .text(" based on ", { continued: true } )
    .fillColor(pdfColors[3])
    .text(pdfMsgObj.displayAttribute);
    */
    
  doc.fontSize(10);   
  doc.fillColor(pdfColors[4]);
  /*
  doc.text("    Filtered for Zip Codes at most ", { continued: true } )
    .fillColor('black')
    .text(pdfMsgObj.distance, { continued: true } )
    .fillColor(pdfColors[4])
      .text(" miles from the current city");
      */
  doc.image(pdfMsgObj.mapImgFile, 25, 246, { width: 562 } );
  // delete the image file
  fs.unlink(pdfMsgObj.mapImgFile, (err) => {
    if (err) {
      console.log("error deleting ", pdfMsgObj.mapImgFile, " :", err);
    }
  });
  
  // trying to get the image stream  - key should probably be put in environment variable
  let markerStr = '';
  for (let i=0; i<pdfMsgObj.centroidLat.length; i++) {
    markerStr += "markers=color:" + pdfColors[i].replace("#", "0x") + "%7C" + "label:" + (i+1) + "%7C" + pdfMsgObj.centroidLat[i] + ',' + pdfMsgObj.centroidLng[i] + '&';
  }
  let optionsObj = {
	  url: 'https://maps.googleapis.com/maps/api/staticmap',
	  size: '600x300',
	  maptype: 'roadmap',
	  markerStr: markerStr,
	  key: process.env.GOOGLEMAP_KEY, 
  }
//  let googleMap = await staticMap.getStaticMap(optionsObj);
//  doc.image(googleMap, 25, doc.y, { width: 562 } );
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
  for (let k=0; k<pdfMsgObj.bands; k++) {
    n = k % 3;  // n (remainder of k/3) is the column in the display of bands
    m = Math.floor(k/3);  // each row will have 3 bands listed
    startArr.push([40 + n*180, 532 + m*10]);
  }

  // then render the bands
  doc.fillColor(pdfColors[4]);
  i = 0;
  pdfMsgObj.bandArr.forEach(function(element) {
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
  
  let thisSortAttributeMinArr = [];
  let thisSortAttributeMaxArr = [];
  let sortText;
  if (pdfMsgObj.sortAttribute === 'underservedMarkets') {
    thisSortAttributeMinArr = pdfMsgObj.revenuePerCapitaMin;
    thisSortAttributeMaxArr = pdfMsgObj.revenuePerCapitaMax;
    sortText = "Revenue Per Capita";
  } else {
    thisSortAttributeMinArr = pdfMsgObj[pdfMsgObj.sortAttribute + 'Min'];
    thisSortAttributeMaxArr = pdfMsgObj[pdfMsgObj.sortAttribute + 'Max'];
    sortText = formatCamelToDisplay(pdfMsgObj.sortAttribute);
  }
  doc.fillColor(pdfColors[3]);
  doc.fontSize(6);
  doc.text(sortText, 515 - doc.widthOfString(sortText), doc.y);
  // tiny asc or desc triangle
  if (pdfMsgObj.sort === 'desc') {
    doc.polygon( [519, doc.y - 2], [523, doc.y - 2], [521, doc.y - 6]);
  } else {
    doc.polygon( [519, doc.y - 6], [523, doc.y - 6], [521, doc.y - 2]);
  }
  doc.fillAndStroke(pdfColors[3]) 
  for (let i=0; i<pdfMsgObj.zip.length; i++) {
    if (doc.y > 700) {
      doc.text(' ');
      doc.text(' ');
    }
    pdfMsgObj.filterDisplay.toggle = 1;  // this controls the margins of the fields
                      // if it's not reset to 1 between areas
                      // shown, the margins don't work right
                      // see showFilter function
    doc.fillColor(pdfColors[getBand(i, pdfMsgObj)])
    .moveDown(1)
    .fontSize(10)
    .circle(75, doc.y + 5, 7);
    doc.fill()
    .fillColor('#ffffff')
    .text(String.fromCharCode(65 + i), 72, doc.y + 1, { continued: true } )
    .fillColor(pdfColors[getBand(i, pdfMsgObj)])
    .fontSize(15)
    .text("  ", { continued: true } )
    .text(pdfMsgObj.zip[i]) // , { continued: true })
    .fillColor('black')
    .fontSize(13);
	  doc.moveDown(-1);
    xpos = 535 - (doc.widthOfString(thisSortAttributeMinArr[i].toString()) + doc.widthOfString(" - ") + doc.widthOfString(thisSortAttributeMaxArr[i].toString()));
    doc.text(thisSortAttributeMinArr[i], xpos, doc.y, { continued: true } )
    .text(" - ", { continued: true } )
    .text(thisSortAttributeMaxArr[i])
    .fillColor(pdfColors[5])
    .fontSize(8) 
    showFilter(pdfMsgObj, "Total Population: ", 'population', pdfMsgObj.population[i].toLocaleString('en'), null, doc);
    showFilter(pdfMsgObj, "Average Annual Revenue: ", 'averageRevenue', pdfMsgObj.averageRevenueMin[i], pdfMsgObj.averageRevenueMax[i], doc);
    showFilter(pdfMsgObj, "Total Employees: ", 'totalEmployees', pdfMsgObj.totalEmployeesMin[i], pdfMsgObj.totalEmployeesMax[i], doc);
    showFilter(pdfMsgObj, "Revenue Per Capita: ", 'revenuePerCapita', pdfMsgObj.revenuePerCapitaMin[i], pdfMsgObj.revenuePerCapitaMax[i], doc);
    showFilter(pdfMsgObj, "Household Income: ", 'householdIncome', pdfMsgObj.householdIncome[i], null, doc);
    showFilter(pdfMsgObj, "Household Expenditures: ", 'householdExpenditures', pdfMsgObj.householdExpenditures[i], null, doc);
    showFilter(pdfMsgObj, "Median Age: ", 'medianAge', pdfMsgObj.medianAge[i], null, doc);
    showFilter(pdfMsgObj, "Bachelors Degree or Higher: ", 'bachelorsDegreeOrHigher', pdfMsgObj.bachelorsDegreeOrHigher[i], null, doc, '%');
    showFilter(pdfMsgObj, "High School Degree or Higher: ", 'highSchoolOrHigher', Math.round(pdfMsgObj.highSchoolOrHigher[i]), null, doc, '%');
    showFilter(pdfMsgObj, "White Collar Workers: ", 'whiteCollarWorkers', Math.round(pdfMsgObj.whiteCollarWorkers[i]), null, doc, '%');
	  if (pdfMsgObj.filterDisplay.toggle < 0 ) {
		  doc.moveDown(1);
	  }
  }

  // footer text
  pdfMsgObj.customerGraphics.writeFooter(doc, theme);


  // Finalize the pdf file
  doc.end();
//  return pdfMsgObj.stream;
  console.log("PDF output.pdf created");
}

module.exports = {
  generatePDF: generatePDF,
  // setSizeup: setSizeup
}
