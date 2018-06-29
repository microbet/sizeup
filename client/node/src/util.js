
exports.mergeParams = function (primary, secondary) {
  var obj = {};
  primary = primary || {};
  secondary = secondary || {};
  for (var x in primary) {
    if (primary.hasOwnProperty(x)) {
      obj[x] = primary[x];
    }
  }

  for (var x in secondary) {
    if (secondary.hasOwnProperty(x)) {
      obj[x] = secondary[x];
    }
  }
  return obj;
};

exports.formatParams = function (params) {
  var output = '';
  var formattedParams = [];
  if (params != null) {
    for (var x in params) {
      if (params.hasOwnProperty(x)) {
        if (Object.prototype.toString.call(params[x]) === '[object Array]') {
          for (var y = 0; y < params[x].length; y++) {
            formattedParams.push(x + '=' + encodeURIComponent(params[x][y]));
          }
        }
        else if (params[x] && params[x]._type && params[x]._type() == 'sizeup.api.range') {
          var p = [params[x].min(), params[x].max()];
          for (var y = 0; y < p.length; y++) {
            formattedParams.push(x + '=' + encodeURIComponent(p[y]));
          }
        }
        else {
          formattedParams.push(x + '=' + encodeURIComponent(params[x]));
        }
      }
    }
    if (formattedParams.length > 0) {
      output = formattedParams.join('&');
    }
  }
  return output;
};

exports.wrapAsArray = function (val) {
  if( Object.prototype.toString.call( val ) === '[object Array]' ) {
    return val;
  }
  else{
    return [val];
  }
};
