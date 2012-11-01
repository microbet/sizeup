﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
                i = Identity.CreateUser(i, password);
                i.IsApproved = false;
                i.Save();
                Singleton<Mailer>.Instance.SendRegistrationEmail(i);
                FormsAuthentication.SetAuthCookie(i.UserName, false);
                string ReturnUrl = string.IsNullOrWhiteSpace(Request["returnurl"]) ? "/" : Request["returnurl"];
                UserRegistration reg = new UserRegistration()
                {
                    APIKeyId = null,
                    CityId = WebContext.Current.CurrentPlaceId,
                    IndustryId = WebContext.Current.CurrentIndustryId,
                    UserId = i.UserId,
                    Email = i.Email,
                    ReturnUrl = ReturnUrl
                };

                Singleton<Tracker>.Instance.UserRegisteration(reg);
                return Redirect(ReturnUrl);
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
            ViewBag.Verified = false;
            ViewBag.VerificationError = false;
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
            ViewBag.Verified = false;
            ViewBag.VerificationError = false;
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
        public ActionResult BeginResetPassword(string email)
        {
            ViewBag.Header = new Models.Header()
            {
                HideMenu = true
            };
            ViewBag.InvalidPassword = false;
            ViewBag.NotActive = false;
            ViewBag.LockedOut = false;
            ViewBag.PasswordReset = true;
            ViewBag.Verified = false;
            ViewBag.VerificationError = false;
            ViewBag.Email = email;

            var i = Identity.GetUser(email);
            if (i != null)
            {
                Singleton<Mailer>.Instance.SendResetPasswordEmail(i);
            }


            return View("Signin");
        }


        [HttpGet]
        public ActionResult ResetPassword(string key)
        {
            ViewBag.Header = new Models.Header()
            {
                HideMenu = true
            };
            ViewBag.BadCode = false;
            ViewBag.Error = false;

            try
            {
                var user = Identity.DecryptToken(key);
                ViewBag.UserName = user.UserName;
            }
            catch (System.Exception e)
            {
                ViewBag.BadCode = true;
            }
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string key, string password)
        {
            ActionResult returnAction = View("Signin");
            ViewBag.Header = new Models.Header()
            {
                HideMenu = true
            };
            ViewBag.BadCode = false;
            ViewBag.Error = false;
            ViewBag.InvalidPassword = false;
            ViewBag.NotActive = false;
            ViewBag.LockedOut = false;
            ViewBag.PasswordReset = false;
            ViewBag.Verified = false;
            ViewBag.VerificationError = false;
            try
            {
                var user = Identity.DecryptToken(key);
                if (user != null)
                {
                    user.ResetPassword(password);
                }

            }
            catch (System.Exception e)
            {
                ViewBag.Error = true;
                returnAction = View();
            }
            return returnAction;
        }

        [HttpGet]
        public ActionResult ConfirmRegistration(string key)
        {
            ViewBag.Header = new Models.Header()
            {
                HideMenu = true
            };
            ViewBag.BadCode = false;
            ViewBag.Error = false;
            ViewBag.InvalidPassword = false;
            ViewBag.NotActive = false;
            ViewBag.LockedOut = false;
            ViewBag.PasswordReset = false;
            ViewBag.Verified = false;
            ViewBag.VerificationError = false;

            try
            {
                var user = Identity.DecryptToken(key);
                user.IsApproved = true;
                user.Save();
                ViewBag.Verified = true;
            }
            catch (System.Exception e)
            {
                ViewBag.VerificationError = true;
            }
            return View("Signin");
        }


        [HttpGet]
        public ActionResult OptOut(string key)
        {
            ViewBag.Header = new Models.Header()
            {
                HideMenu = true
            };
            ViewBag.BadCode = false;

            try
            {
                var user = Identity.DecryptToken(key);
                ViewBag.OptOut = user.IsOptOut;
                ViewBag.Email = user.Email;
            }
            catch (System.Exception e)
            {
                ViewBag.BadCode = true;
            }
            return View();
        }

        [HttpPost]
        public ActionResult OptOut(string key, bool OptOut)
        {
            ViewBag.Header = new Models.Header()
            {
                HideMenu = true
            };
            ViewBag.BadCode = false;
            ViewBag.OptOut = false;
            try
            {
                var user = Identity.DecryptToken(key);
                user.IsOptOut = OptOut;
                user.Save();
                ViewBag.OptOut = user.IsOptOut;
                ViewBag.Email = user.Email;
            }
            catch (System.Exception e)
            {
                ViewBag.BadCode = true;
            }
            return View();
        }

    }
}
