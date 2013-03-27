using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SizeUp.Core.Web
{
    public class JsonpResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;
            string jsoncallback = request.QueryString["jsoncallback"];
            if (!string.IsNullOrEmpty(jsoncallback))
            {
                if (string.IsNullOrEmpty(base.ContentType))
                {
                    base.ContentType = "application/x-javascript";
                }
                response.Write(string.Format("{0}(", jsoncallback));
            }
            base.ExecuteResult(context);
            if (!string.IsNullOrEmpty(jsoncallback))
            {
                response.Write(")");
            }
        }
    }

    public static class ContollerExtensions
    {
        public static bool IsJsonp(this Controller controller)
        {
            return controller.Request.QueryString["jsoncallback"] != null;
        }

        public static JsonpResult Jsonp(this Controller controller, object data)
        {
            JsonpResult result = new JsonpResult();
            result.Data = data;
            result.ExecuteResult(controller.ControllerContext);
            return result;
        }

        public static JsonpResult Jsonp(this Controller controller, object data, JsonRequestBehavior behavior)
        {
            JsonpResult result = new JsonpResult();
            result.Data = data;
            result.JsonRequestBehavior = behavior;
            //result.ExecuteResult(controller.ControllerContext);
            return result;
        }
    }
}
