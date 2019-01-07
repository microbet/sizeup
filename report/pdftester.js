const testInputs = [
  // note testQuery1 currently fails because "householdExpenditures" has no
  // user advice written for it. This is a correct failure.
  require('./test/testQuery1.json'),
  require('./test/testQuery2.json'),
  require('./test/testQuery3.json'),
  require('./test/testQuery4.json'),
  require('./test/testQuery5.json'),
];
const fs = require("fs");
const mockCustomer = require("./test/mockCustomer.js");
const report = require(".");

// Monkeypatch the Sizeup API with our mock function, since production code
// doesn't know about customer graphics.

var sizeup = require("sizeup-api")({ key:process.env.SIZEUP_KEY });
sizeup.customer = mockCustomer;
report.setSizeup(sizeup);

function done(stream, filename) {
  stream.close(); // Technically not correct, because pdfkit closes the
  // stream on its own, after flushing all pages, during doc.end() .
  // But I close the stream here anyway so that the test will fail if
  // it thinks the operation is done before it's actually done.
  console.log("Wrote ", filename);
}

function fail(e, stream) {
  stream.close();
  console.error("error", e);
}

var sizeup_keys = Object.keys(mockCustomer.mockDatabase);

testInputs.forEach(function(input, i) {
  var filename = "testOutput" + (i+1) + ".pdf";
  var stream = fs.createWriteStream(filename);
  Promise.resolve(report.advertising.generatePDF(
    input.query,
    (i == 1 ? sizeup_keys[0] : sizeup_keys[1]),
    stream,
    input.title))
  .then(() => {
    done(stream, filename);
  })
  .catch(e => { fail(e, stream); });
});
