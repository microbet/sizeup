using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;

namespace SizeUp.Web.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /TEst/

        public ActionResult Index()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var output = Core.DataLayer.BestPlaces.IndustryRanks(context, 25, 3051, Core.DataLayer.Base.Granularity.City);
                return Json(output, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
