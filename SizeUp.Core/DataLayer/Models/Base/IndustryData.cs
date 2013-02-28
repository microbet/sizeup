using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;

namespace SizeUp.Core.DataLayer.Models.Base
{
    public class IndustryData
    {
        public CityCountyMapping Place { get; set; }
        public IQueryable<IndustryDataByCity> City { get; set; }
        public IQueryable<IndustryDataByCounty> County { get; set; }
        public IQueryable<IndustryDataByMetro> Metro { get; set; }
        public IQueryable<IndustryDataByState> State { get; set; }
        public IQueryable<IndustryDataByNation> Nation { get; set; }
    }
}
