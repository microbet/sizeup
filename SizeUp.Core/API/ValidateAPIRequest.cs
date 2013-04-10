using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using System.Configuration;
using SizeUp.Core.Web;
using SizeUp.Data;
namespace SizeUp.Core.API
{
    public class ValidateAPIRequest : BaseAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (IsJsonp && ApiToken != null)
            {
                if (!ValidateToken())
                {
                    throw new HttpException(401, "Api token not valid");
                }
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
                             .Where(i => i.APIKeyDomains.Any(d => d.Domain == Origin))
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
    }
}
