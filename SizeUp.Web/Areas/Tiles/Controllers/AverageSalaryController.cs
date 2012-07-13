using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Spatial;
using Microsoft.SqlServer.Types;

using System.Drawing;
using System.Drawing.Drawing2D;
using SizeUp.Data;
using SizeUp.Core.Tiles;
using SizeUp.Core.Geo;
using SizeUp.Core.Extensions;
using SizeUp.Core;


namespace SizeUp.Web.Areas.Tiles.Controllers
{
    public class AverageSalaryController : Controller
    {
        //
        // GET: /Tiles/AverageSalary/

       
        public ActionResult County(int x, int y, int zoom, int industryId, string colors, string boundingEntityId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                string[] colorArray = colors.Split(',');
                List<GeographyCollection> collection = new List<GeographyCollection>();
                Heatmap tile = new Heatmap(256, 256, x, y, zoom);
                BoundingEntity boundingEntity = new BoundingEntity(boundingEntityId);

                IQueryable<long> ids = context.Counties.Select(i => i.Id);
                if (boundingEntity.EntityType == BoundingEntity.BoundingEntityType.Metro)
                {
                    ids = context.Counties
                       .Where(i => i.MetroId == boundingEntity.EntityId)
                       .Select(i => i.Id);
                }
                else if (boundingEntity.EntityType == BoundingEntity.BoundingEntityType.State)
                {
                    ids = context.Counties
                       .Where(i => i.StateId == boundingEntity.EntityId)
                       .Select(i => i.Id);
                }

                var data = context.IndustryDataByCounties
                    .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter && i.AverageRevenue > 0)
                    .Join(ids, i => i.CountyId, i => i, (i, o) => i)
                    .Select(i => new { i.AverageAnnualSalary, i.CountyId })
                    .ToList();

                var bands = data.NTile(i => i.AverageAnnualSalary, colorArray.Length)
                    .ToList();


                var displayGeos = context.CountyGeographies
                    .Join(ids, i => i.CountyId, i => i, (i, o) => i)
                    .Where(i => i.GeographyClass.Name == "Display")
                    .Select(i => new { i.CountyId, i.Geography.GeographyPolygon })
                    .ToList();


                var bandedGeos = bands.Select(b => b.Join(displayGeos, i => i.CountyId, i => i.CountyId, (i, o) => o).ToList()).ToList();
                var noData = displayGeos.Where(g => !data.Select(ig => ig.CountyId).Contains(g.CountyId)).ToList();


                for (var b = 0; b < bandedGeos.Count; b++)
                {
                    var geoCollection = new GeographyCollection();
                    geoCollection.Geographies.AddRange(bandedGeos[b].Select(i => SqlGeography.Parse(i.GeographyPolygon.AsText())).ToList());
                    geoCollection.Color = colorArray[b];
                    collection.Add(geoCollection);
                }

                var noDataCollection = new GeographyCollection();
                noDataCollection.Geographies = new List<SqlGeography>();
                noDataCollection.Geographies.AddRange(noData.Select(i => SqlGeography.Parse(i.GeographyPolygon.AsText())).ToList());
                collection.Add(noDataCollection);


                tile.Draw(collection);
                var stream = new System.IO.MemoryStream();
                tile.Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return File(stream.GetBuffer(), "image/png");
            }
        }

        public ActionResult State(int x, int y, int zoom, int industryId, string colors)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                string[] colorArray = colors.Split(',');
                List<GeographyCollection> collection = new List<GeographyCollection>();
                Heatmap tile = new Heatmap(256, 256, x, y, zoom);


                var boundingBox = tile.GetBoundingBox(0.2f);
                var boundingGeo = tile.GetBoundingGeography();
                var boundingSpatial = DbGeography.FromText((string)boundingGeo.STAsText().ToSqlString());


                var data = context.IndustryDataByStates
                  .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter && i.AverageRevenue > 0)
                  .Select(i => new { i.StateId, i.AverageAnnualSalary });

                var bands = data.ToList()
                  .NTile(i => i.AverageAnnualSalary, colorArray.Length)
                  .ToList();


                var ids = context.States
                    .Select(i => i.Id);


                var displayGeos = context.StateGeographies
                    .Join(ids, i => i.StateId, i => i, (i, o) => i)
                    .Where(i => i.GeographyClass.Name == "Display")
                    .Select(i => new { i.StateId, i.Geography.GeographyPolygon })
                    .ToList();


                var bandedGeos = bands.Select(b => b.Join(displayGeos, i => i.StateId, i => i.StateId, (i, o) => o).ToList()).ToList();
                var noData = displayGeos.Where(g => !data.Select(ig => ig.StateId).Contains(g.StateId)).ToList();

                for (var b = 0; b < bandedGeos.Count; b++)
                {
                    var geoCollection = new GeographyCollection();
                    geoCollection.Geographies.AddRange(bandedGeos[b].Select(i => SqlGeography.Parse(i.GeographyPolygon.AsText())).ToList());
                    geoCollection.Color = colorArray[b];
                    collection.Add(geoCollection);
                }

                var noDataCollection = new GeographyCollection();
                noDataCollection.Geographies = new List<SqlGeography>();
                noDataCollection.Geographies.AddRange(noData.Select(i => SqlGeography.Parse(i.GeographyPolygon.AsText())).ToList());
                collection.Add(noDataCollection);


                tile.Draw(collection);
                var stream = new System.IO.MemoryStream();
                tile.Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return File(stream.GetBuffer(), "image/png");
            }
        }
    }
}
