using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
namespace SizeUp.Core.API
{
    public class APIPermissions
    {
        protected List<string> roles = new List<string>();
        public bool Advertising
        {
            get { return roles.Any(i => i.ToLower() == "advertising"); }
        }

        public bool IndustryData
        {
            get { return roles.Any(i => i.ToLower() == "industrydata"); }
        }

        public bool Business
        {
            get { return roles.Any(i => i.ToLower() == "business"); }
        }

        public bool BestPlaces
        {
            get { return roles.Any(i => i.ToLower() == "bestplaces"); }
        }

        public bool ConsumerExpenditures
        {
            get { return roles.Any(i => i.ToLower() == "consumerexpenditures"); }
        }

        public bool Demographics
        {
            get { return roles.Any(i => i.ToLower() == "demographics"); }
        }

        public bool Place
        {
            get { return roles.Any(i => i.ToLower() == "place"); }
        }

        public bool Industry
        {
            get { return roles.Any(i => i.ToLower() == "industry"); }
        }

        public bool Widget
        {
            get { return roles.Any(i => i.ToLower() == "widget"); }
        }

        public bool SigninOptional
        {
            get { return roles.Any(i => i.ToLower() == "signinoptional"); }
        }

        public bool CustomTools
        {
            get { return roles.Any(i => i.ToLower() == "customtools"); }
        }


        public APIPermissions(long? APIKeyId)
        {
            using (var context = ContextFactory.APIContext)
            {
                roles = context.Roles.Where(i => i.APIKeyRoleMappings.Any(m => m.APIKeyId == APIKeyId)).Select(i=>i.Name).ToList();   
            }
        }
    }
}
