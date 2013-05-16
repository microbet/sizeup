﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using SizeUp.Core.Extensions;

using SizeUp.Core.DataLayer.Base;
using SizeUp.Core.DataLayer;
using SizeUp.Core.API;
using SizeUp.Api.Controllers;
namespace SizeUp.Api.Areas.Data.Controllers
{
    public class DemographicsController : BaseController
    {
        //
        // GET: /Api/Demographics/

        public ActionResult Index(long id, Granularity granularity )
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Demographics.Get(context, id, granularity);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
