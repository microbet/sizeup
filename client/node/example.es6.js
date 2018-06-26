#!/usr/bin/env node

if (!process.env.SIZEUP_KEY)  return console.error("ERROR: Need $SIZEUP_KEY to authenticate");
const sizeupApi = require('.')(process.env.SIZEUP_KEY);

const logj = r => console.log(JSON.stringify(r,0,2)) || r;
const data = sizeupApi.data;
const eachGeographyName = fn => Promise.all(['City','County','Metro','State','Nation'].map(fn));


return Promise.all([

  sizeupApi.data.getIndustry({ id:7719 }),

  Promise
    .all([
      sizeupApi.data.findIndustry({ term:"grocery" }),
      sizeupApi.data.findPlace({ term:"dallas, tx" }),
    ])
    // .then(logj)
    .then(([ [industry], [place] ]) => Promise.all([
      sizeupApi.data.getAverageRevenue({
        industryId: industry.Id,
        geographicLocationId: place.City.Id
      }),//.then(logj),
      sizeupApi.data.getAverageRevenue({
        industryId: industry.Id,
        geographicLocationId: place.State.Id
      }),
    ]))
    ,

  Promise
    .all([
      data.getIndustryBySeokey("shoe-and-boot-repairing"),
      data.getPlaceBySeokey("california/alameda/oakland-city"),
      Promise.resolve({revenue: 123123})  // KPIs for user's own business
    ]).then(([industry, place, user]) => Promise.all([
      eachGeographyName(geogName =>
        data.getAverageRevenue({geographicLocationId: place[geogName].Id, industryId: industry.Id})),
      eachGeographyName(geogName =>
        data.getAverageRevenuePercentile({value: user.revenue, geographicLocationId: place[geogName].Id, industryId: industry.Id})),
      data.getAverageRevenueBands({granularity:"ZipCode", boundingGeographicLocationId: place.County.Id, bands: 5, industryId: industry.Id}),
      data.getConsumerExpenditureVariables({parentId:1}),
      data.getConsumerExpenditureVariables({parentId:32}),
      data.getConsumerExpenditureVariable({id: 172}),
    ]))
    ,

])
.then(logj)
.catch(console.error)
