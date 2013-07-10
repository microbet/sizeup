﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Web.Controllers;
using SizeUp.Web.Models;
using System.Web.Security;
using System.Text;
using System.Configuration;
using SizeUp.Core;
using SizeUp.Core.Identity;
using SizeUp.Core.Email;
using SizeUp.Data;
using SizeUp.Data.Analytics;
using SizeUp.Core.Web;
using SizeUp.Core.Crypto;
using SizeUp.Core.API;

namespace SizeUp.Web.Areas.Widget.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /Wiget/Signin/

        [HttpGet]
        [Authorize]
        public ActionResult Profile()
        {
            ViewBag.CurrentUser = Identity.CurrentUser;

            return View();
        }

        [HttpGet]
        public ActionResult Signin()
        {
            ViewBag.InvalidPassword = false;
            ViewBag.NotActive = false;
            ViewBag.LockedOut = false;
            ViewBag.PasswordReset = false;
            ViewBag.PasswordResetSent = false;
            ViewBag.Verified = false;
            ViewBag.VerificationError = false;
            ViewBag.VerificationSent = false;
            ViewBag.Email = "";

            return View();
        }

        [HttpPost]
        public ActionResult Signin(string email, string password)
        {

            ViewBag.InvalidPassword = false;
            ViewBag.NotActive = false;
            ViewBag.LockedOut = false;
            ViewBag.PasswordReset = false;
            ViewBag.PasswordResetSent = false;
            ViewBag.Verified = false;
            ViewBag.VerificationError = false;
            ViewBag.VerificationSent = false;
            ViewBag.Email = email;

            if (!Identity.ValidateUser(email, password))
            {
                var user = Identity.GetUser(email);
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
        public ActionResult Register()
        {

            ViewBag.UsernameExists = false;
            ViewBag.Error = false;
            return View();
        }

        [HttpPost]
        public ActionResult Register(string email, string name, string password)
        {

            ViewBag.Name = name;
            ViewBag.Email = email;
            ViewBag.UsernameExists = false;
            ViewBag.Error = false;

            Identity i = new Identity()
            {
                Email = email,
                FullName = name
            };

            try
            {
                APIToken token = APIToken.GetFromCookie();
                long? apikeyid = token != null ? token.APIKeyId : (long?)null;
                i.IsApproved = false;
                i.CreateUser(password);
                Singleton<Mailer>.Instance.SendRegistrationEmail(i);
                FormsAuthentication.SetAuthCookie(i.Email, false);
                UserRegistration reg = new UserRegistration()
                {
                    APIKeyId = apikeyid,
                    CityId = WebContext.Current.CurrentPlace.Id,
                    IndustryId = WebContext.Current.CurrentIndustry.Id,
                    UserId = i.UserId,
                    Email = i.Email,
                    ReturnUrl = string.IsNullOrWhiteSpace(Request["returnurl"]) ? "" : Request["returnurl"]
                };

                Singleton<Tracker>.Instance.UserRegistration(reg);
                FormsAuthentication.RedirectFromLoginPage(i.Email, false);
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
        public ActionResult SendVerification(string email)
        {
            ViewBag.InvalidPassword = false;
            ViewBag.NotActive = false;
            ViewBag.LockedOut = false;
            ViewBag.PasswordReset = false;
            ViewBag.PasswordResetSent = false;
            ViewBag.Verified = false;
            ViewBag.VerificationError = false;
            ViewBag.VerificationSent = true;
            ViewBag.Email = email;

            var i = Identity.GetUser(email);
            if (i != null)
            {
                Singleton<Mailer>.Instance.SendRegistrationEmail(i);
            }


            return View("Signin");
        }

    }
}
