using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SizeUp.Web.Models.ConsumerExpenditures
{
    public class Variable
    {
        public long Id { get; set; }
        public long? ParentId { get; set; }
        public string Name { get; set; }
        public List<Variable> Children { get; set; }
    }
}