using System.Web;
using System.Web.Mvc;
using SizeUp.Core.Diagnostics;

namespace SizeUp.Api
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAndLogAttribute());
        }
    }
}