using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Api = SizeUp.Web.Areas.Api;

namespace SizeUp.Web.Models
{
    public class CurrentInfo
    {
        public Api.Models.City.City CurrentCity { get; set; }
        public Api.Models.Industry.Industry CurrentIndustry { get; set; }
    }
}