using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Web.Areas.Api.Models;
namespace SizeUp.Web.Areas.Api.Controllers
{
    public class CountyController : Controller
    {
        //
        // GET: /Api/County/

        public JsonResult County(int? id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var item = context.Counties.Where(i => i.Id == id);
                var data = item.Select(i => new Models.County.County()
                {
                    Id = i.Id,
                    Name = i.Name,
                    State = i.State.Abbreviation
                }).FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
