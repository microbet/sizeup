using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SizeUp.Web.Areas.Tiles.Controllers
{
    public class AverageSalaryController : Controller
    {
        //
        // GET: /Tiles/AverageSalary/

        public ActionResult Get(int x, int y, int z)
        {
            if(!string.IsNullOrEmpty(Request["countyId"]))
            {

            }
            else if (!string.IsNullOrEmpty(Request["metroId"]))
            {

            }
            else if (!string.IsNullOrEmpty(Request["stateId"]))
            {

            }
            else
            {
                throw new ArgumentException("Must Provide a county, metro, or state id");
            }
            return View();

            //return File();
        }

       

    }
}
