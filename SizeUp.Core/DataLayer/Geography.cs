using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Models;
using System.Data.Spatial;
using SizeUp.Core.Geo;
using System.Data.Objects.SqlClient;


namespace SizeUp.Core.DataLayer
{
    public class Geography
    {
        public static IQueryable<Data.Geography> Get(SizeUpContext context)
        {
            return context.Geographies;
        }

        public static IQueryable<Data.Geography> Calculation(SizeUpContext context)
        {
            return Get(context).Where(i => i.GeographyClass.Name == Geo.GeographyClass.Calculation);
        }

        public static IQueryable<Data.Geography> Display(SizeUpContext context)
        {
            return Get(context).Where(i => i.GeographyClass.Name == Geo.GeographyClass.Display);
        }

        public static IQueryable<Models.ZoomExtent> ZoomExtent(SizeUpContext context, long width)
        {
            var place = Core.DataLayer.Place.Get(context).Select(i => new
            {
                Id = i.Id,
                CityId = i.CityId,
                CountyId = i.CountyId,
                MetroId = i.County.MetroId,
                StateId = i.County.StateId
            });

            double GLOBE_WIDTH = 256; // a constant in Google's map projection
            double ln2 = Math.Log(2);


            var geos = Calculation(context).Select(i => new KeyValue<long, int>
            {
                Key = i.GeographicLocationId,
                Value = (int)Math.Round(SqlFunctions.Log(width * 360 / (i.East - i.West) / GLOBE_WIDTH).Value / ln2) - 1
            });
            var data = place.GroupJoin(geos, o => o.CountyId, i => i.Key, (i, o) => new { place = i, county = o.FirstOrDefault() })
                .GroupJoin(geos, o => o.place.MetroId, i => i.Key, (i, o) => new { place = i.place, county = i.county, metro = o.FirstOrDefault() })
                .GroupJoin(geos, o => o.place.StateId, i => i.Key, (i, o) => new { place = i.place, county = i.county, metro = i.metro, state = o.FirstOrDefault() })
                .Select(i => new Models.ZoomExtent
                {
                    PlaceId = i.place.Id,
                    County = i.county.Value,
                    Metro = i.metro.Value,
                    State = i.state.Value
                })
                .Select(i => new Models.ZoomExtent
                {
                    PlaceId = i.PlaceId,
                    County = i.County <= 6 ? 6 : i.County,
                    Metro = i.Metro <= 5 ? 5 : i.Metro,
                    State = i.State <= 4 ? 4 : i.State
                });
            return data;
        }
    }
}
