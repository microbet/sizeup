using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailChimp;

namespace SizeUp.Core.Email
{
    public class MailChimpMailingList : IMailingList
    {
        protected MCApi API { get; set; }
        public MailChimpMailingList()
        {
            API = new MCApi("test", true);
        }
        
        public bool Subscribe(string email)
        {
           //return API.ListSubscribe(
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
