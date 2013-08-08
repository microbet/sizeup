using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Models
{
    public class Sitemap
    {
        public class Business
        {
            public long Id { get; set; }
            public string SEOKey { get; set; }
            public string State { get; set; }
            public string County { get; set; }
            public string City { get; set; }
            public string Industry { get; set; }
        }
    }
}
