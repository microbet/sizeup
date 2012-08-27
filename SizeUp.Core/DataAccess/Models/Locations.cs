using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.Geo;

namespace SizeUp.Core.DataAccess.Models
{
    public class Locations
    {
        public long Id { get; set; }
        public City City { get; set; }
        public County County { get; set; }
        public Metro Metro { get; set; }
        public State State { get; set; }

        public BoundingEntity CityBoundingEntity { get { return City != null ? new BoundingEntity("c" + City.Id.ToString()) : null; } }
        public BoundingEntity CountyBoundingEntity { get { return City != null ? new BoundingEntity("co" + County.Id.ToString()) : null; } }
        public BoundingEntity MetroBoundingEntity { get { return City != null ? new BoundingEntity("m" + Metro.Id.ToString()) : null; } }
        public BoundingEntity StateBoundingEntity { get { return City != null ? new BoundingEntity("s" + State.Id.ToString()) : null; } }
    }
}
