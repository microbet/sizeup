using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using SizeUp.Core.Email;



namespace SizeUp.Core.Identity
{
    public class Identity
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }
        public bool IsSubscribed { get; set; }
        public string DisplayName { get { return string.IsNullOrWhiteSpace(FullName) ? Email : FullName; } }


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
            var u = Membership.GetUser(UserId);
            string tempPassword = u.ResetPassword();
            u.ChangePassword(tempPassword, password);
        }

        public void Save()
        {
            MailChimpMailingList ml = new MailChimpMailingList();
            var u = Membership.GetUser(UserId);
            var profile = ProfileBase.Create(u.UserName);
            u.Email = Email;
            u.IsApproved = IsApproved;
            profile["FullName"] = FullName;
            profile.Save();
            if (IsSubscribed)
            {
                ml.Subscribe(this);
            }
            else
            {
                ml.Unsubscribe(this);
            }
            Membership.UpdateUser(u);
        }

        public void CreateUser(string password)
        {
            var u = Membership.CreateUser(Email, password, Email);
            UserId = (Guid)u.ProviderUserKey;
            Save();
        }

        public static bool ValidateUser(string email, string password)
        {
            return Membership.ValidateUser(email, password);
        }


        public static Identity GetUser(string Email)
        {
            Identity i = null;
            var user = Membership.GetUser(Email);
            if (user != null)
            {
                i = GetIdentity(user);
            }
            return i;
        }

        public static Identity GetUser(Guid UserId)
        {
            Identity i = null;
            var user = Membership.GetUser(UserId);
            if (user != null)
            {
                i = GetIdentity(user);
            }
            return i;
        }
        
        protected static Identity GetIdentity(MembershipUser membership)
        {
            MailChimpMailingList ml = new MailChimpMailingList();
            Identity user = new Identity();
            var profile = ProfileBase.Create(membership.UserName);
            user.Email = membership.UserName;
            user.IsApproved = membership.IsApproved;
            user.IsLockedOut = membership.IsLockedOut;
            user.FullName = profile["FullName"] as string;
            user.UserId = (Guid)membership.ProviderUserKey;
            user.IsSubscribed = ml.IsSubscribed(user);
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