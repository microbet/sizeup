using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core.Identity;

namespace SizeUp.Core.Email
{
    public interface IMailingList
    {
        bool Subscribe(Identity.Identity identity);
        bool Unsubscribe(Identity.Identity identity);
        bool IsSubscribed(Identity.Identity identity);
    }
}
