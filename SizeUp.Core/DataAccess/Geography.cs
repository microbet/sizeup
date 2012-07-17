using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core.DataAccess.Models;
using System.Drawing.Drawing2D;
using System.Drawing;
using SizeUp.Data;
using SizeUp.Core.Geo;

namespace SizeUp.Core.DataAccess
{
    public static class Geography
    {

        public static IQueryable<DisplayGeography> GetDisplayZips(SizeUpContext context, IQueryable<long> zipCodeIds)
        {
            return context.ZipCodeGeographies
                .Join(zipCodeIds, i => i.ZipCodeId, i => i, (i, o) => i)
                .Where(i => i.GeographyClass.Name == "Display")
                .Select(i => new DisplayGeography { Id = i.ZipCodeId, Geography = i.Geography.GeographyPolygon });
        }

        public static IQueryable<DisplayGeography> GetDisplayCities(SizeUpContext context, IQueryable<long> cityIds)
        {
            return context.CityGeographies
                .Join(cityIds, i => i.CityId, i => i, (i, o) => i)
                .Where(i => i.GeographyClass.Name == "Display")
                .Select(i => new DisplayGeography { Id = i.CityId, Geography = i.Geography.GeographyPolygon });
        }

        public static IQueryable<DisplayGeography> GetDisplayCounties(SizeUpContext context, IQueryable<long> countyIds)
        {
            return context.CountyGeographies
                .Join(countyIds, i => i.CountyId, i => i, (i, o) => i)
                .Where(i => i.GeographyClass.Name == "Display")
                .Select(i => new DisplayGeography { Id = i.CountyId, Geography = i.Geography.GeographyPolygon });
        }

        public static IQueryable<DisplayGeography> GetDisplayMetros(SizeUpContext context, IQueryable<long> metroIds)
        {
            return context.MetroGeographies
                .Join(metroIds, i => i.MetroId, i => i, (i, o) => i)
                .Where(i => i.GeographyClass.Name == "Display")
                .Select(i => new DisplayGeography { Id = i.MetroId, Geography = i.Geography.GeographyPolygon });
        }

        public static IQueryable<DisplayGeography> GetDisplayStates(SizeUpContext context, IQueryable<long> stateIds)
        {
            return context.StateGeographies
                .Join(stateIds, i => i.StateId, i => i, (i, o) => i)
                .Where(i => i.GeographyClass.Name == "Display")
                .Select(i => new DisplayGeography { Id = i.StateId, Geography = i.Geography.GeographyPolygon });
        }




        public static IQueryable<ZipCodeGeography> GetBoundingBoxedZips(SizeUpContext context, BoundingBox BoundingBox)
        {
            return context.ZipCodeGeographies
                   .Where(i => i.GeographyClass.Name == "Calculation")
                   .Where(i =>
                       i.Geography.West < BoundingBox.NorthEast.X &&
                       i.Geography.East > BoundingBox.SouthWest.X &&
                       i.Geography.South < BoundingBox.NorthEast.Y &&
                       i.Geography.North > BoundingBox.SouthWest.Y);
        }

        public static IQueryable<CityGeography> GetBoundingBoxedCities(SizeUpContext context, BoundingBox BoundingBox)
        {
            return context.CityGeographies
                   .Where(i => i.GeographyClass.Name == "Calculation")
                   .Where(i =>
                        i.Geography.West < BoundingBox.NorthEast.X &&
                       i.Geography.East > BoundingBox.SouthWest.X &&
                       i.Geography.South < BoundingBox.NorthEast.Y &&
                       i.Geography.North > BoundingBox.SouthWest.Y);
        }

        public static IQueryable<CountyGeography> GetBoundingBoxedCounties(SizeUpContext context, BoundingBox BoundingBox)
        {
            return context.CountyGeographies
                   .Where(i => i.GeographyClass.Name == "Calculation")
                   .Where(i =>
                        i.Geography.West < BoundingBox.NorthEast.X &&
                       i.Geography.East > BoundingBox.SouthWest.X &&
                       i.Geography.South < BoundingBox.NorthEast.Y &&
                       i.Geography.North > BoundingBox.SouthWest.Y);
        }

        public static IQueryable<MetroGeography> GetBoundingBoxedMetros(SizeUpContext context, BoundingBox BoundingBox)
        {
            return context.MetroGeographies
                   .Where(i => i.GeographyClass.Name == "Calculation")
                   .Where(i =>
                        i.Geography.West < BoundingBox.NorthEast.X &&
                       i.Geography.East > BoundingBox.SouthWest.X &&
                       i.Geography.South < BoundingBox.NorthEast.Y &&
                       i.Geography.North > BoundingBox.SouthWest.Y);
        }

        public static IQueryable<StateGeography> GetBoundingBoxedStates(SizeUpContext context, BoundingBox BoundingBox)
        {
            return context.StateGeographies
                   .Where(i => i.GeographyClass.Name == "Calculation")
                   .Where(i =>
                       i.Geography.West < BoundingBox.NorthEast.X &&
                       i.Geography.East > BoundingBox.SouthWest.X &&
                       i.Geography.South < BoundingBox.NorthEast.Y &&
                       i.Geography.North > BoundingBox.SouthWest.Y);
        }






    }
}
