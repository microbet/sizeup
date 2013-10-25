using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core;
using SizeUp.Data.Analytics;
using SizeUp.Core.API;
using System.Web.Security;
using SizeUp.Core.Analytics;
using System.Drawing;

namespace SizeUp.Web.Areas.Analytics.Controllers
{
    public class TrackerController : Controller
    {

        public ActionResult Index(string t, string u)
        {
            HttpContext.Response.AddHeader("Expires", "-1");
            Guid? userid = null;

            if (User.Identity.IsAuthenticated)
            {
                userid = (Guid)Membership.GetUser().ProviderUserKey;
            }

            PageViewToken token = PageViewToken.ParseToken(t);
            var item = new Data.Analytics.PageView()
            {
               GeographicLocationId = token.GeographicLocationId,
               IndustryId = token.IndustryId,
               Url = Server.UrlDecode(u),
               OriginIP = SizeUp.Core.Web.WebContext.Current.ClientIP,
               UserId = userid
            };

            Singleton<Tracker>.Instance.PageView(item);
            Bitmap bm = new Bitmap(1, 1);
            var stream = new System.IO.MemoryStream();
            bm.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return File(stream.GetBuffer(), "image/png");
        }

       

    }
}
