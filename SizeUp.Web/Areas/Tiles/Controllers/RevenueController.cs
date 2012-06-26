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
    public class RevenueController : Controller
    {
        //
        // GET: /Tiles/Revenue/

        public ActionResult Zip(int x, int y, int zoom, int industryId, string colors, string boundingEntityId)
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

                var geos = context.ZipCodeGeographies
                   .Select(i => new { ZipCodeId = i.ZipCodeId, Display = context.ZipCodeGeographies.Where(g => g.GeographyClass.Name == "Display" && g.ZipCodeId == i.ZipCodeId).FirstOrDefault(), Calculation = context.ZipCodeGeographies.Where(g => g.GeographyClass.Name == "Calculation" && g.ZipCodeId == i.ZipCodeId).FirstOrDefault() })
                    .Where(i => i.Calculation.Geography.North > (double)boundingBox.SouthWest.Y && i.Calculation.Geography.East > (double)boundingBox.SouthWest.X && i.Calculation.Geography.South < (double)boundingBox.NorthEast.Y && i.Calculation.Geography.West < (double)boundingBox.NorthEast.X)
                    .Where(i => i.Calculation.Geography.GeographyPolygon.Intersects(boundingSpatial)).Select(i => new { Id = i.ZipCodeId, Geography = i.Display.Geography.GeographyPolygon }).ToList();

                var filters = context.RevenueByZips
                  .Where(i => i.IndustryId == industryId && i.Revenue > 0);

                var maxes = filters
                   .Select(i => new { i.Year, i.Quarter })
                   .OrderByDescending(i => i.Year)
                   .ThenByDescending(i => i.Quarter);

                filters = filters.Where(i => i.Year == maxes.FirstOrDefault().Year && i.Quarter == maxes.FirstOrDefault().Quarter);


                if (boundingEntity.EntityType != null && boundingEntity.EntityType == BoundingEntity.BoundingEntityType.State)
                {
                    filters = filters.Where(i => i.County.StateId == boundingEntity.EntityId);
                }
                else if (boundingEntity.EntityType != null && boundingEntity.EntityType == BoundingEntity.BoundingEntityType.Metro)
                {
                    filters = filters.Where(i => i.County.MetroId == boundingEntity.EntityId);
                }
                else if (boundingEntity.EntityType != null && boundingEntity.EntityType == BoundingEntity.BoundingEntityType.County)
                {
                    filters = filters.Where(i => i.County.Id == boundingEntity.EntityId);
                }
                var data = filters
                    .GroupBy(i => new { i.Year, i.Quarter, i.ZipCodeId })
                    .Select(i => new { i.Key.ZipCodeId, Revenue = (long)i.Average(g => g.Revenue * 1000) })
                    .ToList();


                var bands = data.NTile(i => i.Revenue, colorArray.Length).ToList();
                var bandedGeos = bands.Select(b => b.Join(geos, i => i.ZipCodeId, i => i.Id, (i, o) => new { o.Id, o.Geography, i.Revenue }).ToList()).ToList();
                var noData = geos.Where(g => !data.Select(ig => ig.ZipCodeId).Contains(g.Id)).ToList();

                for (var b = 0; b < bandedGeos.Count; b++)
                {
                    var geoCollection = new GeographyCollection();
                    geoCollection.Geographies.AddRange(bandedGeos[b].Select(i => SqlGeography.Parse(i.Geography.AsText())).ToList());
                    geoCollection.Color = colorArray[b];
                    collection.Add(geoCollection);
                }

                var noDataCollection = new GeographyCollection();
                noDataCollection.Geographies = new List<SqlGeography>();
                noDataCollection.Geographies.AddRange(noData.Select(i => SqlGeography.Parse(i.Geography.AsText())).ToList());
                collection.Add(noDataCollection);




                tile.Draw(collection);
                var stream = new System.IO.MemoryStream();
                tile.Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return File(stream.GetBuffer(), "image/png");
            }
        }

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
                   .Select(i => new { CountyId = i.CountyId, Display = context.CountyGeographies.Where(g => g.GeographyClass.Name == "Display" && g.CountyId == i.CountyId).FirstOrDefault(), Calculation = context.CountyGeographies.Where(g => g.GeographyClass.Name == "Calculation" && g.CountyId == i.CountyId).FirstOrDefault() })
                    .Where(i => i.Calculation.Geography.North > (double)boundingBox.SouthWest.Y && i.Calculation.Geography.East > (double)boundingBox.SouthWest.X && i.Calculation.Geography.South < (double)boundingBox.NorthEast.Y && i.Calculation.Geography.West < (double)boundingBox.NorthEast.X)
                    .Where(i => i.Calculation.Geography.GeographyPolygon.Intersects(boundingSpatial)).Select(i => new { Id = i.CountyId, Geography = i.Display.Geography.GeographyPolygon }).ToList();


                var filters = context.RevenueByZips
                  .Where(i => i.IndustryId == industryId && i.Revenue > 0);

                var maxes = filters
                   .Select(i => new { i.Year, i.Quarter })
                   .OrderByDescending(i => i.Year)
                   .ThenByDescending(i => i.Quarter);

                filters = filters.Where(i => i.Year == maxes.FirstOrDefault().Year && i.Quarter == maxes.FirstOrDefault().Quarter);


                if (boundingEntity.EntityType != null && boundingEntity.EntityType == BoundingEntity.BoundingEntityType.State)
                {
                    filters = filters.Where(i => i.County.StateId == boundingEntity.EntityId);
                }
                else if (boundingEntity.EntityType != null && boundingEntity.EntityType == BoundingEntity.BoundingEntityType.Metro)
                {
                    filters = filters.Where(i => i.County.MetroId == boundingEntity.EntityId);
                }
                var data = filters
                    .GroupBy(i => new { i.Year, i.Quarter, i.CountyId })
                    .Select(i => new { i.Key.CountyId, Revenue = (long)i.Average(g => g.Revenue * 1000) })
                    .ToList();


                var bands = data.NTile(i => i.Revenue, colorArray.Length).ToList();
                var bandedGeos = bands.Select(b => b.Join(geos, i => i.CountyId, i => i.Id, (i, o) => new { o.Id, o.Geography, i.Revenue }).ToList()).ToList();
                var noData = geos.Where(g => !data.Select(ig => ig.CountyId).Contains(g.Id)).ToList();

                for (var b = 0; b < bandedGeos.Count; b++)
                {
                    var geoCollection = new GeographyCollection();
                    geoCollection.Geographies.AddRange(bandedGeos[b].Select(i => SqlGeography.Parse(i.Geography.AsText())).ToList());
                    geoCollection.Color = colorArray[b];
                    collection.Add(geoCollection);
                }

                var noDataCollection = new GeographyCollection();
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
                List<GeographyCollection> collection = new List<GeographyCollection>();
                Heatmap tile = new Heatmap(256, 256, x, y, zoom);


                var boundingBox = tile.GetBoundingBox(0.2f);
                var boundingGeo = tile.GetBoundingGeography();
                var boundingSpatial = DbGeography.FromText((string)boundingGeo.STAsText().ToSqlString());

                var geos = context.StateGeographies
                   .Select(i => new { StateId = i.StateId, Display = context.StateGeographies.Where(g => g.GeographyClass.Name == "Display" && g.StateId == i.StateId).FirstOrDefault(), Calculation = context.StateGeographies.Where(g => g.GeographyClass.Name == "Calculation" && g.StateId == i.StateId).FirstOrDefault() })
                    .Where(i => i.Calculation.Geography.North > (double)boundingBox.SouthWest.Y && i.Calculation.Geography.East > (double)boundingBox.SouthWest.X && i.Calculation.Geography.South < (double)boundingBox.NorthEast.Y && i.Calculation.Geography.West < (double)boundingBox.NorthEast.X)
                    .Where(i => i.Calculation.Geography.GeographyPolygon.Intersects(boundingSpatial)).Select(i => new { Id = i.StateId, Geography = i.Display.Geography.GeographyPolygon }).ToList();

                var filters = context.RevenueByZips
                 .Where(i => i.IndustryId == industryId && i.Revenue > 0);

                var maxes = filters
                   .Select(i => new { i.Year, i.Quarter })
                   .OrderByDescending(i => i.Year)
                   .ThenByDescending(i => i.Quarter);

                filters = filters.Where(i => i.Year == maxes.FirstOrDefault().Year && i.Quarter == maxes.FirstOrDefault().Quarter);

                var data = filters
                   .GroupBy(i => new { i.Year, i.Quarter, i.StateId })
                   .Select(i => new { i.Key.StateId, Revenue = (long)i.Average(g => g.Revenue * 1000) })
                   .ToList();


                var bands = data.NTile(i => i.Revenue, colorArray.Length).ToList();
                var bandedGeos = bands.Select(b => b.Join(geos, i => i.StateId, i => i.Id, (i, o) => new { o.Id, o.Geography, i.Revenue }).ToList()).ToList();
                var noData = geos.Where(g => !data.Select(ig => ig.StateId).Contains(g.Id)).ToList();

                for (var b = 0; b < bandedGeos.Count; b++)
                {
                    var geoCollection = new GeographyCollection();
                    geoCollection.Geographies.AddRange(bandedGeos[b].Select(i => SqlGeography.Parse(i.Geography.AsText())).ToList());
                    geoCollection.Color = colorArray[b];
                    collection.Add(geoCollection);
                }

                var noDataCollection = new GeographyCollection();
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
