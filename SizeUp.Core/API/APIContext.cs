using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using System.Configuration;
using SizeUp.Core.Web;

namespace SizeUp.Core.API
{
    public class APIContext
    {
        public static APIContext Current
        {
            get
            {
                var context = HttpContext.Current.Items["Sizeup.Core.API.APIContext"] as APIContext;
                if (context == null)
                {
                    context = new APIContext();
                    HttpContext.Current.Items["Sizeup.Core.API.APIContext"] = context;
                }
                return context;
            }
        }
        public bool IsJsonp
        {
            get { return HttpContext.Current.Request.QueryString[ConfigurationManager.AppSettings["API.CallbackName"]] != null; }
        }


        public APIToken ApiToken
        {
            get
            {
                var tokenString = HttpContext.Current.Request.QueryString[ConfigurationManager.AppSettings["API.TokenName"]];
                return APIToken.ParseToken(tokenString);
            }
        }

        public APIToken WidgetToken
        {
            get
            {
                var tokenString = HttpContext.Current.Request.QueryString[ConfigurationManager.AppSettings["API.WidgetTokenName"]];
                return APIToken.ParseToken(tokenString);
            }
        }

        public string Origin
        {
            get
            {
                return HttpContext.Current.Request.QueryString[ConfigurationManager.AppSettings["API.OriginName"]];
            }
        }

        public string Instance
        {
            get
            {
                return HttpContext.Current.Request.QueryString[ConfigurationManager.AppSettings["API.InstanceName"]];
            }
        }

        public string Session
        {
            get
            {
                return APISession.Current.SessionId;
            }
        }
    }
}
