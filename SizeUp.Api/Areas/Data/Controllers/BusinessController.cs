﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using System.Linq.Expressions;
using System.Data.Objects.SqlClient;
using System.Data.Spatial;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using SizeUp.Core.DataLayer;
using System.Configuration;
using SizeUp.Api.Controllers;
using SizeUp.Core.API;
using System.Data.Objects;
namespace SizeUp.Api.Areas.Data.Controllers
{
    public class BusinessController : BaseController
    {
        //
        // GET: /Api/Business/

        
        [APIAuthorize(Role = "Business")]
        public ActionResult Index(long id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Business.Get(context, id);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        
        [APIAuthorize(Role = "Business")]
        public ActionResult At(List<long> industryIds, float lat, float lng)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Business.GetAt(context, new LatLng() { Lat = lat, Lng = lng }, industryIds);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }


        
        [APIAuthorize(Role = "Business")]
        public ActionResult List(List<long> industryIds, long geographicLocationId, int itemCount = 10, int page = 1, int radius = 100, int? employeesMin = -1, int? employeesMax = -1)
        {
            int maxResults = int.Parse(ConfigurationManager.AppSettings["Data.Business.MaxResults"]);
            itemCount = Math.Min(maxResults, itemCount);
            using (var context = ContextFactory.SizeUpContext)
            {
                var centroid = Core.DataLayer.Geography.Get(context)
                    .Where(i => i.GeographicLocationId == geographicLocationId)
                    .Where(i => i.GeographyClass.Name == Core.Geo.GeographyClass.Calculation)
                    .Select(new Core.DataLayer.Projections.Geography.Centroid().Expression)
                    .Select(i=>i.Value)
                    .FirstOrDefault();
  
                var data = Core.DataLayer.Business.ListNear(context, centroid, industryIds)
                    .Where(i => i.Distance < radius)
                    .Join(Core.DataLayer.BusinessData.Get(context).Where(x => (x.Employees >= (employeesMin) || employeesMin == -1) && (x.Employees <= (employeesMax) || employeesMax== -1)), x => x.Entity.Id, y => y.BusinessId, (x, y) => new { DistanceEntity = x, bd = y })
                    .Select(i => i.DistanceEntity)                    
                    .Distinct()
                    .OrderBy(i => i.Distance)
                    .ThenBy(i => i.Entity.Name)
                    .Select(i => i.Entity);

                var output = new
                {
                    Page = page,
                    Count = data.Count(),
                    Items = data.Skip((page-1) * itemCount).Take(itemCount).ToList()
                };

                return Json(output, JsonRequestBehavior.AllowGet);
            }
           
        }

       

    }
}
