using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.EntityClient;
using System.Configuration;

namespace SizeUp.Data
{
    public static class ContextFactory
    {
        private static string SizeUpConnection = ConfigurationManager.ConnectionStrings["SizeUpContext"].ConnectionString;
        public static SizeUpContext SizeUpContext
        {
            get
            {
                var conn = new EntityConnection(SizeUpConnection);
                var context = new SizeUpContext(conn);   
                return context;
            }
        }
    }
}
