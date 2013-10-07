using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Web.Security;
using System.Configuration;
using System.Web.Profile;
using SizeUp.Data;

namespace SizeUp.Utility
{
    class Program
    {
        static void Main(string[] args)
        {
           
                var c1 = Color.FromArgb(255, 0, 0);
                var c2 = Color.FromArgb(255, 255, 0);

                var sections = 5-2;
                //var divisions = sections - 2;

                var deltaR = (c2.R - c1.R) / (sections+1);
                var deltaG = (c2.G - c1.G) / (sections + 1);
                var deltaB = (c2.B - c1.B) / (sections + 1);

                List<Color> colors = new List<Color>();
                colors.Add(c1);
                for (var x = 1; x <= sections; x++)
                {
                    var c = Color.FromArgb((int)(c1.R  + (x*deltaR)), (int)(c1.G + (x*deltaG)), (int)(c1.B + (x*deltaB)));
                    colors.Add(c);
                }
                colors.Add(c2);
                var t = 0;
          
        }
    }
}
