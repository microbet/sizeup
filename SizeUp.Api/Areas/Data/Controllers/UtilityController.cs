using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using SizeUp.Core.Extensions;
using SizeUp.Core.DataLayer;
using SizeUp.Api.Controllers;
using SizeUp.Core.API;
using SizeUp.Core.Tiles;

namespace SizeUp.Api.Areas.Data.Controllers
{
    public class UtilityController : BaseController
    {
        //
        // GET: /Api/Geography/

        //[APIAuthorize(Role = "Place")]
        public ActionResult ColorBands(string startColor, string endColor, int bands)
        {
            ColorBands b = new Core.Tiles.ColorBands(System.Drawing.ColorTranslator.FromHtml(startColor), System.Drawing.ColorTranslator.FromHtml(endColor), bands);
            string[] bandList = b.GetColorBands().ToArray();
            return Json(bandList, JsonRequestBehavior.AllowGet);
        }
    }
}
