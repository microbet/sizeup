using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using SizeUp.Core;
using SizeUp.Data;
using SizeUp.Core.DataAccess.Models;
using SizeUp.Core.Geo;
using System.Data.Objects;

namespace SizeUp.Core.DataAccess
{
    //not too terribly proud of this but.....
    public static class ConsumerExpenditureData
    {
        //this takes a build query and runs it after appending timeslice info for year and quarter to the query filters
        private static IQueryable<ConsumerExpenditureBandItem> RunQuery(SizeUpContext context, string query, ObjectParameter[] parameters = null)
        {
            if (parameters == null)
            {
                parameters = new ObjectParameter[0];
            }
            query = string.Format("{0} and cs.Year = {1} and cs.Quarter = {2}", query, TimeSlice.Year, TimeSlice.Quarter);
            var data = context.CreateQuery<DbDataRecord>(query, parameters)
               .ToList()
               .Select(i => new ConsumerExpenditureBandItem()
               {
                   EntityId = Convert.ToInt64(i["EntityId"]),
                   Value = Convert.ToDouble(i["Value"])
               });
            return data.AsQueryable();
        }

        //builds query for consumer expenditure data on a state aggrigation level only gets one variable at a time
        public static IQueryable<ConsumerExpenditureBandItem> GetStates(SizeUpContext context, string variableName)
        {
            string CSSelect = @"SELECT cs.{0} as Value, cs.StateId as EntityId FROM SizeUpContext.ConsumerExpendituresByStates as cs
            WHERE cs.{0} is not null and cs.{0} > 0";
            string query = string.Format(CSSelect, variableName);
            var data = RunQuery(context, query);
            return data.AsQueryable();
        }

        //builds query for consumer expenditure data on a county aggrigation level only gets one variable at a time also filters by bounding entity
        public static IQueryable<ConsumerExpenditureBandItem> GetCounties(SizeUpContext context, string variableName, BoundingEntity boundingEntity = null)
        {
            string CSSelect = @"SELECT cs.{0} as Value, cs.CountyId as EntityId FROM SizeUpContext.ConsumerExpendituresByCounties as cs {1}
            WHERE cs.{0} is not null and cs.{0} > 0 {2}";

            string join = @"inner join SizeUpContext.Counties as co
                            on co.id = cs.countyid
                            inner join SizeUpContext.Metroes  as m
                            on m.id = co.metroid
                            inner join SizeUpContext.States as s
                            on s.id = co.stateid";

            string where = " and 1=1";

            if (boundingEntity != null && boundingEntity.EntityType == BoundingEntity.BoundingEntityType.State)
            {
                where = string.Format(" and s.id = {0}", boundingEntity.EntityId);
            }
            else if (boundingEntity != null && boundingEntity.EntityType == BoundingEntity.BoundingEntityType.Metro)
            {
                where = string.Format(" and m.id = {0}", boundingEntity.EntityId);
            }
            else if (boundingEntity != null && boundingEntity.EntityType == BoundingEntity.BoundingEntityType.County)
            {
                where = string.Format(" and co.id = {0}", boundingEntity.EntityId);
            }

            string query = string.Format(CSSelect, variableName, join, where);
            var data = RunQuery(context, query);
            return data.AsQueryable();
        }


        //builds query for consumer expenditure data on a zipcode aggrigation level only gets one variable at a time also filters by bounding entity
        public static IQueryable<ConsumerExpenditureBandItem> GetZips(SizeUpContext context, string variableName, BoundingEntity boundingEntity = null)
        {
            string CSSelect = @"SELECT cs.{0} as Value, cs.ZipCodeId as EntityId FROM SizeUpContext.ConsumerExpendituresByZips as cs {1}
            WHERE cs.{0} is not null and cs.{0} > 0 {2}";

            string join = @"inner join SizeUpContext.ZipCodeCountyMappings as zco
                            on zco.zipcodeid = cs.ZipCodeId
                            inner join SizeUpContext.Counties as co
                            on co.id = zco.countyid
                            inner join SizeUpContext.Metroes  as m
                            on m.id = co.metroid
                            inner join SizeUpContext.States as s
                            on s.id = co.stateid";

            string where = " and 1=1";

            if (boundingEntity != null && boundingEntity.EntityType == BoundingEntity.BoundingEntityType.State)
            {
                where = string.Format(" and s.id = {0}", boundingEntity.EntityId);
            }
            else if (boundingEntity != null && boundingEntity.EntityType == BoundingEntity.BoundingEntityType.Metro)
            {
                where = string.Format(" and m.id = {0}", boundingEntity.EntityId);
            }
            else if (boundingEntity != null && boundingEntity.EntityType == BoundingEntity.BoundingEntityType.County)
            {
                where = string.Format(" and co.id = {0}", boundingEntity.EntityId);
            }

            string query = string.Format(CSSelect, variableName, join, where);
            var data = RunQuery(context, query);
            return data.AsQueryable();
        }
    }
}
