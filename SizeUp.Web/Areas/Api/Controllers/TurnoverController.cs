using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.Extensions;
using SizeUp.Web.Areas.Api.Models;
using SizeUp.Core;
using SizeUp.Core.DataAccess;

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class TurnoverController : Controller
    {
        //
        // GET: /Api/Turnover/
        public ActionResult Turnover(long industryId, long placeId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var locations = Locations.Get(context, placeId).FirstOrDefault();
                IQueryable<Models.Turnover.ChartItem> m = null;
                var n = IndustryData.GetNational(context, industryId)
                    .Select(i => new Models.Turnover.ChartItem()
                    {
                        Hires = i.Hires,
                        Separations = i.Separations,
                        Turnover = i.TurnoverRate * 100,
                        Name = "USA"
                    });


                var s = IndustryData.GetState(context, industryId, locations.State.Id)
                    .Select(i => new Models.Turnover.ChartItem()
                    {
                        Hires = i.Hires,
                        Separations = i.Separations,
                        Turnover = i.TurnoverRate * 100,
                        Name = locations.State.Name
                    });

                if (locations.Metro != null)
                {
                    m = IndustryData.GetMetro(context, industryId, locations.Metro.Id)
                         .Select(i => new Models.Turnover.ChartItem()
                         {
                             Hires = i.Hires,
                             Separations = i.Separations,
                             Turnover = i.TurnoverRate * 100,
                             Name = locations.Metro.Name
                         });
                }

                var co = IndustryData.GetCounty(context, industryId, locations.County.Id)
                   .Select(i => new Models.Turnover.ChartItem()
                   {
                       Hires = i.Hires,
                       Separations = i.Separations,
                       Turnover = i.TurnoverRate * 100,
                       Name = locations.County.Name + ", " + locations.State.Abbreviation
                   });

                var data = new Models.Charts.BarChart()
                {
                    Nation = n.FirstOrDefault(),
                    State = s.FirstOrDefault(),
                    Metro = m == null ? null : m.FirstOrDefault(),
                    County = co.FirstOrDefault()
                };


                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Percentile(long industryId, int placeId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var locations = Locations.Get(context, placeId).FirstOrDefault();


                var values = IndustryData.GetCounties(context, industryId)
                    .Select(i => i.TurnoverRate);

                var value = IndustryData.GetCounty(context, industryId, locations.County.Id)
                   .Select(i => i.TurnoverRate)
                   .FirstOrDefault();

                var obj = new
                {
                    Percentile = Core.DataAccess.Math.Percentile(values, (double)value, Core.DataAccess.Math.Order.GreaterThan)
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
