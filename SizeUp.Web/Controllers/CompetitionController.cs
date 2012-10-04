using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Web.Models;
using SizeUp.Core.Serialization;
using SizeUp.Core.Web;
using SizeUp.Data;
using Api = SizeUp.Web.Areas.Api;
namespace SizeUp.Web.Controllers
{
    public class CompetitionController : BaseController
    {
        //
        // GET: /Competition/

        public ActionResult Index()
        {
            ViewBag.Header.ActiveTab = NavItems.Competition;
            using (var context = ContextFactory.SizeUpContext)
            {
                 var raw = context.ConsumerExpenditureVariables.Select(i => new Models.ConsumerExpenditures.Variable(){
                         Id = i.Id,
                         ParentId = i.ParentId,
                         Name = i.Description 
                 }).ToList();

                 List<Models.ConsumerExpenditures.Variable> data = new List<Models.ConsumerExpenditures.Variable>();
                 for (int x =0;x< raw.Count;x++)
                 {
                     raw[x].Children = raw.Where(i => i.ParentId == raw[x].Id).ToList();
                     if (raw[x].ParentId == null)
                     {
                         data.Add(raw[x]);
                     }
                 }

                 ViewBag.ConsumerExpenditureVariables = data;
                 ViewBag.ConsumerExpenditureVariablesJSON = Serializer.ToJSON(data);
                return View();
            }

            
        }

    }
}
