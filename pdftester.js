const pdf = require("./app.js");
const filename = "trav.pdf";
// const stream = require("fs").createWriteStream(filename);

function done() {
//	stream.close();
	console.log("Wrote ", filename);
}

function fail(e) {
//	stream.close();
	console.error("error", e);
}

Promise.all([pdf.generatePDF(
	'totalRevenue',
	[50000, null],
	5,
	16,
	41284,
	8524,
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
	"1243 Main St.",
	"Tuscon",
	"AZ",
	"80976",
	"customer.email@gmail.com",
//	"Customer Business Name", stream)]).then(done()).catch(fail());
	"Customer Business Name", filename)]).then(done()).catch(fail());

