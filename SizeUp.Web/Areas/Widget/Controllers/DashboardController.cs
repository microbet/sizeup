using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Web.Controllers;
using SizeUp.Web.Models;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Web.Areas.Api.Models;
using Microsoft.SqlServer.Types;
namespace SizeUp.Web.Areas.Widget.Controllers
{
    public class DashboardController : BaseController
    {
        //
        // GET: /Wiget/Dashboard/

        public ActionResult Index()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Strings = context.ResourceStrings.Where(i => i.Name.StartsWith("Dashboard")).ToDictionary(i => i.Name, i => i.Value);
                var ind = context.Industries.Where(i => i.Id == CurrentInfo.CurrentIndustry.Id)
                    .Select(i => new
                    {
                        Industry = i,
                        NAICS6 = context.SicToNAICSMappings.Where(m => m.IndustryId == i.Id).Select(m => m.NAICS).FirstOrDefault(),
                        NAICS4 = context.NAICS.Where(n => n.NAICSCode == context.SicToNAICSMappings
                            .Where(m => m.IndustryId == i.Id)
                            .Select(m => m.NAICS)
                            .FirstOrDefault()
                            .NAICSCode.Substring(0, 4)
                       ).FirstOrDefault()
                    })
                    .FirstOrDefault();


                ViewBag.Report = new Models.Business.Report()
                {
                    //CurrentPlace = CurrentInfo.CurrentPlace,
                    IndustryDetails = new Models.Business.IndustryDetails()
                    {
                        Industry = new Areas.Api.Models.Industry.Industry()
                        {
                            Id = ind.Industry.Id,
                            Name = ind.Industry.Name,
                            SEOKey = ind.Industry.SEOKey
                        },
                        NAICS6 = new Areas.Api.Models.NAICS.NAICS()
                        {
                            Id = ind.NAICS6.Id,
                            NAICSCode = ind.NAICS6.NAICSCode,
                            Name = ind.NAICS6.Name
                        },
                        NAICS4 = new Areas.Api.Models.NAICS.NAICS()
                        {
                            Id = ind.NAICS4.Id,
                            NAICSCode = ind.NAICS4.NAICSCode,
                            Name = ind.NAICS4.Name
                        }
                    }
                };

                ViewBag.ReportJSON = SizeUp.Core.Serialization.Serializer.ToJSON(ViewBag.Report);
                ViewBag.Header.ActiveTab = NavItems.Dashboard;
                return View();
            }
        }
    }
}
