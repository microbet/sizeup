using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Web.Models;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Web.Areas.Api.Models;



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
                ViewBag.Regions = context.Divisions.OrderBy(i => i.Region.Name).ThenBy(i => i.Name).Select(i => new Models.BestPlaces.Division()
                {
                    Id = i.Id,
                    RegionName = i.Region.Name,
                    Name = i.Name
                }).ToList();

                ViewBag.States = context.States.OrderBy(i => i.Name).Select(i => new Models.BestPlaces.State()
                {
                    Id = i.Id,
                    Name = i.Name
                }).ToList();


                return View();
            }
        }

    }
}
