using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using System.Configuration;
using SizeUp.Core.Web;

namespace SizeUp.Core.API
{
    public class AllowAPIRequest : ActionFilterAttribute
    {
        
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (IsJsonp)
            {

                if (filterContext.Result is JsonResult)
                {
                    JsonResult r = filterContext.Result as JsonResult;
                    JsonpResult result = new JsonpResult();
                    result.Data = r.Data;
                    result.JsonRequestBehavior = r.JsonRequestBehavior;
                    filterContext.Result = result;
                }
            }
            base.OnActionExecuted(filterContext);
        }

        protected bool IsJsonp
        {
            get { return HttpContext.Current.Request.QueryString[ConfigurationManager.AppSettings["API.CallbackName"]] != null; }
        }
    }
}
