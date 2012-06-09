using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SizeUp.Core.Pins
{
    public class Marker
    {
        public Bitmap Bitmap { get; set; }
        public int Index { get; protected set; }
        protected Graphics Graphics { get; set; }

        public Marker(int index, Image template)
        {
            this.Index = index;
            Bitmap = new Bitmap(20, 29);
            Graphics = Graphics.FromImage(Bitmap);
            Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            Graphics.DrawImage(template, 0, 0);
            if (Index < 10)
            {
                Graphics.DrawString(Index.ToString(), new Font("Trebuchet MS", 9.2f, FontStyle.Bold), new SolidBrush(System.Drawing.Color.White), new RectangleF(-1.6f, 1.5f, 20, 29), new StringFormat() { Alignment = StringAlignment.Center });
            }
            else
            {
                Graphics.DrawString(Index.ToString(), new Font("Trebuchet MS", 8.6f, FontStyle.Bold), new SolidBrush(System.Drawing.Color.White), new RectangleF(-2.2f, 1.5f, 20, 29), new StringFormat() { Alignment = StringAlignment.Center });
            }
        }
    }
}
