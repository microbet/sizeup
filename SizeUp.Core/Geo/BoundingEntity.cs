using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using SizeUp.Data;

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
        public BoundingEntityType? EntityType { get; protected set;}
        public SqlGeography Geography { get; protected set; }
        public long? EntityId { get; protected set; }


        public BoundingEntity(string entityIdCode)
        {
            BoundingEntityId = entityIdCode;
            EntityType = null;
            Geography = null;
            EntityId = null;
            if (string.IsNullOrEmpty(entityIdCode))
            {
                entityIdCode = string.Empty;
            }

            if (entityIdCode.StartsWith("z"))
            {
                EntityId = long.Parse(entityIdCode.Substring(1));
                EntityType = BoundingEntityType.Zip;
                var g = DataContexts.SizeUpContext.ZipCodes.Where(i => i.Id == EntityId).Select(i => i.Geography.Buffer(-100)).FirstOrDefault();
                if (g != null)
                {
                    Geography = SqlGeography.Parse(g.AsText());
                }
            }
            else if (entityIdCode.StartsWith("c"))
            {
                EntityId = long.Parse(entityIdCode.Substring(1));
                EntityType = BoundingEntityType.City;
                var g = DataContexts.SizeUpContext.Cities.Where(i => i.Id == EntityId).Select(i => i.Geography.Buffer(-500)).FirstOrDefault();
                if (g != null)
                {
                    Geography = SqlGeography.Parse(g.AsText());
                }
            }
            else if (entityIdCode.StartsWith("co"))
            {
                EntityId = long.Parse(entityIdCode.Substring(2));
                EntityType = BoundingEntityType.County;
                var g = DataContexts.SizeUpContext.Counties.Where(i => i.Id == EntityId).Select(i => i.Geography.Buffer(-500)).FirstOrDefault();
                if (g != null)
                {
                    Geography = SqlGeography.Parse(g.AsText());
                }
            }
            else if (entityIdCode.StartsWith("m"))
            {
                EntityId = long.Parse(entityIdCode.Substring(1));
                EntityType = BoundingEntityType.Metro;
                var g = DataContexts.SizeUpContext.Metroes.Where(i => i.Id == EntityId).Select(i => i.Geography.Buffer(-500)).FirstOrDefault();
                if (g != null)
                {
                    Geography = SqlGeography.Parse(g.AsText());
                }
            }
            else if (entityIdCode.StartsWith("s"))
            {
                EntityId = long.Parse(entityIdCode.Substring(1));
                EntityType = BoundingEntityType.State;
                var g = DataContexts.SizeUpContext.States.Where(i => i.Id == EntityId).Select(i => i.Geography.Buffer(-2500)).FirstOrDefault();
                if (g != null)
                {
                    Geography = SqlGeography.Parse(g.AsText());
                }
            }
        }
    }
}
