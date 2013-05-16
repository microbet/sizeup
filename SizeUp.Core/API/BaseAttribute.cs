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
    public class BaseAttribute : ActionFilterAttribute
    {
        protected bool IsJsonp
        {
            get { return HttpContext.Current.Request.QueryString[ConfigurationManager.AppSettings["API.CallbackName"]] != null; }
        }


        protected APIToken ApiToken
        {
            get
            {
                var tokenString = HttpContext.Current.Request.QueryString[ConfigurationManager.AppSettings["API.TokenName"]];
                return APIToken.GetToken(tokenString);
            }
        }

        protected string Origin
        {
            get
            {
                return HttpContext.Current.Request.QueryString[ConfigurationManager.AppSettings["API.OriginName"]];
            }
        }

        protected string Session
        {
            get
            {
                return HttpContext.Current.Request.QueryString[ConfigurationManager.AppSettings["API.SessionName"]];
            }
        }
    }
}
