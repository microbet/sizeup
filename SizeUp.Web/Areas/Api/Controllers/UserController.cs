using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SizeUp.Core;
using SizeUp.Core.Identity;
using SizeUp.Core.Email;

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /Api/User/

        public ActionResult Authenticated()
        {
            var output = User.Identity.IsAuthenticated;
            return Json(output, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Signin(string email, string password, bool? persist)
        {
            string response = "";
            if (Membership.ValidateUser(email, password))
            {
                if (persist.HasValue && persist.Value)
                {
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(email, true, (int)FormsAuthentication.Timeout.TotalMinutes);
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    cookie.Expires = authTicket.Expiration;
                    Response.Cookies.Set(cookie);
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(email, false);
                }
                response = "ok";
            }
            else
            {
                MembershipUser thisUser = Membership.GetUser(email);
                if (thisUser == null)
                {
                    response = "failed";
                }
                else if (!thisUser.IsApproved)
                {
                    response = "unvalidated";
                }
                else if (thisUser.IsLockedOut)
                {
                    response = "locked";
                }
                else
                {
                    response = "failed";
                }
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ResetPassword(string email)
        {
            var i = Identity.GetUser(email);
            if (i != null)
            {
                Singleton<Mailer>.Instance.SendResetPasswordEmail(i);
            }

            return Json(true);
        }

    }
}
