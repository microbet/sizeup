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
using SizeUp.Core.Geo;
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

        public BoundingBox GetBoundingBox()
        {
            var ne = Projection.FromPixelToCoordinates(new PointF((X + 1) * Width, Y * Height));
            var sw = Projection.FromPixelToCoordinates(new PointF(X * Width, (Y + 1) * Height));
            return new BoundingBox(sw, ne);
        }
 
        public BoundingBox GetBoundingBox(float bufferPercent)
        {
            var nep = new PointF((X + 1) * Width, Y * Height);
            nep.X = nep.X + (Width * bufferPercent);
            nep.Y = nep.Y - (Height * bufferPercent);
            var swp = new PointF(X * Width, (Y + 1) * Height);
            swp.X = swp.X - (Width * bufferPercent);
            swp.Y = swp.Y + (Height * bufferPercent);
            var ne = Projection.FromPixelToCoordinates(nep);
            var sw = Projection.FromPixelToCoordinates(swp);
            return new BoundingBox(sw, ne);
        }

        public abstract void Draw(List<GeographyEntity> Geographies);
    }
}
