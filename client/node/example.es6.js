#!/usr/bin/env node

if (!process.env.SIZEUP_KEY)  return console.error("ERROR: Need $SIZEUP_KEY to authenticate");
require('.')(process.env.SIZEUP_KEY);  // installs sizeup.* globally; TODO reconsider

const logj = r => console.log(JSON.stringify(r,0,2)) || r;

Promise.all([
    sizeup.api.data.findIndustry({ term:"grocery" }),
    sizeup.api.data.findPlace({ term:"dallas, tx" }),
])
// .then(logj)
.then(([ [industry], [place] ]) => Promise.all([
    sizeup.api.data.getAverageRevenue({
        industryId: industry.Id,
        geographicLocationId: place.City.Id
    }),//.then(logj),
    sizeup.api.data.getAverageRevenue({
        industryId: industry.Id,
        geographicLocationId: place.State.Id
    }),
]))
.then(logj)
.catch(console.error)
