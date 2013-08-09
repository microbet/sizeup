using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using SizeUp.Core.Extensions;
using SizeUp.Core.DataLayer;
using SizeUp.Api.Controllers;
using SizeUp.Core.API;
namespace SizeUp.Api.Areas.Data.Controllers
{
    public class ConsumerExpendituresController : BaseController
    {

        
        [APIAuthorize(Role = "ConsumerExpenditures")]
        public ActionResult Bands(int variableId, long boundingGeographicLocationId, int bands, Core.DataLayer.Granularity granularity)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var output = Core.DataLayer.ConsumerExpenditures.Bands(context, variableId, boundingGeographicLocationId, bands, granularity);
                return Json(output, JsonRequestBehavior.AllowGet);
            }
        }


        
        [APIAuthorize(Role = "ConsumerExpenditures")]
        public ActionResult Variables(int? parentId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.ConsumerExpenditures.Variables(context, parentId).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        
        [APIAuthorize(Role = "ConsumerExpenditures")]
        public ActionResult Variable(int id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.ConsumerExpenditures.Variable(context, id);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        
        [APIAuthorize(Role = "ConsumerExpenditures")]
        public ActionResult VariablePath(int id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.ConsumerExpenditures.VariablePath(context, id);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }


        
        [APIAuthorize(Role = "ConsumerExpenditures")]
        public ActionResult VariableCrosswalk(int id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.ConsumerExpenditures.VariableCrosswalk(context, id);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}




















