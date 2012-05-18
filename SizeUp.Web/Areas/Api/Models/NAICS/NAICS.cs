using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SizeUp.Web.Areas.Api.Models.NAICS
{
    public class NAICS
    {
        public long Id { get; set; }
        public string NAICSCode { get; set; }
        public string Name { get; set; }
    }
}