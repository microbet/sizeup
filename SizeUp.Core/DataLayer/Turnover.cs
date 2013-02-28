using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Base;

namespace SizeUp.Core.DataLayer
{
    public class Turnover : Base.Base
    {
        public static PlaceValues<TurnoverChartItem> Chart(SizeUpContext context, long industryId, long placeId)
        {
            var data = IndustryData.Get(context, industryId)
                .Where(i => i.Place.Id == placeId)
                .Select(i => new PlaceValues<TurnoverChartItem>
                {
                    County = i.County
                            .Select(d => new TurnoverChartItem
                            {
                                Hires = d.Hires,
                                Turnover = d.TurnoverRate * 100,
                                Separations = d.Separations,
                                Name = d.County.Name + ", " + d.County.State.Abbreviation
                            }).FirstOrDefault(),

                    Metro = i.Metro
                            .Select(d => new TurnoverChartItem
                            {
                                Hires = d.Hires,
                                Turnover = d.TurnoverRate * 100,
                                Separations = d.Separations,
                                Name = d.Metro.Name
                            }).FirstOrDefault(),

                    State = i.State
                            .Select(d => new TurnoverChartItem
                            {
                                Hires = d.Hires,
                                Turnover = d.TurnoverRate * 100,
                                Separations = d.Separations,
                                Name = d.State.Name
                            }).FirstOrDefault(),

                    Nation = i.Nation
                            .Select(d => new TurnoverChartItem
                            {                              
                                Hires = d.Hires,
                                Turnover = d.TurnoverRate * 100,
                                Separations = d.Separations,
                                Name = "USA"
                            }).FirstOrDefault()
                }).FirstOrDefault();
            return data;
        }
        
        public static PlaceValues<PercentileItem> Percentile(SizeUpContext context, long industryId, long placeId)
        {
            var raw = new 
            {
                County = context.IndustryDataByCounties.Where(i => i.IndustryId == industryId && i.Year == Year && i.Quarter == Quarter),
                Metro = context.IndustryDataByMetroes.Where(i => i.IndustryId == industryId && i.Year == Year && i.Quarter == Quarter),
                State = context.IndustryDataByStates.Where(i => i.IndustryId == industryId && i.Year == Year && i.Quarter == Quarter),
                Nation = context.IndustryDataByNations.Where(i => i.IndustryId == industryId && i.Year == Year && i.Quarter == Quarter)
            };


            var data = IndustryData.Get(context, industryId)
                .Where(i=>i.Place.Id == placeId)
                .Select(i=> new 
                {
                    County = new
                    {
                        County = i.Place.County,
                        Total = raw.County.Where(d => d.TurnoverRate != null && d.TurnoverRate > 0).Count(),
                        Filtered = raw.County.Where(d => d.TurnoverRate != null && d.TurnoverRate > 0).Where(d => d.TurnoverRate >= i.County.Select(v=>v.TurnoverRate).FirstOrDefault()).Count()
                    },
                    Metro = new
                    {
                        Metro = i.Place.County.Metro,
                        Total = raw.Metro.Where(d => d.TurnoverRate != null && d.TurnoverRate > 0).Count(),
                        Filtered = raw.Metro.Where(d => d.TurnoverRate != null && d.TurnoverRate > 0).Where(d => d.TurnoverRate >= i.Metro.Select(v => v.RevenuePerCapita).FirstOrDefault()).Count()
                    },
                    State = new
                    {
                        State = i.Place.County.State,
                        Total = raw.State.Where(d => d.TurnoverRate != null && d.TurnoverRate > 0).Count(),
                        Filtered = raw.State.Where(d => d.TurnoverRate != null && d.TurnoverRate > 0).Where(d => d.TurnoverRate >= i.State.Select(v => v.RevenuePerCapita).FirstOrDefault()).Count()
                    }
                })               
                .Select(i => new PlaceValues<PercentileItem>
                {
                    County = new PercentileItem
                    {
                        Percentile = i.County.Total > 0 ? (int?)(((decimal)i.County.Filtered / (decimal)i.County.Total) * 100) : null,
                        Name = i.County.County.Name + ", " + i.County.County.State.Abbreviation
                    },
                    Metro = new PercentileItem
                    {
                        Percentile = i.Metro.Total > 0 ? (int?)(((decimal)i.Metro.Filtered / (decimal)i.Metro.Total) * 100) : null,
                        Name = i.Metro.Metro.Name
                    },
                    State = new PercentileItem
                    {
                        Percentile = i.State.Total > 0 ? (int?)(((decimal)i.State.Filtered / (decimal)i.State.Total) * 100) : null,
                        Name = i.State.State.Name
                    }
                }).FirstOrDefault();
            return data;
        }
    }
}
