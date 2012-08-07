using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SizeUp.Web.Areas.Api.Models.Advertising
{
    public class Band<T>
    {
        public T Min { get; set; }
        public T Max { get; set; }
    }
}