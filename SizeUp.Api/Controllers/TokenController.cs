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
        public ActionResult Index(Guid apikey)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                object data = null;         
                var key = context.APIKeys.Where(i => i.KeyValue == apikey).FirstOrDefault();
                var token = APIToken.Create(key.Id);
                data = token.GetToken();
                
                return Json(data, JsonRequestBehavior.AllowGet);         
            }
        }
    }
}
