const fs = require('fs');
const PDFDocument = require('pdfkit');
const https = require('https');
require('dotenv').config();
const request = require('request');
const staticMap = require('./mapGenerator.js');


// note - make labels below show letters, breaks at page break, maybe grey doesn't work
// checkout greyscale

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
        pdfMsgObj.filter = searchObj.filter;
        pdfMsgObj.title = searchObj.title;
	// bands wasn't part of the search obj, so I'm just setting it
	// to 5
	 pdfMsgObj.bands = 5;
	 let bands = pdfMsgObj.bands;
	 pdfMsgObj.sortAttribute = searchObj.ranking_metric.kpi;  // I think
	 let sortAttribute = pdfMsgObj.sortAttribute;
    pdfMsgObj.distance = searchObj.area.distance;
	 let distance = pdfMsgObj.distance;
	 pdfMsgObj.attribute = searchObj.ranking_metric.kpi;  // not sure 
	 let attribute = pdfMsgObj.attribute;
    pdfMsgObj.displayAttribute = formatCamelToDisplay(attribute);
    pdfMsgObj.customerGraphics = customerObj.getReportGraphics(customerKey);
    pdfMsgObj.stream = stream;
    pdfMsgObj.filterDisplay = { toggle: 1 };
	 let placeCompoundKey = searchObj.area.place;
	 let industryKey = searchObj.ranking_metric.industry;
	 let itemCount = 26;  // This isnpt in search obj, so just setting it
	 let order = 'highToLow'; // don't see this is search obj
	 let page = 1;  // not sure what page is
	 let sort = searchObj.ranking_metric.order; // doesn't seem right, but maybe is
	 let totalEmployees = [searchObj.filter.totalEmployees.min, searchObj.filter.totalEmployees.max];
	 let totalRevenue = [searchObj.filter.totalRevenue.min, searchObj.filter.totalRevenue.max];
	 let averageRevenue = searchObj.filter.averageRevenue;
	 let highSchoolOrHigher = searchObj.filter.highSchoolOrHigher.min;
	 let householdExpenditures = [searchObj.filter.householdExpenditures.min, searchObj.filter.householdExpenditures.max];
	 let householdIncome = [searchObj.filter.householdIncome.min, searchObj.filter.householdIncome.max]; 
	 let medianAge = [searchObj.filter.medianAge.min, searchObj.filter.medianAge.max];
	 let revenuePerCapita = [searchObj.filter.revenuePerCapita.min, searchObj.filter.revenuePerCapita.max];
	 let whiteCollarWorkers = searchObj.filter.whiteCollarWorkers.min;
	// I think bachelorsdegree or higher is left out


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

    /**
    * Any given search can have many different filters, but having many filters
    * would make it difficult to have a nice presentation.  In the current state
    * it may cause there to be a second page.  
    */

    if (averageRevenue[0] === 0 && averageRevenue[1] === null) { pdfMsgObj.filterDisplay.averageRevenue = true; }
    if (totalEmployees[0] === 0 && totalEmployees[1] === null) { pdfMsgObj.filterDisplay.totalEmployees = true; }
    if (totalRevenue[0] === 0 && totalRevenue[1] === null) { pdfMsgObj.filterDisplay.totalRevenue = true; }
    if (householdIncome[0] === 0 && householdIncome[1] === null) { pdfMsgObj.filterDisplay.householdIncome = true; }
    if (revenuePerCapita[0] === 0 && revenuePerCapita === null) { pdfMsgObj.filterDisplay.revenuePerCapita = true; }
    if (highSchoolOrHigher != 0) { pdfMsgObj.filterDisplay.highSchoolOrHigher = true; }
    if (medianAge != 0) { pdfMsgObj.filterDisplay.medianAge = true; }
    if (whiteCollarWorkers != 0) { pdfMsgObj.filterDisplay.whiteCollarWorkers = true; }
    pdfMsgObj.filterDisplay.population = true;

    /***
    * Communication with sizeup api
    * Get the info from the sizeup api, then in successCallback put the return info into the pdfMsgObj
    * Then build the pdf with that info
    */

	let place = await sizeup.data.getPlaceBySeokey(
        `${placeCompoundKey.state}/${placeCompoundKey.county}/${placeCompoundKey.city}`);
	let industry = await sizeup.data.getIndustryBySeokey(industryKey);
   let bestPlaces = await sizeup.data.getBestPlacesToAdvertise( { totalEmployees: totalEmployees, highSchoolOrHigher: highSchoolOrHigher, householdExpenditures: householdExpenditures, householdIncome: householdIncome, medianAge: medianAge, revenuePerCapita: revenuePerCapita, whiteCollarWorkers: whiteCollarWorkers, totalRevenue: totalRevenue, bands: bands, industryId: industry[0].Id, order: order, page: page, sort: sort, sortAttribute: sortAttribute, geographicLocationId: place[0].Id, distance: distance, attribute: attribute } );
	let bestPlacesBands = await sizeup.data.getBestPlacesToAdvertiseBands( { totalEmployees: totalEmployees, highSchoolOrHigher: highSchoolOrHigher, householdExpenditures: householdExpenditures, householdIncome: householdIncome, medianAge: medianAge, revenuePerCapita: revenuePerCapita, whiteCollarWorkers: whiteCollarWorkers, totalRevenue: totalRevenue, bands: bands, industryId: industry[0].Id, order: order, page: page, sort: sort, sortAttribute: sortAttribute, geographicLocationId: place[0].Id, distance: distance, attribute: attribute } );
   pdfMsgObj['displayLocation'] = place[0].City.LongName;
   pdfMsgObj['displayIndustry'] = industry[0].Name;
   pdfMsgObj['bandArr'] = bestPlacesBands;
   successCallback(pdfMsgObj, pdfColors, bestPlaces.Items, "Best Places to Advertise"); 
	

	/*
    Promise.all([
      sizeup.data.getPlaceBySeokey(
        `${placeCompoundKey.state}/${placeCompoundKey.county}/${placeCompoundKey.city}`),
      sizeup.data.getIndustryBySeokey(industryKey)
    ]).then(([place, industry]) => {
        sizeup.data.getBestPlacesToAdvertiseBands( { totalEmployees: totalEmployees, highSchoolOrHigher: highSchoolOrHigher, householdExpenditures: householdExpenditures, householdIncome: householdIncome, medianAge: medianAge, revenuePerCapita: revenuePerCapita, whiteCollarWorkers: whiteCollarWorkers, totalRevenue: totalRevenue, bands: bands, industryId: industry[0].Id, order: order, page: page, sort: sort, sortAttribute: sortAttribute, geographicLocationId: place[0].Id, distance: distance, attribute: attribute } )
      Promise.all([
        Promise.resolve(place),
        Promise.resolve(industry),
        sizeup.data.getBestPlacesToAdvertise( { totalEmployees: totalEmployees, highSchoolOrHigher: highSchoolOrHigher, householdExpenditures: householdExpenditures, householdIncome: householdIncome, medianAge: medianAge, revenuePerCapita: revenuePerCapita, whiteCollarWorkers: whiteCollarWorkers, totalRevenue: totalRevenue, bands: bands, industryId: industry[0].Id, order: order, page: page, sort: sort, sortAttribute: sortAttribute, geographicLocationId: place[0].Id, distance: distance, attribute: attribute } ),
        sizeup.data.getBestPlacesToAdvertiseBands( { totalEmployees: totalEmployees, highSchoolOrHigher: highSchoolOrHigher, householdExpenditures: householdExpenditures, householdIncome: householdIncome, medianAge: medianAge, revenuePerCapita: revenuePerCapita, whiteCollarWorkers: whiteCollarWorkers, totalRevenue: totalRevenue, bands: bands, industryId: industry[0].Id, order: order, page: page, sort: sort, sortAttribute: sortAttribute, geographicLocationId: place[0].Id, distance: distance, attribute: attribute } )
      ]).then(([place, industry, bestPlaces, bestPlacesBands]) => {
        pdfMsgObj['displayLocation'] = place[0].City.LongName;
        pdfMsgObj['displayIndustry'] = industry[0].Name;
        pdfMsgObj['bandArr'] = bestPlacesBands;
        successCallback(pdfMsgObj, pdfColors, bestPlaces.Items, "Best Places to Advertise"); 
      })
    }).catch(console.error);
	*/
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
  for (element of result) {
    i++;
    pdfMsgObj['zip'].push(element.ZipCode.Name);
//    console.log(element);   // good for debugging
    pdfMsgObj['centroidLng'].push(element.Centroid.Lng);
    pdfMsgObj['centroidLat'].push(element.Centroid.Lat);
    pdfMsgObj['totalRevenueMin'].push(Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD', maximumFractionDigits: 0, minimumFractionDigits: 0 }).format(element.TotalRevenue.Min));  // throws an error if maxFDig is set to 0, but not minFDig 
    pdfMsgObj['totalRevenueMax'].push(Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD', maximumFractionDigits: 0, minimumFractionDigits: 0 }).format(element.TotalRevenue.Max));
    pdfMsgObj['population'].push(element.Population);
    pdfMsgObj['averageRevenueMin'].push(Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD', maximumFractionDigits: 0, minimumFractionDigits: 0 }).format(element.AverageRevenue.Min));
    pdfMsgObj['averageRevenueMax'].push(Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD', maximumFractionDigits: 0, minimumFractionDigits: 0 }).format(element.AverageRevenue.Max));
    pdfMsgObj['totalEmployeesMin'].push(element.TotalEmployees.Min);
    pdfMsgObj['totalEmployeesMax'].push(element.TotalEmployees.Max);
    pdfMsgObj['revenuePerCapitaMin'].push(Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD', maximumFractionDigits: 0, minimumFractionDigits: 0 }).format(element.RevenuePerCapita.Min));
    pdfMsgObj['revenuePerCapitaMax'].push(Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD', maximumFractionDigits: 0, minimumFractionDigits: 0 }).format(element.RevenuePerCapita.Max));
    pdfMsgObj['householdIncome'].push(Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD', maximumFractionDigits: 0, minimumFractionDigits: 0 }).format(element.HouseholdIncome));
    pdfMsgObj['medianAge'].push(element.MedianAge);
    pdfMsgObj['householdExpenditures'].push(Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD', maximumFractionDigits: 0, minimumFractionDigits: 0 }).format(element.HouseholdExpenditures));
    pdfMsgObj['whiteCollarWorkers'].push(element.WhiteCollarWorkers);
    pdfMsgObj['bachelorsDegreeOrHigher'].push(element.BachelorsDegreeOrHigher);
    pdfMsgObj['highSchoolOrHigher'].push(element.HighSchoolOrHigher);
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
  if (filterObj.totalRevenue) { 
     filterText += 'total revenue between ' + formatDollars(filterObj.totalRevenue.min) + ' and ' + formatDollars(filterObj.totalRevenue.max) + ', ';
  }
  if (filterObj.averageRevenue) { 
     filterText += 'average revenue between ' + formatDollars(filterObj.averageRevenue.min) + ' and ' + formatDollars(filterObj.averageRevenue.max) + ', ';
  }
  if (filterObj.totalEmployees) { 
     filterText += 'between ' + numberWithCommas(filterObj.totalEmployees.min) + ' and ' + numberWithCommas(filterObj.totalEmployees.max) + 'employees, ';
  }
  if (filterObj.revenuePerCapita) { 
     filterText += 'average revenue per capita between ' + formatDollars(filterObj.revenuePerCapita.min) + ' and ' + formatDollars(filterObj.revenuePerCapita.max) + ', ';
  }
  if (filterObj.revenuePerCapita) { 
     filterText += 'household income between ' + formatDollars(filterObj.householdIncome.min) + ' and ' + formatDollars(filterObj.householdIncome.max) + ', ';
  }
  if (filterObj.medianAge) { 
    if (filterObj.medianAge.max) {
      filterText += 'median age between ' + filterObj.medianAge.min + ' and ' + filterObj.medianAge.max;
    }
    else {
      filterText += 'median age greater than ' + filterObj.medianAge.min + ', ';
    }
  }
  if (filterObj.highSchoolOrHigher) { 
     filterText += 'High School graduate rate of ' + filterObj.highSchoolOrHigher.min + '% or greater' + ', ';
  }
  if (filterObj.bachelorsDegreeOrHigher) { 
     filterText += 'bachelors degree rate of ' + filterObj.highSchoolOrHigher.min + '% or greater, ';
  }
  if (filterObj.whiteCollarWorkers) { 
     filterText += 'white collar worker rate of ' + filterObj.whiteCollarWorkers.min + '% or greater, ';
  }
  return filterText;
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
  doc.image(pdfMsgObj.mapImgFile, 25, doc.y, { width: 562 } );
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
    .fillColor(pdfColors[i])
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

//  console.log("pdfmsgob = ", pdfMsgObj);

  // Finalize the pdf file
  doc.end();
//  return pdfMsgObj.stream;
  console.log("PDF output.pdf created");
}

module.exports = {
  generatePDF: generatePDF,
  // setSizeup: setSizeup
}
