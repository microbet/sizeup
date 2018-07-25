// Derived from the sizeup.customer portion of api.sizeup.com/js/data

module.exports = function makeCustomerApi(getData) {
  return {
    getCustomerByKey: function(params, success, error) {
      if (typeof(params) == "string") { params = {"key": params}; }
      var url = '/customer/account/get/';
      return getData(url, params, success, error);
    }
  };
}
