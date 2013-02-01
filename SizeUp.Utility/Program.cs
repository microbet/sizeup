using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Configuration;
using SizeUp.Core.Email;
using SizeUp.Core.Identity;
using System.Web.Profile;

namespace SizeUp.Utility
{
    class Program
    {
        static void Main(string[] args)
        {
            MailChimpMailingList ml = new MailChimpMailingList();
            MembershipUserCollection users = Membership.GetAllUsers();
            int count = 0;
            foreach (MembershipUser u in users)
            {
                count++;
                Console.Clear();
                Console.Write("Processing {0} of {1}", count, users.Count);

                Identity i = Identity.GetUser(u);
                var profile = ProfileBase.Create(u.UserName);
                if (u.IsApproved && !u.IsLockedOut)
                {
                    ml.Subscribe(i);
                    bool optout = false;
                    bool.TryParse(profile["OptOut"] as string, out optout);
                    if (optout)
                    {
                        ml.Unsubscribe(i);
                    }
                }
            }
        }
    }
}
