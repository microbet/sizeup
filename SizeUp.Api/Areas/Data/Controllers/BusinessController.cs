using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using System.Linq.Expressions;
using System.Data.Objects.SqlClient;
using System.Data.Spatial;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using SizeUp.Core.DataLayer;


using SizeUp.Api.Controllers;
namespace SizeUp.Api.Areas.Data.Controllers
{
    public class BusinessController : BaseController
    {
        //
        // GET: /Api/Business/

        public ActionResult Index(long id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Business.Get(context, id);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult At(List<long> industryIds, float lat, float lng)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Business.GetAt(context, new LatLng() { Lat = lat, Lng = lng }, industryIds);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult List(List<long> industryIds, long placeId, int itemCount, int page = 1, int radius = 100)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var centroid = Core.DataLayer.Geography.Centroid(context, Core.DataLayer.Base.Granularity.Place).Where(i => i.Key == placeId).Select(i => i.Value).FirstOrDefault();

                var data = Core.DataLayer.Business.ListNear(context, centroid, industryIds)
                    .Where(i => i.Distance < radius)
                    .OrderBy(i => i.Distance)
                    .ThenBy(i => i.Entity.Name)
                    .Select(i => i.Entity);

                var output = new
                {
                    Page = page,
                    Count = data.Count(),
                    Items = data.Skip((page-1) * itemCount).Take(itemCount).ToList()
                };

                return Json(output, JsonRequestBehavior.AllowGet);
            }
           
        }

       

    }
}
