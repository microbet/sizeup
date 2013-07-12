using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;

namespace SizeUp.Core.DataLayer.Base
{
    public class Metro
    {
        public static IQueryable<Data.Metro> Get(SizeUpContext context)
        {
            return context.Metroes;
        }
    }
}
