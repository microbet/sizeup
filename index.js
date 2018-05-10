// TODO: temporary driver code

if (!process.env.SIZEUP_KEY)  throw new Error("Need $SIZEUP_KEY to authenticate");
require('./api')(process.env.SIZEUP_KEY);  // installs sizeup.* globally; TODO reconsider

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
