using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SizeUp.Web.Areas.Tiles.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Tiles/Base/
        private int ZoomFilterBase = 14;
        protected double GetPolygonTolerance(int zoom)
        {
            return System.Math.Pow(2, ZoomFilterBase - zoom);
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }
    }
}
