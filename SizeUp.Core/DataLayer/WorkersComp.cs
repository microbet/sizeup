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
    public class WorkersComp : Base.Base
    {
        public static PlaceValues<WorkersCompChartItem> Chart(SizeUpContext context, long industryId, long placeId)
        {

            var data = IndustryData.Get(context, industryId)
                .Where(i => i.Place.Id == placeId)
                .Select(i => new PlaceValues<WorkersCompChartItem>
                {
                    State = i.State
                            .Select(d => new WorkersCompChartItem
                            {
                                Average = d.WorkersComp,
                                Rank = d.WorkersCompRank,
                                Name = d.State.Name
                            }).FirstOrDefault()
                }).FirstOrDefault();
            return data;
        }

        public static PlaceValues<PercentageItem> Percentage(SizeUpContext context, long industryId, long placeId, double value)
        {
            var data = IndustryData.Get(context, industryId)
                .Where(i => i.Place.Id == placeId)
                .Select(i=> new 
                {
                    State = i.State.Where(v => v.WorkersComp != null && v.WorkersComp > 0)
                            .Select(d => new
                            {
                                State = d.State,
                                Value = d.WorkersComp
                            }).FirstOrDefault()
                })
                .Select(i => new PlaceValues<PercentageItem>
                {
                    State = new PercentageItem
                    {
                        Percentage = i.State.Value != null ? (int?)(((value - i.State.Value) / i.State.Value) * 100) : null,
                        Name = i.State.State.Name
                    }
                }).FirstOrDefault();

            return data;
        }
    }
}
