using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using System.Configuration;


using SizeUp.Data;
using SizeUp.Data.Analytics;



namespace SizeUp.Core.API
{
    public class APIRequest : BaseAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool valid = false;
            if (IsJsonp)
            {
                Log();
            }
            valid = ValidateToken() && IsJsonp && ApiToken != null;    
            if (!valid)
            {
                throw new HttpException(401, "Api token not valid");
            }
            base.OnActionExecuting(filterContext);
        }

        protected bool ValidateToken()
        {
            var now = DateTime.UtcNow;
            var old = new DateTime(ApiToken.TimeStamp);

            var diff = now - old;
            var minutes = (int)diff.TotalMinutes;
            bool isValid = false;

            if (minutes < int.Parse(ConfigurationManager.AppSettings["Api.TokenExpiration"]))
            {
                if (!HttpContext.Current.Request.IsLocal)
                {
                    using (var context = ContextFactory.SizeUpContext)
                    {
                        isValid = context.APIKeys
                             .Where(i => i.Id == ApiToken.APIKeyId)
                            //.Where(i => i.APIKeyDomains.Any(d => d.Domain == Origin))
                             .Count() > 0;
                    }
                }
                else
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        protected void Log()
        {
            Data.Analytics.APIRequest reg = new Data.Analytics.APIRequest();
            reg.OriginUrl = Origin;
            reg.Session = Session;
            reg.Url = HttpContext.Current.Request.Url.OriginalString;
            reg.APIKeyId = ApiToken!= null ? ApiToken.APIKeyId : (long?)null;
            Singleton<Tracker>.Instance.APIRequest(reg);
        }
    }
}
