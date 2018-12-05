const pdf = require("./app.js");

// Monkeypatch the Sizeup API with our mock function, since production code
// doesn't know about customer graphics.
// testing a change - J

var sizeup = require("sizeup-api")({ key:process.env.SIZEUP_KEY });
sizeup.customer = require("./test/mockCustomer.js");
pdf.setSizeup(sizeup);

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
  stream)]).then(() => {
    done();
  }).catch(fail());   // was working kinda
