using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.Analytics
{
    public class TimeStamp
    {
        public TimeStamp(DateTime dateTime)
        {
            Stamp = dateTime;
        }

        public TimeStamp() : this(DateTime.UtcNow)
        {

        }



        public int Second { get { return Stamp.Second; } }
        public int Minute { get { return Stamp.Minute; } }
        public int Hour { get { return Stamp.Hour; } }
        public int Day { get { return Stamp.Day; } }
        public int Week { get { return new System.Globalization.GregorianCalendar().GetWeekOfYear(Stamp, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Sunday); } }
        public int Month { get { return Stamp.Month; } }
        public int Quarter { get { return (Stamp.Month - 1) / 3 + 1; } }
        public int Year { get { return Stamp.Year; } }
        public DateTime Stamp { get; set; }

    }
}
