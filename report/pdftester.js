const mockSearchObj = require('./test/mockSearch2.json');  // why doesn't import work
																	// here - look at microdb
const pdf = require("./app.js");

// Monkeypatch the Sizeup API with our mock function, since production code
// doesn't know about customer graphics.

// note for trav - why do this instead of pass customer info (customerObj)
// in pdfGenerator?
/* 
var sizeup = require("sizeup-api")({ key:process.env.SIZEUP_KEY });
sizeup.customer = require("./test/mockCustomer.js");
pdf.setSizeup(sizeup);
*/
const customerObj = require("./test/mockCustomer.js");


// Run test code.

const filename = "trav.pdf";
const stream = require("fs").createWriteStream(filename);

function done() {
//  stream.close();
  console.log("Wrote ", filename);
}

function fail(e) {
//  stream.close();
  console.error("error", e);
}

 /* comment out while working on new arguments
Promise.all([pdf.generatePDF(
  'totalRevenue',
  [50000, null],
  5,
  16,
  {state: "texas", county: "dallas", city: "dallas-city"},
  "pizza-restaurant",
  3,
  'highToLow',
  1,
  'desc',
  'totalRevenue',
  [0, null],
  [0, null],
  0,  // a percent
  [0, null],
  [0, null],
  [0, null],
  [0, null],
  0,
  process.env.SIZEUP_KEY,
  customerObj,	
  stream)]).then(() => {
    done();
  }).catch(fail());   // was working kinda
  */
Promise.all([pdf.generatePDF(
  mockSearchObj,
  process.env.SIZEUP_KEY,
  customerObj,	
  stream)]).then(() => {
    done();
  }).catch(fail());   // was working kinda
