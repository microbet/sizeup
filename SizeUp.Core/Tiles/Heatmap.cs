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
    public class Heatmap : Tile
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


        public Heatmap(int Width, int Height, int x, int y, double zoom)
            : base(Width, Height, x, y, zoom)
        {
            
        }

       
        public void Draw(List<GeographyCollection> Geographies)
        {     
            foreach (var geo in Geographies)
            {
                using (GraphicsPath gp = new GraphicsPath())
                {
                    foreach (var g in geo.Geographies)
                    {
                        GeoSink sink = new GeoSink(this.Projection);
                        g.Populate(sink);
                        foreach (var geography in sink.Geographies)
                        {
                            foreach (var figure in geography)
                            {
                                gp.AddPolygon(figure.ToArray());
                            }
                        }
                    }

                    gp.FillMode = FillMode.Winding;
                    gp.Transform(TranslationMatrix);
                    Color c = Color.FromArgb(geo.Opacity, ColorTranslator.FromHtml(geo.Color));
                    SolidBrush brush = new SolidBrush(c);
                    Graphics.FillPath(brush, gp);
                    if (geo.BorderWidth > 0)
                    {
                        Pen p = new Pen(Color.FromArgb(geo.BorderOpacity, ColorTranslator.FromHtml(geo.BorderColor)), geo.BorderWidth);
                        Graphics.DrawPath(p, gp);
                    }
                }
            }
        }
    }
}
