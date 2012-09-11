using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SizeUp.Web.Models.Accessibility
{
    public class Table
    {
        public int Year { get; set; }
        public int City { get; set; }
        public int County { get; set; }
        public int? Metro { get; set; }
        public int State { get; set; }
        public int Nation { get; set; }
    }
}