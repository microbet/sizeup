using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SizeUp.Web.Areas.Api.Models.Maps
{
    public class LatLng
    {
        public double Lat { get; set; }
        public double Lng { get; set; }

        public static LatLng FromText(string text)
        {
            LatLng ll = new LatLng();
            var parsed = text.Split(' ');
            ll.Lat = float.Parse(parsed[0]);
            ll.Lng = float.Parse(parsed[1]);
            return ll;
        }
    }
}