using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SizeUp.Web.Areas;

namespace SizeUp.Web.Models.Business
{
    public class IndustryDetails
    {
        public Areas.Api.Models.Industry.Industry Industry { get; set; }
        public Areas.Api.Models.NAICS.NAICS NAICS6 { get; set; }
        public Areas.Api.Models.NAICS.NAICS NAICS4 { get; set; }
    }
}