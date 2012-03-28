using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SizeUp.Data;
using System.Web;

namespace SizeUp.Core.Web
{
    public class WebContext
    {
        public static WebContext Current
        {
            get
            {
                var context = HttpContext.Current.Items["Sizeup.Core.Web.WebContext"] as WebContext;
                if (context == null)
                {
                    context = new WebContext();
                    HttpContext.Current.Items["Sizeup.Core.Web.WebContext"] = context;
                }
                return context;
            }
        }

        public City DetectedCity
        {
            get
            {
                return Geo.GeoCoder.GetPlaceByIPAddress();
            }
        }

        public Industry CurrentIndustry
        {
            get
            {
                var c = System.Web.HttpContext.Current;
                var ind = c.Items["Sizeup.Core.Web.WebContext.CurrentIndustry"] as Industry;
                var cookie = c.Request.Cookies["industry"];
                int? id = cookie == null ? null : int.Parse(cookie.Value) as int?;
                if (ind == null && id.HasValue)
                {
                    ind = DataContexts.SizeUpContext.Industries.Where(i => i.Id == id).FirstOrDefault();
                    c.Items["Sizeup.Core.Web.WebContext.CurrentIndustry"] = ind;
                }
                return ind;
            }
            set
            {
                var c = System.Web.HttpContext.Current;
                if (value != null)
                {
                    HttpCookie cookie = new HttpCookie("industry", value.Id.ToString());
                    cookie.Expires = DateTime.Now.AddDays(7.0);
                    c.Response.Cookies.Add(cookie);
                }
                else
                {
                    HttpCookie cookie = new HttpCookie("industry", "");
                    cookie.Expires = DateTime.MinValue;
                    c.Response.Cookies.Add(cookie);
                }
            }
        }

         public City CurrentCity
         {
             get
             {
                 var c = System.Web.HttpContext.Current;
                 var ind = c.Items["Sizeup.Core.Web.WebContext.CurrentCity"] as City;
                 var cookie = c.Request.Cookies["city"];
                 int? id = cookie == null ? null : int.Parse(cookie.Value) as int?;
                 if (ind == null && id.HasValue)
                 {
                     ind = DataContexts.SizeUpContext.Cities.Where(i => i.Id == id).FirstOrDefault();
                     c.Items["Sizeup.Core.Web.WebContext.CurrentCity"] = ind;
                 }
                 return ind;
             }
             set
             {
                 var c = System.Web.HttpContext.Current;
                 if (value != null)
                 {
                     HttpCookie cookie = new HttpCookie("city", value.Id.ToString());
                     cookie.Expires = DateTime.Now.AddDays(7.0);
                     c.Response.Cookies.Add(cookie);
                 }
                 else
                 {
                     HttpCookie cookie = new HttpCookie("city", "");
                     cookie.Expires = DateTime.MinValue;
                     c.Response.Cookies.Add(cookie);
                 }
             }
         }
    }
}
