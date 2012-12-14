using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace SizeUp.Web.Areas.Api.Models.State
{
    public class State
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string SEOKey { get; set; }
        public Models.Shared.LatLng Centroid { get; set; }
        public Models.Shared.LatLng SouthWest { get; set; }
        public Models.Shared.LatLng NorthEast { get; set; }
    }
}