using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Web.Models;
using SizeUp.Data;
using SizeUp.Core.Web;




namespace SizeUp.Web.Controllers
{
    public class BestPlacesController : BaseController
    {
        //
        // GET: /Competition/

        public ActionResult Index(string industry)
        {
            if (CurrentInfo.CurrentIndustry == null)
            {
                throw new HttpException(404, "Page Not Found");
            }
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Regions = Core.DataLayer.Place.List(context).Select(i => i.Region).Distinct().ToList();
                ViewBag.States = Core.DataLayer.Place.List(context).Select(i => i.State).Distinct().OrderBy(i => i.Name).ToList();
                return View();
            }
        }

        public ActionResult PickIndustry()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                return View();
            }
        }

    }
}
