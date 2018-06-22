#!/usr/bin/env node

// Example driver code


if (!process.env.SIZEUP_KEY)  return console.error("ERROR: Need $SIZEUP_KEY to authenticate");
var sizeupApi = require('.')(process.env.SIZEUP_KEY);  // TODO: return promise from factory to auth

var onSuccess = function(result) { console.log(JSON.stringify(result,0,2)); };
var onError = function(exc) { console.error(exc); };

sizeupApi.data.findPlace(
    { term:"fresno", maxResults:10 },
    onSuccess, onError
);

sizeupApi.data.findPlace(
    { term:"san francisco", maxResults:3 }
)
    .then(onSuccess)
    .catch(onError)

sizeupApi.data.getAverageSalaryBands(
    {
        boundingGeographicLocationId: 130073,
        industryId: 8589,
        granularity: sizeupApi.granularity.COUNTY,
        bands: 7
    },
    onSuccess, onError
);
