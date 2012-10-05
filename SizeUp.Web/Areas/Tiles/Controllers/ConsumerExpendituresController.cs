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
    public class ConsumerExpendituresController : Controller
    {
        //
        // GET: /Tiles/Revenue/
        public ActionResult Zip(int x, int y, int zoom, string colors, int variableId, string boundingEntityId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                
                string[] colorArray = colors.Split(',');
                Heatmap tile = new Heatmap(256, 256, x, y, zoom);
                BoundingEntity boundingEntity = new BoundingEntity(boundingEntityId);
                var BoundingBox = tile.GetBoundingBox(0.2f);

                var variableName = context.ConsumerExpenditureVariables.Where(i => i.Id == variableId).Select(i => i.Variable).FirstOrDefault();


                var data = ConsumerExpenditureData.GetZips(context, variableName, boundingEntity);

                var bands = data.NTile(i => i.Value, colorArray.Length)
                    .ToList();


                IQueryable<long> ids = ZipCodes.GetBounded(context, boundingEntity)
                    .Select(i => i.Id);

                var geoIds = Core.DataAccess.Geography.GetBoundingBoxedZips(context, BoundingBox)
                    .Join(ids, i => i.ZipCodeId, i => i, (i, o) => i)
                    .Select(i => i.ZipCodeId);

                var displayGeos = Core.DataAccess.Geography.GetDisplayZips(context, geoIds, zoom).ToList();


                var bandedGeos = bands.Select(b => b.Join(displayGeos, i => i.EntityId, i => i.Id, (i, o) => o).ToList()).ToList();
                var noData = displayGeos.Where(g => !data.Select(ig => ig.EntityId).Contains(g.Id)).ToList();


                var collection = Heatmap.CreateCollections(colorArray, bandedGeos, noData);

                tile.Draw(collection);
                var stream = new System.IO.MemoryStream();
                tile.Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return File(stream.GetBuffer(), "image/png");
            }
        }

        public ActionResult County(int x, int y, int zoom, string colors, int variableId, string boundingEntityId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                string[] colorArray = colors.Split(',');
                Heatmap tile = new Heatmap(256, 256, x, y, zoom);
                BoundingEntity boundingEntity = new BoundingEntity(boundingEntityId);
                var BoundingBox = tile.GetBoundingBox(0.2f);
                               
                var variableName = context.ConsumerExpenditureVariables.Where(i => i.Id == variableId).Select(i => i.Variable).FirstOrDefault();


                var data = ConsumerExpenditureData.GetCounties(context, variableName, boundingEntity);
                var bands = data.NTile(i => i.Value, colorArray.Length)
                    .ToList();

                IQueryable<long> ids = Counties.GetBounded(context, boundingEntity)
                    .Select(i => i.Id);

                var geoIds = Core.DataAccess.Geography.GetBoundingBoxedCounties(context, BoundingBox)
                    .Join(ids, i => i.CountyId, i => i, (i, o) => i)
                    .Select(i => i.CountyId);

                var displayGeos = Core.DataAccess.Geography.GetDisplayCounties(context, geoIds, zoom).ToList();

                var bandedGeos = bands.Select(b => b.Join(displayGeos, i => i.EntityId, i => i.Id, (i, o) => o).ToList()).ToList();
                var noData = displayGeos.Where(g => !data.Select(ig => ig.EntityId).Contains(g.Id)).ToList();


                var collection = Heatmap.CreateCollections(colorArray, bandedGeos, noData);

                tile.Draw(collection);
                var stream = new System.IO.MemoryStream();
                tile.Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return File(stream.GetBuffer(), "image/png");
            }
        }

        public ActionResult State(int x, int y, int zoom, string colors, int variableId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                string[] colorArray = colors.Split(',');
                Heatmap tile = new Heatmap(256, 256, x, y, zoom);
                var BoundingBox = tile.GetBoundingBox(0.2f);

                var variableName = context.ConsumerExpenditureVariables.Where(i => i.Id == variableId).Select(i => i.Variable).FirstOrDefault();


                var data = ConsumerExpenditureData.GetCounties(context, variableName);
                var bands = data.NTile(i => i.Value, colorArray.Length)
                    .ToList();

                var geoIds = Core.DataAccess.Geography.GetBoundingBoxedStates(context, BoundingBox)
                     .Select(i => i.StateId);

                var displayGeos = Core.DataAccess.Geography.GetDisplayStates(context, geoIds, zoom).ToList();



                var bandedGeos = bands.Select(b => b.Join(displayGeos, i => i.EntityId, i => i.Id, (i, o) => o).ToList()).ToList();
                var noData = displayGeos.Where(g => !data.Select(ig => ig.EntityId).Contains(g.Id)).ToList();

                var collection = Heatmap.CreateCollections(colorArray, bandedGeos, noData);

                tile.Draw(collection);
                var stream = new System.IO.MemoryStream();
                tile.Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return File(stream.GetBuffer(), "image/png");
            }
        }
    }
}
