using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SizeUp.Core;
using SizeUp.Core.Serialization;
using System.Dynamic;

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Api/User/

        public ActionResult GetDashboardValues()
        {
           // dynamic dynamicContext = new DynamicContext();

            dynamic obj = new System.Dynamic.ExpandoObject();
            var cookie = Request.Cookies["dashboardValues"];

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
            return Json(((ExpandoObject)obj).ToDictionary(item => item.Key, item => item.Value), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetDashboardValues()
        {
            HttpCookie cookie = new HttpCookie("dashboardValues");
            if (Request.Form.AllKeys.Contains("businessSize"))
            {
                cookie.Values.Add("businessSize", Request["businessSize"]);
            }
            if (Request.Form.AllKeys.Contains("businessType"))
            {
                cookie.Values.Add("businessType", Request["businessType"]);
            }
            if (Request.Form.AllKeys.Contains("employees"))
            {
                cookie.Values.Add("employees", Request["employees"]);
            }
            if (Request.Form.AllKeys.Contains("healthcareCost"))
            {
                cookie.Values.Add("healthcareCost", Request["healthcareCost"]);
            }
            if (Request.Form.AllKeys.Contains("revenue"))
            {
                cookie.Values.Add("revenue", Request["revenue"]);
            }
            if (Request.Form.AllKeys.Contains("salary"))
            {
                cookie.Values.Add("salary", Request["salary"]);
            }
            if (Request.Form.AllKeys.Contains("workersComp"))
            {
                cookie.Values.Add("workersComp", Request["workersComp"]);
            }
            if (Request.Form.AllKeys.Contains("yearStarted"))
            {
                cookie.Values.Add("yearStarted", Request["yearStarted"]);
            }
            Response.Cookies.Add(cookie);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

       

    }
}
