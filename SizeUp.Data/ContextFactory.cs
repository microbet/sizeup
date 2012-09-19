using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.EntityClient;
using System.Configuration;
using SizeUp.Data.Analytics;

namespace SizeUp.Data
{
    public static class ContextFactory
    {
        private static string SizeUpConnection = ConfigurationManager.ConnectionStrings["SizeUpContext"].ConnectionString;
        private static string AnalyticsConnection = ConfigurationManager.ConnectionStrings["AnalyticsContext"].ConnectionString;
        public static SizeUpContext SizeUpContext
        {
            get
            {
                var conn = new EntityConnection(SizeUpConnection);
                var context = new SizeUpContext(conn);
                return context;
            }
        }

        public static AnalyticsContext AnalyticsContext
        {
            get
            {
                var conn = new EntityConnection(AnalyticsConnection);
                var context = new AnalyticsContext(conn);
                return context;
            }
        }
    }
}
