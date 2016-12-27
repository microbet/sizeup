using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Data.Analytics;
using SizeUp.Core.API;
using SizeUp.Core.Email;
using SizeUp.Core.Identity;

namespace SizeUp.Core.Analytics
{
    public class Tracker
    {
        public void UserRegistration(UserRegistration reg)
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
            reg.Session = APIContext.Current.Session;
            reg.APIKeyId = APIContext.Current.ApiToken != null ? APIContext.Current.ApiToken.APIKeyId : (long?)null;
            reg.WidgetAPIKeyId = APIContext.Current.WidgetToken != null ? APIContext.Current.WidgetToken.APIKeyId : (long?)null;

            Task.Factory.StartNew(() =>
            {
                using (var context = ContextFactory.AnalyticsContext)
                {
                    context.UserRegistrations.AddObject(reg);
                    context.SaveChanges();
                }
            });
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
            reg.Session = APIContext.Current.Session;
            Task.Factory.StartNew(() =>
            {
                using (var context = ContextFactory.AnalyticsContext)
                {
                    context.LongRequests.AddObject(reg);
                    context.SaveChanges();
                }
            });
        }

        public void Exception(Data.Analytics.Exception reg)
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
            reg.Session = APIContext.Current.Session;
            Task.Factory.StartNew(() =>
            {
                using (var context = ContextFactory.AnalyticsContext)
                {
                    Mailer mail = new Mailer();
                    context.Exceptions.AddObject(reg);
                    context.SaveChanges();
                }
            });
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
            reg.Session = APIContext.Current.Session;


            Task.Factory.StartNew(() =>
            {
                using (var context = ContextFactory.AnalyticsContext)
                {
                    context.RelatedCompetitors.AddObject(reg);
                    context.SaveChanges();
                }
            });
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
            reg.Session = APIContext.Current.Session;

            Task.Factory.StartNew(() =>
            {
                using (var context = ContextFactory.AnalyticsContext)
                {
                    context.RelatedBuyers.AddObject(reg);
                    context.SaveChanges();
                }
            });
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
            reg.Session = APIContext.Current.Session;

            Task.Factory.StartNew(() =>
            {
                using (var context = ContextFactory.AnalyticsContext)
                {
                    context.RelatedSuppliers.AddObject(reg);
                    context.SaveChanges();
                }
            });
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
            reg.Session = APIContext.Current.Session;
            reg.WidgetAPIKeyId = APIContext.Current.WidgetToken != null ? APIContext.Current.WidgetToken.APIKeyId : (long?)null;

            Task.Factory.StartNew(() =>
            {
                using (var context = ContextFactory.AnalyticsContext)
                {
                    context.BusinessAttributes.AddObject(reg);
                    context.SaveChanges();
                }
            });
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
            reg.Session = APIContext.Current.Session;
            reg.WidgetAPIKeyId = APIContext.Current.WidgetToken != null ? APIContext.Current.WidgetToken.APIKeyId : (long?)null;

            Task.Factory.StartNew(() =>
            {
                using (var context = ContextFactory.AnalyticsContext)
                {
                    context.CompetitorAttributes.AddObject(reg);
                    context.SaveChanges();
                }
            });
        }

        public void APIRequest(APIRequest reg)
        {
            TimeStamp stamp = new TimeStamp();
            reg.Day = stamp.Day;
            reg.Hour = stamp.Hour;
            reg.Minute = stamp.Minute;
            reg.Second = stamp.Second;
            reg.Month = stamp.Month;
            reg.Quarter = stamp.Quarter;
            reg.Year = stamp.Year;
            reg.Week = stamp.Week;
            reg.Timestamp = stamp.Stamp;
            reg.Session = APIContext.Current.Session;
            reg.Instance = APIContext.Current.Instance;
            reg.APIKeyId = APIContext.Current.ApiToken != null ? APIContext.Current.ApiToken.APIKeyId : (long?)null;
            reg.WidgetAPIKeyId = APIContext.Current.WidgetToken != null ? APIContext.Current.WidgetToken.APIKeyId : (long?)null;

            Task.Factory.StartNew(() =>
            {
                using (var context = ContextFactory.AnalyticsContext)
                {
                    context.APIRequests.AddObject(reg);
                    context.SaveChanges();
                }
            });
        }

        public void PageView(PageView reg)
        {
            TimeStamp stamp = new TimeStamp();
            reg.Day = stamp.Day;
            reg.Hour = stamp.Hour;
            reg.Minute = stamp.Minute;
            reg.Second = stamp.Second;
            reg.Month = stamp.Month;
            reg.Quarter = stamp.Quarter;
            reg.Year = stamp.Year;
            reg.Week = stamp.Week;
            reg.Timestamp = stamp.Stamp;
            reg.Session = APIContext.Current.Session;
            reg.WidgetAPIKeyId = APIContext.Current.WidgetToken != null ? APIContext.Current.WidgetToken.APIKeyId : (long?)null;

            Task.Factory.StartNew(() =>
            {
                using (var context = ContextFactory.AnalyticsContext)
                {
                    context.PageViews.AddObject(reg);
                    context.SaveChanges();
                }
            });
        }

        public void AdvertisingReportLoaded(AdvertisingAttribute reg)
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
            reg.Session = APIContext.Current.Session;
            reg.WidgetAPIKeyId = APIContext.Current.WidgetToken != null ? APIContext.Current.WidgetToken.APIKeyId : (long?)null;

            Task.Factory.StartNew(() =>
            {
                using (var context = ContextFactory.AnalyticsContext)
                {
                    context.AdvertisingAttributes.AddObject(reg);
                    context.SaveChanges();
                }
            });
        }

        public void IndustrySubscriptionsUpdated(Guid userId, List<IndustrySubscription> industrySubscriptions)
        {
            Task.Factory.StartNew(() =>
            {
                using (var context = ContextFactory.AnalyticsContext)
                {
                    foreach (var industry in context.IndustrySubscriptions.Where(x => x.UserId == userId))
                    {
                        context.IndustrySubscriptions.DeleteObject(industry);
                    }

                    industrySubscriptions.ForEach( i => {
                        context.IndustrySubscriptions.AddObject(i);
                    });
                    
                    context.SaveChanges();
                }
            });
        }
    }
}
