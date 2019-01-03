const testInputs = [
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
 // stream.close();
  console.log("Wrote ", filename);
}

function fail(e, stream) {
//  stream.close();
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
