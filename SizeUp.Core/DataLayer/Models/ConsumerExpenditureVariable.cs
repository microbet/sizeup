using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Models
{
    public class ConsumerExpenditureVariable
    {
        public long Id { get; set; }
        public long? ParentId {get;set;}
        public string Variable { get; set; }
        public string Description { get; set; }
        public bool HasChildren { get; set; }
    }
}
