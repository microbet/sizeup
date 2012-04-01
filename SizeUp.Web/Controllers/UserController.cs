using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SizeUp.Core;
using SizeUp.Core.Identity;
using SizeUp.Core.Email;
using SizeUp.Data;
using SizeUp.Data.Analytics;
using SizeUp.Core.Web;

namespace SizeUp.Web.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /User/
        [HttpGet]
        public ActionResult Register()
        {
            ViewBag.Header = new Models.Header()
            {
                HideMenu = true
            };

            ViewBag.UsernameExists = false;
            ViewBag.Error = false;
            return View();
        }

        [HttpPost]
        public ActionResult Register(string email, string name, string password)
        {
            ViewBag.Header = new Models.Header()
            {
                HideMenu = true
            };

            ViewBag.Name = name;
            ViewBag.Email = email;
            ViewBag.UsernameExists = false;
            ViewBag.Error = false;

            Identity i = new Identity()
            {
                UserName = email,
                Email = email,
                FullName = name
            };

            try
            {
                i = IdentityContext.CreateUser(i, password);
                i.IsApproved = false;
                IdentityContext.UpdateUser(i);
                Singleton<Mailer>.Instance.SendRegistrationEmail(i);
                FormsAuthentication.SetAuthCookie(i.UserName, false);

                UserRegistration reg = new UserRegistration()
                {
                    APIKeyId = null,
                    CityId = WebContext.Current.CurrentCity != null ? WebContext.Current.CurrentCity.Id : null as long?,
                    IndustryId = WebContext.Current.CurrentIndustry != null ? WebContext.Current.CurrentIndustry.Id: null as long?,
                    UserId = i.UserId,
                    Email = i.Email,
                    ReturnUrl = string.IsNullOrWhiteSpace(Request["returnurl"]) ? "" : Request["returnurl"]
                };
                Singleton<Tracker>.Instance.UserRegisteration(reg);
                FormsAuthentication.RedirectFromLoginPage(i.UserName, false);
            }
            catch (MembershipCreateUserException ex)
            {
                if (ex.StatusCode == MembershipCreateStatus.DuplicateUserName || ex.StatusCode == MembershipCreateStatus.InvalidPassword)
                {
                    ViewBag.UsernameExists = true;
                }
                else
                {
                    ViewBag.Error = true;
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Signin()
        {
            ViewBag.Header = new Models.Header()
            {
                HideMenu = true
            };
            ViewBag.InvalidPassword = false;
            ViewBag.NotActive = false;
            ViewBag.LockedOut = false;
            ViewBag.PasswordReset = false;
            ViewBag.Email = "";
            return View();
        }

        [HttpPost]
        public ActionResult Signin(string email, string password)
        {
            ViewBag.Header = new Models.Header()
            {
                HideMenu = true
            };

            ViewBag.InvalidPassword = false;
            ViewBag.NotActive = false;
            ViewBag.LockedOut = false;
            ViewBag.PasswordReset = false;
            ViewBag.Email = email;

            if (!IdentityContext.ValidateUser(email, password))
            {
                var user = IdentityContext.GetUser(email);
                if (user != null && !user.IsApproved)
                {
                    ViewBag.NotActive = true;
                }
                else if (user != null && user.IsLockedOut)
                {
                    ViewBag.LockedOut = true;
                }
                else
                {
                    ViewBag.InvalidPassword = true;
                }  
            }
            else
            {
                bool persist = Request["persist"] != null;
                if (persist)
                {
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(email, true, (int)FormsAuthentication.Timeout.TotalMinutes);
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    cookie.Expires = authTicket.Expiration;
                    HttpContext.Response.Cookies.Set(cookie);
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(email, false);
                }
                FormsAuthentication.RedirectFromLoginPage(email, persist);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            return Redirect(Server.UrlDecode(Request["returnurl"]));
        }

        [HttpGet]
        public ActionResult ResetPassword(string email)
        {
            ViewBag.Header = new Models.Header()
            {
                HideMenu = true
            };
            ViewBag.InvalidPassword = false;
            ViewBag.NotActive = false;
            ViewBag.LockedOut = false;
            ViewBag.PasswordReset = true;
            ViewBag.Email = email;

            var i = IdentityContext.GetUser(email);
            if (i != null)
            {
                Singleton<Mailer>.Instance.SendResetPasswordEmail(i);
            }


            return View("Signin");
        }

    }
}
