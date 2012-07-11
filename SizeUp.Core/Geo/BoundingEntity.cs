using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using SizeUp.Data;
using System.Data.Spatial;

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
        public DbGeography DbGeography { get; protected set; }
        public long? EntityId { get; protected set; }


        public BoundingEntity(string entityIdCode)
        {
            using (var context = ContextFactory.SizeUpContext)
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
                    var g = context.ZipCodeGeographies
                        .Where(i=>i.GeographyClass.Name == "Calculation")
                        .Where(i => i.ZipCodeId == EntityId).Select(i => i.Geography.GeographyPolygon).FirstOrDefault();
                    if (g != null)
                    {
                        Geography = SqlGeography.Parse(g.AsText());
                        DbGeography = g;
                    }
                }
                else if (entityIdCode.StartsWith("co"))
                {
                    EntityId = long.Parse(entityIdCode.Substring(2));
                    EntityType = BoundingEntityType.County;
                    var g = context.CountyGeographies
                        .Where(i => i.GeographyClass.Name == "Calculation")
                        .Where(i => i.CountyId == EntityId).Select(i => i.Geography.GeographyPolygon).FirstOrDefault();
                    if (g != null)
                    {
                        Geography = SqlGeography.Parse(g.AsText());
                        DbGeography = g;
                    }
                }
                else if (entityIdCode.StartsWith("c"))
                {
                    EntityId = long.Parse(entityIdCode.Substring(1));
                    EntityType = BoundingEntityType.City;
                    var g = context.CityGeographies
                        .Where(i => i.GeographyClass.Name == "Calculation")
                        .Where(i => i.CityId == EntityId).Select(i => i.Geography.GeographyPolygon).FirstOrDefault();
                    if (g != null)
                    {
                        Geography = SqlGeography.Parse(g.AsText());
                        DbGeography = g;
                    }
                }
                else if (entityIdCode.StartsWith("m"))
                {
                    EntityId = long.Parse(entityIdCode.Substring(1));
                    EntityType = BoundingEntityType.Metro;
                    var g = context.MetroGeographies
                        .Where(i=>i.GeographyClass.Name == "Calculation")
                        .Where(i => i.MetroId == EntityId).Select(i => i.Geography.GeographyPolygon).FirstOrDefault();
                    if (g != null)
                    {
                        Geography = SqlGeography.Parse(g.AsText());
                        DbGeography = g;
                    }
                }
                else if (entityIdCode.StartsWith("s"))
                {
                    EntityId = long.Parse(entityIdCode.Substring(1));
                    EntityType = BoundingEntityType.State;
                    var g = context.StateGeographies
                        .Where(i=>i.GeographyClass.Name == "Calculation")
                        .Where(i => i.StateId == EntityId).Select(i => i.Geography.GeographyPolygon).FirstOrDefault();
                    if (g != null)
                    {
                        Geography = SqlGeography.Parse(g.AsText());
                        DbGeography = g;
                    }
                }
            }
        }
    }
}
