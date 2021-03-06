﻿using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Models;

namespace SizeUp.Core.DataLayer
{
    public class IndustryData
    {
        public static IQueryable<Data.IndustryData> Get(SizeUpContext context)
        {
            return context.IndustryDatas
                .Where(i => i.Year == CommonFilters.TimeSlice.Industry.Year && i.Quarter == CommonFilters.TimeSlice.Industry.Quarter && i.Industry.IsActive && !i.Industry.IsDisabled);
        }
    }
}
