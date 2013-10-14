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
using SizeUp.Core.DataLayer;
using SizeUp.Core.DataLayer.Models;
using System.Data.Objects.SqlClient;

namespace SizeUp.Api.Areas.Tiles.Controllers
{
    public class AverageSalaryController : BaseController
    {
        //
        // GET: /Tiles/AverageSalary/

        public ActionResult Index(int x, int y, int zoom, long industryId, long boundingGeographicLocationId, string startColor, string endColor, int bands, Core.DataLayer.Granularity granularity, int width = 256, int height = 256)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                Heatmap tile = new Heatmap(width, height, x, y, zoom);
                BoundingBox boundingBox = tile.GetBoundingBox(TileBuffer);
                double tolerance = GetPolygonTolerance(zoom);
                var boundingGeo = boundingBox.GetDbGeography();

                var gran = Enum.GetName(typeof(Core.DataLayer.Granularity), granularity);

                var geos = Core.DataLayer.GeographicLocation.Get(context)
                    .Where(i => i.Granularity.Name == gran)
                    .Where(i => i.GeographicLocations.Any(g => g.Id == boundingGeographicLocationId));

                var data = Core.DataLayer.IndustryData.Get(context).Where(i => i.IndustryId == industryId);

                var list = geos
                    .GroupJoin(data, i => i.Id, o => o.GeographicLocationId, (i, o) => new { IndustryData = o, GeographicLocation = i })
                    .Select(i => new KeyValue<DbGeography, Band<double>>
                    {
                        Key = i.GeographicLocation.Geographies.Where(g => g.GeographyClass.Name == Core.Geo.GeographyClass.Display)
                        .Select(g => SqlSpatialFunctions.Reduce(g.Polygon, tolerance).Intersection(boundingGeo)).FirstOrDefault(),
                        Value = i.IndustryData.Select(d => d.Bands.Where(b => b.Attribute.Name == IndustryAttribute.AverageAnnualSalary).Select(b => new Band<double> { Min = (double)b.Min.Value, Max = (double)b.Max.Value }).FirstOrDefault()).FirstOrDefault()
                    }).ToList();

                var quantiles = list
                   .Where(i => i.Value != null)
                   .NTileDescending(i => i.Value.Max, bands);

                ColorBands colorBands = new Core.Tiles.ColorBands(System.Drawing.ColorTranslator.FromHtml("#" + startColor), System.Drawing.ColorTranslator.FromHtml("#" + endColor), quantiles.Count());
                string[] bandList = colorBands.GetColorBands().ToArray();

                var validValues = quantiles
                    .Select((i, index) => i.Where(g => g.Key != null).Select(g => new GeographyEntity() { Geography = SqlGeography.Parse(g.Key.AsText()), Color = bandList[index] }))
                    .SelectMany(i => i)
                    .ToList();

                var invalidValues = list
                    .Where(i => i.Value == null)
                    .Where(i => i.Key != null)
                    .Select(g => new GeographyEntity() { Geography = SqlGeography.Parse(g.Key.AsText()) })
                    .ToList();

                var output = validValues.Union(invalidValues).ToList();

                tile.Draw(output);
                var stream = new System.IO.MemoryStream();
                tile.Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return File(stream.GetBuffer(), "image/png");
            }
        }
    }
}
