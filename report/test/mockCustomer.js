const path = require("path");

var mockCustomerGraphics = {

  custAddress: "1243 Main St.",
  custCity: "Tuscon",
  custState: "AZ",
  custZip: "80976",
  custEmail: "customer.email@gmail.com",
  custBizName: "Customer Business Name",
  custLogo: "smlogo.png",

  writeHeader: function(doc, theme, startX, startY, height, width) {
    doc.font('Helvetica-Bold');
    doc.fontSize(22);
    doc.fillColor('blue');
    let widthBizName = doc.widthOfString(this.custBizName);
    let startBizName = (width - widthBizName - 200);
    doc.text(this.custBizName, startBizName, startY -5 );
    doc.image(path.join(__dirname, this.custLogo), startX + width - 200, startY - 15, { width: 120 } );
  },

  writeFooter: function(doc, theme) {
    doc.text(' ');
    doc.font('Helvetica-Bold');
    doc.fontSize(15);
    let widthBizName = doc.widthOfString(this.custBizName);
    doc.fillColor(theme.text.color);
    let startBizName = 306 - (widthBizName/2);
    doc.text(this.custBizName, startBizName, 730);
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
  
  writeHeader: function(doc, theme, startX, startY, height, width) {
    doc.font('Helvetica-Bold');
    doc.text(' ');
    // TODO pending available file: - done, J
    doc.font('./fonts/SourceSansPro-Light.otf');
    doc.fontSize(25);
    doc.fillColor('blue');
    let widthBizName = doc.widthOfString(this.custBizName);
    let startBizName = (width - widthBizName - 175);
    doc.text(this.custBizName, startBizName, startY);
    doc.image(path.join(__dirname, this.custLogo), startX + width - 175, startY - 15, { width: 120 } );
  },

  writeFooter: function(doc, theme) {
    doc.text(' ');
    doc.image(path.join(__dirname, "gauge-home.png"), 80, 725);
    // TODO pending available file:  - done, J
    doc.font('./fonts/SourceSansPro-Light.otf');
    doc.fillColor('black');
    doc.fontSize(12);
    doc.text('How is My Business Performing?', 140, 725, { continued: true, width: 100});
    doc.image(path.join(__dirname, "marketing-home.png"), 250, 730);
    doc.text(' ');
    doc.text('Where should I advertise?', 310, 725, { continued: true, width: 100, align : 'left' });
    doc.image(path.join(__dirname, this.custLogo), 420, 730, { width: 120 } );
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
