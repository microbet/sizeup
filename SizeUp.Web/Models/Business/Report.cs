using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SizeUp.Web.Areas.Api;
using SizeUp.Web.Areas.Api.Models.Place;

namespace SizeUp.Web.Models.Business
{
    public class Report
    {
        public IndustryDetails IndustryDetails { get; set; }
        public Place CurrentPlace { get; set; }
    }
}