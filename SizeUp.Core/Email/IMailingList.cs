using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.Email
{
    public interface IMailingList
    {
        public bool Subscribe(string email);
        public bool Unsubscribe(string email);
        public bool Subscribe(List<string> emails);
        public bool Unsubscribe(List<string> emails);
    }
}
