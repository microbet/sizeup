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
            dynamic obj = new System.Dynamic.ExpandoObject();
            string key = string.Format("cv-{0}-{1}", placeId, industryId);
            var cookie = Request.Cookies[key];
            object output = null;
            if (!User.Identity.IsAuthenticated)
            {
                if (cookie != null)
                {
                    if (cookie.Values.AllKeys.Contains("competitor"))
                    {
                        obj.competitor = cookie.Values["competitor"].Split(',').Select(i => long.Parse(i)).ToList();
                    }
                    if (cookie.Values.AllKeys.Contains("supplier"))
                    {
                        obj.supplier = cookie.Values["supplier"].Split(',').Select(i => long.Parse(i)).ToList();
                    }
                    if (cookie.Values.AllKeys.Contains("buyer"))
                    {
                        obj.buyer = cookie.Values["buyer"].Split(',').Select(i => long.Parse(i)).ToList();
                    }
                    if (cookie.Values.AllKeys.Contains("consumerExpenditureVariable"))
                    {
                        obj.consumerExpenditureVariable = long.Parse(cookie.Values["consumerExpenditureVariable"]);
                    }
                    if (cookie.Values.AllKeys.Contains("rootId"))
                    {
                        obj.rootId = long.Parse(cookie.Values["rootId"]);
                    }
                }
            }
            else
            {
                using (var context = ContextFactory.UserDataContext)
                {
                    var user = Membership.GetUser(User.Identity.Name);
                    Guid userid = (Guid)user.ProviderUserKey;
                    var item = context.CompetitorAttributes.Where(i => i.UserId == userid && i.PlaceId == placeId && i.IndustryId == industryId).FirstOrDefault();
                    if (item != null)
                    {
                        if (!string.IsNullOrEmpty(item.Competitors))
                        {
                            obj.competitor = item.Competitors.Split(',').Select(i => long.Parse(i)).ToList();
                        }
                        if (!string.IsNullOrEmpty(item.Suppliers))
                        {
                            obj.supplier = item.Suppliers.Split(',').Select(i => long.Parse(i)).ToList();
                        }
                        if (!string.IsNullOrEmpty(item.Buyers))
                        {
                            obj.buyer = item.Buyers.Split(',').Select(i => long.Parse(i)).ToList();
                        }
                        if (item.RootId.HasValue)
                        {
                            obj.rootId = item.RootId;
                        }
                        if (item.ComsumerExpenditureId.HasValue)
                        {
                            obj.consumerExpenditureVariable = item.ComsumerExpenditureId;
                        }
                    }
                }   
            }
            output = ((ExpandoObject)obj).ToDictionary(item => item.Key, item => item.Value);
            return Json(output, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult SetCompetitionValues(long placeId, long industryId)
        {
            string key = string.Format("cv-{0}-{1}", placeId, industryId);
            HttpCookie cookie = new HttpCookie(key);
            CompetitorAttribute attr = new CompetitorAttribute();
            attr.PlaceId = placeId;
            attr.IndustryId = industryId;

            if (Request.Form.AllKeys.Contains("competitor"))
            {
                var ids = Form.IntValues("competitor");
                cookie.Values.Add("competitor", string.Join(",", ids));
                attr.Competitors = string.Join(",", ids);
            }
            if (Request.Form.AllKeys.Contains("supplier"))
            {
                var ids = Form.IntValues("supplier");
                cookie.Values.Add("supplier", string.Join(",", ids));
                attr.Suppliers = string.Join(",", ids);
            }
            if (Request.Form.AllKeys.Contains("buyer"))
            {
                var ids = Form.IntValues("buyer");
                cookie.Values.Add("buyer", string.Join(",", ids));
                attr.Buyers = string.Join(",", ids);
            }
            if (Request.Form.AllKeys.Contains("rootId"))
            {
                var id = Form.StringValue("rootId");
                cookie.Values.Add("rootId", id);
                attr.RootId = int.Parse(id);
            }
            if (Request.Form.AllKeys.Contains("consumerExpenditureVariable"))
            {
                var id = Form.StringValue("consumerExpenditureVariable");
                cookie.Values.Add("consumerExpenditureVariable", id);
                attr.ComsumerExpenditureId = int.Parse(id);
            }


            Response.Cookies.Add(cookie);

            if (User.Identity.IsAuthenticated)
            {
                using (var context = ContextFactory.UserDataContext)
                {
                    Func<string[],string[], bool> tester = (string[] a, string[] b) => (a.Length == b.Length && a.Intersect(b).Count() == a.Length);

                    var user = Membership.GetUser(User.Identity.Name);
                    Guid userid = (Guid)user.ProviderUserKey;
                    attr.UserId = userid;
                    var item = context.CompetitorAttributes.Where(i => i.UserId == userid && i.PlaceId == placeId && i.IndustryId == industryId).FirstOrDefault();
                    if (item != null)
                    {
                        //test for changes... update if there are and then insert into analytics else ignore
                        if (
                            tester(item.Competitors != null ? item.Competitors.Split(',') : "".Split(','), attr.Competitors!=null ? attr.Competitors.Split(',') : "".Split(',')) ||
                            tester(item.Suppliers != null ? item.Suppliers.Split(',') : "".Split(','), attr.Suppliers!=null ? attr.Suppliers.Split(',') : "".Split(',')) ||
                            tester(item.Buyers != null ? item.Buyers.Split(',') : "".Split(','), attr.Buyers!=null ? attr.Buyers.Split(','): "".Split(',')) ||
                            item.RootId != attr.RootId ||
                            item.ComsumerExpenditureId != attr.ComsumerExpenditureId
                            
                            )
                        {
                            item.Competitors = attr.Competitors;
                            item.Buyers = attr.Buyers;
                            item.Suppliers = attr.Suppliers;
                            item.RootId = attr.RootId;
                            item.ComsumerExpenditureId = attr.ComsumerExpenditureId;
                            context.SaveChanges();
                            //fire analytics
                        }
                    }
                    else
                    {
                        context.CompetitorAttributes.AddObject(attr);
                        context.SaveChanges();
                    }
                }
            }


            return Json(true, JsonRequestBehavior.AllowGet);
        }

       

    }
}
