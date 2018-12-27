const mockSearchObj = require('./test/mockSearch.json');  // why doesn't import work
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
// const customerObj = require("./test/mockCustomer.js");
const customerObj = require("./test/defaultCustomer.js");


// Run test code.

const filename = "trav.pdf";
const stream = require("fs").createWriteStream(filename);

function done(stream) {
 // stream.close();
  console.log("Wrote ", filename);
}

function fail(e) {
//  stream.close();
  console.error("error", e);
}

Promise.all([pdf.generatePDF(
  mockSearchObj,
  process.env.SIZEUP_KEY,
  customerObj,	
  stream)]).then(() => {
    done();
  }).catch(fail());  
