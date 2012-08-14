using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SizeUp.Web.Models
{
    public class Option<T>
    {
        public T Id { get; set; }
        public string Name { get; set; }
    }
}