using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Core.API;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SizeUp.Data;
using System.Web.Http;
using System.Net;
using System.Net.Http;

namespace SizeUp.Api.Controllers
{
    public class JSController : Controller
    {
        //
        // GET: /JS/

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            requestContext.HttpContext.Response.AddHeader("Expires", "-1");
            Response.ContentType = "text/javascript";
            APISession.Create();       
        }

        protected ActionResult InvalidApikeyArg(string apikey)
        {
            Response.StatusCode = 400;
            return Content("Argument \"apikey\" is missing or misformatted.\nReceived: " + apikey + "\nExpected: A valid GUID, in the format apikey=xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx, where 'x' is a hexadecimal digit.", "text/plain");
        }

        public ActionResult Index(String apikey, string wt = "")
        {
            Guid apikeyGuid;
            try
            {
                apikeyGuid = new Guid(apikey);
            }
            catch (ArgumentNullException) { return InvalidApikeyArg(apikey); }
            catch (FormatException) { return InvalidApikeyArg(apikey); }
            catch (OverflowException) { return InvalidApikeyArg(apikey); }

            APIToken token = null;
            APIToken widgetToken = null;
            Core.DataLayer.Models.Customer customer = null;

            if (!string.IsNullOrWhiteSpace(wt))
            {
                widgetToken = APIToken.ParseToken(wt);
            }
            using (var context = ContextFactory.APIContext)
            {
                var k = context.APIKeys.Where(i => i.KeyValue == apikeyGuid && i.IsActive).FirstOrDefault();
                if (k == null)
                {
                    Response.StatusCode = 401;
                    return Content("The product key (\"apikey\") is invalid. Please see https://www.sizeup.com/developers/documentation for help.", "text/plain");
                    // It would be nice to do this instead, but first we need to plug in a page somewhere to
                    // render the reason. Else you get an opaque HTTP 500 error.
                    // throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized) {
                    //     ReasonPhrase = "Product key not recognized."
                    // });
                }
                token = APIToken.Create(k.Id);

                using (var sizeupContext = ContextFactory.SizeUpContext) {
                    try
                    {
                        customer = SizeUp.Core.DataLayer.Customer.GetCustomerByKey(context, sizeupContext, apikeyGuid);
                    }
                    catch (System.Data.ObjectNotFoundException exc)
                    {
                        // This is actually an error, but the error is a real possibility and I don't
                        // want it to abort the function. An entire API refactor is planned, which will
                        // eventually remove the possibility of failure here.
                        // TODO: if we get a logging framework, log the error.
                    }
                }

            }

            ViewBag.Customer = customer;
            ViewBag.Token = token.GetToken();
            ViewBag.SessionId = APISession.Current.SessionId;
            ViewBag.InstanceId = RandomString.Get(25);
            ViewBag.WidgetToken = widgetToken != null ? widgetToken.GetToken() : "";
            return View();
        }

        public ActionResult Data()
        {
            if (APIContext.Current.ApiToken.IsValid && !APIContext.Current.ApiToken.IsExpired)
            {
                ViewBag.Permissions = APIContext.Current.APIPermissions;
            }
            else
            {
                throw new HttpException(403, "Invalid API Key");
            }
            return View();
        }

        public ActionResult Range()
        {
            return View();
        }

        public ActionResult Granularity()
        {
            return View();
        }

        public ActionResult Overlay()
        {
            return View();
        }

        public ActionResult Attributes()
        {
            return View();
        }

        public ActionResult OverlayAttributes()
        {
            if (APIContext.Current.ApiToken.IsValid && !APIContext.Current.ApiToken.IsExpired)
            {
                ViewBag.Permissions = APIContext.Current.APIPermissions;
            }
            else
            {
                throw new HttpException(403, "Invalid API Key");
            }  
            return View();
        }



    }
}
