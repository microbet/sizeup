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
        public Businesses(int Width, int Height, int x, int y, int zoom)
            : base(Width, Height, x, y, zoom)
        {

        }

        public override void Draw(List<GeographyEntity> Geographies)
        {
            float width = (float)(0.2f * Zoom + 0.5f);
            float height = (float)(0.2f * Zoom + 0.5f);
            Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var xoff = TranslationMatrix.OffsetX;
            var yoff = TranslationMatrix.OffsetY;
            foreach (var geo in Geographies)
            {
                Color c = Color.FromArgb(geo.Opacity, ColorTranslator.FromHtml(geo.Color));
                SolidBrush brush = new SolidBrush(c);

                Color borderc = Color.FromArgb(geo.BorderOpacity, ColorTranslator.FromHtml(geo.BorderColor));
                Pen borderPen = new Pen(borderc, geo.BorderWidth);

                PointF p = this.Projection.FromCoordinatesToPixel(new PointF((float)geo.Geography.Long, (float)geo.Geography.Lat));
                Graphics.FillEllipse(brush, p.X - width + xoff, p.Y + height + yoff, width * 2, height * 2);
                Graphics.DrawEllipse(borderPen, p.X - width + xoff, p.Y + height + yoff, width * 2, height * 2);
               
            }
        }
    }
}
