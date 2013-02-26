using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.DataAccess;
using SizeUp.Core;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using SizeUp.Core.Extensions;
using SizeUp.Web.Areas.Api.Models;


namespace SizeUp.Web.Areas.Api.Controllers
{
    public class AverageRevenueController : BaseController
    {
        //
        // GET: /Api/AverageRevenue/

        public ActionResult AverageRevenue(long industryId, long placeId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = context.CityCountyMappings
                    .Where(i => i.Id == placeId)
                    .Select(i => new
                    {
                        City = i.City.IndustryDataByCities
                            .Where(d => d.IndustryId == industryId)
                            .Where(d => d.AverageRevenue != null && d.AverageRevenue > 0)
                            .Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter)
                            .Where(d => i.City.BusinessDataByCities.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= 3)   
                            .Select(d => new 
                            {
                                Value = (long)d.AverageRevenue,
                                Name = i.City.Name + ", " + i.City.State.Abbreviation
                            }).FirstOrDefault(),

                        County = i.County.IndustryDataByCounties
                            .Where(d => d.IndustryId == industryId)
                            .Where(d => d.AverageRevenue != null && d.AverageRevenue > 0)
                            .Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter)
                            .Where(d => i.County.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= 3)   
                            .Select(d => new
                            {
                                Value = (long)d.AverageRevenue,
                                Name = i.County.Name + ", " + i.City.State.Abbreviation
                            }).FirstOrDefault(),

                        Metro = i.County.Metro.IndustryDataByMetroes
                            .Where(d => d.IndustryId == industryId)
                            .Where(d => d.AverageRevenue != null && d.AverageRevenue > 0)
                            .Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter)
                            .Where(d => i.County.Metro.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= 3)   
                            .Select(d => new
                            {
                                Value = (long)d.AverageRevenue,
                                Name = i.County.Metro.Name
                            }).FirstOrDefault(),
                        
                        State = i.County.State.IndustryDataByStates
                            .Where(d => d.IndustryId == industryId)
                            .Where(d => d.AverageRevenue != null && d.AverageRevenue > 0)
                            .Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter)
                            .Where(d => i.County.State.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= 3)   
                            .Select(d => new
                            {
                                Value = (long)d.AverageRevenue,
                                Name = i.City.State.Name
                            }).FirstOrDefault(),

                        Nation = context.IndustryDataByNations
                            .Where(d => d.IndustryId == industryId)
                            .Where(d => d.AverageRevenue != null && d.AverageRevenue > 0)
                            .Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter)
                            .Where(d => context.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= 3)   
                            .Select(d => new
                            {
                                Value = (long)d.AverageRevenue,
                                Median = (long)d.MedianRevenue,
                                Name = "USA"
                            }).FirstOrDefault()
                    
                    
                    }).FirstOrDefault();


                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Percentile(long industryId, double value)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var revenues = BusinessData.GetByNation(context, industryId)
                    .Where(i => i.Revenue != null)
                    .Select(i => i.Revenue);


                var percentile = Core.DataAccess.Math.Percentile(revenues, (long)value);

                var obj = new
                {
                    Percentile = percentile
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult BandsByZip(long industryId, int bands, string boundingEntityId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                BoundingEntity boundingEntity = new BoundingEntity(boundingEntityId);

                var zips = ZipCodes.GetBounded(context, boundingEntity)
                    .Select(i => i.Id);

                var data = IndustryData.GetZipCodes(context, industryId)
                    .Where(i => i.AverageRevenue > 0)
                    .Join(zips, i => i.ZipCodeId, i => i, (i, o) => i)
                    .Select(i => i.AverageRevenue)
                    .ToList()
                    .NTile(i => i, bands)
                    .Select(b => new Models.AverageRevenue.Band() { Min = b.Min(i => i), Max = b.Max(i => i) })
                    .ToList();

                Models.AverageRevenue.Band old = null;
                foreach (var band in data)
                {
                    if (old != null)
                    {
                        old.Max = band.Min;
                    }
                    old = band;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BandsByCounty(long industryId, int bands, string boundingEntityId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                BoundingEntity boundingEntity = new BoundingEntity(boundingEntityId);

                var ids = Counties.GetBounded(context, boundingEntity)
                    .Select(i => i.Id);

                var data = IndustryData.GetCounties(context, industryId)
                    .Where(i => i.AverageRevenue > 0)
                    .Join(ids, i => i.CountyId, i => i, (i, o) => i)
                    .Select(i => i.AverageRevenue)
                    .ToList()
                    .NTile(i => i, bands)
                    .Select(b => new Models.AverageRevenue.Band() { Min = b.Min(i => i), Max = b.Max(i => i) })
                    .ToList();

                Models.AverageRevenue.Band old = null;
                foreach (var band in data)
                {
                    if (old != null)
                    {
                        old.Max = band.Min;
                    }
                    old = band;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BandsByState(long industryId, int bands)
        {
            using (var context = ContextFactory.SizeUpContext)
            {

                var data = IndustryData.GetStates(context, industryId)
                    .Where(i => i.AverageRevenue > 0)
                    .Select(i => i.AverageRevenue)
                    .ToList()
                    .NTile(i => i, bands)
                    .Select(b => new Models.AverageRevenue.Band() { Min = b.Min(i => i), Max = b.Max(i => i) })
                    .ToList();

                Models.AverageRevenue.Band old = null;
                foreach (var band in data)
                {
                    if (old != null)
                    {
                        old.Max = band.Min;
                    }
                    old = band;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
