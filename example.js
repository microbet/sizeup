#!/usr/bin/env node

// Example driver code


if (!process.env.SIZEUP_KEY)  return console.error("ERROR: Need $SIZEUP_KEY to authenticate");
require('./src/api')(process.env.SIZEUP_KEY);  // installs sizeup.* globally; TODO reconsider

var onSuccess = function(result) { console.log(JSON.stringify(result,0,2)); };
var onError = function(exc) { console.error(exc); };

sizeup.api.data.findPlace(
    { term:"fresno", maxResults:10 },
    onSuccess, onError
);

sizeup.api.data.findPlace(
    { term:"san francisco", maxResults:3 },
    onSuccess, onError
);

sizeup.api.data.getAverageSalaryBands(
    {
        boundingGeographicLocationId: 130073,
        industryId: 8589,
        granularity: sizeup.api.granularity.COUNTY,
        bands: 7
    },
    onSuccess, onError
);
