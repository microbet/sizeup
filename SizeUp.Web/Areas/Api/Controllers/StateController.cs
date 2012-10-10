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
    public class StateController : BaseController
    {
        //
        // GET: /Api/State/

        public JsonResult State(long? id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var item = context.States.Where(i => i.Id == id);
                var data = item.Select(i => new Models.State.State()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Abbreviation = i.Abbreviation,
                    SEOKey = i.SEOKey
                }).FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
