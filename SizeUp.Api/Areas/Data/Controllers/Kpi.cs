using SizeUp.Data;
using SizeUp.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace SizeUp.Api.Areas.Data.Controllers
{
    public class Kpi
    {
        // Band, FormatBands, and Format are salvaged (copied) from SizeUp.Web:AccessibilityController.cs).

        public class Band
        {
            public string Min { get; set; }
            public string Max { get; set; }
            public List<string> Items { get; set; }

        }

        public class LabeledValue
        {
            public string Label { get; set; }
            public long? Value { get; set; }
        }

        public static List<Band> GetKpiBands(
            SizeUpContext context, long industryId, long locationId, string granularityName,
            Expression<Func<IndustryData, bool>> filter,
            Expression<Func<IndustryData, LabeledValue>> selector, int numBands
        ) {
            return Core.DataLayer.IndustryData.Get(context)
                .Where(i => i.GeographicLocation.GeographicLocations.Any(gl => gl.Id == locationId))
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.GeographicLocation.Granularity.Name == granularityName)
                .Where(filter)
                .Select(selector)
                .ToList()
                .NTileDescending(i => i.Value, numBands)
                .Select(i => new Kpi.Band
                {
                    Min = string.Format("${0}", Kpi.Format(i.Min(v => v.Value.Value))),
                    Max = string.Format("${0}", Kpi.Format(i.Max(v => v.Value.Value))),
                    Items = i.Select(v => v.Label).ToList()
                })
                .ToList();
        }

        public static void testMethod(dynamic ViewBag)
        {
            return;
        }

        public static void GetKpiModel(
            dynamic ViewBag, SizeUpContext context,
            long industryId, long locationId, Core.DataLayer.Granularity granularity,
            Expression<Func<IndustryData, bool>> filter,
            Expression<Func<IndustryData, LabeledValue>> selector, string kpiName, int numBands
        ) {
            var gran = Enum.GetName(typeof(Core.DataLayer.Granularity), granularity);

            var data = Kpi.GetKpiBands(context, industryId, locationId, gran, filter, selector, numBands);

            // probably obsolete
            ViewBag.BoundingEntity = context.GeographicLocations
                .Where(i => i.Id == locationId)
                .Select(i => i.LongName)
                .FirstOrDefault();
            // current
            ViewBag.Area = ViewBag.BoundingEntity;
            ViewBag.Bands = Kpi.FormatBands(data);
            ViewBag.Industry = context.Industries
                .Where(i => i.Id == industryId)
                .Select(i => i.Name)
                .FirstOrDefault();
            ViewBag.Kpi = kpiName;
            ViewBag.LevelOfDetail = Kpi.TranslateGranularity(granularity);
            ViewBag.Q = numBands.ToString();
            ViewBag.ThemeUrl = System.Configuration.ConfigurationManager.AppSettings["Theme.Url"];

            ViewBag.Query = string.Format(
                "Rank {0} in {1}, by {2} of {3} businesses, in {4} quantiles",
                ViewBag.LevelOfDetail, ViewBag.Area, ViewBag.Kpi, ViewBag.Industry, ViewBag.Q
            );

        }

        public static List<Band> FormatBands(List<Band> bands)
        {
            Band old = null;
            foreach (var band in bands)
            {
                if (old != null)
                {
                    old.Max = band.Min;
                }
                old = band;
            }
            return bands;
        }

        public static string Format(double val)
        {
            string output = "";
            if (val < 10)
            {
                output = string.Format("{0:0.0}", System.Math.Round(val, 1));
            }
            else if (val >= 10 && val < 10000)
            {
                output = string.Format("{0:0}", System.Math.Round(val, 1));
            }
            else if (val >= 10000 && val < 1000000)
            {
                output = string.Format("{0:0.0}K", System.Math.Round((val / 1000), 1));
            }
            else if (val >= 1000000 && val < 1000000000)
            {
                output = string.Format("{0:0.0}M", System.Math.Round((val / 1000000), 1));
            }
            else if (val >= 1000000000)
            {
                output = string.Format("{0:0.0}B", System.Math.Round((val / 1000000000), 1));
            }
            return output;
        }

        public static string Format(long val)
        {
            string output = "";
            if (val < 10)
            {
                output = string.Format("{0:0}", val);
            }
            else if (val >= 10 && val < 10000)
            {
                output = string.Format("{0:0}", val);
            }
            else if (val >= 10000 && val < 1000000)
            {
                output = string.Format("{0:0.0}K", System.Math.Round(((double)val / 1000), 1));
            }
            else if (val >= 1000000 && val < 1000000000)
            {
                output = string.Format("{0:0.0}M", System.Math.Round(((double)val / 1000000), 1));
            }
            else if (val >= 1000000000)
            {
                output = string.Format("{0:0.0}B", System.Math.Round(((double)val / 1000000000), 1));
            }
            return output;
        }

        /**
         * Also lifted from AccessibilityController code. Not to endorse how this works, just
         * relocating the code verbatim for now.
         */
        public static string TranslateGranularity(Core.DataLayer.Granularity granularity)
        {
            if (granularity == Core.DataLayer.Granularity.ZipCode)
            {
                return "Zip Codes";
            }
            else if (granularity == Core.DataLayer.Granularity.County)
            {
                return "Counties";
            }
            else if (granularity == Core.DataLayer.Granularity.State)
            {
                return "States";
            }
            else
            {
                throw new KeyNotFoundException("Could not resolve granularity: " + granularity.ToString());
            }
        }
    }
}