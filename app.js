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

// I should build testing into this by making these choices random

function getRandElement(inputArray) {
	return inputArray[Math.floor(Math.random() * inputArray.length)]
}

function getRandRange(min, max, maxmax="unlimited") {
	let randMin = Math.floor((Math.random() * (max - min)) + min);
	let randMax = Math.floor((Math.random() * (max - randMin)) + randMin);
	if (maxmax === "unlimited") {
		if (Math.random() > 0.5) { maxmax = null; }
	}
	return [randMin, randMax]
}

const attribute = getRandElement(['revenuePerCapita', 'totalRevenue', 'averageRevenue', 'underservedMarkets', 'totalEmployees', 'householdIncome']);
const averageRevenue = getRandRange(0, 50000000);
//const attribute = 'underservedMarkets'
//const averageRevenue = [2500000, 50000000];
const bands = 5;
const distance = 16;
const geographicLocationId = 41284;
const industryId = 8524;
const itemCount = 3;
const order = 'lotToHigh';
const page = 1;
const sort = 'asc';
const sortAttribute = 'underservedMarkets';
const totalEmployees = [0, null];
const totalRevenue = [0, null];
const highSchoolOrHigher = 0;  // a percent
const householdExpenditures = [0, null];
const householdIncome = [0, null];
const medianAge = [0, null];
const revenuePerCapita = [0, null];
const whiteCollarWorkers = 0;

console.log("search criteria: ");
console.log("attribute: ", attribute);
console.log("averageRevenue: ", averageRevenue);

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

/*
* this is used for the temporary image file name.  I think there is a better
* way to do this with the temp directory, but I don't see it immediately and
* I'm afraid that might cause a bug when moving to a production env I don't 
* know at this point - also basically copypasta from Gabriele Romanato with
* minor changes.
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
pdfMsgObj.imgFile = '';
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

let pdfColors = [   // was more elegant as object, but I iterate over this in loops later
			'#dc3545', // red
			'#28a745', // green
			'#007bff', // blue
			'#fd7e14', // orange
			'#343a40', // dark grey
			'#6c757d', // gray
	]
	
	/***
	reference - colors
	--blue:#007bff;		--indigo:#6610f2;		--purple:#6f42c1;		--pink:#e83e8c;
	--red:#dc3545;		--orange:#fd7e14;		--yellow:#ffc107;		--green:#28a745;
	--teal:#20c997;		--cyan:#17a2b8;			--white:#fff;			--gray:#6c757d;
	--gray-dark:#343a40; --primary:#007bff;		--secondary:#6c757d;	--success:#28a745;
	--info:#17a2b8;		--warning:#ffc107;		--danger:#dc3545;		--light:#f8f9fa;
	--dark:#343a40;
	*/


function buildPdfMsg(addedMsg, target='') {
//		console.log("building pdf...");
	if (target) {
		pdfMsgObj[target] = addedMsg;
	} else {
		pdfMsgObj.msg = pdfMsgObj.msg.concat(addedMsg + '\n');
	}
}
// describe the query, place, industry, KPI, search distance, filters

// console lot for testing/dev
console.log("totalEmployees: ", totalEmployees);
console.log("highSchoolOrHigher: ",  highSchoolOrHigher);
console.log("householdExpenditures: ", householdExpenditures);
console.log("householdIncome: ", householdIncome);
console.log("medianAge: ", medianAge);
console.log("revenuePerCapita: ", revenuePerCapita);
console.log("whiteCollarWorkers: ", whiteCollarWorkers);
console.log("totalRevenue: ", totalRevenue);
console.log("bands: ", bands);
console.log("industryId: ", industryId);
console.log("order: ", order);
console.log("page: ", page);
console.log("sort: ", sort);
console.log("sortAttribute: ", sortAttribute);
console.log("geographicLocationId: ", geographicLocationId);
console.log("distance: ", distance);
console.log("attribute: ", attribute); 

Promise.all([
	sizeup.data.getPlace({ id: geographicLocationId }),
	sizeup.data.getIndustry( { id: industryId }),
	sizeup.data.getBestPlacesToAdvertise( { totalEmployees: totalEmployees, highSchoolOrHigher: highSchoolOrHigher, householdExpenditures: householdExpenditures, householdIncome: householdIncome, medianAge: medianAge, revenuePerCapita: revenuePerCapita, whiteCollarWorkers: whiteCollarWorkers, totalRevenue: totalRevenue, bands: bands, industryId: industryId, order: order, page: page, sort: sort, sortAttribute: sortAttribute, geographicLocationId: geographicLocationId, distance: distance, attribute: attribute } ),
]).then(([place, industry, bestPlaces]) => {
		console.log(bestPlaces);
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
 *	 parameter/value to the pdfMsgObj in here
 */

function bestCallback(result, msg="success") {
//	result.forEach(function(element) {
	var i=0;
	for (element of result) {
		i++;
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
		if (i >= 3) { break; }
//	});
	}
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
// console.log("lat = ", pdfMsgObj.centroidLat);
// console.log("lng = ", pdfMsgObj.centroidLng);
//construct the string for markers
let markerStr = '';
for (let i=0; i<pdfMsgObj.centroidLat.length; i++) {
	markerStr += "markers=color:" + pdfColors[i].replace("#", "0x") + "%7C" + "label:" + (i+1) + "%7C" + pdfMsgObj.centroidLat[i] + ',' + pdfMsgObj.centroidLng[i] + '&';
}
//	console.log(markerStr);
	// image scale needs to be based on the the search distance or the most distant pin, but that's harder
// let center = pdfMsgObj.centroidLat[0] + ',' + pdfMsgObj.centroidLng[0];  // not really the center
	//	const url = 'https://maps.googleapis.com/maps/api/staticmap?center=' + center + '&zoom=11&size=600x300&maptype=roadmap&' + markerStr + 'key=AIzaSyBYmAqm62QJXA2XRi1KkKVtWa6-BVTZ7WE';
	const url = 'https://maps.googleapis.com/maps/api/staticmap?size=600x300&maptype=roadmap&' + markerStr + 'key=AIzaSyBYmAqm62QJXA2XRi1KkKVtWa6-BVTZ7WE';
//	console.log("url = ", url);
	var download = function(uri, filename, callback){
		request.head(uri, function(err, res, body){
	//		console.log('content-type:', res.headers['content-type']);
	//		console.log('content-length:', res.headers['content-length']);
			request(uri).pipe(fs.createWriteStream(filename)).on('close', callback);
		});
	};
// name of this file needs to be at least close to unique and then deleted
let imgFile = IDGenerator();
imgFile = imgFile + '.png';
//console.log("start unique number ", imgFile, " end unique number");
	download(url,  imgFile, function(){
 // 		console.log('done');
		pdfMsgObj.imgFile = imgFile;
		buildPdf();
	});
}

function buildPdf() {
	
	// Create a document
	let doc = new PDFDocument;
	
	// pipe its output to a file
	let writeStream = fs.createWriteStream('output.pdf');  // this will output to stream or something
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
	doc.fillColor(pdfColors[4])
	doc.text("Best places to advertise in the ", { continued: true } )
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
		
	doc.fontSize(10);		
	doc.fillColor(pdfColors[4]);
	doc.text("    Filtered for Zip Codes at most ", { continued: true } )
		.fillColor('black')
		.text(pdfMsgObj.distance, { continued: true } )
		.fillColor(pdfColors[4])
			.text(" miles from the current city");
	doc.moveDown();
	doc.image(pdfMsgObj.imgFile, 25, doc.y, { width: 562 } );
	// delete the image file
	fs.unlink(pdfMsgObj.imgFile, (err) => {
		if (err) {
			console.log("error deleting ", pdfMsgObj.imgFile, " :", err);
		} else {
			console.log(pdfMsgObj.imgFile, " was deleted");
		}
	});
	 
	// doc.image('https://maps.googleapis.com/maps/api/staticmap?center=Brooklyn+Bridge,New+York,NY&zoom=13&size=600x300&maptype=roadmap&markers=color:blue%7Clabel:S%7C40.702147,-74.015794&markers=color:green%7Clabel:G%7C40.711614,-74.012318&markers=color:red%7Clabel:C%7C40.718217,-73.998284&key=AIzaSyBYmAqm62QJXA2XRi1KkKVtWa6-BVTZ7WE');
	doc.moveDown();
	doc.fillColor(pdfColors[5]);
	doc.fontSize(10);
	doc.text("This is a list of Zip Codes with the highest combined business revenue in the ", 75, doc.y, { continued: true } )
		.text(pdfMsgObj.displayIndustry)
		.text("industry. You should consider using this list if you are selling to businesses or consumers and want to")
		.text("know where the most money is being made in your industry.")
		.moveDown(2);
	let xpos = 250;
	const listHeight = 220;  // pixels given to the bottom section
	doc.y = 720 - listHeight;
	let secondMargin = 400;
	let skipX = 0;
//	numFields = pdfMsgObj.zip.length;
//	fieldHeight = listHeight/numFields;
//	fieldFontHeight = Math.round(fieldHeight/6);
//	subFieldFontHeight = Math.round(fieldFontHeight/2);
//I think I'm going to give up dynamic font sizing as it makes other things hard/impossible
	// I'm going to have to get at the length to make sure the page
	// doesn't break
	for (let i=0; i<pdfMsgObj.zip.length; i++) {
	//	doc.fillColor(colors.orange)
		doc.fillColor(pdfColors[i])
//		.fontSize(fieldFontHeight) //.fontSize(15)
		.fontSize(10)
		// gonna try to draw a little circle here
		.circle(75, doc.y + 5, 7)
		.fill()
		.fillColor('#ffffff')
	//	.rect(75, doc.y + 2, 1, fieldFontHeight/2)
//		.moveTo(75, doc.y + fieldFontHeight/8)
//		.lineTo(75, doc.y + fieldFontHeight/1.8)
	//	.fill() // no function, I left off here, stroke seems to only be black, fill doesn't get the linewidth
		.text(i + 1, 72, doc.y + 1, { continued: true } )
		.fillColor(pdfColors[i])
		.fontSize(15)
		.text("  ", { continued: true } )
		.text(pdfMsgObj.zip[i], { continued: true })
		.fillColor('black')
		.fontSize(13);
		xpos = 400 - (doc.widthOfString(pdfMsgObj.totalRevenueMin[i]) + doc.widthOfString(" - ") + doc.widthOfString(pdfMsgObj.totalRevenueMax[i]));
	//	console.log("the y position two is: ");
//	console.log(doc.y);
		doc.text(pdfMsgObj.totalRevenueMin[i], xpos, doc.y, { continued: true } )
		.text(" - ", { continued: true } )
		.text(pdfMsgObj.totalRevenueMax[i])
		.fillColor(pdfColors[5])
		// I need to figure out which of these are reasonable values to display as well as how many of them
		// in order to make it look ok and not take up too much space
		.fontSize(8) // .fontSize(subFieldFontHeight) //.fontSize(8)
		.text("Total Population: ", 100, doc.y, { continued: true } )
		.text((pdfMsgObj.population[i]).toLocaleString('en'), { continued: true } )
		.text("Average Annual Revenue: ", secondMargin - doc.widthOfString("Average Annual Revenue: "), doc.y, { continued: true })
		.text(pdfMsgObj.averageRevenueMin[i], { continued: true } )
		.text(" - ", { continued: true } )
		.text(pdfMsgObj.averageRevenueMax[i])
		.text("Total Employees: ", 100, doc.y, { continued: true } )
		.text(pdfMsgObj.totalEmployeesMin[i], { continued: true } )
		.text(" - ", { continued: true } )
		.text(pdfMsgObj.totalEmployeesMax[i], { continued: true } );
		console.log("start");
		console.log(pdfMsgObj.totalEmployeesMin[i]);
		console.log(doc.widthOfString(pdfMsgObj.totalEmployeesMin[i]));
		console.log(doc.widthOfString("41"));  // left off here, throwing NaN for some reason
		doc.text("Revenue Per Capita: ", secondMargin - 
			doc.widthOfString("Total Employees: ") - 
			doc.widthOfString(pdfMsgObj.totalEmployeesMin[i]), 
		//	doc.widthOfString(" - ") - 
		//	doc.widthOfString(pdfMsgObj.totalEmployeesMax[i]),
			doc.y, { continued: true } )
		.text(pdfMsgObj.revenuePerCapitaMin[i], { continued: true } )
		.text(" - ", { continued: true } )
		.text(pdfMsgObj.revenuePerCapitaMax[i])
		.text("Household Income: ", 100, doc.y, { continued: true } )
		.text(pdfMsgObj.householdIncome[i], { continued: true } )
		.text("Household Expenditures (Average): ", 300, doc.y, { continued: true } )
		.text(pdfMsgObj.householdExpenditures[i])
		.text("Median Age: ", 100, doc.y, { continued: true } )
		.text(pdfMsgObj.medianAge[i], { continued: true } )
		.text("Bachelors Degree or Higher: ", 300, doc.y, { continued: true } )
		.text((pdfMsgObj.bachelorsDegreeOrHigher[i] * 100).toFixed(1), { continued: true } )
		.text("%")
		.text("High School Degree or Higher: ", 100, doc.y, { continued: true } )
		.text((pdfMsgObj.highSchoolOrHigher[i] * 100).toFixed(1), { continued: true } )
		.text("%", { continued: true } )
		.text("White Collar Workers: ", 300, doc.y, { continued: true } )
		.text((pdfMsgObj.whiteCollarWorkers[i] * 100).toFixed(1), { continued: true } )
		.text("%");
		console.log("the y position three is: ");
	}
	// I want to test if it's breaking the page
//	let blockLen = doc.y - yStart;
//	console.log("yEnd: ", doc.y, ", yStart: ", yStart, ", blockLen: ", blockLen);

	// Finalize the pdf file
	doc.end();
//	let url = "https://maps.googleapis.com/maps/api/staticmap?center=Brooklyn+Bridge,New+York,NY&zoom=13&size=600x300&maptype=roadmap&markers=color:blue%7Clabel:S%7C40.702147,-74.015794&markers=color:green%7Clabel:G%7C40.711614,-74.012318&markers=color:red%7Clabel:C%7C40.718217,-73.998284&key=AIzaSyBYmAqm62QJXA2XRi1KkKVtWa6-BVTZ7WE";
}
