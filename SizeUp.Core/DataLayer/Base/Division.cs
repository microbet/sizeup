﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;

namespace SizeUp.Core.DataLayer.Base
{
    public class Division
    {
        public static IQueryable<Data.Division> Get(SizeUpContext context)
        {
            IQueryable<Data.Division> output = context.Divisions;
            return output;
        }
    }
}
