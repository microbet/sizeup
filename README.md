# SizeUp API: node SDK and CLI

## SDK usage

```javascript
require('sizeup-api')(process.env.SIZEUP_KEY);  // makes global.sizeup

sizeup.api.data.findPlace(
    { term:"fresno", maxResults:10 },
    function(result) { console.log(JSON.stringify(result,0,2)); },
    function(exc) { console.error(exc); }
);

// data functions return a Promise when called without the function args
sizeup.api.data.findPlace(
    { term:"fresno", maxResults:10 }
)
.then(function(result) { console.log(JSON.stringify(result,0,2)); })
.catch(console.error);
```

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

Each `sizeup` subcommand (e.g., `findPlace`) is a function in `sizeup.api.data`, per the [The API Documentation](http://www.sizeup.com/developers/documentation).

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
