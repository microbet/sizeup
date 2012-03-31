using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using SizeUp.Data.Analytics;


namespace SizeUp.Data
{
    public static class DataContexts
    {


        public static SizeUpContext SizeUpContext
        {
            get
            {
                SizeUpContext context;
                if (HttpContext.Current.Items["SizeUp.Data.Context.SizeUpDataContext"] != null)
                {
                    context = HttpContext.Current.Items["SizeUp.Data.Context.SizeUpDataContext"] as SizeUpContext;
                }
                else
                {
                    context = new SizeUpContext();
                    HttpContext.Current.Items["SizeUp.Data.Context.SizeUpDataContext"] = context;
                }
                return context;
            }
        }

        public static AnalyticsContext AnalyticsContext
        {
            get
            {
                AnalyticsContext context;
                if (HttpContext.Current.Items["SizeUp.Data.Context.AnalyticsDataContext"] != null)
                {
                    context = HttpContext.Current.Items["SizeUp.Data.Context.AnalyticsDataContext"] as AnalyticsContext;
                }
                else
                {
                    context = new AnalyticsContext();
                    HttpContext.Current.Items["SizeUp.Data.Context.AnalyticsDataContext"] = context;
                }
                return context;
            }
        }
    }
}
