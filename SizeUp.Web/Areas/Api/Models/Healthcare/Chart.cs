using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SizeUp.Web.Areas.Api.Models.Healthcare
{
    public class Chart
    {
        public long? State { get; set; }
        public int? StateRank { get; set; }

        public long? Industry { get; set; }
        public int? IndustryRank { get; set; }

        public long? FirmSize { get; set; }
        public int? FirmSizeRank { get; set; }


    }
}