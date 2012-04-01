using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;
using SizeUp.Core.Identity;


namespace SizeUp.Core.Email
{
    public class Mailer
    {
        public void SendRegistrationEmail(Identity.Identity user)
        {

        }

        public void SendResetPasswordEmail(Identity.Identity user)
        {
            var strings = Data.DataContexts.SizeUpContext.ResourceStrings.Where(i => i.Name.StartsWith("PasswordReset.Email")).ToList();
            var template = strings.Where(i => i.Name == "PasswordReset.Email.Body").Select(i => i.Value).FirstOrDefault();
            var subject = strings.Where(i => i.Name == "PasswordReset.Email.Subject").Select(i => i.Value).FirstOrDefault();
            var uri = HttpContext.Current.Request.Url;
            var t = Templates.TemplateFactory.GetTemplate(template);
            t.Add("User", user);
            t.Add("PasswordResetKey", GetPasswordResetKey(user));
            t.Add("OptOutKey", GetOptOutKey(user));
            t.Add("AppDomain", uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port);
            string body = t.Render();
            SendMail(user.Email, subject, body);
        }

        public void SendMail(string to, string subject, string body)
        {
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.To.Add(new MailAddress(to));
            message.Subject = subject;
            message.Body = body;
            SmtpClient client = new SmtpClient();
            client.Send(message);          
        }

        protected string GetOptOutKey(Identity.Identity user)
        {
            Crypto.Token<Guid> token = new Crypto.Token<Guid>()
            {
                Salt = DateTime.Now,
                Value = user.UserId
            };

            byte[] data = Serialization.Serializer.ToBytes(token);
            byte[] salt = Encoding.Unicode.GetBytes(ConfigurationManager.AppSettings["Crypto.Salt"]);
            string cypher = Crypto.Crypto.Encrypt(data, ConfigurationManager.AppSettings["Crypto.Password"], salt);
            return cypher;
        }

        protected string GetPasswordResetKey(Identity.Identity user)
        {
            Crypto.Token<Guid> token = new Crypto.Token<Guid>()
            {
                Salt = DateTime.Now,
                Value = user.UserId
            };

            byte[] data = Serialization.Serializer.ToBytes(token);
            byte[] salt = Encoding.Unicode.GetBytes(ConfigurationManager.AppSettings["Crypto.Salt"]);
            string cypher = Crypto.Crypto.Encrypt(data, ConfigurationManager.AppSettings["Crypto.Password"], salt);
            return cypher;
        }
    }
}
