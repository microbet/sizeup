using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SizeUp.Web.Areas.Api.Models;

namespace SizeUp.Web.Areas.Api.Models.Place
{
    public class Place
    {
        public long Id { get; set; }
        public string DisplayName { get; set; }
        public City.City City { get; set; }
        public County.County County { get; set; }
        public Metro.Metro Metro { get; set; }
        public State.State State { get; set; }
    }
}