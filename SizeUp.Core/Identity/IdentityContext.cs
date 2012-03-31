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

        public static Identity CreateUser(Identity User, string Password)
        {
            var u = Membership.CreateUser(User.UserName, Password, User.Email);
            u.IsApproved = User.IsApproved;
            Membership.UpdateUser(u);
            if (!string.IsNullOrWhiteSpace(User.FullName))
            {
                var profile = ProfileBase.Create(User.UserName);
                profile["FullName"] = User.FullName;
                profile.Save();
            }
            return BindIdentity(u, User);
        }

        public static void UpdateUser(Identity User)
        {
            var u = Membership.GetUser(User.UserName);
            u.Email = User.Email;
            u.IsApproved = User.IsApproved;
            Membership.UpdateUser(u);
            var profile = ProfileBase.Create(User.UserName);
            profile["fullName"] = User.FullName;
            profile.Save();
        }

        public static bool ValidateUser(string username, string password)
        {
            return Membership.ValidateUser(username, password);
        }


        public static Identity GetUser(string Username)
        {
            Identity i = null;
            var user = Membership.GetUser(Username);
            if (user != null)
            {
                i = BindIdentity(user, new Identity());
            }
            return i;
        }

        protected static Identity BindIdentity(MembershipUser membership, Identity user)
        {
            var profile = ProfileBase.Create(membership.UserName);
            user.UserName = membership.UserName;
            user.Email = membership.UserName;
            user.IsApproved = membership.IsApproved;
            user.IsLockedOut = membership.IsLockedOut;
            user.FullName = profile["FullName"] as string;
            user.UserId = (Guid)membership.ProviderUserKey;
            return user;
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
