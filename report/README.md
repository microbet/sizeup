# pdfGenerator

Generate a pdf which displays the output from an advertising query.  Display results by zip code on a google map with pins.  Display information on each result including the specific results for the filters.  Show the filters used in the query.

## Getting Started

Clone project from Bitbucket.  The repository is at sizeupteam/sizeup.git.  The project is located in the report directory.

### Prerequisites

Node.js.  Latest testing has been on 10.14.2.  Packages used include, dotenv, pdfkit, query-string, request, and sizeup-api. A sizeup key, a static google map key and a customer key are required.  At this point in development the sizeup and google map keys are stored as environment variables and the costemer is mocked. 

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

```
{
  "title": "This report provided by your friendly neighborhood business banker",
  "template": "What are the ${granularity} places in ${area}, ranked by ${ranking_metric}, given ${filters}?",
  "granularity": "ZipCode",
  "area": {
    "template": "within ${distance} of ${place}",
    "distance": "11",
    "place": {
      "state": "texas",
      "county": "tarrant",
      "city": "fort-worth-city"
    }
  },
  "ranking_metric": {
    "template": "${kpi} in ${industry} (${order})",
    "kpi": "householdExpenditures",
    "industry": "shoe-and-footwear-stores",
    "order": "desc"
  },
  "filter": {
    "totalRevenue": {
      "min": "100000",
      "max": "100000000000"
    },
    "averageRevenue": {
      "min": "50000",
      "max": "50000000"
    },
    "totalEmployees": {
      "min": "10",
      "max": "1000000"
    },
    "revenuePerCapita": {
      "min": "5",
      "max": "15000"
    },
    "householdIncome": {
      "min": "10000",
      "max": "250000"
    },
    "householdExpenditures": {
      "min": "10000",
      "max": "250000"
    },
    "medianAge": {
      "min": "5",
      "max": "80"
    },
    "highSchoolOrHigher": {
      "min": "2"
    },
    "whiteCollarWorkers": {
      "min": "4"
    },
    "bachelorsDegreeOrHigher": {
      "min": "1"
    }
  }
}

```
