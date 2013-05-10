using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using System.Configuration;
using SizeUp.Core;
using SizeUp.Core.Web;
using SizeUp.Data;
using SizeUp.Data.Analytics;

namespace SizeUp.Core.API
{
    public class LogAPIRequest : BaseAttribute
    {

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (IsJsonp)
            {
                Log();
            }
            base.OnResultExecuted(filterContext);
        }

        protected void Log()
        {
            APIRequest reg = new APIRequest();
            reg.OriginUrl = Origin;
            reg.Session = Session;
            reg.Url = HttpContext.Current.Request.Url.OriginalString;
            reg.APIKeyId = ApiToken.APIKeyId; 
            Singleton<Tracker>.Instance.APIRequest(reg);
        }
    }
}
