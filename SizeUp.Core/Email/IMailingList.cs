using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.Email
{
    public interface IMailingList
    {
        bool Subscribe(string email);
        bool Unsubscribe(string email);
        bool Subscribe(List<string> emails);
        bool Unsubscribe(List<string> emails);
    }
}
