using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace SizeUp.Web.Areas.Api.Models.County
{
    public class County
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string SEOKey { get; set; }
        public Models.Shared.LatLng Centroid { get; set; }
    }
}