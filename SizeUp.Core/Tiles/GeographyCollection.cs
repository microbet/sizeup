﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;

namespace SizeUp.Core.Tiles
{
    public class GeographyCollection
    {
        public GeographyCollection()
        {
            Color = "#C0C0C0";
            Opacity = 160;
            BorderColor = "#333333";
            BorderWidth = 0.25f;
            BorderOpacity = 50;
            Geographies = new List<SqlGeography>();
        }
        public string Color { get; set; }
        public int Opacity { get; set; }
        public int BorderOpacity { get; set; }
        public float BorderWidth { get; set; }
        public string BorderColor { get; set; }
        public List<SqlGeography> Geographies { get; set; }
    }
}
