using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using Api = SizeUp.Web.Areas.Api;
namespace SizeUp.Web.Controllers
{
    public class BaseController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            var data = new Models.Header();

            using (var context = new SizeUpContext())
            {
                var info = new Models.CurrentInfo()
                {
                    CurrentCity = context.Cities.Where(i => i.Id == WebContext.Current.CurrentCityId).Select(i => new Api.Models.City.City()
                    {
                        Id = i.Id,
                        County = i.County.Name,
                        Name = i.Name,
                        State = i.State.Abbreviation,
                        SEOKey = i.SEOKey
                    }).FirstOrDefault(),

                    CurrentIndustry = context.Industries.Where(i => i.Id == WebContext.Current.CurrentIndustryId).Select(i => new Api.Models.Industry.Industry()
                    {
                        Id = i.Id,
                        Name = i.Name,
                        SEOKey = i.SEOKey
                    }).FirstOrDefault()
                };

                ViewBag.CurrentInfo = info;
            }

            ViewBag.Header = data;
           


        }
    }
}
