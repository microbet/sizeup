using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.SqlServer.Types;

namespace SizeUp.Core.Tiles
{
    public abstract class Tile
    {
        public int Height { get; protected set; }
        public int Width { get;  protected set; }
        public int Y { get; protected set; }
        public int X { get;  protected set; }
        public double Zoom { get; protected set; }
        public Bitmap Bitmap { get; set; }
        protected Graphics Graphics { get; set; }
        protected GoogleMapsAPIProjection Projection { get; set; }
        protected Matrix TranslationMatrix = new Matrix();
           //probasbly need to convert this to sw and ne becuase at zoom levels less than 4 we get wonky bounding boxes
        public Tile(int Width, int Height, int x, int y, double Zoom)
        {
            this.Width = Width;
            this.Height = Height;
            this.Y = y;
            this.X = x;
            this.Zoom = Zoom;
            TranslationMatrix.Translate(-1 * x * Width, -1 * y * Height);
            Projection = new GoogleMapsAPIProjection(Zoom);
            Bitmap = new Bitmap(Width, Height);
            Graphics = Graphics.FromImage(Bitmap);
            Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            Graphics.SmoothingMode = SmoothingMode.HighQuality;
            Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        }

        public SqlGeography GetBoundingGeography()
        {
            var ne = Projection.FromPixelToCoordinates(new PointF((X + 1)*Width, Y*Height));
            var sw = Projection.FromPixelToCoordinates(new PointF(X*Width, (Y + 1)*Height));
            return SqlGeography.Parse(string.Format("POLYGON (({0} {2}, {1} {2}, {1} {3}, {0} {3}, {0} {2}))", sw.X, ne.X, sw.Y, ne.Y));
        }

        public SqlGeography GetBoundingGeography(SqlGeography geography)
        {
            var box = GetBoundingGeography();

            if (geography != null)
            {
                box = box.STIntersection(geography);
            }
            return box;
        }
    }
}
