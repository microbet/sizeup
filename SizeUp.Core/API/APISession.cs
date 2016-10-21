using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;


namespace SizeUp.Core.API
{
    public class APISession
    {
        protected APISession() { }
        protected static string SessionCookieKey { get { return "sessionId"; } }

        protected string _sessionid;
        public string SessionId { get { return _sessionid; } }

        public static APISession Current
        {
            get
            {
                var s = new APISession();
                s._sessionid = Load();
                return s;
            }
        }
        public static void Create()
        {
            if (string.IsNullOrEmpty(Current.SessionId))
            {
                var s = new APISession();
                s._sessionid = RandomString.Get(25);
                s.Save();
            }
        }
        
        protected void Save()
        {
            HttpCookie kc = SizeUp.Core.Web.CookieFactory.Create(SessionCookieKey);
            kc.Value = _sessionid;
            kc.Expires = DateTime.Now.AddYears(1);
            HttpContext.Current.Response.Cookies.Add(kc);
        }

        protected static string Load()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[SessionCookieKey];
            var token = cookie != null ? cookie.Value : "";
            return token;
        }        
    }
}
