using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SizeUp.Data;
using System.Web;

namespace SizeUp.Core.API
{
    public class APIAuthorize : ActionFilterAttribute
    {
        public string Role { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            using (var context = ContextFactory.APIContext)
            {
                if (APIContext.Current.ApiToken != null)
                {
                    var authorized = context.APIKeyRoleMappings
                        .Where(i => i.APIKeyId == APIContext.Current.ApiToken.APIKeyId)
                        .Where(i => i.Role.Name.ToLower() == Role.ToLower())
                        .Any();
                    if (!authorized)
                    {
                        throw new HttpException(403, "API Call Not Authorized");
                    }
                }
                else
                {
                    throw new HttpException(403, "API Token Not Supplied");
                }


            }
        }
    }
}
