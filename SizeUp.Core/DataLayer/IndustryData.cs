﻿using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Models;

namespace SizeUp.Core.DataLayer
{
    public class IndustryData
    {
        public static IQueryable<Data.IndustryData> Get(SizeUpContext context)
        {
            return context.IndustryDatas
                .Where(i => i.Year == CommonFilters.TimeSlice.Industry.Year && i.Quarter == CommonFilters.TimeSlice.Industry.Quarter && i.Industry.IsActive && !i.Industry.IsDisabled);
        }


        public static IQueryable<Data.IndustryData> Get(SizeUpContext context, Granularity granularity)
        {
            var gran = Enum.GetName(typeof(Granularity), granularity);
            return Get(context)
                .Where(i => i.GeographicLocation.Granularity.Name == gran);
        }

        public static IQueryable<Data.IndustryData> Get(SizeUpContext context, Granularity granularity, long placeId, Granularity boundingGranularity)
        {
            var data = Get(context, granularity);
            var place = Core.DataLayer.Place.Get(context)
                .Where(i => i.Id == placeId)
                .FirstOrDefault();

            if (boundingGranularity == Granularity.City)
            {
                data = data.Where(i => i.GeographicLocation.GeographicLocations.Any(g=>g.Id == place.CityId));
            }
            else if (boundingGranularity == Granularity.County)
            {
                data = data.Where(i => i.GeographicLocation.GeographicLocations.Any(g => g.Id == place.CountyId));
            }
            else if (boundingGranularity == Granularity.Metro)
            {
                data = data.Where(i => i.GeographicLocation.GeographicLocations.Any(g => g.Id == place.County.MetroId));
            }
            else if (boundingGranularity == Granularity.State)
            {
                data = data.Where(i => i.GeographicLocation.GeographicLocations.Any(g => g.Id == place.County.StateId));
            }
            else if (boundingGranularity == Granularity.Nation)
            {
                data = data.Where(i => i.GeographicLocation.GeographicLocations.Any(g => g.Id == place.County.State.NationId));
            }
            return data;
        }
    }
}
