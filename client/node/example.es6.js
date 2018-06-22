#!/usr/bin/env node

if (!process.env.SIZEUP_KEY)  return console.error("ERROR: Need $SIZEUP_KEY to authenticate");

require('.')(process.env.SIZEUP_KEY).then(sizeupApi => {
  const logj = r => console.log(JSON.stringify(r,0,2)) || r;
  const data = sizeupApi.data;

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
        data.getAverageRevenue({geographicLocationId: place.City.Id, industryId: industry.Id}),
        data.getAverageRevenue({geographicLocationId: place.County.Id, industryId: industry.Id}),
        data.getAverageRevenue({geographicLocationId: place.Metro.Id, industryId: industry.Id}),
        data.getAverageRevenue({geographicLocationId: place.State.Id, industryId: industry.Id}),
        data.getAverageRevenue({geographicLocationId: place.Nation.Id, industryId: industry.Id}),
        data.getAverageRevenuePercentile({value: user.revenue, geographicLocationId: place.City.Id, industryId: industry.Id}),
        data.getAverageRevenuePercentile({value: user.revenue, geographicLocationId: place.County.Id, industryId: industry.Id}),
        data.getAverageRevenuePercentile({value: user.revenue, geographicLocationId: place.Metro.Id, industryId: industry.Id}),
        data.getAverageRevenuePercentile({value: user.revenue, geographicLocationId: place.State.Id, industryId: industry.Id}),
        data.getAverageRevenuePercentile({value: user.revenue, geographicLocationId: place.Nation.Id, industryId: industry.Id}),
        data.getAverageRevenueBands({granularity:"ZipCode", boundingGeographicLocationId: place.County.Id, bands: 5, industryId: industry.Id}),
        data.getConsumerExpenditureVariables({parentId:1}),
        data.getConsumerExpenditureVariables({parentId:32}),
        data.getConsumerExpenditureVariable({id: 172}),
        data.getBusinessesByIndustry({
          industryIds: [10526,9664],
          geographicLocationId: place.Id, itemCount: 0, page: 1,
        }),
        data.getBusinessesByIndustry({
          industryIds: [10526],
          geographicLocationId: place.Id, itemCount: 0, page: 1,
        }),
        data.getBusinessesByIndustry({
          industryIds: [9664],
          geographicLocationId: place.Id, itemCount: 0, page: 1,
        }),
      ]))
      ,

  ])
  .then(logj)

})
.catch(console.error)
