using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SizeUp.Web.Areas.Api;

namespace SizeUp.Web.Models.Business
{
    public class Locations
    {
        public Areas.Api.Models.City.City City { get; set; }
        public Areas.Api.Models.County.County County { get; set; }
        public Areas.Api.Models.Metro.Metro Metro { get; set; }
        public Areas.Api.Models.State.State State { get; set; }
    }
}