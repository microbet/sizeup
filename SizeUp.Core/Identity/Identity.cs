using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Profile;
using System.Web.Security;




namespace SizeUp.Core.Identity
{
    public class Identity
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }
        public bool IsOptOut { get; set; }
        public string DisplayName { get { return string.IsNullOrWhiteSpace(FullName) ? UserName : FullName; } }

        /// <summary>
        /// Gets a unique base64 encoded, encrypted token representing the userId
        /// </summary>
        /// <returns></returns>
        public string GetEncryptedToken()
        {
            byte[] salt = Encoding.Unicode.GetBytes(ConfigurationManager.AppSettings["Crypto.Salt"]);
            byte[] data = UserId.ToByteArray();
            Random r = new Random();
            byte[] d = new byte[4];
            r.NextBytes(d);
            data = d.Concat(data).ToArray();
            return Crypto.Crypto.Encrypt(data, ConfigurationManager.AppSettings["Crypto.Password"], salt);
        }

        /// <summary>
        /// Returns the Identity object that generated the cypher from the GetEncryptedToken method
        /// </summary>
        /// <param name="cypher"></param>
        /// <returns></returns>
        public static Identity DecryptToken(string cypher)
        {
            byte[] salt = Encoding.Unicode.GetBytes(ConfigurationManager.AppSettings["Crypto.Salt"]);
            byte[] data = Crypto.Crypto.Decrypt(cypher, ConfigurationManager.AppSettings["Crypto.Password"], salt);
            data = data.Skip(4).ToArray();
            return Identity.GetUser(new Guid(data));
        }

        public void ResetPassword(string password)
        {
            var u = Membership.GetUser(UserName);
            string tempPassword = u.ResetPassword();
            u.ChangePassword(tempPassword, password);
        }

        public void Save()
        {
            var u = Membership.GetUser(UserName);
            SaveIdentity(u, this);
        }
       

        /*AVAST! Static methods be yonder this point....ARRRG!**/

        public static Identity CreateUser(Identity User, string Password)
        {
            var u = Membership.CreateUser(User.UserName, Password, User.Email);
            SaveIdentity(u, User);
            return BindIdentity(u, User);
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

        public static Identity GetUser(Guid UserId)
        {
            Identity i = null;
            var user = Membership.GetUser(UserId);
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
            user.Email = membership.Email;
            user.IsApproved = membership.IsApproved;
            user.IsLockedOut = membership.IsLockedOut;
            user.FullName = profile["FullName"] as string;
            user.IsOptOut = string.IsNullOrWhiteSpace(profile["OptOut"] as string) ? false : bool.Parse(profile["OptOut"] as string);
            user.UserId = (Guid)membership.ProviderUserKey;
            return user;
        }

        protected static Identity SaveIdentity(MembershipUser membership, Identity user)
        {
            var profile = ProfileBase.Create(membership.UserName);
            membership.Email = user.Email;
            membership.IsApproved = user.IsApproved;
            profile["FullName"] = user.FullName;
            profile["OptOut"] = user.IsOptOut.ToString();
            user.UserId = (Guid)membership.ProviderUserKey;
            profile.Save();
            Membership.UpdateUser(membership);
            return user;
        }

        public static Identity CurrentUser
        {
            get
            {
                Identity i = null;
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    i = HttpContext.Current.Items["Sizeup.Core.Identity.CurrentUser"] as Identity;
                    if (i == null)
                    {
                        var username = HttpContext.Current.User.Identity.Name;
                        i = GetUser(username);
                        HttpContext.Current.Items["Sizeup.Core.Identity.CurrentUser"] = i;
                    }
                }
                return i;
            }
        }
    }
}