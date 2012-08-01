using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SizeUp.Core.Web
{
    public static class QueryString
    {
        public static int? IntValue(string index)
        {
            int? v = null;
            if (HttpContext.Current.Request.QueryString[index] != null)
            {
                int x;
                if (int.TryParse(HttpContext.Current.Request.QueryString[index], out x))
                {
                    v = x;
                }
            }
            return v;
        }

        public static int?[] IntValues(string index)
        {
            int?[] v = null;
            if (HttpContext.Current.Request.QueryString[index] != null)
            {
                var arr = HttpContext.Current.Request.QueryString.GetValues(index);
                v = new int?[arr.Length];
                int x;
                for (int i = 0; i < arr.Length;i++ )
                {
                    if (int.TryParse(arr[i], out x))
                    {
                        v[i] = x;
                    }
                }
            }
            return v;
        }

        public static string StringValue(string index)
        {
            string v = null;
            if (HttpContext.Current.Request.QueryString[index] != null)
            {
                v = HttpContext.Current.Request.QueryString[index];
            }
            return v;
        }
    }
}
