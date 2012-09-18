using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.SqlServer.Types;
using SizeUp.Core.DataAccess.Models;

namespace SizeUp.Core.Tiles
{
    public class GeographyBoundary : Tile
    {
        public GeographyBoundary(int Width, int Height, int x, int y, double zoom)
            : base(Width, Height, x, y, zoom)
        {
            
        }

       
        public override void Draw(List<GeographyCollection> Geographies)
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
                    gp.Transform(TranslationMatrix);
                    if (geo.BorderWidth > 0)
                    {
                        Pen p = new Pen(Color.FromArgb(geo.BorderOpacity, ColorTranslator.FromHtml(geo.BorderColor)), geo.BorderWidth);
                        Graphics.DrawPath(p, gp);
                    }
                }
            }
        }

        public static List<GeographyCollection> CreateCollections(string color, IEnumerable<DisplayGeography> Geographies)
        {
            List<GeographyCollection> c = new List<GeographyCollection>();

     
            var geoCollection = new GeographyCollection();
            geoCollection.Geographies.AddRange(Geographies.Select(i => SqlGeography.Parse(i.Geography.AsText())).ToList());
            geoCollection.BorderColor = color;
            geoCollection.BorderOpacity = 200;
            geoCollection.BorderWidth = 2;
            c.Add(geoCollection);
       

            return c;
        }
    }
}


