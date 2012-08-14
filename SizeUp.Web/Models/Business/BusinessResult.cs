using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SizeUp.Web.Models.Business
{
    public class BusinessResult
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string WebSite { get; set; }
        public bool IsPublic { get; set; }
        public string StateSEO { get; set; }
        public string CountySEO { get; set; }
        public string CitySEO { get; set; }
        public string IndustrySEO { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public string SEOKey { get; set; }
    }
}