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
    public class BestIndustriesController : BaseController
    {
        //
        // GET: /Widget/TopPlaces/

        public ActionResult Index()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                return View();
            }
        }

    }
}
