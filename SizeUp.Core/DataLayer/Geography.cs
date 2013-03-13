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

            var city = context.Cities
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
            /*
            var place = context.CityCountyMappings
                .Select(i => new KeyValue<long, Data.Geography>
                {
                    Key = i.City.CityGeographies.Where(cg => cg.GeographyClass.Name == Core.Geo.GeographyClass.Calculation).Select(cg => cg.Geography.GeographyPolygon).FirstOrDefault().Intersection(i.County.CountyGeographies.Where(g => g.GeographyClass.Name == Core.Geo.GeographyClass.Calculation).Select(g => g.Geography.GeographyPolygon).FirstOrDefault()),
                    Value = i.Id
                });

            */
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
           /* else if (granularity == Granularity.Place)
            {
                output = place;
            }*/
            return output;
        }

        public static IQueryable<KeyValue<DbGeography, long>> CalculationPolygon(SizeUpContext context, Granularity granularity)
        {
            IQueryable<KeyValue<DbGeography, long>> output = new List<KeyValue<DbGeography, long>>().AsQueryable();//wnpty set
            var zip = context.ZipCodes
               .Select(i => new KeyValue<DbGeography, long>
               {
                   Key = i.ZipCodeGeographies.Where(g => g.GeographyClass.Name == Core.Geo.GeographyClass.Calculation).Select(g => g.Geography.GeographyPolygon).FirstOrDefault(),
                   Value = i.Id
               });

            var city = context.Cities
               .Select(i => new KeyValue<DbGeography, long>
               {
                   Key = i.CityGeographies.Where(g => g.GeographyClass.Name == Core.Geo.GeographyClass.Calculation).Select(g => g.Geography.GeographyPolygon).FirstOrDefault(),
                   Value = i.Id
               });

            var county = context.Counties
                .Select(i => new KeyValue<DbGeography, long>
                {
                    Key = i.CountyGeographies.Where(g => g.GeographyClass.Name == Core.Geo.GeographyClass.Calculation).Select(g => g.Geography.GeographyPolygon).FirstOrDefault(),
                    Value = i.Id
                });

            var place = context.CityCountyMappings
                .Select(i => new KeyValue<DbGeography, long>
                {
                    Key = i.City.CityGeographies.Where(cg => cg.GeographyClass.Name == Core.Geo.GeographyClass.Calculation).Select(cg => cg.Geography.GeographyPolygon).FirstOrDefault().Intersection(i.County.CountyGeographies.Where(g => g.GeographyClass.Name == Core.Geo.GeographyClass.Calculation).Select(g => g.Geography.GeographyPolygon).FirstOrDefault()),
                    Value = i.Id
                });


            var metro = context.Metroes
                .Select(i => new KeyValue<DbGeography, long>
                {
                    Key = i.MetroGeographies.Where(g => g.GeographyClass.Name == Core.Geo.GeographyClass.Calculation).Select(g => g.Geography.GeographyPolygon).FirstOrDefault(),
                    Value = i.Id
                });

            var state = context.States
                .Select(i => new KeyValue<DbGeography, long>
                {
                    Key = i.StateGeographies.Where(g => g.GeographyClass.Name == Core.Geo.GeographyClass.Calculation).Select(g => g.Geography.GeographyPolygon).FirstOrDefault(),
                    Value = i.Id
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

        public static LatLng Centroid(SizeUpContext context, long id, Granularity granularity)
        {
            var data = CalculationPolygon(context, granularity)
                .Where(i => i.Value == id)
                .Select(i => DbGeometry.FromBinary(i.Key.AsBinary()).ConvexHull.Centroid)
                .Select(i => DbGeography.FromBinary(i.AsBinary()))
                .Select(i => new LatLng
                {
                    Lat = (double)i.Latitude,
                    Lng = (double)i.Longitude
                })
                .FirstOrDefault();
            return data;
        }

        public static BoundingBox BoundingBox(SizeUpContext context, long id, Granularity granularity)
        {

            var data = CalculationPolygon(context, granularity)
                .Where(i => i.Value == id)
                .Select(i => DbGeometry.FromBinary(i.Key.AsBinary()).Envelope)
                .Select(i => DbGeography.FromBinary(i.AsBinary()))
                .Select(i => new BoundingBox
                {
                    SouthWest = new LatLng
                    {
                        Lat = (double)i.PointAt(1).Latitude,
                        Lng = (double)i.PointAt(1).Longitude
                    },
                    NorthEast = new LatLng
                    {
                        Lat = (double)i.PointAt(3).Latitude,
                        Lng = (double)i.PointAt(3).Longitude
                    }
                })
                .FirstOrDefault();
            return data;
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
    }
}
