const testInputs = [
  require('./test/testQuery1.json'),
  require('./test/testQuery2.json'),
  require('./test/testQuery3.json')
];
const mockCustomer = require("./test/mockCustomer.js");
const pdf = require("./app.js");

// Monkeypatch the Sizeup API with our mock function, since production code
// doesn't know about customer graphics.

// note for trav - why do this instead of pass customer info (customerObj)
// in pdfGenerator?
//
// answer from trav. First, what you call customerObj isn't a customer info
// object; it's a service object that returns customer info when you call
// functions on it. The PDF report consumer (that's who this test script is
// pretending to be) doesn't know how to create a service object like that.
// It just knows the customer key. The PDF report software (that's what you're
// writing) does know how to create a service object like that.
//
// However, this test script is doing more than _just_ pretending to be a
// consumer. It also sets up the test harness. It's testing your PDF software;
// your PDF software needs a customer service object; but this script
// isn't testing the customer service object. So we just mock up a simple
// customer service object that won't introduce noise or inconvenience into
// the test. The following lines are how we do that.

var sizeup = require("sizeup-api")({ key:process.env.SIZEUP_KEY });
sizeup.customer = mockCustomer;
pdf.setSizeup(sizeup);

// Run test code.

const filename = "trav.pdf";
const stream = require("fs").createWriteStream(filename);

const filename2 = "trav2.pdf";
const stream2 = require("fs").createWriteStream(filename2);

const filename3 = "trav3.pdf";
const stream3 = require("fs").createWriteStream(filename3);

function done(stream) {
 // stream.close();
  console.log("Wrote ", filename); // You have a bug here now,
  // because you reused code by copying and pasting. This was
  // a singleton script. You need to properly modularize it if
  // you want to reuse it for multiple filenames.
}

function fail(e) {
//  stream.close();
  console.error("error", e);
}

var sizeup_keys = Object.keys(mockCustomer.mockDatabase);

Promise.all([pdf.generatePDF(
  testInputs[0],
  sizeup_keys[1],
  stream)]).then(() => {
    done();
  }).catch(fail());  

Promise.all([pdf.generatePDF(
  testInputs[1],
  sizeup_keys[0],
  stream2)]).then(() => {
    done();
  }).catch(fail());  

Promise.all([pdf.generatePDF(
  testInputs[2],
  sizeup_keys[1],
  stream3)]).then(() => {
    done();
  }).catch(fail());  
