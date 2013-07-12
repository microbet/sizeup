using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SizeUp.Core.DataLayer.Base
{
    public static class TimeSlice
    {
        public static class Demographics
        {
            public static int Year { get { return int.Parse(ConfigurationManager.AppSettings["TimeSlice.Demographics.Year"]); } }
            public static int Quarter { get { return int.Parse(ConfigurationManager.AppSettings["TimeSlice.Demographics.Quarter"]); } }
        }

        public static class Industry
        {
            public static int Year { get { return int.Parse(ConfigurationManager.AppSettings["TimeSlice.Industry.Year"]); } }
            public static int Quarter { get { return int.Parse(ConfigurationManager.AppSettings["TimeSlice.Industry.Quarter"]); } }
        }

        public static class ConsumerExpenditures
        {
            public static int Year { get { return int.Parse(ConfigurationManager.AppSettings["TimeSlice.ConsumerExpenditures.Year"]); } }
            public static int Quarter { get { return int.Parse(ConfigurationManager.AppSettings["TimeSlice.ConsumerExpenditures.Quarter"]); } }
        }
    }
}
