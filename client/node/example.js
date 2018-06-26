#!/usr/bin/env node

if (!process.env.SIZEUP_KEY)  return console.error("ERROR: Need $SIZEUP_KEY to authenticate");
var sizeupApi = require('.')({ key:process.env.SIZEUP_KEY });

sizeupApi.data.findPlace({ term:"san francisco", maxResults:3 })
  .then(onSuccess)
  .catch(console.error)

sizeupApi.data.getAverageSalaryBands({
    boundingGeographicLocationId: 130073,
    industryId: 8589,
    granularity: sizeupApi.granularity.COUNTY,
    bands: 7
  })
  .then(onSuccess)
  .catch(console.error)


// Old style: callbacks
sizeupApi.data.findPlace(
  { term:"fresno", maxResults:2 },
  onSuccess, console.error
);

function onSuccess(result) { console.log(JSON.stringify(result,0,2)); };
