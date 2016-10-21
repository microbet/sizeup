using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
//using MailChimp;
using PerceptiveMCAPI;
using SizeUp.Core.Identity;

namespace SizeUp.Core.Email
{
    public class MailChimpMailingList : IMailingList
    {
        protected string APIKey
        {
            get
            {
                return ConfigurationManager.AppSettings["MailChimpAPIKey"];
            }
        }

        protected string ListId
        {
            get
            {
                return ConfigurationManager.AppSettings["MailChimpListId"];
            }
        }

        protected bool IsAvailable
        {
            get
            {
                return !string.IsNullOrEmpty(APIKey) && !string.IsNullOrEmpty(ListId);
            }
        }

       
        
        public bool Subscribe(Identity.Identity identity)
        {
            bool r = false;
            if (IsAvailable)
            {
                Dictionary<string, object> mergeVars = new Dictionary<string,object>();
                mergeVars.Add("FullName", identity.FullName);
                var input = new PerceptiveMCAPI.Types.listSubscribeInput(APIKey, ListId, identity.Email, mergeVars, EnumValues.emailType.html, false, true, true, false);
                var output = new PerceptiveMCAPI.Methods.listSubscribe(input).Execute();
                r = output.result;

            }
            return r;
        }

        public bool Unsubscribe(Identity.Identity identity)
        {
            bool r = false;
            if (IsAvailable)
            {
                var input = new PerceptiveMCAPI.Types.listUnsubscribeInput(APIKey, ListId, identity.Email, false, false, false);
                var output = new PerceptiveMCAPI.Methods.listUnsubscribe(input).Execute();
                r = output.result;
            }
            return r;
        }

        public bool IsSubscribed(Identity.Identity identity)
        {
            bool r = false;
            if (IsAvailable)
            {
                var input = new PerceptiveMCAPI.Types.listMemberInfoInput(APIKey, ListId, identity.Email);
                var output = new PerceptiveMCAPI.Methods.listMemberInfo(input).Execute();
                r = output.result.status == EnumValues.listMembers_status.subscribed;
            }
            return r;
        }
    }
}
