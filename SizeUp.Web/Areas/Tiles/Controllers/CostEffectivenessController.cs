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
using SizeUp.Core.DataAccess;


namespace SizeUp.Web.Areas.Tiles.Controllers
{
    public class CostEffectivenessController : Controller
    {
        //
        // GET: /Tiles/Revenue/

        public ActionResult County(int x, int y, int zoom, int industryId, string colors, string boundingEntityId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                string[] colorArray = colors.Split(',');
                Heatmap tile = new Heatmap(256, 256, x, y, zoom);
                BoundingEntity boundingEntity = new BoundingEntity(boundingEntityId);
                var BoundingBox = tile.GetBoundingBox(0.2f);

                IQueryable<long> ids = Counties.GetBounded(context, boundingEntity)
                    .Select(i => i.Id);

                var data = IndustryData.GetCounties(context, industryId)
                    .Where(i => i.CostEffectiveness > 0)
                    .Join(ids, i => i.CountyId, i => i, (i, o) => i)
                    .Select(i => new { i.CostEffectiveness, i.CountyId })
                    .ToList();

                var bands = data.NTile(i => i.CostEffectiveness, colorArray.Length)
                    .ToList();

                var geoIds = Core.DataAccess.Geography.GetBoundingBoxedCounties(context, BoundingBox)
                    .Join(ids, i => i.CountyId, i => i, (i, o) => i)
                    .Select(i => i.CountyId);

                var displayGeos = Core.DataAccess.Geography.GetDisplayCounties(context, geoIds, zoom).ToList();

                var bandedGeos = bands.Select(b => b.Join(displayGeos, i => i.CountyId, i => i.Id, (i, o) => o).ToList()).ToList();
                var noData = displayGeos.Where(g => !data.Select(ig => ig.CountyId).Contains(g.Id)).ToList();


                var collection = Heatmap.CreateCollections(colorArray, bandedGeos, noData);

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
                Heatmap tile = new Heatmap(256, 256, x, y, zoom);
                var BoundingBox = tile.GetBoundingBox(0.2f);

                var data = IndustryData.GetStates(context,industryId)
                  .Where(i => i.CostEffectiveness > 0)
                  .Select(i => new { i.StateId, i.CostEffectiveness });

                var bands = data.ToList()
                  .NTile(i => i.CostEffectiveness, colorArray.Length)
                  .ToList();

                var geoIds = Core.DataAccess.Geography.GetBoundingBoxedStates(context, BoundingBox)
                     .Select(i => i.StateId);

                var displayGeos = Core.DataAccess.Geography.GetDisplayStates(context, geoIds, zoom).ToList();


                var bandedGeos = bands.Select(b => b.Join(displayGeos, i => i.StateId, i => i.Id, (i, o) => o).ToList()).ToList();
                var noData = displayGeos.Where(g => !data.Select(ig => ig.StateId).Contains(g.Id)).ToList();

                var collection = Heatmap.CreateCollections(colorArray, bandedGeos, noData);

                tile.Draw(collection);
                var stream = new System.IO.MemoryStream();
                tile.Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return File(stream.GetBuffer(), "image/png");
            }
        }

    }
}
