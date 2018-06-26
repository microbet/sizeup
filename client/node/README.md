# SizeUp API: node SDK and CLI

## SDK usage

### Modern ES6 style, using Promises
```javascript
const sizeupApi = require('.')(process.env.SIZEUP_KEY);
Promise
  .all([
    sizeupApi.data.findPlace({ term:"fresno", maxResults:2 }),
    sizeupApi.data.findIndustry({ term:"grocery" }),
  ])
  .then( result => console.log(JSON.stringify(result,0,2)) )
  .catch(console.error)
```

### Old style
```javascript
var sizeupApi = require('sizeup-api')(process.env.SIZEUP_KEY);

// Old style: callbacks
sizeupApi.data.findPlace({ term:"fresno", maxResults:10 },
  onSuccess, console.error );
sizeupApi.data.findIndustry({ term:"grocery" }),
  onSuccess, console.error );
function onSuccess(result) { console.log(JSON.stringify(result,0,2)); };
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

Each `sizeup` subcommand (e.g., `findPlace`) is a function in `sizeupApi.data` — or `sizeup.api.data` in the [The API Documentation](http://www.sizeup.com/developers/documentation).

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
