using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.Extensions;
using SizeUp.Web.Areas.Api.Models;
using SizeUp.Core;
using SizeUp.Core.DataAccess;


namespace SizeUp.Web.Areas.Api.Controllers
{
    public class HealthCareController : Controller
    {
        //
        // GET: /Api/HealthCare/

        public ActionResult HealthCare(long industryId, long placeId, long? employees)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var locations = Locations.Get(context, placeId).FirstOrDefault();

                var raw = IndustryData.GetState(context, industryId, locations.State.Id)
                    .Select(i => new
                    {
                        i.HealthcareByState,
                        i.HealthcareByIndustry,
                        i.Healthcare0To9Employees,
                        i.Healthcare10To24Employees,
                        i.Healthcare25To99Employees,
                        i.Healthcare100To999Employees,
                        i.Healthcare1000orMoreEmployees,
                        i.Healthcare0To9EmployeesRank,
                        i.Healthcare10To24EmployeesRank,
                        i.Healthcare25To99EmployeesRank,
                        i.Healthcare100To999EmployeesRank,
                        i.Healthcare1000orMoreEmployeesRank,
                        i.HealthcareByIndustryRank,
                        i.HealthcareByStateRank
                    })
                    .FirstOrDefault();


                Models.Healthcare.Chart data = new Models.Healthcare.Chart();
               

                data.Industry = raw.HealthcareByIndustry;
                data.IndustryRank = raw.HealthcareByIndustryRank;
                data.State = raw.HealthcareByState;
                data.StateRank = raw.HealthcareByStateRank;

                if (employees <= 9)
                {
                    data.FirmSize = raw.Healthcare0To9Employees;
                    data.FirmSizeRank = raw.Healthcare0To9EmployeesRank;
                }
                else if (employees <= 24)
                {
                    data.FirmSize = raw.Healthcare10To24Employees;
                    data.FirmSizeRank = raw.Healthcare10To24EmployeesRank;
                }
                else if (employees <= 99)
                {
                    data.FirmSize = raw.Healthcare25To99Employees;
                    data.FirmSizeRank = raw.Healthcare25To99EmployeesRank;
                }
                else if (employees <= 999)
                {
                    data.FirmSize = raw.Healthcare100To999Employees;
                    data.FirmSizeRank = raw.Healthcare100To999EmployeesRank;
                }
                else if (employees >= 1000)
                {
                    data.FirmSize = raw.Healthcare1000orMoreEmployees;
                    data.FirmSizeRank = raw.Healthcare1000orMoreEmployeesRank;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Percentage(int industryId, long placeId, double value)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var locations = Locations.Get(context, placeId).FirstOrDefault();

                var data = IndustryData.GetState(context, industryId, locations.State.Id)
                    .Select(i => i.HealthcareByState.Value)
                    .FirstOrDefault();

                object obj = null;
                if (data != 0)
                {
                    obj = new
                    {
                        Percentage = (int)(((value - data) / data) * 100)
                    };
                }
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
