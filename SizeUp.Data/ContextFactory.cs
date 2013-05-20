using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.EntityClient;
using System.Configuration;
using SizeUp.Data.Analytics;
using SizeUp.Data.UserData;

namespace SizeUp.Data
{
    public static class ContextFactory
    {
        public static SizeUpContext SizeUpContext
        {
            get
            {
                var conn = new EntityConnection(ConfigurationManager.ConnectionStrings["SizeUpContext"].ConnectionString);
                var context = new SizeUpContext(conn);
                return context;
            }
        }

        public static AnalyticsContext AnalyticsContext
        {
            get
            {
                var conn = new EntityConnection(ConfigurationManager.ConnectionStrings["AnalyticsContext"].ConnectionString);
                var context = new AnalyticsContext(conn);
                return context;
            }
        }

        public static UserDataContext UserDataContext
        {
            get
            {
                var conn = new EntityConnection(ConfigurationManager.ConnectionStrings["UserDataContext"].ConnectionString);
                var context = new UserDataContext(conn);
                return context;
            }
        }
    }
}
