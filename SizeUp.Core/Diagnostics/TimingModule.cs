using System;
using System.Diagnostics;
using System.Web;
using SizeUp.Data;
using SizeUp.Data.Analytics;

namespace SizeUp.Core.Diagnostics
{
    public class TimingModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
            context.EndRequest += OnEndRequest;
        }

        void OnBeginRequest(object sender, System.EventArgs e)
        {
            var stopwatch = new Stopwatch();
            HttpContext.Current.Items["Stopwatch"] = stopwatch;
            stopwatch.Start();
        }

        void OnEndRequest(object sender, System.EventArgs e)
        {
            Stopwatch stopwatch = (Stopwatch)HttpContext.Current.Items["Stopwatch"];
            try
            {
                stopwatch.Stop();
                TimeSpan ts = stopwatch.Elapsed;
                if (ts.TotalMilliseconds > 1500)
                {
                    using (var context = ContextFactory.AnalyticsContext)
                    {
                        LongRequest reg = new LongRequest()
                        {

                            RequestUrl = HttpContext.Current.Request.Url.OriginalString,
                            RequestTime = (int)ts.TotalMilliseconds
                        };

                        Singleton<Tracker>.Instance.LongRequest(reg);
                    }
                }
            }
            catch (Exception) { }
        }
    }
}
