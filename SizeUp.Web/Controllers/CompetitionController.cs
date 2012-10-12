using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Web.Models;
using SizeUp.Core.Serialization;
using SizeUp.Core.Web;
using SizeUp.Data;
using Api = SizeUp.Web.Areas.Api;
namespace SizeUp.Web.Controllers
{
    public class CompetitionController : BaseController
    {
        //
        // GET: /Competition/

        public ActionResult Index()
        {
            ViewBag.Header.ActiveTab = NavItems.Competition;
            return View();
        }

    }
}
