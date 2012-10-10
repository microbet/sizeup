using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.DataAccess;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using SizeUp.Core.Extensions;
using SizeUp.Web.Areas.Api.Models;
using SizeUp.Core;


namespace SizeUp.Web.Areas.Api.Controllers
{
    public class WorkersCompController : BaseController
    {
        //
        // GET: /Api/WorkersComp/

        public ActionResult WorkersComp(long industryId, long placeId, int? employees)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var locations = Locations.Get(context, placeId).FirstOrDefault();


                var s = IndustryData.GetState(context, industryId, locations.State.Id)
                    .Select(i => new Models.WorkersComp.ChartItem()
                    {
                        Rank = i.WorkersCompRank.Value,
                        Average = i.WorkersComp.Value,
                        Name = locations.State.Name
                    });

                var data = new Models.Charts.BarChart()
                {
                    State = s.FirstOrDefault()
                };

                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Percentage(int industryId, long placeId, double value)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var locations = Locations.Get(context, placeId).FirstOrDefault();

                var data = IndustryData.GetState(context, industryId, locations.State.Id)
                    .Select(i => i.WorkersComp.Value)
                    .FirstOrDefault();

                object obj = null;
                if (data != 0)
                {
                    obj = new
                    {
                        Percentage = (int)(((value - data) / data) * 100)
                    };
                }
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
