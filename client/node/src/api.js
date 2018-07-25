var util = require('./util');
var makeCustomerApi = require('./customer');
var makeDataApi = require('./data');
var loaderMod = require('./loader');


module.exports = function makeApi(args) {
  var apiKey = args.key;
  if (!apiKey) {
    throw new Error("Argument \"key\" is missing. You need a product key to authenticate. Please see https://www.sizeup.com/developers/documentation for help.");  // TODO: no
  }
  var loader = loaderMod(apiKey);
  if (args.serviceUrl) {
    var pattern = /^(.*):\/\/(.*)$/;
    var match = args.serviceUrl.match(pattern);
    if (match) {
      loader.me.currentLocation = { protocol: match[1], domain: match[2] };
    } else {
      throw new Error("Argument \"serviceUrl\" must match pattern " + pattern.source);
    }
  }
  return {
    customer: makeCustomerApi(loader.getData),
    data: makeDataApi(loader.getData),
    granularity: require('./granularity'),
    attributes: require('./attributes'),
    loader: loader
  }
};
