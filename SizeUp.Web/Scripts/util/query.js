var exported_objects = {
  
  keys: {
    compoundPlaceKey: function (place) {
      return {
        state: place.State.SEOKey,
        county: place.County.SEOKey,
        city: place.City.SEOKey
      };
    }
  },

  http: {

    flatten: function(obj, result=null, prefix="") {
      result = result || {};
      for (var key in obj) {
        if (typeof(obj[key]) == "object") {
          flatten(obj[key], result, prefix + key + "."); // recursive
        } else {
          result[prefix + key] = obj[key]; // base case
        }
      }
      return result;
    },

    // BROWSER ONLY: fills out an HTMLFormElement with the report's parameters,
    // by adding flat fields to the query string (for HTTP GET).
    getReport: function (form, report) {
      $(form).find("input").remove();
      $(form).attr("method", "get");
      var flat = flatten(report);
      for (var i in flat) {
        var input = $("<input type='hidden'>").attr("name", i).attr("value", escape(flat[i]));
        $(form).append(input);
      }
    },

    // TODO(twilson) - not functional yet.
    // BROWSER ONLY: fills out an HTMLFormElement with the report's parameters, by
    // setting HTTP headers and submitting the query in the request body (for HTTP POST).
    postReport: function (form, report) {
      $(form).find("input").remove();
      $(form).attr("method", "post");
      // TODO...
    }
  }
}

// Export objects: first to sizeup.core.namespace system, designed for
// browsers. If that doesn't exist, presume we're in ES5 Node environment.
if (typeof sizeup !== "undefined" && typeof sizeup.core !== "undefined") {
    sizeup.core.namespace("sizeup.util.query");
    sizeup.util.query = exported_objects;
} else {
    module.exports = exported_objects;
}
