using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Base;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Core.Geo;
namespace SizeUp.Core.DataLayer
{
    public class Business
    {
        public static Models.Business Get(SizeUpContext context, long id)
        {
            var year = DateTime.UtcNow.Year;

            var data = Base.Business.Get(context)
                .Where(i => i.Id == id)
                .Select(i => new Models.Business
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
                }).FirstOrDefault();
            return data;
        }

        public static Models.Business GetAt(SizeUpContext context, LatLng latLng, List<long> industryIds)
        {
            var year = DateTime.UtcNow.Year;
            var data = Base.Business.Distance(context, latLng)
                .Where(i => industryIds.Contains(i.Entity.IndustryId.Value))
                .OrderBy(i => i.Distance)
                .Select(i => new Models.Business
                {
                    Id = i.Entity.Id,
                    Name = i.Entity.Name,
                    Lat = i.Entity.Lat,
                    Lng = i.Entity.Long,
                    Street = i.Entity.Address,
                    City = i.Entity.City,
                    State = i.Entity.State.Abbreviation,
                    Zip = i.Entity.ZipCode.Zip,
                    Phone = i.Entity.Phone,
                    Url = i.Entity.PrimaryWebURL,
                    IsHomeBased = i.Entity.WorkAtHomeFlag == "1",
                    IsFirm = i.Entity.FirmCode == "2",
                    IsPublic = i.Entity.PublicCompanyIndicator == "1" || i.Entity.PublicCompanyIndicator == "2",
                    YearsInBusiness = year - (i.Entity.YearEstablished ?? i.Entity.YearAppeared) ?? null,
                    IndustryId = i.Entity.IndustryId
                }).FirstOrDefault();
            return data;
        }

        public static IQueryable<Models.Base.DistanceEntity<Models.Business>> ListNear(SizeUpContext context, LatLng latLng, List<long> industryIds)
        {
            var year = DateTime.UtcNow.Year;
            var data = Base.Business.Distance(context, latLng)
                .Where(i => industryIds.Contains(i.Entity.IndustryId.Value))
                .Select(i => new Models.Base.DistanceEntity<Models.Business>
                {
                    Distance = i.Distance,
                    Entity = new Models.Business
                    {
                        Id = i.Entity.Id,
                        Name = i.Entity.Name,
                        Lat = i.Entity.Lat,
                        Lng = i.Entity.Long,
                        Street = i.Entity.Address,
                        City = i.Entity.City,
                        State = i.Entity.State.Abbreviation,
                        Zip = i.Entity.ZipCode.Zip,
                        Phone = i.Entity.Phone,
                        Url = i.Entity.PrimaryWebURL,
                        IsHomeBased = i.Entity.WorkAtHomeFlag == "1",
                        IsFirm = i.Entity.FirmCode == "2",
                        IsPublic = i.Entity.PublicCompanyIndicator == "1" || i.Entity.PublicCompanyIndicator == "2",
                        YearsInBusiness = year - (i.Entity.YearEstablished ?? i.Entity.YearAppeared) ?? null,
                        IndustryId = i.Entity.IndustryId
                    }
                });
            return data;
        }


        public static IQueryable<Models.Business> ListIn(SizeUpContext context, Core.Geo.BoundingBox boundingBox, List<long> industryIds)
        {
            var year = DateTime.UtcNow.Year;
            var data = Base.Business.In(context, boundingBox)
                .Where(i => industryIds.Contains(i.IndustryId.Value))
                .Select(i => new Models.Business
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

                });
            return data;
        }

    }
}
