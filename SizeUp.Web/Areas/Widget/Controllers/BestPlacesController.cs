using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Web.Models;
using SizeUp.Data;
using SizeUp.Core.Web;


namespace SizeUp.Web.Areas.Widget.Controllers
{
    public class BestPlacesController : BaseController
    {
        //
        // GET: /Widget/TopPlaces/

        public ActionResult Index()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Regions = Core.DataLayer.Division.Get(context).ToList();
                ViewBag.States = Core.DataLayer.State.Get(context).OrderBy(i => i.Name).ToList();
                return View();
            }
        }

    }
}
