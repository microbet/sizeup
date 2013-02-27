using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Data;


namespace SizeUp.Core.DataLayer
{
    public class YearStarted : Base
    {
        /*
        public IQueryable<PlaceValues<int>> Chart(SizeUpContext context, long industryId, long placeId, int startYear, int endYear)
        {
            var data = context.CityCountyMappings
                .Where(i => i.Id == placeId)
                .Select(i => new
                {
                    City = i.City.BusinessDataByCities
                        .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                        .Where(d=>d.YearAppeared != null && d.YearEstablished != null)
                        .Where(d => d.Year == Year && d.Quarter == Quarter),

                    County = i.County.BusinessDataByCounties
                        .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                        .Where(d=>d.YearAppeared != null && d.YearEstablished != null)
                        .Where(d => d.Year == Year && d.Quarter == Quarter),

                    Metro = i.County.Metro.BusinessDataByCounties
                        .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                        .Where(d=>d.YearAppeared != null && d.YearEstablished != null)
                        .Where(d => d.Year == Year && d.Quarter == Quarter),

                    State = i.County.State.BusinessDataByCounties
                        .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                        .Where(d=>d.YearAppeared != null && d.YearEstablished != null)
                        .Where(d => d.Year == Year && d.Quarter == Quarter),

                    Nation = context.BusinessDataByCounties
                        .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                        .Where(d=>d.YearAppeared != null && d.YearEstablished != null)
                        .Where(d => d.Year == Year && d.Quarter == Quarter)
                })
                .Select(i => new
                {
                    City = new
                    {
                        City = i.City.FirstOrDefault().City,
                        Years = i.City.Select(d=> d.YearEstablished ?? d.YearAppeared)
                    },
                    County = new
                    {
                        County = i.County.FirstOrDefault().County,
                        Years = i.County.Select(d => d.YearEstablished ?? d.YearAppeared)
                    },
                    Metro = new
                    {
                        Metro = i.Metro.FirstOrDefault().Metro,
                        Years = i.Metro.Select(d => d.YearEstablished ?? d.YearAppeared)
                    },
                    State = new
                    {
                        State = i.State.FirstOrDefault().State,
                        Years = i.State.Select(d => d.YearEstablished ?? d.YearAppeared)
                    },
                    Nation = new
                    {
                        Total = i.Nation.Count(),
                        Years = i.Nation.Select(d => d.YearEstablished ?? d.YearAppeared)
                    }
                })
                .Select(i => new PlaceValues<PercentileItem>
                {
                    City = new PercentileItem
                    {
                        Percentile = i.City.Total > MinimumBusinessCount ? (int?)(((decimal)i.City.Filtered / (decimal)i.City.Total) * 100) : null,
                        Name = i.City.City.Name + ", " + i.City.City.State.Abbreviation
                    },
                    County = new PercentileItem
                    {
                        Percentile = i.County.Total > MinimumBusinessCount ? (int?)(((decimal)i.County.Filtered / (decimal)i.County.Total) * 100) : null,
                        Name = i.County.County.Name + ", " + i.County.County.State.Abbreviation
                    },
                    Metro = new PercentileItem
                    {
                        Percentile = i.Metro.Total > MinimumBusinessCount ? (int?)(((decimal)i.Metro.Filtered / (decimal)i.Metro.Total) * 100) : null,
                        Name = i.Metro.Metro.Name
                    },
                    State = new PercentileItem
                    {
                        Percentile = i.State.Total > MinimumBusinessCount ? (int?)(((decimal)i.State.Filtered / (decimal)i.State.Total) * 100) : null,
                        Name = i.State.State.Name
                    },
                    Nation = new PercentileItem
                    {
                        Percentile = i.Nation.Total > MinimumBusinessCount ? (int?)(((decimal)i.Nation.Filtered / (decimal)i.Nation.Total) * 100) : null,
                        Name = "USA"
                    }
                });
                
        }
         * */
    }
}
