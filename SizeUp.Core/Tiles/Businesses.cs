using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.SqlServer.Types;

namespace SizeUp.Core.Tiles
{



    public class Businesses : Tile
    {
        public Businesses(int Width, int Height, int x, int y, double zoom)
            : base(Width, Height, x, y, zoom)
        {

        }


        public override void Draw(List<GeographyCollection> Geographies)
        {
            float width = (float)(0.2f * Zoom + 0.5f) * 2;
            float height = (float)(0.2f * Zoom + 0.5f) * 2;

            Graphics.Transform = TranslationMatrix;
            foreach (var geo in Geographies)
            {
                Color c = Color.FromArgb(geo.Opacity, ColorTranslator.FromHtml(geo.Color));
                SolidBrush brush = new SolidBrush(c);
                foreach (var g in geo.Geographies)
                {
                    PointF p = this.Projection.FromCoordinatesToPixel(new PointF((float)g.Long.Value, (float)g.Lat.Value));

                    Graphics.FillEllipse(brush, p.X - width / 2, p.Y - height / 2, width, height);
                }
            }
        }
    }
}
