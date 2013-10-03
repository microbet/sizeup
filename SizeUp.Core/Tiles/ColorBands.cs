using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SizeUp.Core.Tiles
{
    public class ColorBands
    {
        public Color StartColor { get; set; }
        public Color EndColor { get; set; }
        public int Bands { get; set; }
        public ColorBands(Color startColor, Color endColor, int bands)
        {
            StartColor = startColor;
            EndColor = endColor;
            Bands = bands;
        }
        public List<string> GetColorBands()
        {
            var sections = Bands - 2;

            var deltaR = (EndColor.R - StartColor.R) / (sections + 1);
            var deltaG = (EndColor.G - StartColor.G) / (sections + 1);
            var deltaB = (EndColor.B - StartColor.B) / (sections + 1);

            List<Color> colors = new List<Color>();
            colors.Add(StartColor);
            for (var x = 1; x <= sections; x++)
            {
                var c = Color.FromArgb((int)(StartColor.R + (x * deltaR)), (int)(StartColor.G + (x * deltaG)), (int)(StartColor.B + (x * deltaB)));
                colors.Add(c);
            }
            colors.Add(EndColor);
            return colors.Select(i => System.Drawing.ColorTranslator.ToHtml(i)).ToList();
        }
    }
}
