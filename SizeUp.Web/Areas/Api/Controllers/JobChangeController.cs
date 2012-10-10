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
    public class JobChangeController : BaseController
    {
        //
        // GET: /Api/JobChange/

        public ActionResult JobChange(int industryId, int placeId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var locations = Locations.Get(context, placeId).FirstOrDefault();
                IQueryable<Models.JobChange.ChartItem> m = null;
                var n = IndustryData.GetNational(context, industryId)
                    .Select(i => new Models.JobChange.ChartItem()
                    {
                        NetJobChange = i.NetJobChange,
                        JobGains = i.JobGains,
                        JobLosses = i.JobLosses,
                        Name = "USA"
                    });

                var s = IndustryData.GetState(context, industryId, locations.State.Id)
                     .Select(i => new Models.JobChange.ChartItem()
                    {
                        NetJobChange = i.NetJobChange,
                        JobGains = i.JobGains,
                        JobLosses = i.JobLosses,
                        Name = locations.State.Name
                    });

                if (locations.Metro != null)
                {

                    m = IndustryData.GetMetro(context, industryId, locations.Metro.Id)
                        .Select(i => new Models.JobChange.ChartItem()
                        {
                            NetJobChange = i.NetJobChange,
                            JobGains = i.JobGains,
                            JobLosses = i.JobLosses,
                            Name = locations.Metro.Name
                        });
                }

                var co = IndustryData.GetCounty(context, industryId, locations.County.Id)
                   .Select(i => new Models.JobChange.ChartItem()
                   {
                       NetJobChange = i.NetJobChange,
                       JobGains = i.JobGains,
                       JobLosses = i.JobLosses,
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
    }
}
