const mockSearchObj = require('./test/mockSearch.json');  
const mockSearchObj2 = require('./test/mockSearch2.json'); 
const mockSearchObj3 = require('./test/mockSearch3.json'); 
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
const customerObj2 = require("./test/defaultCustomer.js");


// Run test code.

const filename = "trav.pdf";
const stream = require("fs").createWriteStream(filename);

const filename2 = "trav2.pdf";
const stream2 = require("fs").createWriteStream(filename2);

const filename3 = "trav3.pdf";
const stream3 = require("fs").createWriteStream(filename3);

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

Promise.all([pdf.generatePDF(
  mockSearchObj2,
  process.env.SIZEUP_KEY,
  customerObj2,	
  stream2)]).then(() => {
    done();
  }).catch(fail());  

Promise.all([pdf.generatePDF(
  mockSearchObj3,
  process.env.SIZEUP_KEY,
  customerObj,	
  stream3)]).then(() => {
    done();
  }).catch(fail());  
