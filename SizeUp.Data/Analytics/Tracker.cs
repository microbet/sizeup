using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Data.Analytics
{
    public class Tracker
    {
        protected int Second { get { return DateTime.Now.Second; } }
        protected int Minute { get { return DateTime.Now.Minute; } }
        protected int Hour { get { return DateTime.Now.Hour; } }
        protected int Day { get { return DateTime.Now.Day; } }
        protected int Week { get { return new System.Globalization.GregorianCalendar().GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Sunday); } }
        protected int Month { get { return DateTime.Now.Month; } }
        protected int Quarter { get { return (DateTime.Now.Month - 1) / 3 + 1; } }
        protected int Year { get { return DateTime.Now.Year; } }

        public void UserRegisteration(UserRegistration reg)
        {
            reg.Day = Day;
            reg.Hour = Hour;
            reg.Minute = Minute;
            reg.Month = Month;
            reg.Quarter = Quarter;
            reg.Year = Year;
            reg.Week = Week;
            reg.Timestamp = DateTime.Now;
            DataContexts.AnalyticsContext.UserRegistrations.AddObject(reg);
            DataContexts.AnalyticsContext.SaveChanges();
        }
    }
}
