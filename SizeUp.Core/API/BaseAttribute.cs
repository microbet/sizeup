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

        private APIToken _token = null;
        protected APIToken ApiToken
        {
            get
            {
                if (_token == null)
                {
                    var tokenString = HttpContext.Current.Request.QueryString[ConfigurationManager.AppSettings["API.TokenName"]];
                    _token = APIToken.GetToken(tokenString);
                }
                return _token;
            }
        }

        protected string Origin
        {
            get
            {
                return HttpContext.Current.Request.QueryString[ConfigurationManager.AppSettings["API.OriginName"]];
            }
        }
    }
}
