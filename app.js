const fs = require('fs');
const PDFDocument = require('pdfkit');
const https = require('https');
require('dotenv').config();
const sizeup = require("sizeup-api")({ key:process.env.SIZEUP_KEY });
const request = require('request');
	
/****
* the function is going to take this stuff as inputs, but I'm just setting it here
* for development
*/

// Starting to work on testing by making some of the search choices random

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

// const attribute = getRandElement(['revenuePerCapita', 'totalRevenue', 'averageRevenue', 'underservedMarkets', 'totalEmployees', 'householdIncome']);
// const averageRevenue = getRandRange(0, 50000000);
// randRange function probably needs to favor the low end
// and it maybe that the api wants some of these things very rounded off
const attribute = 'totalRevenue';
const averageRevenue = [50000, null];
const bands = 5;
const distance = 16;
const geographicLocationId = 41284;
const industryId = 8524;
const itemCount = 3;
const order = 'highToLow';
const page = 1;
const sort = 'desc';
const sortAttribute = attribute;
const totalEmployees = [0, null];
const totalRevenue = [0, null];
const highSchoolOrHigher = 0;  // a percent
const householdExpenditures = [0, null];
const householdIncome = [0, null];
const medianAge = [0, null];
const revenuePerCapita = [0, null];
const whiteCollarWorkers = 0;
console.log("attribute = ", attribute);

// this function is to display the search criteria to the user as capitalized words
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

let pdfMsgObj = {
	displayAttribute: formatCamelToDisplay(attribute),
	distance: distance,
	bands: bands,
	imgFile: '',
	zip: [],
	totalRevenueMin: [],
	totalRevenueMax: [],
	population: [],
	avgRevenueMin: [],
	avgRevenueMax: [],
	totalEmployeesMin: [],
	totalEmployeesMax: [],
	householdIncome: [],
	medianAge: [],
	householdExpenditures: [],
	whiteCollarWorkers: [],
	bachelorsDegreeOrHigher: [],
	highSchoolOrHigher: [],
	averageRevenueMin: [],
	averageRevenueMax: [],
	revenuePerCapitaMin: [],
	revenuePerCapitaMax: [],
	centroidLat: [],
	centroidLng: [],
	attribute: attribute,
	sortAttribute: sortAttribute,
	bandArr: [],
}

let pdfColors = [   // was more elegant as object, but I iterate over this in loops later
	'#dc3545', // red
	'#28a745', // green
	'#007bff', // blue
	'#fd7e14', // orange
	'#343a40', // dark grey
	'#6c757d', // gray
	'#e5e3df', // very light grey
]

	/***
	reference - colors from sizeup
	--blue:#007bff;		--indigo:#6610f2;		--purple:#6f42c1;		--pink:#e83e8c;
	--red:#dc3545;		--orange:#fd7e14;		--yellow:#ffc107;		--green:#28a745;
	--teal:#20c997;		--cyan:#17a2b8;			--white:#fff;			--gray:#6c757d;
	--gray-dark:#343a40; --primary:#007bff;		--secondary:#6c757d;	--success:#28a745;
	--info:#17a2b8;		--warning:#ffc107;		--danger:#dc3545;		--light:#f8f9fa;
	--dark:#343a40;
	*/

let filterDisplay = { toggle: 1 };

if (averageRevenue[0] === 0 && averageRevenue[1] === null) { filterDisplay.averageRevenue = true; }
if (totalEmployees[0] === 0 && totalEmployees[1] === null) { filterDisplay.totalEmployees = true; }
if (totalRevenue[0] === 0 && totalRevenue[1] === null) { filterDisplay.totalRevenue = true; }
if (householdIncome[0] === 0 && householdIncome[1] === null) { filterDisplay.householdIncome = true; }
if (revenuePerCapita[0] === 0 && revenuePerCapita === null) { filterDisplay.revenuePerCapita = true; }
if (highSchoolOrHigher != 0) { filterDisplay.highSchoolOrHigher = true; }
if (medianAge != 0) { filterDisplay.medianAge = true; }
if (whiteCollarWorkers != 0) { filterDisplay.whiteCollarWorkers = true; }
filterDisplay.population = true;

// console.log(averageRevenue, " is the avgR value");
// console.log(filterDisplay.averageRevenue, " is the avgR");

	/***
	reference - colors from sizeup
	--blue:#007bff;		--indigo:#6610f2;		--purple:#6f42c1;		--pink:#e83e8c;
	--red:#dc3545;		--orange:#fd7e14;		--yellow:#ffc107;		--green:#28a745;
	--teal:#20c997;		--cyan:#17a2b8;			--white:#fff;			--gray:#6c757d;
	--gray-dark:#343a40; --primary:#007bff;		--secondary:#6c757d;	--success:#28a745;
	--info:#17a2b8;		--warning:#ffc107;		--danger:#dc3545;		--light:#f8f9fa;
	--dark:#343a40;
	*/

// describe the query, place, industry, KPI, search distance, filters - todo along with customer info

// console log for testing/dev
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

/***
* Communication with sizeup api
* Get the info from the sizeup api, then in successCallback put the return info into the pdfMsgObj
* Then build the pdf with that info
*/

Promise.all([
	sizeup.data.getPlace({ id: geographicLocationId }),
	sizeup.data.getIndustry( { id: industryId }),
	sizeup.data.getBestPlacesToAdvertise( { totalEmployees: totalEmployees, highSchoolOrHigher: highSchoolOrHigher, householdExpenditures: householdExpenditures, householdIncome: householdIncome, medianAge: medianAge, revenuePerCapita: revenuePerCapita, whiteCollarWorkers: whiteCollarWorkers, totalRevenue: totalRevenue, bands: bands, industryId: industryId, order: order, page: page, sort: sort, sortAttribute: sortAttribute, geographicLocationId: geographicLocationId, distance: distance, attribute: attribute } ),
	sizeup.data.getBestPlacesToAdvertiseBands( { totalEmployees: totalEmployees, highSchoolOrHigher: highSchoolOrHigher, householdExpenditures: householdExpenditures, householdIncome: householdIncome, medianAge: medianAge, revenuePerCapita: revenuePerCapita, whiteCollarWorkers: whiteCollarWorkers, totalRevenue: totalRevenue, bands: bands, industryId: industryId, order: order, page: page, sort: sort, sortAttribute: sortAttribute, geographicLocationId: geographicLocationId, distance: distance, attribute: attribute } ),
	]).then(([place, industry, bestPlaces, bestPlacesBands]) => {
		successCallback(bestPlaces.Items, "Best Places to Advertise"); // note: would you do this instead of putting the forEach loop right here?
		pdfMsgObj['displayLocation'] = place[0].City.LongName;
		pdfMsgObj['displayIndustry'] = industry[0].Name;
		bandsCallback(bestPlacesBands, "Best Places to Advertise Bands");
		// console.log(bestPlaces);
	}).then(startPdf).catch(console.error)

function bandsCallback(result, msg="success") {
	console.log("in bands");
	console.log(result);
	pdfMsgObj.bandArr = result;
	console.log("out of bands");
}	
	
/**
 *  successCallback puts the return info into the pdfMsgObj.  The result (Items) is an array
 *  so loop through to build the arrays which are members of the pdfMsgObj
 *  A lot of formatting in here.  Max output is 3.  It's hard to present more on one page.
 */

function successCallback(result, msg="success") {
	let i=0;
	for (element of result) {
		i++;
		pdfMsgObj['zip'].push(element.ZipCode.Name);
//		console.log(element);   // good for debugging
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
	}
}

function failureCallback(error) {
	console.log("failure: " + error);
}

/****
 * note: If more than one pdf style is necessary it would not be hard
 * to make a template and then some markup for inserting values of the
 * pdfMsgObj.  If there is generally some problem with the html2pdf
 * scripts it might not be that hard to develop a markup for basic
 * html, but adding specific things for something like pdfkit where
 * you can specify position - draw lines etc with its functions
 */

/***
*  startPdf creates the static map image using static google map api.
*  Perhaps the image could be streamed into the pdf and supposedly 
*  that is possible with pdfkit, but after trying for too long we
*  decided to just download the image and then bring it into the pdf.
*  It is given a virtually unique file name and deleted after it 
*  is brought into the pdf. 
*/
 
function startPdf() {
// just keeping those urls as a reference	
//	const url = 'https://maps.googleapis.com/maps/api/staticmap?center=Brooklyn+Bridge,New+York,NY&zoom=13&size=600x300&maptype=roadmap&markers=color:green%7Clabel:S%7C40.702147,-74.015794&markers=color:orange%7Clabel:G%7C40.711614,-74.012318&markers=color:red%7Clabel:C%7C40.718217,-73.998284&key=AIzaSyBYmAqm62QJXA2XRi1KkKVtWa6-BVTZ7WE';
// https://maps.googleapis.com/maps/api/js/ViewportInfoService.GetViewportInfo?1m6&1m2&1d28.541763002486213&2d-100.75242339877633&2m2&1d31.49107851274312&2d-95.13921000828736&2u9&4sen-US&5e0&6sm@442000000&7b0&8e0&callback=_xdc_._f0kl74&key=AIzaSyBYmAqm62QJXA2XRi1KkKVtWa6-BVTZ7WE&token=19066

	// building the markers string for the pins on the map, then download the static map

	let markerStr = '';
	for (let i=0; i<pdfMsgObj.centroidLat.length; i++) {
		markerStr += "markers=color:" + pdfColors[i].replace("#", "0x") + "%7C" + "label:" + (i+1) + "%7C" + pdfMsgObj.centroidLat[i] + ',' + pdfMsgObj.centroidLng[i] + '&';
	}
	const url = 'https://maps.googleapis.com/maps/api/staticmap?size=600x300&maptype=roadmap&' + markerStr + 'key=AIzaSyBYmAqm62QJXA2XRi1KkKVtWa6-BVTZ7WE';
	var download = function(uri, filename, callback){
		request.head(uri, function(err, res, body){
			request(uri).pipe(fs.createWriteStream(filename)).on('close', callback);
		});
	};

	let imgFile = IDGenerator();  // create the random/unique name
	imgFile = imgFile + '.png';
	download(url,  imgFile, function(){
		pdfMsgObj.imgFile = imgFile;
		buildPdf();
	});
}

// todo - when there are 3 items (probably any odd number) it displays ok on the first
// iteration, but on the second there are two on the right and one on the left

function showFilter(label, param, min, max, doc, suffix='') {
	let startX;
	if (filterDisplay[param] && param !== pdfMsgObj.sortAttribute) {
		if (filterDisplay.toggle > 0) {
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
		if (filterDisplay.toggle < 0) {
			doc.text('');
			doc.moveDown();
		}
		filterDisplay.toggle = filterDisplay.toggle * -1;
	}
}
	
/****
 * this function is building the pdf using the info from the pdfMsgObj and using 
 * pdfkit module
 */

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
	
	// start writing text 
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
	
	// here is where I should put the bands
	// the bands are inside pdfMsgObj.bandArr
	doc.fontSize(8);
	doc.fillColor(pdfColors[4]);
	doc.moveDown(0.5);

	console.log("bands = ", pdfMsgObj.bands);
	doc.rect( 25, 446, 561, 22 );
	doc.fillAndStroke('#f3f3f3');
	
	//  construct an array of x,y starting points for the bands
	let j = 0, n = 0; m = 0;
	let bandMinText, widthMinText, widthDash;
	let startArr = [];
	for (let k=0; k<pdfMsgObj.bands; k++) {
		n = k % 3;  // n (remainder of k/3) is the column in the display of bands
		m = Math.floor(k/3);  // each row will have 3 bands listed
		startArr.push([40 + n*180, 450 + m*10]);
	}
	// startArr = [[40, 450 + m*10], [220, 450], [400, 450], [40, 460], [220, 460]];	

	// then render the bands
	doc.fillColor(pdfColors[4]);
	pdfMsgObj.bandArr.forEach(function(element) {
		bandMinText = Intl.NumberFormat('en-US', { 
			style: 'currency',
			currency: 'USD',
			maximumFractionDigits: 0,
			minimumFractionDigits: 0 
		}).format(element.Min);
		doc.text(bandMinText, startArr[j][0], startArr[j][1]); //, { continued: true } )
		widthMinText = doc.widthOfString(bandMinText);
		doc.text(' - ', startArr[j][0] + widthMinText, startArr[j][1]); //, { continued: true } );
		widthDash = doc.widthOfString(' - ');
		doc.text(Intl.NumberFormat('en-US', { 
			style: 'currency', 
			currency: 'USD', 
			maximumFractionDigits: 0, 
			minimumFractionDigits: 0 
		}).format(element.Max), startArr[j][0] + widthMinText + widthDash, startArr[j][1]);
		j++;
	});
	
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
	let secondMargin = 300;
	let skipX = 0;

//  leaving the comments below for now.  I had implimented it with variable font sizes
//  to make it fill the page better, but that was making precise layout difficult
//  it's still a possibility, but some variable line spacing may be enough
//	numFields = pdfMsgObj.zip.length;
//	fieldHeight = listHeight/numFields;
//	fieldFontHeight = Math.round(fieldHeight/6);
//	subFieldFontHeight = Math.round(fieldFontHeight/2);
//I think I'm going to give up dynamic font sizing as it makes other things hard/impossible
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
	//	console.log("am I not here");
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
		filterDisplay.toggle = 1;  // this controls the margins of the fields
											// if it's not reset to 1 between areas
											// shown, the margins don't work right
											// see showFilter function
		doc.fillColor(pdfColors[i])
		.moveDown(1)
		.fontSize(10)
		.circle(75, doc.y + 5, 7);
		console.log(doc.x, " is docx");
		doc.fill()
		.fillColor('#ffffff')
		.text(i + 1, 72, doc.y + 1, { continued: true } )
		.fillColor(pdfColors[i])
		.fontSize(15)
		.text("  ", { continued: true } )
		.text(pdfMsgObj.zip[i], { continued: true })
		.fillColor('black')
		.fontSize(13);
	//	console.log(thisSortAttributeMinArr[i], "here");
		xpos = 400 - (doc.widthOfString(thisSortAttributeMinArr[i].toString()) + doc.widthOfString(" - ") + doc.widthOfString(thisSortAttributeMaxArr[i].toString()));
		doc.text(thisSortAttributeMinArr[i], xpos, doc.y, { continued: true } )
		.text(" - ", { continued: true } )
		.text(thisSortAttributeMaxArr[i])
		.fillColor(pdfColors[5])
		.fontSize(8) 
		showFilter("Total Population: ", 'population', pdfMsgObj.population[i].toLocaleString('en'), null, doc);
		showFilter("Average Annual Revenue: ", 'averageRevenue', pdfMsgObj.averageRevenueMin[i], pdfMsgObj.averageRevenueMax[i], doc);
		showFilter("Total Employees: ", 'totalEmployees', pdfMsgObj.totalEmployeesMin[i], pdfMsgObj.totalEmployeesMax[i], doc);
		showFilter("Revenue Per Capita: ", 'revenuePerCapita', pdfMsgObj.revenuePerCapitaMin[i], pdfMsgObj.revenuePerCapitaMax[i], doc);
		showFilter("Household Income: ", 'householdIncome', pdfMsgObj.householdIncome[i], null, doc);
		showFilter("Household Expenditures: ", 'householdExpenditures', pdfMsgObj.householdExpenditures[i], null, doc);
		showFilter("Median Age: ", 'medianAge', pdfMsgObj.medianAge[i], null, doc);
		showFilter("Bachelors Degree or Higher: ", 'bachelorsDegreeOrHigher', pdfMsgObj.bachelorsDegreeOrHigher[i], null, doc, '%');
		showFilter("High School Degree or Higher: ", 'highSchoolOrHigher', pdfMsgObj.highSchoolOrHigher[i], null, doc, '%');
		showFilter("White Collar Workers: ", 'whiteCollarWorkers', pdfMsgObj.whiteCollarWorkers[i], null, doc, '%');
		doc.moveDown();
	}

	// Finalize the pdf file
	doc.end();
}
