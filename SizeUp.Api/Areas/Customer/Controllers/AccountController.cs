using SizeUp.Api.Controllers;
using SizeUp.Core.DataLayer;
using SizeUp.Data;
using SizeUp.Data.API;
using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace SizeUp.Api.Areas.Customer.Controllers
{

    public class AccountController : BaseController
    {
        // GET: /customer/get?key=KEY

        protected ActionResult InvalidApikeyArg(string apikey)
        {
            Response.StatusCode = 400;
            return Content("Argument \"apikey\" is missing or misformatted.\nReceived: " + apikey + "\nExpected: A valid GUID, in the format apikey=xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx, where 'x' is a hexadecimal digit.", "text/plain");
        }

        public ActionResult Get(string key)
        {
            Guid _key;
            try
            {
                _key = new Guid(key);
            }
            catch (ArgumentNullException) { return InvalidApikeyArg(key); }
            catch (FormatException) { return InvalidApikeyArg(key); }
            catch (OverflowException) { return InvalidApikeyArg(key); }

            try
            {
                using (var apiContext = ContextFactory.APIContext)
                {
                    using (var sizeupContext = ContextFactory.SizeUpContext)
                    {
                        return Json(
                            SizeUp.Core.DataLayer.Customer.GetCustomerByKey(apiContext, sizeupContext, _key),
                            JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (System.Data.ObjectNotFoundException exc)
            {
                Response.StatusCode = 401;
                return Content("The product key (\"key=" + exc.Message + "\") is invalid. Please see https://api.sizeup.com/documentation/ for help.", "text/plain");
            }
        }

    }
}
