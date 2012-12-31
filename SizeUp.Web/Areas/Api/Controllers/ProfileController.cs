using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SizeUp.Core;
using SizeUp.Core.Serialization;
using System.Dynamic;
using SizeUp.Data;
using System.Web.Security;
using SizeUp.Data.UserData;
using SizeUp.Core.Web;

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class ProfileController : BaseController
    {
        //
        // GET: /Api/User/

        public ActionResult GetDashboardValues(long placeId, long industryId)
        {
            dynamic obj = new System.Dynamic.ExpandoObject();
            string key = string.Format("dv-{0}-{1}", placeId, industryId);
            var cookie = Request.Cookies[key];
            object output = null;
            if (!User.Identity.IsAuthenticated)
            {
                if (cookie != null)
                {
                    if (cookie.Values.AllKeys.Contains("businessSize"))
                    {
                        obj.businessSize = cookie.Values["businessSize"];
                    }
                    if (cookie.Values.AllKeys.Contains("businessType"))
                    {
                        obj.businessType = cookie.Values["businessType"];
                    }
                    if (cookie.Values.AllKeys.Contains("employees"))
                    {
                        obj.employees = cookie.Values["employees"];
                    }
                    if (cookie.Values.AllKeys.Contains("healthcareCost"))
                    {
                        obj.healthcareCost = cookie.Values["healthcareCost"];
                    }
                    if (cookie.Values.AllKeys.Contains("revenue"))
                    {
                        obj.revenue = cookie.Values["revenue"];
                    }
                    if (cookie.Values.AllKeys.Contains("salary"))
                    {
                        obj.salary = cookie.Values["salary"];
                    }
                    if (cookie.Values.AllKeys.Contains("workersComp"))
                    {
                        obj.workersComp = cookie.Values["workersComp"];
                    }
                    if (cookie.Values.AllKeys.Contains("yearStarted"))
                    {
                        obj.yearStarted = cookie.Values["yearStarted"];
                    }
                }
            }
            else
            {
                using (var context = ContextFactory.UserDataContext)
                {
                    var user = Membership.GetUser(User.Identity.Name);
                    Guid userid = (Guid)user.ProviderUserKey;
                    var item = context.BusinessAttributes.Where(i => i.UserId == userid && i.PlaceId == placeId && i.IndustryId == industryId).FirstOrDefault();
                    if (item != null)
                    {
                        if (!string.IsNullOrEmpty(item.BusinessSize))
                        {
                            obj.businessSize = item.BusinessSize;
                        }
                        if (!string.IsNullOrEmpty(item.BusinessType))
                        {
                            obj.businessType = item.BusinessType;
                        }
                        if (item.Employees.HasValue)
                        {
                            obj.employees = item.Employees;
                        }
                        if (item.HealthcareCost.HasValue)
                        {
                            obj.healthcareCost = item.HealthcareCost;
                        }
                        if (item.Revenue.HasValue)
                        {
                            obj.revenue = item.Revenue;
                        }
                        if (item.AverageSalary.HasValue)
                        {
                            obj.salary = item.AverageSalary;
                        }
                        if (item.WorkersComp.HasValue)
                        {
                            obj.workersComp = item.WorkersComp;
                        }
                        if (item.YearStarted.HasValue)
                        {
                            obj.yearStarted = item.YearStarted;
                        }
                    }
                }
            }
            output = ((ExpandoObject)obj).ToDictionary(item => item.Key, item => item.Value);
            return Json(output, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetDashboardValues(long placeId, long industryId)
        {
            string key = string.Format("dv-{0}-{1}", placeId, industryId);
            HttpCookie cookie = new HttpCookie(key);
            BusinessAttribute attr = new BusinessAttribute();
            attr.PlaceId = placeId;
            attr.IndustryId = industryId;

            if (Request.Form.AllKeys.Contains("businessSize"))
            {
                cookie.Values.Add("businessSize", Request["businessSize"]);
                attr.BusinessSize = Request["businessSize"];
            }
            if (Request.Form.AllKeys.Contains("businessType"))
            {
                cookie.Values.Add("businessType", Request["businessType"]);
                attr.BusinessType = Request["businessType"];
            }
            if (Request.Form.AllKeys.Contains("employees"))
            {
                cookie.Values.Add("employees", Request["employees"]);
                attr.Employees = int.Parse(Request["employees"]);
            }
            if (Request.Form.AllKeys.Contains("healthcareCost"))
            {
                cookie.Values.Add("healthcareCost", Request["healthcareCost"]);
                attr.HealthcareCost = int.Parse(Request["healthcareCost"]);
            }
            if (Request.Form.AllKeys.Contains("revenue"))
            {
                cookie.Values.Add("revenue", Request["revenue"]);
                attr.Revenue = int.Parse(Request["revenue"]);
            }
            if (Request.Form.AllKeys.Contains("salary"))
            {
                cookie.Values.Add("salary", Request["salary"]);
                attr.AverageSalary = int.Parse(Request["salary"]);
            }
            if (Request.Form.AllKeys.Contains("workersComp"))
            {
                cookie.Values.Add("workersComp", Request["workersComp"]);
                attr.WorkersComp = decimal.Parse(Request["workersComp"]);
            }
            if (Request.Form.AllKeys.Contains("yearStarted"))
            {
                cookie.Values.Add("yearStarted", Request["yearStarted"]);
                attr.YearStarted = int.Parse(Request["yearStarted"]);
            }
            Response.Cookies.Add(cookie);

            if (User.Identity.IsAuthenticated)
            {
                using (var context = ContextFactory.UserDataContext)
                {
                    var user = Membership.GetUser(User.Identity.Name);
                    Guid userid = (Guid)user.ProviderUserKey;
                    attr.UserId = userid;
                    var item = context.BusinessAttributes.Where(i => i.UserId == userid && i.PlaceId == placeId && i.IndustryId == industryId).FirstOrDefault();
                    if (item != null)
                    {
                        //test for changes... update if there are and then insert into analytics else ignore
                        if (
                            item.AverageSalary != attr.AverageSalary ||
                            item.BusinessSize != attr.BusinessSize ||
                            item.BusinessType != attr.BusinessType ||
                            item.Employees != attr.Employees ||
                            item.HealthcareCost != attr.HealthcareCost ||
                            item.Revenue != attr.Revenue ||
                            item.WorkersComp != attr.WorkersComp ||
                            item.YearStarted != attr.YearStarted
                            )
                        {
                            item.AverageSalary = attr.AverageSalary;
                            item.BusinessSize = attr.BusinessSize;
                            item.BusinessType = attr.BusinessType;
                            item.Employees = attr.Employees;
                            item.HealthcareCost = attr.HealthcareCost;
                            item.Revenue = attr.Revenue;
                            item.WorkersComp = attr.WorkersComp;
                            item.YearStarted = attr.YearStarted;
                            context.SaveChanges();
                            //fire analytics
                        }
                    }
                    else
                    {
                        context.BusinessAttributes.AddObject(attr);
                        context.SaveChanges();
                    }
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }






        public ActionResult GetCompetitionValues(long placeId, long industryId)
        {
            string key = string.Format("cv-{0}-{1}", placeId, industryId);
            var cookie = Request.Cookies[key];
            Models.Profile.CompetitionValues Values = new Models.Profile.CompetitionValues();

            if (!User.Identity.IsAuthenticated)
            {
                if (cookie != null)
                {
                    if (cookie.Values.AllKeys.Contains("competitorIds"))
                    {
                        Values.CompetitorIds = cookie.Values["competitorIds"].Split(',').Select(i => long.Parse(i)).ToList();
                    }
                    if (cookie.Values.AllKeys.Contains("supplierIds"))
                    {
                        Values.SupplierIds = cookie.Values["supplierIds"].Split(',').Select(i => long.Parse(i)).ToList();
                    }
                    if (cookie.Values.AllKeys.Contains("buyerIds"))
                    {
                        Values.BuyerIds = cookie.Values["buyerIds"].Split(',').Select(i => long.Parse(i)).ToList();
                    }
                    if (cookie.Values.AllKeys.Contains("consumerExpenditureId"))
                    {
                        Values.ConsumerExpenditureId = long.Parse(cookie.Values["consumerExpenditureId"]);
                    }
                }
            }
            else
            {
                using (var context = ContextFactory.UserDataContext)
                {
                    var user = Membership.GetUser(User.Identity.Name);
                    Guid userid = (Guid)user.ProviderUserKey;

                    //Values = context.CompetitorAttributes.Where(i => i.UserId == userid && i.PlaceId == placeId && i.IndustryId == industryId).FirstOrDefault();
                }   
            }
            return Json(Values, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult SetCompetitionValues(long placeId, long industryId)
        {
            string key = string.Format("cv-{0}-{1}", placeId, industryId);
            HttpCookie cookie = new HttpCookie(key);



            if (Request.Form.AllKeys.Contains("competitorIds"))
            {
                var ids = QueryString.IntValues("competitorIds");
                cookie.Values.Add("competitorIds", string.Join(",", ids));
                //attr.BusinessSize = Request["businessSize"];
            }
            if (Request.Form.AllKeys.Contains("supplierIds"))
            {
                var ids = QueryString.IntValues("supplierIds");
                cookie.Values.Add("supplierIds", string.Join(",", ids));
                //attr.BusinessSize = Request["businessSize"];
            }
            if (Request.Form.AllKeys.Contains("buyerIds"))
            {
                var ids = QueryString.IntValues("buyerIds");
                cookie.Values.Add("buyerIds", string.Join(",", ids));
                //attr.BusinessSize = Request["businessSize"];
            }


            Response.Cookies.Add(cookie);


            return Json(true, JsonRequestBehavior.AllowGet);
        }

       

    }
}
