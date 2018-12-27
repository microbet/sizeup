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
const mockCustomerService = require("./test/mockCustomer.js");

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

var sizeup_keys = Object.keys(mockCustomerService.mockDatabase);

Promise.all([pdf.generatePDF(
  mockSearchObj,
  sizeup_keys[1],
  mockCustomerService,
  stream)]).then(() => {
    done();
  }).catch(fail());  

Promise.all([pdf.generatePDF(
  mockSearchObj2,
  sizeup_keys[0],
  mockCustomerService,
  stream2)]).then(() => {
    done();
  }).catch(fail());  

Promise.all([pdf.generatePDF(
  mockSearchObj3,
  sizeup_keys[1],
  mockCustomerService,
  stream3)]).then(() => {
    done();
  }).catch(fail());  
