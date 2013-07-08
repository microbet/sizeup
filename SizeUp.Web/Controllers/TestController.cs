using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using System.Data.EntityClient;
using System.Configuration;

namespace SizeUp.Web.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /TEst/

        public ActionResult Index()
        {
            var conn = new EntityConnection(ConfigurationManager.ConnectionStrings["TestContext"].ConnectionString);
            var c = new SizeUp.Data.Test.TestContext(conn);
            c.Cities.Where(i => i.Name.Contains("San Fran"))
                .Select(i => i.GeographicLocation.Geographies.Where(g=>g.GeographyClass.Name == "Calculation"))
                .ToList();

            return Json(null, JsonRequestBehavior.AllowGet);

            /*
            using (var context = ContextFactory.SizeUpContext)
            {
                var output = Core.DataLayer.BestPlaces.IndustryRanks(context, 25, 3051, Core.DataLayer.Base.Granularity.City);
                return Json(output, JsonRequestBehavior.AllowGet);
            }
            */
        }

    }
}
