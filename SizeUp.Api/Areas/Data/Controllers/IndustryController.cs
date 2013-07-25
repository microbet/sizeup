using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.DataLayer;
using SizeUp.Core.API;
using SizeUp.Api.Controllers;
namespace SizeUp.Api.Areas.Data.Controllers
{
    public class IndustryController : BaseController
    {
        //
        // GET: /Api/Industry/
        
        [APIAuthorize(Role = "Industry")]
        public ActionResult Index(List<long> id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Industry.Get(context, id).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        
        [APIAuthorize(Role = "Industry")]
        public ActionResult Search(string term, int maxResults = 35)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Industry.Search(context, term).Take(maxResults).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
