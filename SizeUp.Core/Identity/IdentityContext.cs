using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Profile;
using System.Web.Security;


namespace SizeUp.Core.Identity
{
    public class IdentityContext
    {
        public static IdentityContext Current
        {
            get
            {
                var context = HttpContext.Current.Items["Sizeup.Core.Identity.IdentityContext"] as IdentityContext;
                if (context == null)
                {
                    context = new IdentityContext();
                    HttpContext.Current.Items["Sizeup.Core.Identity.IdentityContext"] = context;
                }
                return context;
            }
        }

        public static void CreateUser(Identity User)
        {

        }

        public static void UpdateUser(Identity User)
        {

        }


        public static Identity GetUser(string Username)
        {
            var user = Membership.GetUser(Username);
            var profile = ProfileBase.Create(Username);
            Identity i = new Identity();
            i.UserName = user.UserName;
            i.Email = user.UserName;
            i.IsApproved = user.IsApproved;
            i.IsLockedOut = user.IsLockedOut;
            i.FullName = profile["FullName"] as string;
            return i;
        }


        public Identity User
        {
            get
            {
                Identity i = null;
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    i = HttpContext.Current.Items["Sizeup.Core.Identity.Current.User"] as Identity;
                    if (i == null)
                    {
                        var username = HttpContext.Current.User.Identity.Name;
                        i = GetUser(username);
                        HttpContext.Current.Items["Sizeup.Core.Identity.Current.User"] = i;
                    }
                }
                return i;
            }
        }
    }
}
