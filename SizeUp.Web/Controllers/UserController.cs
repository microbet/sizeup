using System;
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
        [Authorize]
        public ActionResult Profile()
        {
            ViewBag.CurrentUser = Identity.CurrentUser;

            return View();
        }



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
                Email = email,
                FullName = name
            };

            try
            {
                i.IsApproved = false;
                i.CreateUser(password);
                Singleton<Mailer>.Instance.SendRegistrationEmail(i);
                FormsAuthentication.SetAuthCookie(i.Email, false);
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

                Singleton<Tracker>.Instance.UserRegistration(reg);
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
            ViewBag.PasswordResetSent = false;
            ViewBag.Verified = false;
            ViewBag.VerificationError = false;
            ViewBag.VerificationSent = false;
            ViewBag.Email = "";

            return View();
        }

        [HttpGet]
        public ActionResult SigninRedirect()
        {
            return RedirectPermanent("/user/signin");
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
                string ReturnUrl = string.IsNullOrWhiteSpace(Request["returnurl"]) ? "/" : Request["returnurl"];
                return Redirect(ReturnUrl);
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
            ViewBag.PasswordReset = false;
            ViewBag.PasswordResetSent = true;
            ViewBag.Verified = false;
            ViewBag.VerificationError = false;
            ViewBag.VerificationSent = false;
            ViewBag.Email = email;

            var i = Identity.GetUser(email);
            if (i != null)
            {
                Singleton<Mailer>.Instance.SendResetPasswordEmail(i);
            }


            return View("Signin");
        }

        [HttpGet]
        public ActionResult SendVerification(string email)
        {
            ViewBag.Header = new Models.Header()
            {
                HideMenu = true
            };
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
                ViewBag.UserName = user.Email;
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
            ViewBag.PasswordResetSent = false;
            ViewBag.Verified = false;
            ViewBag.VerificationSent = false;
            ViewBag.VerificationError = false;
            try
            {
                var user = Identity.DecryptToken(key);
                if (user != null)
                {
                    user.ResetPassword(password);
                    ViewBag.PasswordReset = true;
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
            ViewBag.PasswordResetSent = false;
            ViewBag.Verified = false;
            ViewBag.VerificationError = false;
            ViewBag.VerificationSent = false;

            try
            {
                var user = Identity.DecryptToken(key);
                user.IsApproved = true;
                user.IsSubscribed = true;
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
                ViewBag.Email = user.Email;
                ViewBag.OptOut = !user.IsSubscribed;
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
            try
            {
                var user = Identity.DecryptToken(key);
                user.IsSubscribed = !OptOut;
                user.Save();
                ViewBag.Email = user.Email;
                ViewBag.OptOut = !user.IsSubscribed;
            }
            catch (System.Exception e)
            {
                ViewBag.BadCode = true;
            }
            return View();
        }

    }
}
