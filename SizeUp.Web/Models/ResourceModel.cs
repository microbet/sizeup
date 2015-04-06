using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SizeUp.Web.Models
{
    public class ResourceModel
    {
        [AllowHtml]
        public string ResourceHtml { get; set; }

        public bool SaveHtml { get; set; }

        public string ClientId { get; set; }
        public string Section { get; set; }
    }
}