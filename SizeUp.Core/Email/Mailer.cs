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
            using (var context = new Data.SizeUpContext())
            {
                var strings = context.ResourceStrings.Where(i => i.Name.StartsWith("Registration.Email")).ToList();
                var template = strings.Where(i => i.Name == "Registration.Email.Body").Select(i => i.Value).FirstOrDefault();
                var subject = strings.Where(i => i.Name == "Registration.Email.Subject").Select(i => i.Value).FirstOrDefault();
                var uri = HttpContext.Current.Request.Url;
                var t = Templates.TemplateFactory.GetTemplate(template);
                t.Add("User", user);
                t.Add("ConfirmKey", HttpContext.Current.Server.UrlEncode(user.GetEncryptedToken()));
                t.Add("OptOutKey", HttpContext.Current.Server.UrlEncode(user.GetEncryptedToken()));
                t.Add("AppDomain", uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port);
                string body = t.Render();
                SendMail(user.Email, subject, body);
            }
        }

        public void SendResetPasswordEmail(Identity.Identity user)
        {
            using (var context = new Data.SizeUpContext())
            {
                var strings = context.ResourceStrings.Where(i => i.Name.StartsWith("PasswordReset.Email")).ToList();
                var template = strings.Where(i => i.Name == "PasswordReset.Email.Body").Select(i => i.Value).FirstOrDefault();
                var subject = strings.Where(i => i.Name == "PasswordReset.Email.Subject").Select(i => i.Value).FirstOrDefault();
                var uri = HttpContext.Current.Request.Url;
                var t = Templates.TemplateFactory.GetTemplate(template);
                t.Add("User", user);
                t.Add("PasswordResetKey", HttpContext.Current.Server.UrlEncode(user.GetEncryptedToken()));
                t.Add("OptOutKey", HttpContext.Current.Server.UrlEncode(user.GetEncryptedToken()));
                //t.Add("AppDomain", uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port);
                //t.Add("AppDomain", (HttpContext.Current.Request.IsSecureConnection ? "https" : "http") + Uri.SchemeDelimiter + uri.Host);
                t.Add("AppDomain", (ConfigurationManager.AppSettings["HostingEnvironment"] == "LOCALHOST" ? "http" : "https") + Uri.SchemeDelimiter + uri.Host);
                string body = t.Render();
                SendMail(user.Email, subject, body);
            }
        }

        public void SendExceptionMail(Data.Analytics.Exception reg)
        {
            using (var context = new Data.SizeUpContext())
            {
                var strings = context.ResourceStrings.Where(i => i.Name.StartsWith("Exception.Email")).ToList();
                var template = strings.Where(i => i.Name == "Exception.Email.Body").Select(i => i.Value).FirstOrDefault();
                var subject = strings.Where(i => i.Name == "Exception.Email.Subject").Select(i => i.Value).FirstOrDefault();
                var recipients = strings.Where(i => i.Name == "Exception.Email.Recipients").Select(i => i.Value).FirstOrDefault().Split(',').ToList();
                var uri = HttpContext.Current.Request.Url;
                var t = Templates.TemplateFactory.GetTemplate(template);
                t.Add("Exception", reg.ExceptionText);
                //t.Add("PasswordResetKey", HttpContext.Current.Server.UrlEncode(user.GetEncryptedToken()));
                //t.Add("OptOutKey", HttpContext.Current.Server.UrlEncode(user.GetEncryptedToken()));
                //t.Add("AppDomain", uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port);
                //t.Add("AppDomain", (HttpContext.Current.Request.IsSecureConnection ? "https" : "http") + Uri.SchemeDelimiter + uri.Host);
                t.Add("AppDomain", (ConfigurationManager.AppSettings["HostingEnvironment"] == "LOCALHOST" ? "http" : "https") + Uri.SchemeDelimiter + uri.Host);
                string body = t.Render();


                recipients.AsParallel().ForAll( r => SendMail(r, subject, body));
            }
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
    }
}
