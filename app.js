const fs = require('fs');
const PDFDocument = require('pdfkit');
const https = require('https');
require('dotenv').config();
const sizeup = require("sizeup-api")({ key:process.env.SIZEUP_KEY });
const request = require('request');

console.log('PDF get start');
	
/****
* the function is going to take this stuff as inputs, but I'm just setting it here
* for development
*/

const attribute = 'totalRevenue';
const averageRevenue = [750000, 0];
const bands = 5;
const distance = 6;
const geographicLocationId = 41284;
const industryId = 8524;
const itemCount = 3;
const order = 'highToLow';
const page = 1;
const sort = 'desc';
const sortAttribute = 'totalRevenue';
const totalEmployees = [10, null];
const totalRevenue = [200000, 10000000];
const highSchoolOrHigher = 5;  // a percent
const householdExpenditures = [20000, 100000];
const householdIncome = [20000, 200000];
const medianAge = [3, 90];
const revenuePerCapita = [2, 15000];
const whiteCollarWorkers = 3;

function formatCamelToDisplay(input) {
	input_arr = input.split('');
//	console.log("in the function");
	input_arr.forEach(function(element, index, input_arr) {
		if (element == element.toUpperCase()) {
			input_arr[index] = ' ' + element;
		}
	});
	input_arr[0] = input_arr[0].toUpperCase();
	return input_arr.join('');
}

/****
 * this function is building the pdf.  maybe it should be moved
 * the second parameter is for dynamically named properties of the pdfMsgObj
 * the function changes pdfMsgObj because objects are passed by reference
 * or something like that in javascript and that means I can use the parameters
 * kind of like global variables without using global variables.
 * this object will be used in the buildPdf function which creates the pdf
 */
let pdfMsgObj = {msg: 'Best places to advertise in the '};
pdfMsgObj.displayAttribute = formatCamelToDisplay(attribute);
pdfMsgObj.distance = distance;
pdfMsgObj.zip = [];
pdfMsgObj.totalRevenueMin = [];
pdfMsgObj.totalRevenueMax = [];
pdfMsgObj.population = [];
pdfMsgObj.avgRevenueMin = [];
pdfMsgObj.avgRevenueMax = [];
pdfMsgObj.totalEmployeesMin = [];
pdfMsgObj.totalEmployeesMax = [];
pdfMsgObj.revenuePerCapitaMax = [];
pdfMsgObj.householdIncome = [];
pdfMsgObj.medianAge = [];
pdfMsgObj.householdExpenditures = [];
pdfMsgObj.whiteCollarWorkers = [];
pdfMsgObj.bachelorsDegreeOrHigher = [];
pdfMsgObj.highSchoolOrHigher = [];
pdfMsgObj.averageRevenueMin = [];
pdfMsgObj.averageRevenueMax = [];
pdfMsgObj.revenuePerCapitaMin = [];
pdfMsgObj.revenuePerCapitaMax = [];
pdfMsgObj.centroidLat = [];
pdfMsgObj.centroidLng = [];

function buildPdfMsg(addedMsg, target='') {
//		console.log("building pdf...");
	if (target) {
		pdfMsgObj[target] = addedMsg;
	} else {
		pdfMsgObj.msg = pdfMsgObj.msg.concat(addedMsg + '\n');
	}
}
// describe the query, place, industry, KPI, search distance, filters

Promise.all([
	sizeup.data.getPlace({ id: geographicLocationId }),
	sizeup.data.getIndustry( { id: industryId }),
	sizeup.data.getBestPlacesToAdvertise( { totalEmployees: totalEmployees, highSchoolOrHigher: highSchoolOrHigher, householdExpenditures: householdExpenditures, householdIncome: householdIncome, medianAge: medianAge, revenuePerCapita: revenuePerCapita, whiteCollarWorkers: whiteCollarWorkers, totalRevenue: totalRevenue, bands: bands, industryId: industryId, order: order, page: page, sort: sort, sortAttribute: sortAttribute, geographicLocationId: geographicLocationId, distance: distance, attribute: attribute } ),
]).then(([place, industry, bestPlaces]) => {
		successCallback(place[0].City.LongName, "display location name"); // just for debug
		successCallback(industry[0].Name, "display industry name"); // just for debug
		bestCallback(bestPlaces.Items, "Best Places to Advertise"); // note: would you do this instead of putting the forEach loop right here?
		pdfMsgObj['displayLocation'] = place[0].City.LongName;
		pdfMsgObj['displayIndustry'] = industry[0].Name;
	}).then(startPdf).catch(console.error)

function successCallback(result, msg="success") {
//	console.log(msg + ": ");
//		console.log(typeof result);
//		console.log(result);
//		console.log('ending here');
//	buildPdfMsg(msg + ': ' + result);
}

/**
 * note: I had passed things like element.TotalRevenue.Min to a function
 * which then built up a pdf string or object depending on parameters
 * and T suggested generalizing the function, but when doing that it 
 * seemed like a function was unnecessary and I could just add the 
 * parameter/value to the pdfMsgObj in here
 */

function bestCallback(result, msg="success") {
	result.forEach(function(element) {
//		console.log(element.Centroid);
		pdfMsgObj['zip'].push(element.ZipCode.Name);
//		console.log(element.ZipCode.Zip);
//		console.log(element.TotalRevenue.Max);
//		console.log(element);
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
	});
	// ok this zip, total rev min and max need to be an array
}

function failureCallback(error) {
	console.log("failure: " + error);
}
	/*  the pic needs to be downloaded first and the pdf
	 *  built in a callback
	 */
/*
	*/

/****
 * note: If more than one pdf style is necessary it would not be hard
 * to make a template and then some markup for inserting values of the
 * pdfMsgObj.  If there is generally some problem with the html2pdf
 * scripts it might not be that hard to develop a markup for basic
 * html, but adding specific things for something like pdfkit where
 * you can specify position - draw lines etc with its functions
 */

function startPdf() {
	// Need to make the image first so it is ready for the PDF
	// I tried putting it into the PDF straight from buffer, but that
	// was taking too long, so I will save it to a file with a unique name
//	const url = 'https://maps.googleapis.com/maps/api/staticmap?center=Brooklyn+Bridge,New+York,NY&zoom=13&size=600x300&maptype=roadmap&markers=color:green%7Clabel:S%7C40.702147,-74.015794&markers=color:orange%7Clabel:G%7C40.711614,-74.012318&markers=color:red%7Clabel:C%7C40.718217,-73.998284&key=AIzaSyBYmAqm62QJXA2XRi1KkKVtWa6-BVTZ7WE';
// https://maps.googleapis.com/maps/api/js/ViewportInfoService.GetViewportInfo?1m6&1m2&1d28.541763002486213&2d-100.75242339877633&2m2&1d31.49107851274312&2d-95.13921000828736&2u9&4sen-US&5e0&6sm@442000000&7b0&8e0&callback=_xdc_._f0kl74&key=AIzaSyBYmAqm62QJXA2XRi1KkKVtWa6-BVTZ7WE&token=19066
console.log("lat = ", pdfMsgObj.centroidLat);
console.log("lng = ", pdfMsgObj.centroidLng);
//construct the string for markers
let markerStr = '';
for (let i=0; i<pdfMsgObj.centroidLat.length; i++) {
		markerStr += "markers=color:blue%7C" + pdfMsgObj.centroidLat[i] + ',' + pdfMsgObj.centroidLng[i] + '&';
}
	console.log(markerStr);
let center = pdfMsgObj.centroidLat[0] + ',' + pdfMsgObj.centroidLng[0];  // not really the center
		const url = 'https://maps.googleapis.com/maps/api/staticmap?center=' + center + '&zoom=11&size=600x300&maptype=roadmap&' + markerStr + 'key=AIzaSyBYmAqm62QJXA2XRi1KkKVtWa6-BVTZ7WE';
	console.log("url = ", url);
	var download = function(uri, filename, callback){
		request.head(uri, function(err, res, body){
			console.log('content-type:', res.headers['content-type']);
			console.log('content-length:', res.headers['content-length']);
			request(uri).pipe(fs.createWriteStream(filename)).on('close', callback);
		});
	};

	download(url, 'google.png', function(){
  		console.log('done');
		buildPdf();
	});
}

function buildPdf() {
	
	colors = {
			orange: '#fd7e14',
			graydark: '#343a40',
			gray: '#6c757d',
	}
	
	/***
	reference - colors
	--blue:#007bff;		--indigo:#6610f2;		--purple:#6f42c1;		--pink:#e83e8c;
	--red:#dc3545;		--orange:#fd7e14;		--yellow:#ffc107;		--green:#28a745;
	--teal:#20c997;		--cyan:#17a2b8;			--white:#fff;			--gray:#6c757d;
	--gray-dark:#343a40; --primary:#007bff;		--secondary:#6c757d;	--success:#28a745;
	--info:#17a2b8;		--warning:#ffc107;		--danger:#dc3545;		--light:#f8f9fa;
	--dark:#343a40;
	*/
	
	// Create a document
	let doc = new PDFDocument;
	
	// pipe its output to a file
	let writeStream = fs.createWriteStream('output.pdf');
	doc.pipe(writeStream);
	
	// Draw a rectangle - this will be szu-industry-and-locationXsSm-container
	doc.save()
		.moveTo(30, 30)
		.lineTo(600, 30)
		.lineTo(600, 90)
		.lineTo(30, 90)
		.fill('#0ea1ff');
	
	// Embed a font, set the font size, and render some text
	doc.fontSize(15);
	doc.moveDown(2);
	doc.fillColor(colors.graydark)
	doc.text("Best places to advertise in the ", { continued: true } )
		.fillColor(colors.orange)
		.text(pdfMsgObj.displayIndustry, { continued: true } )
		.fillColor(colors.graydark)
		.text(" industry near ", { continued: true } )
	    .fillColor(colors.orange) 
		.text(pdfMsgObj.displayLocation, { continued: true } )
		.fillColor(colors.graydark)
		.text(" based on ", { continued: true } )
		.fillColor(colors.orange)
		.text(pdfMsgObj.displayAttribute);
		
	doc.fontSize(10);		
	doc.fillColor(colors.graydark);
	doc.text("    Filtered for Zip Codes at most ", { continued: true } )
		.fillColor('black')
		.text(pdfMsgObj.distance, { continued: true } )
		.fillColor(colors.greydark)
			.text(" miles from the current city");
	doc.moveDown();
	doc.image('google.png', 25, doc.y, { width: 562 } );
	
// I'm going to have to pipe this image in, which probably means putting this whole 
	// function in a promise.all
	// doc.image('https://maps.googleapis.com/maps/api/staticmap?center=Brooklyn+Bridge,New+York,NY&zoom=13&size=600x300&maptype=roadmap&markers=color:blue%7Clabel:S%7C40.702147,-74.015794&markers=color:green%7Clabel:G%7C40.711614,-74.012318&markers=color:red%7Clabel:C%7C40.718217,-73.998284&key=AIzaSyBYmAqm62QJXA2XRi1KkKVtWa6-BVTZ7WE');
	doc.moveDown();
	doc.fillColor(colors.gray);
	doc.fontSize(10);
	doc.text("This is a list of Zip Codes with the highest combined business revenue in the ", 75, doc.y, { continued: true } )
		.text(pdfMsgObj.displayIndustry)
		.text("industry. You should consider using this list if you are selling to businesses or consumers and want to")
		.text("know where the most money is being made in your industry.")
		.moveDown(2);
	let xpos = 250;
	let yStart = doc.y;
	// I'm going to have to get at the length to make sure the page
	// doesn't break
	for (let i=0; i<pdfMsgObj.zip.length; i++) {
		doc.fillColor(colors.orange)
		.fontSize(15)
		.text(pdfMsgObj.zip[i], 75, doc.y, { continued: true })
		.fillColor('black');
		xpos = 400 - (doc.widthOfString(pdfMsgObj.totalRevenueMin[i]) + doc.widthOfString(" - ") + doc.widthOfString(pdfMsgObj.totalRevenueMax[i]));
		console.log("the y position two is: ");
	console.log(doc.y);
		doc.text(pdfMsgObj.totalRevenueMin[i], xpos, doc.y, { continued: true } )
		.text(" - ", { continued: true } )
		.text(pdfMsgObj.totalRevenueMax[i])
		.fillColor('gray')
		.fontSize(8)
		.text("Total Population: ", 100, doc.y, { continued: true } )
		.text((pdfMsgObj.population[i]).toLocaleString('en'))
		.text("Average Annual Revenue: ", 100, doc.y, { continued: true })
		.text(pdfMsgObj.averageRevenueMin[i], { continued: true } )
		.text(" - ", { continued: true } )
		.text(pdfMsgObj.averageRevenueMax[i])
		.text("Total Employees: ", 100, doc.y, { continued: true } )
		.text(pdfMsgObj.totalEmployeesMin[i], { continued: true } )
		.text(" - ", { continued: true } )
		.text(pdfMsgObj.totalEmployeesMax[i])
		.text("Revenue Per Capita: ", 100, doc.y, { continued: true } )
		.text(pdfMsgObj.revenuePerCapitaMin[i], { continued: true } )
		.text(" - ", { continued: true } )
		.text(pdfMsgObj.revenuePerCapitaMax[i])
		.text("Household Income: ", 100, doc.y, { continued: true } )
		.text(pdfMsgObj.householdIncome[i])
		.text("Household Expenditures (Average): ", 100, doc.y, { continued: true } )
		.text(pdfMsgObj.householdExpenditures[i])
		.text("Median Age: ", 100, doc.y, { continued: true } )
		.text(pdfMsgObj.medianAge[i])
		.text("Bachelors Degree or Higher: ", 100, doc.y, { continued: true } )
		.text((pdfMsgObj.bachelorsDegreeOrHigher[i] * 100).toFixed(1), { continued: true } )
		.text("%")
		.text("High School Degree or Higher: ", 100, doc.y, { continued: true } )
		.text((pdfMsgObj.highSchoolOrHigher[i] * 100).toFixed(1), { continued: true } )
		.text("%")
		.text("White Collar Workers: ", 100, doc.y, { continued: true } )
		.text((pdfMsgObj.whiteCollarWorkers[i] * 100).toFixed(1), { continued: true } )
		.text("%");
		console.log("the y position three is: ");
	}
	// I want to test if it's breaking the page
	let blockLen = doc.y - yStart;
	console.log("yEnd: ", doc.y, ", yStart: ", yStart, ", blockLen: ", blockLen);

	// Finalize the pdf file
	doc.end();
//	let url = "https://maps.googleapis.com/maps/api/staticmap?center=Brooklyn+Bridge,New+York,NY&zoom=13&size=600x300&maptype=roadmap&markers=color:blue%7Clabel:S%7C40.702147,-74.015794&markers=color:green%7Clabel:G%7C40.711614,-74.012318&markers=color:red%7Clabel:C%7C40.718217,-73.998284&key=AIzaSyBYmAqm62QJXA2XRi1KkKVtWa6-BVTZ7WE";
}
