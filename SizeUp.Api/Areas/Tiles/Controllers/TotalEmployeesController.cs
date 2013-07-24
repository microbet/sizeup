﻿using System;
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
    public class TotalEmployeesController : BaseController
    {
        //
        // GET: /Tiles/Revenue/

        public ActionResult Index(int x, int y, int zoom, long industryId, long placeId, string[] colors, Core.DataLayer.Granularity granularity = Core.DataLayer.Granularity.State, Core.DataLayer.Granularity boundingGranularity = Core.DataLayer.Granularity.Nation, int width = 256, int height = 256)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                Heatmap tile = new Heatmap(width, height, x, y, zoom);
                BoundingBox boundingBox = tile.GetBoundingBox(TileBuffer);
                double tolerance = GetPolygonTolerance(zoom);
                var boundingGeo = boundingBox.GetDbGeography();


                var geos = Core.DataLayer.GeographicLocation.In(context, granularity, placeId, boundingGranularity);
                var data = Core.DataLayer.IndustryData.Get(context).Where(i => i.IndustryId == industryId);

                var list = geos
                    .GroupJoin(data, i => i.Id, o => o.GeographicLocationId, (i, o) => new { IndustryData = o, GeographicLocation = i })
                    .Select(i => new KeyValue<DbGeography, long?>
                    {
                        Key = i.GeographicLocation.Geographies.Where(g => g.GeographyClass.Name == Core.Geo.GeographyClass.Display)
                        .Select(g => SqlSpatialFunctions.Reduce(g.Polygon, tolerance).Intersection(boundingGeo)).FirstOrDefault(),
                        Value = i.IndustryData.Select(d => d.TotalEmployees).FirstOrDefault()
                    }).ToList();

                var validValues = list
                    .Where(i => i.Value != null && i.Value > 0)
                    .NTileDescending(i => i.Value, colors.Length)
                    .Select((i, index) => i.Where(g => g.Key != null).Select(g => new GeographyEntity() { Geography = SqlGeography.Parse(g.Key.AsText()), Color = colors[index] }))
                    .SelectMany(i => i)
                    .ToList();

                var invalidValues = list
                    .Where(i => i.Value == null || i.Value <= 0)
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
