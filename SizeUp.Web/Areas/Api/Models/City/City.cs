using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace SizeUp.Web.Areas.Api.Models.City
{
    public class City
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string SEOKey { get; set; }
        public string TypeName { get; set; }
        public Models.Shared.LatLng Centroid { get; set; }
    }
}