using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using SizeUp.Data;
using System.Data.Spatial;
using System.Drawing;

namespace SizeUp.Core.Geo
{
    public class BoundingEntity
    {
        public enum BoundingEntityType
        {
            Zip,
            City,
            County,
            Metro,
            State
        }

        public string BoundingEntityId { get; protected set; }
        public BoundingEntityType? EntityType { get; protected set; }
        public long? EntityId { get; protected set; }

        public BoundingEntity(string entityIdCode)
        {

            BoundingEntityId = entityIdCode;
            EntityType = null;
            EntityId = null;
            if (string.IsNullOrEmpty(entityIdCode))
            {
                entityIdCode = string.Empty;
            }

            if (entityIdCode.StartsWith("z"))
            {
                EntityId = long.Parse(entityIdCode.Substring(1));
                EntityType = BoundingEntityType.Zip;
            }
            else if (entityIdCode.StartsWith("co"))
            {
                EntityId = long.Parse(entityIdCode.Substring(2));
                EntityType = BoundingEntityType.County;
            }
            else if (entityIdCode.StartsWith("c"))
            {
                EntityId = long.Parse(entityIdCode.Substring(1));
                EntityType = BoundingEntityType.City;
            }
            else if (entityIdCode.StartsWith("m"))
            {
                EntityId = long.Parse(entityIdCode.Substring(1));
                EntityType = BoundingEntityType.Metro;
            }
            else if (entityIdCode.StartsWith("s"))
            {
                EntityId = long.Parse(entityIdCode.Substring(1));
                EntityType = BoundingEntityType.State;
            }

        }
    }
}
