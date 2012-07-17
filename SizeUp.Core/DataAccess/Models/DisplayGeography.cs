using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Spatial;

namespace SizeUp.Core.DataAccess.Models
{
    public class DisplayGeography
    {
        public long Id { get; set; }
        public DbGeography Geography { get; set; }
    }
}
