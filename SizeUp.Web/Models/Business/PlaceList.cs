using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SizeUp.Web.Models.Business
{
    public class PlaceList
    {
        public string Key { get; set; }
        public List<Place> Places { get; set; }
    }
}