using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SizeUp.Data
{
    public static class DataContexts
    {


        public static SizeUpContext SizeUpContext
        {
            get
            {
                SizeUpContext context;
                if (HttpContext.Current.Items["SizeUp.Data.Context.SizeUpDataContext"] != null)
                {
                    context = HttpContext.Current.Items["SizeUp.Data.Context.SizeUpDataContext"] as SizeUpContext;
                }
                else
                {
                    context = new SizeUpContext();
                    HttpContext.Current.Items["SizeUp.Data.Context.SizeUpDataContext"] = context;
                }
                return context;
            }
        }
    }
}
