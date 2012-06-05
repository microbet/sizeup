using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SizeUp.Data;
using System.Web;
using SizeUp.Core.Identity;

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

        public long? CurrentIndustryId
        {
            get
            {
                {
                    var c = System.Web.HttpContext.Current;
                    var cookie = c.Request.Cookies["industry"];
                    long? id = cookie == null ? null : long.Parse(cookie.Value) as long?;
                    return id;
                }
            }
            set
            {
                var c = System.Web.HttpContext.Current;
                if (value != null)
                {
                    HttpCookie cookie = new HttpCookie("industry", value.ToString());
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

         public long? CurrentCityId
         {
             get
             {
                 var c = System.Web.HttpContext.Current;
                 var cookie = c.Request.Cookies["city"];
                 long? id = cookie == null ? null : long.Parse(cookie.Value) as long?;
                 return id;
             }
             set
             {
                 var c = System.Web.HttpContext.Current;
                 if (value != null)
                 {
                     HttpCookie cookie = new HttpCookie("city", value.ToString());
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
