﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Models
{
    public class City : GeographicLocation
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string SEOKey { get; set; }
        public string TypeName { get; set; }
        public List<County> Counties { get; set; }
    }
}
