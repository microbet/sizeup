using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Util;

namespace SizeUp.Core.Web
{
    public class RequestValidator : System.Web.Util.RequestValidator
    {
        protected override bool IsValidRequestString(System.Web.HttpContext context, string value, RequestValidationSource requestValidationSource, string collectionKey, out int validationFailureIndex)
        {
            bool outVal = false;
            validationFailureIndex = -1;  //Set a default value for the out parameter.

            if (requestValidationSource == RequestValidationSource.QueryString)
            {
                outVal = true;
            }
            else if (requestValidationSource == RequestValidationSource.Form)
            {
                outVal = true;
            }
            else
            {
                outVal = base.IsValidRequestString(context, value, requestValidationSource, collectionKey, out validationFailureIndex);
            }
            return outVal;
        }
    }
}
