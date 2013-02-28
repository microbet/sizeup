using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Models
{
    public class HealthcareChart
    {
        public long? State { get; set; }
        public int? StateRank { get; set; }

        public long? Industry { get; set; }
        public int? IndustryRank { get; set; }

        public long? FirmSize { get; set; }
        public int? FirmSizeRank { get; set; }
    }
}
