const pdf = require("./app.js");
const filename = "trav.pdf";
pdf.generatePDF(
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
	"Customer Business Name", filename); // .then(done).catch(fail);.
