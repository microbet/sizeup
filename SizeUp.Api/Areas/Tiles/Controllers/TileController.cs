using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Data.Spatial;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.SqlServer.Types;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Core.Extensions;
using SizeUp.Core.Geo;
using SizeUp.Core.Tiles;
using SizeUp.Data;

namespace SizeUp.Api.Areas.Tiles.Controllers
{
    public class Section
    {
        public float min { get; set; }
        public float max { get; set; }
        public long geoId { get; set; }
    }

    public class Band
    {
        public float min { get; set; }
        public float max { get; set; }
        public string color { get; set; }
        public List<Section> band { get; set; }
    }

    public class TilesData
    {
        public int x { get; set; }
        public int y { get; set; }
        public int zoom { get; set; }
        public string endColor { get; set; }
        public string startColor { get; set; }
        public List<Band> Bands { get; set; }
    }

    public class TileController : Controller
    {
        private int ZoomFilterBase = 14;
        protected float TileBuffer = 0.3f;
        protected double GetPolygonTolerance(int zoom)
        {
            return Math.Pow(2, ZoomFilterBase - zoom);
        }
        //
        // GET: /Tiles/Tile/
        [HttpPost]
        public ActionResult Index(TilesData tilesData)
        {
            Heatmap tile = new Heatmap(256, 256, tilesData.x, tilesData.y, tilesData.zoom);
            BoundingBox boundingBox = tile.GetBoundingBox(TileBuffer);
            double tolerance = GetPolygonTolerance(tilesData.zoom);
            var boundingGeo = boundingBox.GetDbGeography();


            using (var context = ContextFactory.SizeUpContext)
            {
                var kvf = new List<KeyValue<DbGeography, Band<double>>>();
                if (tilesData != null && tilesData.Bands != null)
                {
                    var geoIdList = (from b in tilesData.Bands from s in b.band select s.geoId).ToList();
                    var goegraphies = context.Geographies.Where(i => geoIdList.Contains(i.Id));

                    var kv = goegraphies.Select(i => new KeyValue<DbGeography, double>()
                    {
                        Key = SqlSpatialFunctions.Reduce(i.Polygon, tolerance).Intersection(boundingGeo),
                        Value = i.Id
                    }).ToList();

                    kvf = (from tl in tilesData.Bands
                           from t in tl.band
                           from k in kv
                           where k.Value == t.geoId
                           select
                               new KeyValue<DbGeography, Band<double>>(k.Key, new Band<double>() { Min = t.min, Max = t.max }))
                        .ToList();
                }

                var quantiles = kvf
                       .Where(i => i.Value != null)
                       .NTileDescending(i => i.Value.Max, 5);

                ColorBands colorBands = new ColorBands(ColorTranslator.FromHtml("#" + tilesData.startColor), ColorTranslator.FromHtml("#" + tilesData.endColor), quantiles.Count());
                string[] bandList = colorBands.GetColorBands().ToArray();

                var validValues = quantiles
                    .Select((i, index) => i.Where(g => g.Key != null).Select(g => new GeographyEntity() { Geography = SqlGeography.Parse(g.Key.AsText()), Color = bandList[index] }))
                    .SelectMany(i => i)
                    .ToList();

                var invalidValues = kvf
                    .Where(i => i.Value == null)
                    .Where(i => i.Key != null)
                    .Select(g => new GeographyEntity() { Geography = SqlGeography.Parse(g.Key.AsText()) })
                    .ToList();

                var output = validValues.Union(invalidValues).ToList();

                tile.Draw(output);
                var stream = new MemoryStream();
                tile.Bitmap.Save(stream, ImageFormat.Png);
                return File(stream.GetBuffer(), "image/png");
            }
        }

        public ActionResult GeographyBoundary(int x, int y, int zoom, long geographicLocationId, int width = 256, int height = 256)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                GeographyBoundary tile = new GeographyBoundary(width, height, x, y, zoom);
                BoundingBox boundingBox = tile.GetBoundingBox(TileBuffer);
                double tolerance = GetPolygonTolerance(zoom);
                var boundingGeo = boundingBox.GetDbGeography();

                var geos = context.GeographicLocations
                    .Where(i => i.Id == geographicLocationId)
                    .SelectMany(i => i.Geographies.Where(g => g.GeographyClass.Name == Core.Geo.GeographyClass.Display)
                    .Select(g => SqlSpatialFunctions.Reduce(g.Polygon, tolerance).Intersection(boundingGeo)))
                    .Where(g => g != null)
                    .ToList()
                    .Select(g => new GeographyEntity()
                    {
                        Geography = SqlGeography.Parse(g.AsText()),
                        BorderColor = "#6495ED ",
                        BorderOpacity = 200,
                        BorderWidth = 2
                    })
                    .ToList();

                tile.Draw(geos);
                var stream = new System.IO.MemoryStream();
                tile.Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return File(stream.GetBuffer(), "image/png");
            }
        }
        public ActionResult Businesses(int x, int y, int zoom, List<long> industryIds = null, string color = "#ff5522 ", int width = 256, int height = 256)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                industryIds = industryIds == null ? new List<long>() : industryIds;
                Businesses tile = new Businesses(width, height, x, y, zoom);
                var boundingBox = tile.GetBoundingBox(TileBuffer);

                List<GeographyEntity> geos = new List<GeographyEntity>();
                if (industryIds.Count > 0)
                {
                    var buisnesses = Core.DataLayer.Business.In(context, boundingBox)
                      .Where(i => industryIds.Contains(i.IndustryId.Value))
                      .Select(i => new { Lat = i.Lat, Lng = i.Long });

                    var opacity = Math.Max(128, Math.Min(255, (zoom - 5) * 25));
                    var borderOpacity = Math.Max(0, Math.Min(255, 25 * (zoom - 13) + 125));

                    geos = buisnesses.ToList()
                        .Select(i => new GeographyEntity()
                        {
                            Geography = SqlGeography.Parse(string.Format("POINT({0} {1})", i.Lng, i.Lat)),
                            Color = color,
                            Opacity = opacity,
                            BorderWidth = 1,
                            BorderColor = "#000000 ",
                            BorderOpacity = borderOpacity
                        })
                        .ToList();
                }

                tile.Draw(geos);

                var stream = new System.IO.MemoryStream();
                tile.Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return File(stream.GetBuffer(), "image/png");
            }
        }

    }
}