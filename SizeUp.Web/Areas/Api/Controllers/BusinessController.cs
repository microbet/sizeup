using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using System.Linq.Expressions;
using System.Data.Objects.SqlClient;
using Microsoft.SqlServer.Types;
namespace SizeUp.Web.Areas.Api.Controllers
{
    public class BusinessController : Controller
    {
        //
        // GET: /Api/Business/

        public ActionResult Business(long id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var item = context.Businesses.Where(i => i.Id == id);
                var data = item.Select(i => new Models.Business.Business()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Lat = i.Lat,
                    Lng = i.Long,
                    Street = i.Address,
                    City = i.City,
                    State = i.State.Abbreviation,
                    Zip = i.ZipCode.Zip,
                    Phone = i.Phone,
                    Url = i.PrimaryWebURL
                }).FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BusinessAt(List<long> industryIds, float lat, float lng)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var scalar = 69.1 * Math.Cos(lat / 57.3);
                var year = DateTime.Now.Year;
                var item = context.Businesses.Where(i => industryIds.Contains(i.IndustryId.Value));
                item = item.Where(i => i.BusinessStatusCode != "1" || i.BusinessStatusCode != "3");
                var data = item.Select(i => new
                {
                    Distance = Math.Pow(Math.Pow(((double)i.Lat.Value - lat) * 69.1, 2) + Math.Pow(((double)i.Long.Value - lng) * scalar, 2), 0.5),
                    Business = new Models.Business.Business()
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Lat = i.Lat,
                        Lng = i.Long,
                        Street = i.Address,
                        City = i.City,
                        State = i.State.Abbreviation,
                        Zip = i.ZipCode.Zip,
                        Phone = i.Phone,
                        Url = i.PrimaryWebURL,
                        IsHomeBased = i.WorkAtHomeFlag == "1",
                        IsFirm = i.FirmCode == "2",
                        IsPublic = i.PublicCompanyIndicator == "1" || i.PublicCompanyIndicator == "2",
                        YearsInBusiness = year - (i.YearEstablished ?? i.YearAppeared) ?? null,
                        IndustryId = i.IndustryId
                    }
                })
                .OrderBy(i => i.Distance)
                .Select(i => i.Business)
                .FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BusinessList(List<long> industryIds, long cityId, int itemCount, int page = 1, int radius = 100)
        {
            if (!User.Identity.IsAuthenticated)
            {
                page = 1;
                itemCount = 3;
            }

            using (var context = ContextFactory.SizeUpContext)
            {
                var city = context.CityGeographies
                    .Where(i => i.CityId == cityId && i.GeographyClass.Name == "Calculation")
                    .Select(i => i.Geography.GeographyPolygon).FirstOrDefault();

                var geo = SqlGeography.Parse(city.AsText());
                var geom = SqlGeometry.STGeomFromWKB(geo.STAsBinary(), (int)geo.STSrid);
                geom = geom.STCentroid();
                geo = SqlGeography.Parse(geom.STAsText().ToSqlString());
                var lat = (double)geo.STPointN(1).Lat;
                var lng = (double)geo.STPointN(1).Long;

               var scalar = 69.1 * Math.Cos(lat / 57.3);

                var year = DateTime.Now.Year;
                var item = context.Businesses.Where(i => industryIds.Contains(i.IndustryId.Value));
                item = item.Where(i => i.BusinessStatusCode != "1" || i.BusinessStatusCode != "3");
                var projection = item.Select(i => new {
                    Distance = Math.Pow(Math.Pow(((double)i.Lat.Value - lat) * 69.1, 2) + Math.Pow(((double)i.Long.Value - lng) * scalar, 2), 0.5),
                    Business = new Models.Business.Business()
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Lat = i.Lat,
                        Lng = i.Long,
                        Street = i.Address,
                        City = i.City,
                        State = i.State.Abbreviation,
                        Zip = i.ZipCode.Zip,
                        Phone = i.Phone,
                        Url = i.PrimaryWebURL,
                        IsHomeBased = i.WorkAtHomeFlag == "1",
                        IsFirm = i.FirmCode == "2",
                        IsPublic = i.PublicCompanyIndicator == "1" || i.PublicCompanyIndicator == "2",
                        YearsInBusiness = year - (i.YearEstablished ?? i.YearAppeared) ?? null,
                        IndustryId = i.IndustryId
                    }
                }).Where(i => i.Distance < radius);

                var data = projection.OrderBy(i => i.Distance)
                    .ThenBy(i => i.Business.Name)
                    .Select(i => i.Business);

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
