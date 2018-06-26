# SizeUp API: node SDK and CLI

## SDK usage

### Modern ES6 style, using Promises
```javascript
const sizeup = require('.')({ key:process.env.SIZEUP_KEY });
const logj = r => console.log(JSON.stringify(r,0,2)) || r;

Promise
  .all([
    sizeup.data.findIndustry({ term:"grocery" }),
    sizeup.data.findPlace({ term:"fresno", maxResults:2 }),
  ])
  // .then(logj)                       // for debugging
  .then(([ [industry], [place] ]) =>
    sizeup.data.getAverageRevenue({
      industryId: industry.Id,
      geographicLocationId: place.City.Id
    })
  )
  .then(logj)                          // final output
  .catch(console.error)
```

### Old style
```javascript
var sizeup = require('sizeup-api')({ key:process.env.SIZEUP_KEY });

// Old style: callbacks
sizeup.data.findPlace({ term:"fresno", maxResults:2 },
  console.log, console.error );
sizeup.data.findIndustry({ term:"grocery" }),
  console.log, console.error );
```

See also [the ES6 example](./example.es6.js) and [the old-style example](./example.js).


## CLI usage

(After `npm install -g sizeup-api`)

```bash
export SIZEUP_KEY=...
sizeup findPlace '{"term":"fresno"}'
sizeup findIndustry '{"term":"tech"}'
sizeup getAverageSalaryBands '{
  "boundingGeographicLocationId": 130073,
  "industryId": 8589,
  "granularity": "County",
  "bands": 7
}'
```

Each `sizeup` subcommand (e.g., `findPlace`) is a function in `sizeup.data` — or `sizeup.api.data` in the [The API Documentation](http://www.sizeup.com/developers/documentation).

The `granularity` and `attributes` values as used in the Documentation can be provided directly as CamelCase strings, as in the last example (`getAverageSalaryBands`: `"granularity": "County"`), above.

### granularity
```
ZIP_CODE: 'ZipCode',
CITY: 'City',
COUNTY: 'County',
PLACE: 'Place',
METRO: 'Metro',
STATE: 'State',
NATION: 'Nation'
```

### attributes
```
TOTAL_REVENUE: 'TotalRevenue',
AVERAGE_REVENUE: 'AverageRevenue',
REVENUE_PER_CAPITA: 'RevenuePerCapita',
TOTAL_EMPLOYEES: 'TotalEmployees',
AVERAGE_EMPLOYEES: 'AverageEmployees',
EMPLOYEES_PER_CAPITA: 'EmployeesPerCapita'
```

## Read The Friendly Manual

[The API Documentation](http://www.sizeup.com/developers/documentation)
