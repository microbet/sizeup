# pdfGenerator

Generate a pdf which displays the output from an advertising query.  Display results by zip code on a google map with pins.  Display information on each result including the specific results for the filters.  Show the filters used in the query.

## Getting Started

Clone project from Bitbucket.  The repository is at sizeupteam/sizeup.git.  The project is located in the report directory.

### Prerequisites

- Node.js version 6.14.
- A key for the SizeUp API.
- A key for the Google Static Maps API.

### Installation

Install node, clone the repository and run npm install to install the necessary packages.  Installing node varies on your machine.  Go to nodejs.org for the [download](https://nodejs.org/en/download/) .

If you don't have it, [install git on your machine.](https://git-scm.com/downloads)

Go to your Bitbucket account, navigate to the [repository](https://bitbucket.org/sizeupteam/sizeup/src/master/) and get the command to clone by clicking the "Clone" link.

Create a directory on your machine for the project and run the clone command from inside that directory.

## Running tests

pdftester.js ( located in the "report" subdirectory ) is used for testing the script.  When everything is installed `node pdftester.js` will run the script and save the ouput in your local "report" sub-directory.  The test file is saved as "trav.pdf". 

## Deployment

In production, someone with access to the sizeup-api, either installed as an npm package or querying it via http, with a valid size-up key should be able to generate this pdf using this function:

`sizeup.pdf.generatePDF( query, key, stream)`

Where query is a size-up query object, key is a size-up key, and the stream is the container used for the pdf data which can then be used by the customer (eg saved to a pdf file).

## Example sizeup query object

See sample queries in test/ directory.

## Customer info header/footer

Header and footer are obtained from 

```
sizeup.customer.getReportGraphics(customerKey)
```

specifically,

```
sizeup.customer.getReportGraphics(customerKey).writeHeader(doc, theme)
```
and
```
sizeup.customer.getReportGraphics(customerKey).writeFooter(doc, theme)
```
a customer specific example of which looks like:
```
 writeHeader: function(doc, theme) {
    doc.text(' ');
    doc.font('./fonts/SourceSansPro-Light.otf');
    doc.fontSize(25);
    doc.fillColor('blue');
    doc.text('Business Name', 45);
    doc.image(path.join(__dirname, this.custLogo), 425, doc.y - 32, { width: 120 } );
  }
```
These functions are passed the doc object and can break the pdf if they are out of place or too large.  They should not take more than about 60 px height and 563 in width.  The header should start at about y = 30 and footer at about y = 725
