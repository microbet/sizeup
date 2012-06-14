using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web;

namespace SizeUp.Core.Pins
{
    public class PushPin
    {
        public Bitmap Bitmap { get; set; }
        protected Graphics Graphics { get; set; }
        public Color Color {get; protected set;}
        public PushPin(Color color)
        {
            this.Color = color;
            Image template = Image.FromFile(HttpContext.Current.Server.MapPath("/content/images/pinTemplate.png"));
            Bitmap = new Bitmap(15, 35);
            Graphics = Graphics.FromImage(Bitmap);
            Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            Graphics.FillEllipse(new SolidBrush(Color), new Rectangle(0, 0, 14, 14));
            Graphics.DrawImage(template, 0, 0);
        }
    }
}
