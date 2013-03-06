using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Base;
using SizeUp.Core.DataLayer.Models.Base;
using System.Data.Spatial;

namespace SizeUp.Core.DataLayer
{
    public class Geography
    {
        public static IQueryable<KeyValue<DbGeography, long>> CalculationPolygon(SizeUpContext context, long id, Granularity granularity)
        {
            IQueryable<KeyValue<DbGeography, long>> output = new List<KeyValue<DbGeography, long>>().AsQueryable();//wnpty set
            var zip = context.ZipCodes
               .Select(i => new KeyValue<DbGeography, long>
               {
                   Key = i.ZipCodeGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => g.Geography.GeographyPolygon).FirstOrDefault(),
                   Value = i.Id
               });

            var city = context.Cities
               .Select(i => new KeyValue<DbGeography, long>
               {
                   Key = i.CityGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => g.Geography.GeographyPolygon).FirstOrDefault(),
                   Value = i.Id
               });

            var county = context.Counties
                .Select(i => new KeyValue<DbGeography, long>
                {
                    Key = i.CountyGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => g.Geography.GeographyPolygon).FirstOrDefault(),
                    Value = i.Id
                });

            var place = context.CityCountyMappings
                .Select(i => new KeyValue<DbGeography, long>
                {
                    Key = i.City.CityGeographies.Where(cg => cg.GeographyClass.Name == "Calculation").Select(cg => cg.Geography.GeographyPolygon).FirstOrDefault().Intersection(i.County.CountyGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => g.Geography.GeographyPolygon).FirstOrDefault()),
                    Value = i.Id
                });


            var metro = context.Metroes
                .Select(i => new KeyValue<DbGeography, long>
                {
                    Key = i.MetroGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => g.Geography.GeographyPolygon).FirstOrDefault(),
                    Value = i.Id
                });

            var state = context.States
                .Select(i => new KeyValue<DbGeography, long>
                {
                    Key = i.StateGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => g.Geography.GeographyPolygon).FirstOrDefault(),
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

        public static Models.LatLng Centroid(SizeUpContext context, long id, Granularity granularity)
        {
            var data = CalculationPolygon(context, id, granularity)
                .Where(i => i.Value == id)
                .Select(i => DbGeometry.FromBinary(i.Key.AsBinary()).ConvexHull.Centroid)
                .Select(i => DbGeography.FromBinary(i.AsBinary()))
                .Select(i => new Models.LatLng
                {
                    Lat = (double)i.Latitude,
                    Lng = (double)i.Longitude
                })
                .FirstOrDefault();
            return data;
        }

        public static Models.BoundingBox BoundingBox(SizeUpContext context, long id, Granularity granularity)
        {

            var data = CalculationPolygon(context, id, granularity)
                .Where(i => i.Value == id)
                .Select(i => DbGeometry.FromBinary(i.Key.AsBinary()).Envelope)
                .Select(i => DbGeography.FromBinary(i.AsBinary()))
                .Select(i => new Models.BoundingBox
                {
                    SouthWest = new Models.LatLng
                    {
                        Lat = (double)i.PointAt(1).Latitude,
                        Lng = (double)i.PointAt(1).Longitude
                    },
                    NorthEast = new Models.LatLng
                    {
                        Lat = (double)i.PointAt(3).Latitude,
                        Lng = (double)i.PointAt(3).Longitude
                    }
                })
                .FirstOrDefault();
            return data; 
        }
    }
}
