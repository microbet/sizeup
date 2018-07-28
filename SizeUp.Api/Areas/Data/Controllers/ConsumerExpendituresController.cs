using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.EntityClient;
using System.Data.SqlClient;
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
using SizeUp.Data;
namespace SizeUp.Api.Areas.Data.Controllers
{
    public class ConsumerExpendituresController : BaseController
    {

        public class Payload
        {
            public string Name { get; set; }
            public long Value { get; set; }

        }

        public List<Payload> GetPayloads(string variable, int granularityId, long boundingGeographicLocationId)
        {
            string queryString = string.Format(
                "select LongName, {0} " +
                "from dbo.ConsumerExpenditures as ce " +
                "join dbo.GeographicLocation as gl on ce.GeographicLocationId = gl.Id " +
                "where ce.GeographicLocationId in ( " +
                "SELECT gl.GeographicLocationId " +
                "FROM dbo.GeographicLocation as g " +
                "join dbo.GeographicLocationGeographicLocation as gl on g.Id = gl.GeographicLocationId " +
                "where GranularityId={1} and gl.IntersectedGeographicLocationId = {2})",
                variable, granularityId, boundingGeographicLocationId);
            var conn = new EntityConnection(ConfigurationManager.ConnectionStrings["SizeUpContext"].ConnectionString);
            List<Payload> t = new List<Payload>();
            using (SqlConnection connection = new SqlConnection(conn.StoreConnection.ConnectionString))
            {
                SqlCommand command =
                    new SqlCommand(queryString, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data. 
                while (reader.Read())
                {
                    t.Add(new Payload() { Name = reader[0].ToString(), Value = Convert.ToInt64(reader[1]) });
                }

                // Call Close when done reading.
                reader.Close();
            }
            return t;
        }

        [APIAuthorize(Role = "ConsumerExpenditures")]
        public ActionResult Bands(
            int variableId, long boundingGeographicLocationId, int bands,
            Core.DataLayer.Granularity granularity, string contentType = "*/*")
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                if ("text/html".Equals(contentType))
                {
                    var variable = context.ConsumerExpenditureVariables
                        .Where(i => i.Id == variableId)
                        .FirstOrDefault();
                    var gran = Enum.GetName(typeof(Core.DataLayer.Granularity), granularity);

                    int granularityId = 0;
                    if (granularity == Core.DataLayer.Granularity.ZipCode)
                    {
                        granularityId = 1;
                    }
                    else if (granularity == Core.DataLayer.Granularity.City)
                    {
                        granularityId = 2;
                    }
                    else if (granularity == Core.DataLayer.Granularity.County)
                    {
                        granularityId = 3;
                    }
                    else if (granularity == Core.DataLayer.Granularity.Place)
                    {
                        granularityId = 4;
                    }
                    else if (granularity == Core.DataLayer.Granularity.Metro)
                    {
                        granularityId = 5;
                    }
                    else if (granularity == Core.DataLayer.Granularity.State)
                    {
                        granularityId = 6;
                    }
                    else if (granularity == Core.DataLayer.Granularity.Nation)
                    {
                        granularityId = 7;
                    }

                    var data = context.ConsumerExpenditures
                        .Where(i => i.Year == CommonFilters.TimeSlice.ConsumerExpenditures.Year && i.Quarter == CommonFilters.TimeSlice.ConsumerExpenditures.Quarter)
                        .Where(i => i.GeographicLocation.Granularity.Name == gran)
                        .Where(i => i.GeographicLocation.GeographicLocations.Any(g => g.Id == boundingGeographicLocationId));

                    int count = data.ToList().Count();

                    var output = GetPayloads(variable.Variable, granularityId, boundingGeographicLocationId)
                    .NTileDescending(i => i.Value, bands)
                       .Select(i => new Kpi.Band
                       {
                           Min = string.Format("${0}", Kpi.Format(i.Min(v => v.Value))),
                           Max = string.Format("${0}", Kpi.Format(i.Max(v => v.Value))),
                           Items = i.Select(v => v.Name).ToList()
                       })
                       .ToList();

                    ViewBag.Area = context.GeographicLocations
                        .Where(i => i.Id == boundingGeographicLocationId)
                        .Select(i => i.LongName)
                        .FirstOrDefault();
                    ViewBag.Bands = Kpi.FormatBands(output);
                    ViewBag.Expenditure = variable;
                    ViewBag.LevelOfDetail = Kpi.TranslateGranularity(granularity);
                    ViewBag.Q = bands.ToString();
                    ViewBag.ThemeUrl = System.Configuration.ConfigurationManager.AppSettings["Theme.Url"];
                    ViewBag.Query = string.Format(
                        "Rank {0} in {1}, by {2} Consumer Spending on {3}, in {4} quantiles",
                        ViewBag.LevelOfDetail, ViewBag.Area,
                        variable.Variable.StartsWith("X") ? "Average" : "Total",
                        variable.Description, ViewBag.Q
                    );
                    return View("Heatmap");
                }
                else
                {
                    var output = Core.DataLayer.ConsumerExpenditures.Bands(context, variableId, boundingGeographicLocationId, bands, granularity);
                    return Json(output, JsonRequestBehavior.AllowGet);
                }
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




















