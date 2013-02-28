using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;

namespace SizeUp.Core.DataLayer.Models.Base
{
    public class BusinessData
    {
        public CityCountyMapping Place { get; set; }
        public IQueryable<BusinessDataByCity> City { get; set; }
        public IQueryable<BusinessDataByCounty> County { get; set; }
        public IQueryable<BusinessDataByCounty> Metro { get; set; }
        public IQueryable<BusinessDataByCounty> State { get; set; }
        public IQueryable<BusinessDataByCounty> Nation { get; set; }
    }
}
