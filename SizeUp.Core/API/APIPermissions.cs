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
        protected List<Data.API.Role> roles = new List<Data.API.Role>();
        public bool Advertising
        {
            get { return roles.Any(i => i.Name.ToLower() == "advertising"); }
        }

        public bool IndustryData
        {
            get { return roles.Any(i => i.Name.ToLower() == "industrydata"); }
        }

        public bool Business
        {
            get { return roles.Any(i => i.Name.ToLower() == "business"); }
        }

        public bool BestPlaces
        {
            get { return roles.Any(i => i.Name.ToLower() == "bestplaces"); }
        }

        public bool ConsumerExpenditures
        {
            get { return roles.Any(i => i.Name.ToLower() == "consumerexpenditures"); }
        }

        public bool Demographics
        {
            get { return roles.Any(i => i.Name.ToLower() == "demographics"); }
        }

        public bool Place
        {
            get { return roles.Any(i => i.Name.ToLower() == "place"); }
        }

        public bool Industry
        {
            get { return roles.Any(i => i.Name.ToLower() == "industry"); }
        }

        public APIPermissions(long APIKeyId)
        {
            using (var context = ContextFactory.APIContext)
            {
                roles = context.Roles.Where(i => i.APIKeyRoleMappings.Any(m => m.APIKeyId == APIKeyId)).ToList();   
            }
        }
    }
}
