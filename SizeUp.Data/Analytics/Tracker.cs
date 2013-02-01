using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;

namespace SizeUp.Data.Analytics
{
    public class Tracker
    {
        public void UserRegisteration(UserRegistration reg)
        {
            TimeStamp stamp = new TimeStamp();
            reg.Day = stamp.Day;
            reg.Hour = stamp.Hour;
            reg.Minute = stamp.Minute;
            reg.Month = stamp.Month;
            reg.Quarter = stamp.Quarter;
            reg.Year = stamp.Year;
            reg.Week = stamp.Week;
            reg.Timestamp = stamp.Stamp;
            using (var context = ContextFactory.AnalyticsContext)
            {
                context.UserRegistrations.AddObject(reg);
                context.SaveChanges();
            }
        }

        public void LongRequest(LongRequest reg)
        {
            TimeStamp stamp = new TimeStamp();
            reg.Day = stamp.Day;
            reg.Hour = stamp.Hour;
            reg.Minute = stamp.Minute;
            reg.Month = stamp.Month;
            reg.Quarter = stamp.Quarter;
            reg.Year = stamp.Year;
            reg.Week = stamp.Week;
            reg.Timestamp = stamp.Stamp;
            using (var context = ContextFactory.AnalyticsContext)
            {
                context.LongRequests.AddObject(reg);
                context.SaveChanges();
            }
        }

        public void Exception(Exception reg)
        {
            TimeStamp stamp = new TimeStamp();
            reg.Day = stamp.Day;
            reg.Hour = stamp.Hour;
            reg.Minute = stamp.Minute;
            reg.Month = stamp.Month;
            reg.Quarter = stamp.Quarter;
            reg.Year = stamp.Year;
            reg.Week = stamp.Week;
            reg.Timestamp = stamp.Stamp;
            using (var context = ContextFactory.AnalyticsContext)
            {
                context.Exceptions.AddObject(reg);
                context.SaveChanges();
            }
        }

        public void PlaceIndustrySearch(PlaceIndustrySearch reg)
        {
            TimeStamp stamp = new TimeStamp();
            reg.Day = stamp.Day;
            reg.Hour = stamp.Hour;
            reg.Minute = stamp.Minute;
            reg.Month = stamp.Month;
            reg.Quarter = stamp.Quarter;
            reg.Year = stamp.Year;
            reg.Week = stamp.Week;
            reg.Timestamp = stamp.Stamp;
            using (var context = ContextFactory.AnalyticsContext)
            {
                context.PlaceIndustrySearches.AddObject(reg);
                context.SaveChanges();
            }
        }


        public void RelatedCompetitor(RelatedCompetitor reg)
        {
            TimeStamp stamp = new TimeStamp();
            reg.Day = stamp.Day;
            reg.Hour = stamp.Hour;
            reg.Minute = stamp.Minute;
            reg.Month = stamp.Month;
            reg.Quarter = stamp.Quarter;
            reg.Year = stamp.Year;
            reg.Week = stamp.Week;
            reg.Timestamp = stamp.Stamp;
            using (var context = ContextFactory.AnalyticsContext)
            {
                context.RelatedCompetitors.AddObject(reg);
                context.SaveChanges();
            }
        }

        public void RelatedBuyer(RelatedBuyer reg)
        {
            TimeStamp stamp = new TimeStamp();
            reg.Day = stamp.Day;
            reg.Hour = stamp.Hour;
            reg.Minute = stamp.Minute;
            reg.Month = stamp.Month;
            reg.Quarter = stamp.Quarter;
            reg.Year = stamp.Year;
            reg.Week = stamp.Week;
            reg.Timestamp = stamp.Stamp;
            using (var context = ContextFactory.AnalyticsContext)
            {
                context.RelatedBuyers.AddObject(reg);
                context.SaveChanges();
            }
        }

        public void RelatedSupplier(RelatedSupplier reg)
        {
            TimeStamp stamp = new TimeStamp();
            reg.Day = stamp.Day;
            reg.Hour = stamp.Hour;
            reg.Minute = stamp.Minute;
            reg.Month = stamp.Month;
            reg.Quarter = stamp.Quarter;
            reg.Year = stamp.Year;
            reg.Week = stamp.Week;
            reg.Timestamp = stamp.Stamp;
            using (var context = ContextFactory.AnalyticsContext)
            {
                context.RelatedSuppliers.AddObject(reg);
                context.SaveChanges();
            }
        }

        public void BusinessAttribute(BusinessAttribute reg)
        {
            TimeStamp stamp = new TimeStamp();
            reg.Day = stamp.Day;
            reg.Hour = stamp.Hour;
            reg.Minute = stamp.Minute;
            reg.Month = stamp.Month;
            reg.Quarter = stamp.Quarter;
            reg.Year = stamp.Year;
            reg.Week = stamp.Week;
            reg.Timestamp = stamp.Stamp;
            using (var context = ContextFactory.AnalyticsContext)
            {
                context.BusinessAttributes.AddObject(reg);
                context.SaveChanges();
            }
        }

        public void CompetitorAttribute(CompetitorAttribute reg)
        {
            TimeStamp stamp = new TimeStamp();
            reg.Day = stamp.Day;
            reg.Hour = stamp.Hour;
            reg.Minute = stamp.Minute;
            reg.Month = stamp.Month;
            reg.Quarter = stamp.Quarter;
            reg.Year = stamp.Year;
            reg.Week = stamp.Week;
            reg.Timestamp = stamp.Stamp;
            using (var context = ContextFactory.AnalyticsContext)
            {
                context.CompetitorAttributes.AddObject(reg);
                context.SaveChanges();
            }
        }
    }
}
