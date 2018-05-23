if (global.sizeup)  throw new Error("Cannot have global sizeup already");

global.sizeup = {};  // Yikes! NOTE: CONSIDER REVISING: loaded scripts depend on sizeup being global, or at least otherwise injected
global.window = global.window || {sizeup:sizeup};  // see?
sizeup.api = {};  // yep: that's global sizeup; the requires below install under here: sizeup.api

var api = require('./src/api.js');

// The scripts are unmodified: they depend on the global window semantics on browser, as hacked above.
require('./src/data');         // installs window.sizeup.api.data
require('./src/attributes');   // installs window.sizeup.api.attributes
require('./src/granularity');  // installs window.sizeup.api.granularity
// TODO: if we modify the above required modules we needn't use global.sizeup, BUT then we can't use fresh and unmodified data.js in the future

module.exports = api;
