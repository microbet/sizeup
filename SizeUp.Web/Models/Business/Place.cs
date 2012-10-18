using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SizeUp.Web.Models.Business
{
    public class Place
    {
        public long CityId { get; set; }
        public string CityName { get; set; }
        public string CitySEOKey { get; set; }
        public long CountyId { get; set; }
        public string CountyName { get; set; }
        public string CountySEOKey { get; set; }
        public string CityType { get; set; }
        public bool DisplayType { get; set; }
        public long StateId { get; set; }
        public string StateName { get; set; }
        public string StateSEOKey { get; set; }
    }
}