using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SizeUp.Data
{
    public static class TimeSlice
    {
        public static int Year
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["TimeSlice.Year"]);
            }
        }

        public static int Quarter
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["TimeSlice.Quarter"]);
            }
        }
    }
}
