using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Models;

namespace SizeUp.Core.DataLayer
{
    public static class CommonFilters
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

        public static int MinimumBusinessCount
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["Data.MinimumBusinessCount"]);
            }
        }
    }
}
