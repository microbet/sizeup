using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.DataLayer;
using SizeUp.Core.API;

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class IndustryController : BaseController
    {
        //
        // GET: /Api/Industry/



        [HttpGet]
        public ActionResult Current()
        {
            var data = SizeUp.Core.Web.WebContext.Current.CurrentIndustry != null ? SizeUp.Core.Web.WebContext.Current.CurrentIndustry : null;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Current(long id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
               /* var c = context.Industries.Where(i => i.Id == id).FirstOrDefault();
                if (c != null)
                {
                    WebContext.Current.CurrentIndustryId = id;
                }*/
                //deprecated
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
