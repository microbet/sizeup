using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.DataAccess;
using SizeUp.Core;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using SizeUp.Core.Extensions;
using SizeUp.Web.Areas.Api.Models;
using SizeUp.Core.DataAccess.Models;

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class ConsumerExpendituresController : BaseController
    {

        public ActionResult Index(string aggregationLevel, int variableId, int bands, string boundingEntityId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var variableName = context.ConsumerExpenditureVariables.Where(i => i.Id == variableId).Select(i => i.Variable).FirstOrDefault();
                BoundingEntity boundingEntity = new BoundingEntity(boundingEntityId);

                IQueryable<ConsumerExpenditureBandItem> data = null;
                if (aggregationLevel == "state")
                {
                    data = ConsumerExpenditureData.GetStates(context, variableName);
                }
                else if (aggregationLevel == "county")
                {
                    data = ConsumerExpenditureData.GetCounties(context, variableName, boundingEntity);
                }
                else if (aggregationLevel == "zip")
                {
                    data = ConsumerExpenditureData.GetZips(context, variableName, boundingEntity);
                }

                var bandData =
                    data.NTile(i => i.Value, bands)
                     .Select(b => new Models.ConsumerExpenditures.Band() { Min = b.Min(i => i.Value), Max = b.Max(i => i.Value) })
                     .ToList();

                Models.ConsumerExpenditures.Band old = null;
                foreach (var band in bandData)
                {
                    if (old != null)
                    {
                        old.Max = band.Min;
                    }
                    old = band;
                }

                return Json(bandData, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Variables(int? parentId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = context.ConsumerExpenditureVariables
                    .Where(i => parentId == null ? i.ParentId == null : i.ParentId == parentId)
                    .Select(i => new
                    {
                        i.Id,
                        i.ParentId,
                        i.Description,
                        i.Variable,
                        HasChildren = context.ConsumerExpenditureVariables.Where(c => c.ParentId == i.Id).Count() > 0
                    })
                    .ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Variable(int id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = context.ConsumerExpenditureVariables
                    .Where(i => i.Id == id)
                    .Select(i => new
                    {
                        i.Id,
                        i.ParentId,
                        i.Description,
                        i.Variable,
                        HasChildren = context.ConsumerExpenditureVariables.Where(c => c.ParentId == i.Id).Count() > 0
                    })
                    .FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult VariablePath(int id)
        {
            /* 
                i know what youre thinking.... and you are absolutely correct. I should be shot/hung/beaten/tortured/unthinkable
                i hated myself while i did this
                i wanted to commit sepiku
                i died a little inside
                it does work...and its not unbearibly slow
                so unless something comes to light that this is breaking the server....ill leave as is
                trying to pretend that it was only a dream
                hoping noone will notice
                wishing there was another way (without stored sprocs)
                OMG LOOK UNICORNS!
                arent you supposed to be looking at some other method?
                i think so....
                nothing to see here, move along
                        
            */
 

            using (var context = ContextFactory.SizeUpContext)
            {
                List<ConsumerExpenditureVariable> vars = new List<ConsumerExpenditureVariable>();
                ConsumerExpenditureVariable v = context.ConsumerExpenditureVariables.Where(i => i.Id == id).FirstOrDefault();
                vars.Add(v);
                while(v.ParentId!=null){
                v = context.ConsumerExpenditureVariables.Where(i => i.Id == v.ParentId).FirstOrDefault();
                    vars.Add(v);
                }

                var data = vars.Select(i => new
                    {
                        i.Id,
                        i.ParentId,
                        i.Description,
                        i.Variable,
                        HasChildren = context.ConsumerExpenditureVariables.Where(c => c.ParentId == i.Id).Count() > 0
                    })
                    .Reverse()
                    .ToList();
                    
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}




















