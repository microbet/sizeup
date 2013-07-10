using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.Serialization;
using SizeUp.Core.DataLayer;

namespace SizeUp.Web.Controllers
{
    public class BaseController : Controller
    {
        protected Core.DataLayer.Models.CurrentInfo CurrentInfo { get; set; }
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            var data = new Models.Header();

            CurrentInfo = new Core.DataLayer.Models.CurrentInfo();
            CurrentInfo.CurrentIndustry = WebContext.Current.CurrentIndustry;
            CurrentInfo.CurrentPlace = WebContext.Current.CurrentPlace;
            ViewBag.CurrentInfo = CurrentInfo;
            ViewBag.CurrentInfoJSON = Serializer.ToJSON(CurrentInfo);
            ViewBag.Header = data;
               
        }
    }      
}
