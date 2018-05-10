// TODO: temporary driver code
require('./api')();  // installs sizeup.* globally

sizeup.api.data.findPlace(
    { term:"fresno", maxResults:10 },
    function(result) { console.log(JSON.stringify(result,0,2)); },
    function(exc) { console.error(exc); }
);
