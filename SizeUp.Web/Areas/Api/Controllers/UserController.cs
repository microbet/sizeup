﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SizeUp.Core;
using SizeUp.Core.Identity;
using SizeUp.Core.Email;
using SizeUp.Core.Analytics;
using SizeUp.Data.Analytics;
using SizeUp.Data;

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /Api/User/

        [HttpPost]
        public ActionResult Signin(string email, string password, bool? persist)
        {
            string response = "";
            if (Membership.ValidateUser(email, password))
            {
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(email, true, (int)FormsAuthentication.Timeout.TotalMinutes);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie cookie = SizeUp.Core.Web.CookieFactory.Create(FormsAuthentication.FormsCookieName, encryptedTicket);
                if (persist.HasValue && persist.Value)
                {
                    cookie.Expires = authTicket.Expiration;
                }
                Response.Cookies.Set(cookie);
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
            if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                HttpCookie cookie = SizeUp.Core.Web.CookieFactory.Create(FormsAuthentication.FormsCookieName);
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
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
            else
            {
                //Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Bad Request: Invalid email.");
            }

            return Json(true);
        }
        [HttpPost]
        public ActionResult Register(string email, string password, string name, long? cityId, long? industryId)
        {
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
                string ReturnUrl = string.IsNullOrWhiteSpace(Request["returnurl"]) ? "/" : Request["returnurl"];

                UserRegistration reg = new UserRegistration()
                {
                    APIKeyId = null,
                    CityId = cityId,
                    IndustryId = industryId,
                    UserId = i.UserId,
                    Email = i.Email,
                    ReturnUrl = ReturnUrl
                };

                Singleton<Tracker>.Instance.UserRegistration(reg);
            }
            catch (MembershipCreateUserException ex)
            {
                //Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Bad Request: " + ex.Message);
            }
            catch (System.Exception ex)
            {
                //Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json("Server Error: " + ex.Message);
            }
            
            return Json(true);
        }

        [HttpPost]
        public ActionResult SendVerification(string email)
        {
            try
            {
                var i = Identity.GetUser(email);
                if (i != null)
                {
                    Singleton<Mailer>.Instance.SendRegistrationEmail(i);
                }

            }
            catch (System.Exception ex)
            {
                return Json("Server Error: " + ex.Message);
            }
            return Json(true);
        }


        [HttpPost]
        public ActionResult Password(string password)
        {

            Identity.CurrentUser.ResetPassword(password);
            return Json(true);
        }

        [HttpPost]
        public ActionResult Profile(Identity identity)
        {
            identity.UserId = Identity.CurrentUser.UserId;
            identity.Email = Identity.CurrentUser.Email;
            identity.IsApproved = Identity.CurrentUser.IsApproved;
            identity.IsLockedOut = Identity.CurrentUser.IsLockedOut;
            identity.Save();

            if (Request.Form.AllKeys.Contains("IndustrySubscriptions"))
            {
                var json = Request.Form["IndustrySubscriptions"];

                var industryIds = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<long[]>(json).ToList();

                List<IndustrySubscription> industrySubscriptions = new List<IndustrySubscription>();
                industryIds.ForEach(i => industrySubscriptions.Add(new IndustrySubscription() { UserId = identity.UserId, IndustryId = i }));
                Singleton<Tracker>.Instance.IndustrySubscriptionsUpdated(identity.UserId, industrySubscriptions);
            }

            return Json(true);
        }

        [HttpGet]
        public ActionResult GetSubscribedIndustries()
        {
            List<long> subscribedIndustryIds = new List<long>();
            List<Core.DataLayer.Models.Industry> subscribedIndustries = new List<Core.DataLayer.Models.Industry>();
            if (User.Identity.IsAuthenticated)
            {
                var userId = (Guid)Membership.GetUser().ProviderUserKey;
                using (var context = ContextFactory.AnalyticsContext)
                {
                    subscribedIndustryIds = context.IndustrySubscriptions.Where(x => x.UserId == userId)
                        .Select(x => x.IndustryId)
                        .ToList<long>();
                }

                using (var context = ContextFactory.SizeUpContext)
                {
                    subscribedIndustries = Core.DataLayer.Industry.Get(context, subscribedIndustryIds)
                        .ToList();
                    //return Json(subscribedIndustries, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(subscribedIndustries, JsonRequestBehavior.AllowGet);
        }


    

    }
}
