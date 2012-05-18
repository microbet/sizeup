using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SizeUp.Web.Areas.Api;

namespace SizeUp.Web.Models.Business
{
    public class Report
    {
        public IndustryDetails IndustryDetails { get; set; }
        public Locations Locations { get; set; }
        public Areas.Api.Models.Maps.LatLng MapCenter { get; set; }
    }
}