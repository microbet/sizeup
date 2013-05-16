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

        private long? _currentCityId = null;
        private long? _currentIndustryId = null;
        private Feature? _startFeature = null;


        public long? CurrentIndustryId
        {
            get
            {
                if(_currentIndustryId==null)
                {
                    var c = System.Web.HttpContext.Current;
                    var cookie = c.Request.Cookies["industry"];
                    _currentIndustryId = cookie == null ? null : long.Parse(cookie.Value) as long?;
                }
                return _currentIndustryId;
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
                _currentIndustryId = value;
            }
        }

        public long? CurrentPlaceId
        {
            get
            {
                if (_currentCityId == null)
                {
                    var c = System.Web.HttpContext.Current;
                    var cookie = c.Request.Cookies["city"];
                    _currentCityId = cookie == null ? null : long.Parse(cookie.Value) as long?;
                }
                return _currentCityId;
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
                _currentCityId = value;
            }
        }


        public Feature? StartFeature
        {
            get
            {
                if (_startFeature == null)
                {
                    var c = System.Web.HttpContext.Current;
                    var cookie = c.Request.Cookies["startFeature"];
                    _startFeature = null;
                    if (cookie != null)
                    {
                        Feature f;
                        if (Enum.TryParse<Feature>(cookie.Value, out f))
                        {
                            _startFeature = f;
                        }
                    }
                }
                return _startFeature;
            }
            set
            {
                var c = System.Web.HttpContext.Current;
                if (value != null)
                {
                    HttpCookie cookie = new HttpCookie("startFeature", Enum.GetName(typeof(Feature), value));
                    cookie.Expires = DateTime.Now.AddDays(7.0);
                    c.Response.Cookies.Add(cookie);
                }
                else
                {
                    HttpCookie cookie = new HttpCookie("startFeature", "");
                    cookie.Expires = DateTime.MinValue;
                    c.Response.Cookies.Add(cookie);
                    _startFeature = null;
                }
                _startFeature = value;
            }
        }

        public string Domain
        {
            get
            {
                var host = HttpContext.Current.Request.Url.Host;
                return host.StartsWith("www.", StringComparison.CurrentCultureIgnoreCase) ? host.Replace("www.", "") : host;
            }
        }
    }
}
