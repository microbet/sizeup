using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SizeUp.Web.Models.Business
{
    public class BusinessItem
    {
        public SizeUp.Data.Business Business { get; set; }
        public SizeUp.Data.State State { get; set; }
        public SizeUp.Data.Industry Industry { get; set; }
        public SizeUp.Data.ZipCode ZipCode { get; set; }
    }
}