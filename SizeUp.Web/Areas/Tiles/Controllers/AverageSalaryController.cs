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

namespace SizeUp.Web.Areas.Tiles.Controllers
{
    public class AverageSalaryController : Controller
    {
        //
        // GET: /Tiles/AverageSalary/

        public ActionResult Zip(int x, int y, int zoom, int industryId, string colors, string boundingEntityId)
        {
            string[] colorArray = colors.Split(',');
            List<Heatmap.GeographyCollection> collection = new List<Heatmap.GeographyCollection>();
            Heatmap tile = new Heatmap(256, 256,  x, y, zoom);

            var boundingBox = tile.GetBoundingGeography(BoundingEntity.Get(boundingEntityId));
            var boundingSpacial = DbGeography.FromText((string)boundingBox.STAsText().ToSqlString());

            var geos = DataContexts.SizeUpContext.ZipCodes.Where(i => i.Geography.Intersects(boundingSpacial)).Select(i => new { i.Id, i.Geography }).ToList();






            tile.Draw(collection);
            var stream = new System.IO.MemoryStream();
            tile.Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return File(stream.GetBuffer(), "image/png");
        }

        public ActionResult County(int x, int y, int zoom, int industryId, string colors, string boundingEntityId)
        {
            string[] colorArray = colors.Split(',');
            List<Heatmap.GeographyCollection> collection = new List<Heatmap.GeographyCollection>();
            Heatmap tile = new Heatmap(256, 256, x, y, zoom);

            var boundingBox = tile.GetBoundingGeography(BoundingEntity.Get(boundingEntityId));
            var boundingSpacial = DbGeography.FromText((string)boundingBox.STAsText().ToSqlString());

            var geos = DataContexts.SizeUpContext.Counties.Where(i => i.Geography.Intersects(boundingSpacial)).Select(i => new { i.Id, i.Geography }).ToList();

            var geod = geos.Select(i => SqlGeography.Parse(i.Geography.AsText())).ToList();
           


            tile.Draw(collection);
            var stream = new System.IO.MemoryStream();
            tile.Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return File(stream.GetBuffer(), "image/png");
        }

        public ActionResult State(int x, int y, int zoom, int industryId, string colors, string boundingEntityId)
        {
            string[] colorArray = colors.Split(',');
            List<Heatmap.GeographyCollection> collection = new List<Heatmap.GeographyCollection>();
            Heatmap tile = new Heatmap(256, 256, x, y, zoom);

            var boundingBox = tile.GetBoundingGeography(BoundingEntity.Get(boundingEntityId));
            var boundingSpacial = DbGeography.FromText((string)boundingBox.STAsText().ToSqlString());

            var geos = DataContexts.SizeUpContext.States.Where(i => i.Geography.Intersects(boundingSpacial)).Select(i => new { i.Id, i.Geography }).ToList();





            tile.Draw(collection);
            var stream = new System.IO.MemoryStream();
            tile.Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return File(stream.GetBuffer(), "image/png");
        }
    }
}
