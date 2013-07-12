using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SizeUp.Core.DataLayer.Base
{
    public class Base
    {
        protected static int MinimumBusinessCount
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["Data.MinimumBusinessCount"]);
            }
        }
    }
}
