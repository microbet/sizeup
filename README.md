# SizeUp API and CLI

## API SDK usage

```
require('sizeup-api')(process.env.SIZEUP_KEY);

sizeup.api.data.findPlace(
    { term:"fresno", maxResults:10 },
    function(result) { console.log(JSON.stringify(result,0,2)); },
    function(exc) { console.error(exc); }
);
```

## CLI usage

(After `npm install -g sizeup-api`)

```
export SIZEUP_KEY=...
sizeup findPlace '{"term":"fresno"}'
sizeup findIndustry '{"term":"tech"}'
```

## Read The Friendly Manual

[Here](http://www.sizeup.com/developers/documentation)
