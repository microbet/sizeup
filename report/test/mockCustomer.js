const path = require("path");

var mockCustomerGraphics = {

  custAddress: "1243 Main St.",
  custCity: "Tuscon",
  custState: "AZ",
  custZip: "80976",
  custEmail: "customer.email@gmail.com",
  custBizName: "Customer Business Name",
  custLogo: "smlogo.png",

  writeHeader: function(doc, theme) {
    doc.font('Helvetica-Bold');
    doc.fontSize(22);
    doc.fillColor('white');
    let widthBizName = doc.widthOfString(this.custBizName);
    let startBizName = 245 - (widthBizName/2);
    doc.text(this.custBizName, startBizName, 50);
    doc.image(path.join(__dirname, this.custLogo), 425, doc.y - 32, { width: 120 } );
  },

  writeFooter: function(doc, theme) {
    doc.text(' ');
    doc.font('Helvetica-Bold');
    doc.fontSize(15);
    let widthBizName = doc.widthOfString(this.custBizName);
    doc.fillColor(theme.text.color);
    let startBizName = 306 - (widthBizName/2);
    doc.text(this.custBizName, startBizName, 673);
    doc.fontSize(12);
    doc.fillColor('black');
    let addressString = this.custAddress + ' ' + this.custCity + ' ' + this.custZip;
    let widthAddress = doc.widthOfString(addressString);
    let startAddress = 306 - widthAddress/2;
    doc.text(addressString, startAddress, doc.y);
    let widthEmail = doc.widthOfString(this.custEmail);
    let startEmail = 306 - widthEmail/2;
    doc.text(this.custEmail, startEmail, doc.y);
  }
};

var mockDefaultGraphics = {
  custBizName: "Business Intelligence for All",
  custLogo: "SizeUp.png",
  
  writeHeader: function(doc, theme) {
    doc.font('Helvetica-Bold');
    doc.text(' ');
    // TODO pending available file: doc.font('./fonts/SourceSansPro-Light.otf');
    doc.fontSize(28);
    doc.fillColor('white');
    let widthBizName = doc.widthOfString(this.custBizName);
    let startBizName = 245 - (widthBizName/2);
    doc.text(this.custBizName, startBizName, 45);
    doc.image(path.join(__dirname, this.custLogo), 425, doc.y - 32, { width: 120 } );
  },

  writeFooter: function(doc, theme) {
    doc.text(' ');
    doc.image(path.join(__dirname, "gauge-home.png"), 80, 678);
    // TODO pending available file: doc.font('./fonts/SourceSansPro-Light.otf');
    doc.fillColor('black');
    doc.fontSize(12);
    doc.text('How is My Business Performing?', 140, 683, { continued: true, width: 100});
    doc.image(path.join(__dirname, "marketing-home.png"), 250, 678);
    doc.text(' ');
    doc.text('Where should I advertise?', 310, 683, { continued: true, width: 100, align : 'left' });
    doc.image(path.join(__dirname, this.custLogo), 420, 678, { width: 120 } );
  }
};

var customerGraphicsDatabase = {
  "6388E63C-3D44-472B-A424-712395B1AD51": mockDefaultGraphics,
  "12345678-1234-1234-1234-123456789ABC": mockCustomerGraphics
};

function mockGetReportGraphics(customerKey) {
  if (customerKey in customerGraphicsDatabase) {
    return customerGraphicsDatabase[customerKey];
  } else {
    throw Error(`Not found: Customer key ${customerKey}`);
  }
}

module.exports = {
  getReportGraphics: mockGetReportGraphics,
  mockDatabase: customerGraphicsDatabase
};
