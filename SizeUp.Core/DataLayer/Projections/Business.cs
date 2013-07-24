using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Models;

namespace SizeUp.Core.DataLayer.Projections
{
    public static class Business
    {
        public class Default : Projection<Data.Business, Models.Business>
        {
            public override Expression<Func<Data.Business, Models.Business>> Expression
            {
                get
                {
                    return i => new Models.Business()
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
                        YearsInBusiness = DateTime.UtcNow.Year - i.BusinessDatas.Where(b=>b.Year == CommonFilters.TimeSlice.Industry.Year && b.Quarter == CommonFilters.TimeSlice.Industry.Quarter && b.Id == i.Id).Select(b=>b.YearStarted).FirstOrDefault(),
                        IndustryId = i.IndustryId,
                        SEOKey = i.SEOKey
                    };
                }
            }
        }

        public class Distance : Projection<DistanceEntity<Data.Business>, DistanceEntity<Models.Business>>
        {
            public override Expression<Func<DistanceEntity<Data.Business>, DistanceEntity<Models.Business>>> Expression
            {
                get
                {
                    return i => new DistanceEntity<Models.Business>
                    {

                        Distance = i.Distance,
                        Entity = new Models.Business()
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
                            YearsInBusiness = DateTime.UtcNow.Year - i.Entity.BusinessDatas.Where(b => b.Year == CommonFilters.TimeSlice.Industry.Year && b.Quarter == CommonFilters.TimeSlice.Industry.Quarter && b.Id == i.Entity.Id).Select(b => b.YearStarted).FirstOrDefault(),
                            IndustryId = i.Entity.IndustryId,
                            SEOKey = i.Entity.SEOKey
                        }
                    };
                }
            }
        }
    }
}
