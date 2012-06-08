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
                List<Heatmap.GeographyCollection> collection = new List<Heatmap.GeographyCollection>();
                Heatmap tile = new Heatmap(256, 256, x, y, zoom);
                BoundingEntity boundingEntity = new BoundingEntity(boundingEntityId);

                var boundingBox = tile.GetBoundingGeography(boundingEntity.Geography);
                var boundingSpatial = DbGeography.FromText((string)boundingBox.STAsText().ToSqlString());

                var geos = context.Counties.Where(i => i.Geography.Intersects(boundingSpatial)).Select(i => new { i.Id, i.Geography }).ToList();

                var naics = context.SicToNAICSMappings.Where(i => i.IndustryId == industryId).Select(i => i.NAICS);
                var filters = context.AverageSalaryByCounties
                    .Where(i => i.NAICSId == naics.FirstOrDefault().Id && i.AverageSalary > 0);
                var max = filters.Select(i => i.Year)
                   .OrderByDescending(i => i);
                filters = filters.Where(i => i.Year == max.FirstOrDefault());

                if (boundingEntity.EntityType != null && boundingEntity.EntityType == BoundingEntity.BoundingEntityType.State)
                {
                    filters = filters.Where(i => i.County.StateId == boundingEntity.EntityId);
                }
                else if (boundingEntity.EntityType != null && boundingEntity.EntityType == BoundingEntity.BoundingEntityType.Metro)
                {
                    filters = filters.Where(i => i.County.MetroId == boundingEntity.EntityId);
                }
                var data = filters.OrderBy(i => i.AverageSalary)
                    .Select(i => new { i.AverageSalary, i.CountyId })
                    .ToList();


                var bands = data.NTile(i => i.AverageSalary, colorArray.Length).ToList();
                var bandedGeos = bands.Select(b => b.Join(geos, i => i.CountyId, i => i.Id, (i, o) => new { o.Id, o.Geography, i.AverageSalary }).ToList()).ToList();
                var noData = geos.Where(g => !data.Select(ig => ig.CountyId).Contains(g.Id)).ToList();

                for (var b = 0; b < bandedGeos.Count; b++)
                {
                    var geoCollection = new Heatmap.GeographyCollection();
                    geoCollection.Geographies.AddRange(bandedGeos[b].Select(i => SqlGeography.Parse(i.Geography.AsText())).ToList());
                    geoCollection.Color = colorArray[b];
                    collection.Add(geoCollection);
                }

                var noDataCollection = new Heatmap.GeographyCollection();
                noDataCollection.Geographies = new List<SqlGeography>();
                noDataCollection.Geographies.AddRange(noData.Select(i => SqlGeography.Parse(i.Geography.AsText())).ToList());
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
                List<Heatmap.GeographyCollection> collection = new List<Heatmap.GeographyCollection>();
                Heatmap tile = new Heatmap(256, 256, x, y, zoom);


                var boundingBox = tile.GetBoundingGeography();
                var boundingSpatial = DbGeography.FromText((string)boundingBox.STAsText().ToSqlString());

                var geos = context.States.Where(i => i.Geography.Intersects(boundingSpatial)).Select(i => new { i.Id, i.Geography }).ToList();
                var naics = context.SicToNAICSMappings.Where(i => i.IndustryId == industryId).Select(i => i.NAICS);
                var filters = context.AverageSalaryByStates
                    .Where(i => i.NAICSId == naics.FirstOrDefault().Id && i.AverageSalary > 0);

                var max = filters.Select(i => i.Year)
                   .OrderByDescending(i => i);
                filters = filters.Where(i => i.Year == max.FirstOrDefault());


                var data = filters.OrderBy(i => i.AverageSalary)
                    .Select(i => new { i.AverageSalary, i.StateId })
                    .ToList();

                var bands = data.NTile(i => i.AverageSalary, colorArray.Length).ToList();
                var bandedGeos = bands.Select(b => b.Join(geos, i => i.StateId, i => i.Id, (i, o) => new { o.Id, o.Geography, i.AverageSalary }).ToList()).ToList();
                var noData = geos.Where(g => !data.Select(ig => ig.StateId).Contains(g.Id)).ToList();

                for (var b = 0; b < bandedGeos.Count; b++)
                {
                    var geoCollection = new Heatmap.GeographyCollection();
                    geoCollection.Geographies.AddRange(bandedGeos[b].Select(i => SqlGeography.Parse(i.Geography.AsText())).ToList());
                    geoCollection.Color = colorArray[b];
                    collection.Add(geoCollection);
                }

                var noDataCollection = new Heatmap.GeographyCollection();
                noDataCollection.Geographies = new List<SqlGeography>();
                noDataCollection.Geographies.AddRange(noData.Select(i => SqlGeography.Parse(i.Geography.AsText())).ToList());
                collection.Add(noDataCollection);




                tile.Draw(collection);
                var stream = new System.IO.MemoryStream();
                tile.Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return File(stream.GetBuffer(), "image/png");
            }
        }
    }
}
