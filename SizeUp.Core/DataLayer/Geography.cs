﻿using System;
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
    }
}
