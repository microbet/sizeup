using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Base;
using SizeUp.Core.DataLayer.Models.Base;
using System.Data.Spatial;
using SizeUp.Core.Geo;
using System.Data.Objects.SqlClient;


namespace SizeUp.Core.DataLayer
{
    public class Geography
    {
        public static IQueryable<KeyValue<long, Data.Geography>> CalculationGeography(SizeUpContext context, Granularity granularity)
        {
            IQueryable<KeyValue<long, Data.Geography>> output = new List<KeyValue<long, Data.Geography>>().AsQueryable();//wnpty set
            var zip = context.ZipCodes
               .Select(i => new KeyValue<long, Data.Geography>
               {
                   Value = i.ZipCodeGeographies.Where(g => g.GeographyClass.Name == Core.Geo.GeographyClass.Calculation).Select(g => g.Geography).FirstOrDefault(),
                   Key = i.Id
               });

            var city = Base.City.Get(context)
               .Select(i => new KeyValue<long, Data.Geography>
               {
                   Value = i.CityGeographies.Where(g => g.GeographyClass.Name == Core.Geo.GeographyClass.Calculation).Select(g => g.Geography).FirstOrDefault(),
                   Key = i.Id
               });

            var county = context.Counties
                .Select(i => new KeyValue<long, Data.Geography>
                {
                    Value = i.CountyGeographies.Where(g => g.GeographyClass.Name == Core.Geo.GeographyClass.Calculation).Select(g => g.Geography).FirstOrDefault(),
                    Key = i.Id
                });
            
            var place = context.CityCountyMappings
                .Select(i => new KeyValue<long, Data.Geography>
                {
                    Value = i.PlaceGeographies.Where(g => g.GeographyClass.Name == Core.Geo.GeographyClass.Calculation).Select(g => g.Geography).FirstOrDefault(),
                    Key = i.Id
                });

            
            var metro = context.Metroes
                .Select(i => new KeyValue<long, Data.Geography>
                {
                    Value = i.MetroGeographies.Where(g => g.GeographyClass.Name == Core.Geo.GeographyClass.Calculation).Select(g => g.Geography).FirstOrDefault(),
                    Key = i.Id
                });

            var state = context.States
                .Select(i => new KeyValue<long, Data.Geography>
                {
                    Value = i.StateGeographies.Where(g => g.GeographyClass.Name == Core.Geo.GeographyClass.Calculation).Select(g => g.Geography).FirstOrDefault(),
                    Key = i.Id
                });

            if (granularity == Granularity.ZipCode)
            {
                output = zip;
            }
            else if (granularity == Granularity.City)
            {
                output = city;
            }
            else if (granularity == Granularity.County)
            {
                output = county;
            }
            else if (granularity == Granularity.Metro)
            {
                output = metro;
            }
            else if (granularity == Granularity.State)
            {
                output = state;
            }
            else if (granularity == Granularity.Place)
            {
                output = place;
            }
            return output;
        }

       

        public static IQueryable<KeyValue<long, LatLng>> Centroid(SizeUpContext context, Granularity granularity)
        {
            var data = CalculationGeography(context, granularity)
                .Select(i => new KeyValue<long, LatLng>
                {
                    Key = i.Key,
                    Value = new LatLng
                    {
                        Lat = i.Value.CenterLat.Value,
                        Lng = i.Value.CenterLong.Value
                    }
                });
            return data;
        }

        public static IQueryable<KeyValue<long, BoundingBox>> BoundingBox(SizeUpContext context, Granularity granularity)
        {

            var data = CalculationGeography(context, granularity)
                .Select(i => new KeyValue<long, BoundingBox>
                {
                    Key = i.Key,
                    Value = new BoundingBox
                    {
                        SouthWest = new LatLng
                        {
                            Lat = i.Value.South,
                            Lng = i.Value.West
                        },
                        NorthEast = new LatLng
                        {
                            Lat = i.Value.North,
                            Lng = i.Value.East
                        }
                    }
                });
            return data;
        }


        public static IQueryable<Models.ZoomExtent> ZoomExtent(SizeUpContext context, long width)
        {
            var place = Core.DataLayer.Base.Place.Get(context).Select(i => new
            {
                Id = i.Id,
                CityId = i.CityId,
                CountyId = i.CountyId,
                MetroId = i.County.MetroId,
                StateId = i.County.StateId
            });

            double GLOBE_WIDTH = 256; // a constant in Google's map projection
            double ln2 = Math.Log(2);


            var county = CalculationGeography(context, Granularity.County).Select(i => new KeyValue<long, int>
            {
                Key = i.Key,
                Value = (int)Math.Round(SqlFunctions.Log(width * 360 / (i.Value.East - i.Value.West) / GLOBE_WIDTH).Value / ln2) //- 1
            });
            var metro = CalculationGeography(context, Granularity.Metro).Select(i => new KeyValue<long, int>
            {
                Key = i.Key,
                Value = (int)Math.Round(SqlFunctions.Log(width * 360 / (i.Value.East - i.Value.West)  / GLOBE_WIDTH).Value / ln2) - 1
            });
            var state = CalculationGeography(context, Granularity.State).Select(i => new KeyValue<long, int>
            {
                Key = i.Key,
                Value = (int)Math.Round(SqlFunctions.Log(width * 360 / (i.Value.East - i.Value.West) / GLOBE_WIDTH).Value / ln2) - 1
            });

            var data = place.GroupJoin(county, o => o.CountyId, i => i.Key, (i, o) => new { place = i, county = o.FirstOrDefault() })
                .GroupJoin(metro, o => o.place.MetroId, i => i.Key, (i, o) => new { place = i.place, county = i.county, metro = o.FirstOrDefault() })
                .GroupJoin(state, o => o.place.StateId, i => i.Key, (i, o) => new { place = i.place, county = i.county, metro = i.metro, state = o.FirstOrDefault() })
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
