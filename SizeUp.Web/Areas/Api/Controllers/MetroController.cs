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
    public class MetroController : BaseController
    {
        //
        // GET: /Api/Metro/

        public JsonResult Metro(int? id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var item = context.Metroes.Where(i => i.Id == id);
                var data = item.Select(i => new Models.Metro.Metro()
                {
                    Id = i.Id,
                    Name = i.Name
                }).FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
