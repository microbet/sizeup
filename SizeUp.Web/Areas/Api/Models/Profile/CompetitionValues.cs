using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SizeUp.Web.Areas.Api.Models.Profile
{
    public class CompetitionValues
    {
        public long? ConsumerExpenditureId { get; set; }
        public List<long> CompetitorIds { get; set; }
        public List<long> SupplierIds { get; set; }
        public List<long> BuyerIds { get; set; }
    }
}