using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.API;

namespace SizeUp.Web.Areas.Widget.Controllers
{
    public class BaseController : SizeUp.Web.Controllers.BaseController
    {
        //
        // GET: /Widget/Base/

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            var c = requestContext.HttpContext.Request.Cookies["theme"];
            var theme = "";
            if (c != null)
            {
                theme = c.Value;
            }
            ViewBag.Theme = theme.ToLower();
            ViewBag.WidgetToken = "";

            if (APIContext.Current.WidgetToken != null)
            {
                using (var context = ContextFactory.APIContext)
                {
                    var currentKey = APIContext.Current.WidgetToken.APIKeyId;
                    var api = context.APIKeys.Where(i => i.Id == currentKey).Select(i => i.Name).FirstOrDefault();
                    ViewBag.APIName = api;
                }
                ViewBag.WidgetToken = HttpUtility.UrlEncode(APIContext.Current.WidgetToken.GetToken());
            }
            // if (Core.API.APIContext.Current.ApiToken != null && !Core.API.APIContext.Current.ApiToken.IsValid)
            // {
            //throw new HttpException(403, "Invalid API Key");
            // }
            base.Initialize(requestContext);
        }

        public Core.DataLayer.Models.Customer GetCustomer(SizeUp.Data.API.APIContext context, Guid key)
        {
            Core.DataLayer.Models.Customer customer = null;
            using (var sizeupContext = ContextFactory.SizeUpContext)
            {
                try
                {
                    customer = SizeUp.Core.DataLayer.Customer.GetCustomerByKey(context, sizeupContext, key);
                }
                catch (System.Data.ObjectNotFoundException exc)
                {
                    // This is actually an error, but the error is a real possibility and I don't
                    // want it to abort the function. An entire API refactor is planned, which will
                    // eventually remove the possibility of failure here.
                    // TODO: if we get a logging framework, log the error.
                }
            }
            return customer;
        }
    }
}
