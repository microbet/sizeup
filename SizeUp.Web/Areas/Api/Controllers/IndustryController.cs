using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.DataLayer;

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class IndustryController : BaseController
    {
        //
        // GET: /Api/Industry/
        public ActionResult Industry(long id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Industry.Get(context, id);
                return this.Jsonp(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult List(List<long> ids)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Industry.List(context, ids);
                return this.Jsonp(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Current()
        {
            var id = SizeUp.Core.Web.WebContext.Current.CurrentIndustryId;
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Industry.Get(context, id);
                return this.Jsonp(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Current(long id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var c = context.Industries.Where(i => i.Id == id).FirstOrDefault();
                if (c != null)
                {
                    WebContext.Current.CurrentIndustryId = id;
                }
                return this.Jsonp(c != null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Search(string term, int maxResults = 35)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Industry.Search(context, term).Take(maxResults).ToList();
                return this.Jsonp(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
