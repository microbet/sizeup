using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SizeUp.Web.Helpers
{
    public static class HtmlExtensions
    {
        public static string DecodeHtmlHelper(this HtmlHelper helper, MvcHtmlString helperToDecode)
        {
            if (helperToDecode != null) return HttpUtility.HtmlDecode(helperToDecode.ToString());

            return string.Empty;
        }
    }
}