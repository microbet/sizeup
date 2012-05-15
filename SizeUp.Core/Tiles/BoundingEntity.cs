using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using SizeUp.Data;

namespace SizeUp.Core.Tiles
{
    public static class BoundingEntity
    {
        public static SqlGeography Get(string entityIdcode)
        {
            if (string.IsNullOrEmpty(entityIdcode))
            {
                entityIdcode = string.Empty;
            }

            SqlGeography geo = null;
            if(entityIdcode.StartsWith("z"))
            {
                long id = long.Parse(entityIdcode.Substring(1));
                var g = DataContexts.SizeUpContext.ZipCodes.Where(i => i.Id == id).Select(i => i.Geography.Buffer(-100)).FirstOrDefault();
                if (g != null)
                {
                    geo = SqlGeography.Parse(g.AsText());
                }
            }
            else if (entityIdcode.StartsWith("c"))
            {
                long id = long.Parse(entityIdcode.Substring(1));
                var g = DataContexts.SizeUpContext.Cities.Where(i => i.Id == id).Select(i => i.Geography.Buffer(-500)).FirstOrDefault();
                if (g != null)
                {
                    geo = SqlGeography.Parse(g.AsText());
                }
            }
            else if (entityIdcode.StartsWith("co"))
            {
                long id = long.Parse(entityIdcode.Substring(2));
                var g = DataContexts.SizeUpContext.Counties.Where(i => i.Id == id).Select(i => i.Geography.Buffer(-500)).FirstOrDefault();
                if (g != null)
                {
                    geo = SqlGeography.Parse(g.AsText());
                }
            }
            else if (entityIdcode.StartsWith("m"))
            {
                long id = long.Parse(entityIdcode.Substring(1));
                var g = DataContexts.SizeUpContext.Metroes.Where(i => i.Id == id).Select(i => i.Geography.Buffer(-500)).FirstOrDefault();
                if (g != null)
                {
                    geo = SqlGeography.Parse(g.AsText());
                }
            }
            else if (entityIdcode.StartsWith("s"))
            {
                long id = long.Parse(entityIdcode.Substring(1));
                var g = DataContexts.SizeUpContext.States.Where(i => i.Id == id).Select(i => i.Geography.Buffer(-2500)).FirstOrDefault();
                if (g != null)
                {
                    geo = SqlGeography.Parse(g.AsText());
                }
            }
            return geo;
        }
    }
}
