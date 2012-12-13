using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Data.Analytics;
using System.Web;

namespace SizeUp.Core.Diagnostics
{
    public class HandleErrorAndLogAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var e = filterContext.Exception;
            if (e is HttpException && ((HttpException)e).GetHttpCode() != 404)
            {
                SizeUp.Data.Analytics.Exception reg = new SizeUp.Data.Analytics.Exception()
                {

                    RequestUrl = HttpContext.Current.Request.Url.OriginalString,
                    ExceptionText = e.Message,
                    InnerExceptionText = e.InnerException != null ? e.InnerException.Message : null,
                    StackTrace = e.StackTrace
                };
                if (!filterContext.ExceptionHandled)
                {
                    Singleton<Tracker>.Instance.Exception(reg);
                }
            }
            base.OnException(filterContext);
        }
    }

}
