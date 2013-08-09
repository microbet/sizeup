using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Data.Objects.SqlClient;
using Microsoft.SqlServer.Types;
using System.Data.Spatial;
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
    public class GeographyBoundaryController : BaseController
    {
        //
        // GET: /Tiles/GeographyBoundary/

        public ActionResult Index(int x, int y, int zoom, long geographicLocationId, int width = 256, int height = 256)
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
                    .Where(g=> g != null)
                    .ToList()
                    .Select(g => new GeographyEntity()
                    { 
                        Geography = SqlGeography.Parse(g.AsText()),
                        BorderColor = "#6495ED",
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

    }
}
