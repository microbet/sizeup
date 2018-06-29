var util = require('./util');
var makeDataApi = require('./data');
var makeGetData = require('./loader');


module.exports = function makeApi(args) {
  var apiKey = args.key;
  if (!apiKey)  throw new Error("Need apiKey to authenticate");  // TODO: no
  return {
    data: makeDataApi(makeGetData(apiKey)),
    granularity: require('./granularity'),
    attributes: require('./attributes'),
  }
};
