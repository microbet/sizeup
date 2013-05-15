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
        [APIRequest]
        public ActionResult Index(long id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Industry.Get(context, id);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        [APIRequest]
        public ActionResult List(List<long> ids)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Industry.List(context, ids);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [APIRequest]
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
