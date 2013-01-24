using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MailChimp;


namespace SizeUp.Core.Email
{
    public class MailChimpMailingList : IMailingList
    {
        protected MCApi API { get; set; }
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

        public MailChimpMailingList()
        {
            if (!string.IsNullOrEmpty(APIKey) && !string.IsNullOrEmpty(ListId))
            {
                API = new MCApi(APIKey, true);
            }
        }
        
        public bool Subscribe(string email)
        {
            bool r = false;
            if(API!=null){
                r = API.ListSubscribe(ListId, email, null, new MailChimp.Types.Opt<MailChimp.Types.List.SubscribeOptions>(new MailChimp.Types.List.SubscribeOptions(){ SendWelcome = false, UpdateExisting = true}));
            }
            return r;
        }

        public bool Unsubscribe(string email)
        {
            throw new NotImplementedException();
        }

        public bool Subscribe(List<string> emails)
        {
            throw new NotImplementedException();
        }

        public bool Unsubscribe(List<string> emails)
        {
            throw new NotImplementedException();
        }
    }
}
