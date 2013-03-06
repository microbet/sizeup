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
using SizeUp.Core.DataLayer.Base;
using SizeUp.Core.DataLayer.Models.Base;
using System.Data.Objects.SqlClient;
namespace SizeUp.Web.Areas.Tiles.Controllers
{
    public class EmployeesPerCapitaController : BaseController
    {
        //
        // GET: /Tiles/Revenue/
        public ActionResult Index(int x, int y, int zoom, long industryId, long placeId, string[] colors, Granularity granularity = Granularity.State, Granularity boundingGranularity = Granularity.Nation)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                Heatmap tile = new Heatmap(256, 256, x, y, zoom);
                BoundingBox boundingBox = tile.GetBoundingBox(.2f);
                double tolerance = GetPolygonTolerance(zoom);


                IQueryable<KeyValue<DbGeography, double?>> values = new List<KeyValue<DbGeography, double?>>().AsQueryable();//empty set
                if (granularity == Granularity.ZipCode)
                {
                    var entities = Core.DataLayer.ZipCode.In(context, placeId, boundingGranularity);
                    var data = IndustryData.ZipCode(context).Where(i => i.IndustryId == industryId);
                    values = entities.GroupJoin(data, i => i.Id, i => i.ZipCodeId, (e, d) => new KeyValue<DbGeography, double?>
                    {
                        Key = e.ZipCodeGeographies.Where(g => g.GeographyClass.Name == "Display")
                        .Select(g => SqlSpatialFunctions.Reduce(g.Geography.GeographyPolygon, tolerance).Intersection(boundingBox.DbGeography)).FirstOrDefault(),
                        Value = d.Select(v => v.EmployeesPerCapita).DefaultIfEmpty(null).FirstOrDefault()
                    });
                }
                else if (granularity == Granularity.County)
                {
                    var entities = Core.DataLayer.County.In(context, placeId, boundingGranularity);
                    var data = IndustryData.County(context).Where(i => i.IndustryId == industryId);
                    values = entities.GroupJoin(data, i => i.Id, i => i.CountyId, (e, d) => new KeyValue<DbGeography, double?>
                    {
                        Key = e.CountyGeographies.Where(g => g.GeographyClass.Name == "Display")
                        .Select(g => SqlSpatialFunctions.Reduce(g.Geography.GeographyPolygon, tolerance).Intersection(boundingBox.DbGeography)).FirstOrDefault(),
                        Value = d.Select(v => v.EmployeesPerCapita).DefaultIfEmpty(null).FirstOrDefault()
                    });
                }
                else if (granularity == Granularity.State)
                {
                    var entities = Core.DataLayer.State.In(context, placeId, boundingGranularity);
                    var data = IndustryData.State(context).Where(i => i.IndustryId == industryId);
                    values = entities.GroupJoin(data, i => i.Id, i => i.StateId, (e, d) => new KeyValue<DbGeography, double?>
                    {
                        Key = e.StateGeographies.Where(g => g.GeographyClass.Name == "Display")
                        .Select(g => SqlSpatialFunctions.Reduce(g.Geography.GeographyPolygon, tolerance).Intersection(boundingBox.DbGeography)).FirstOrDefault(),
                        Value = d.Select(v => v.EmployeesPerCapita).DefaultIfEmpty(null).FirstOrDefault()
                    });
                }

                var list = values.ToList();

                var validValues = list
                    .Where(i => i.Value != null && i.Value > 0)
                    .NTile(i => i.Value, colors.Length)
                    .Select((i, index) => i.Where(g => g.Key != null).Select(g => new GeographyEntity() { Geography = SqlGeography.Parse(g.Key.AsText()), Color = colors[index] }))
                    .SelectMany(i => i)
                    .ToList();

                var invalidValues = list
                    .Where(i => i.Value == null || i.Value <= 0 && i.Key != null)
                    .Select(g => new GeographyEntity() { Geography = SqlGeography.Parse(g.Key.AsText()) })
                    .ToList();

                var geos = validValues.Union(invalidValues).ToList();

                tile.Draw(geos);
                var stream = new System.IO.MemoryStream();
                tile.Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return File(stream.GetBuffer(), "image/png");
            }
        }

    }
}
