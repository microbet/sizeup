using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core;
using SizeUp.Core.Web;
using SizeUp.Core.API;
namespace SizeUp.Api.Controllers
{
    public class TokenController : BaseController
    {
        public ActionResult Index()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var token = APIToken.Create(APIContext.Current.ApiToken.APIKeyId);
                var data = token.GetToken();             
                return Json(data, JsonRequestBehavior.AllowGet);         
            }
        }
    }
}
