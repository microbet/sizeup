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

                var boundingBox = tile.GetBoundingBox(0.2f);
                var boundingGeo = tile.GetBoundingGeography(boundingEntity.Geography);
                var boundingSpatial = DbGeography.FromText((string)boundingGeo.STAsText().ToSqlString());


                var geos = context.CountyGeographies
                   .Where(i => i.GeographyClass.Name == "Calculation" && i.Geography.GeographyPolygon.Intersects(boundingEntity.DbGeography.Buffer(-1)))
                   .Select(i => i.CountyId);

                var data = context.IndustryDataByCounties
                    .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter && i.AverageRevenue > 0)
                    .Join(geos, i => i.CountyId, i => i, (i, o) => i)
                    .Select(i => new { i.AverageAnnualSalary, i.CountyId })
                    .ToList();

                var bands = data.NTile(i => i.AverageAnnualSalary, colorArray.Length)
                    .ToList();

                var zipIds = context.CountyGeographies
                  .Where(i => i.GeographyClass.Name == "Calculation")
                  .Where(i => i.Geography.North > (double)boundingBox.SouthWest.Y && i.Geography.East > (double)boundingBox.SouthWest.X && i.Geography.South < (double)boundingBox.NorthEast.Y && i.Geography.West < (double)boundingBox.NorthEast.X)
                  .Where(i => i.Geography.GeographyPolygon.Intersects(boundingSpatial))
                  .Select(i => i.CountyId);


                var displayGeos = context.CountyGeographies
                    .Join(zipIds, i => i.CountyId, i => i, (i, o) => i)
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


                var stateIds = context.StateGeographies
                    .Where(i => i.GeographyClass.Name == "Calculation")
                    .Where(i => i.Geography.North > (double)boundingBox.SouthWest.Y && i.Geography.East > (double)boundingBox.SouthWest.X && i.Geography.South < (double)boundingBox.NorthEast.Y && i.Geography.West < (double)boundingBox.NorthEast.X)
                    .Where(i => i.Geography.GeographyPolygon.Intersects(boundingSpatial))
                    .Select(i => i.StateId);


                var displayGeos = context.StateGeographies
                    .Join(stateIds, i => i.StateId, i => i, (i, o) => i)
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
