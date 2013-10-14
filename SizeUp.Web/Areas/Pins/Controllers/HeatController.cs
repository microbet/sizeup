using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Text;
using Svg;
namespace SizeUp.Web.Areas.Pins.Controllers
{
    public class HeatController : Controller
    {
        //
        // GET: /Pins/Business/

        public ActionResult Marker(string color, int width = 13, int height = 22)
        {
            string path = @"<svg version='1.1' id='marker' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' x='0' y='0' width='{1}' height='{2}' viewBox='0 0 93 153'>
	                            <path fill='{0}' stroke-width='6' stroke='#716464' d='M48.665,148.667h-3.251l-0.073-1.423c-0.949-18.441-11.127-49.288-26.517-68.987C9.619,66.474,6.339,55.916,6.339,49.964c0-22.7,18.258-41.167,40.7-41.167S87.74,27.264,87.74,49.964c0,5.952-3.28,16.51-12.485,28.293c-15.39,19.699-25.567,50.546-26.517,68.987L48.665,148.667L48.665,148.667z'/>
                            </svg>";

            path = string.Format(path, color, width, height);
            byte[] byteArray = Encoding.UTF8.GetBytes(path);


            System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
            var doc = Svg.SvgDocument.Open(stream);
            var bitmap = doc.Draw();
            var outStream = new System.IO.MemoryStream();
            bitmap.Save(outStream, System.Drawing.Imaging.ImageFormat.Png);
            return File(outStream.GetBuffer(), "image/png");
        }

      


    }
}
