using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Tiles;
using System.Linq.Expressions;
using System.Data.Objects.SqlClient;
using Microsoft.SqlServer.Types;
using System.Data.Spatial;
using SizeUp.Core.DataLayer;

namespace SizeUp.Api.Areas.Tiles.Controllers
{
    public class BusinessesController : BaseController
    {
        //
        // GET: /Tiles/Business/

        public ActionResult Index(int x, int y, int zoom, List<long> industryIds = null, string color = "#ff5522", int width = 256, int height = 256)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                industryIds = industryIds == null ? new List<long>() : industryIds;
                Businesses tile = new Businesses(width, height, x, y, zoom);
                var boundingBox = tile.GetBoundingBox(TileBuffer);

                List<GeographyEntity> geos = new List<GeographyEntity>();
                if (industryIds.Count > 0)
                {
                    var buisnesses = Core.DataLayer.Base.Business.In(context, boundingBox)
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
                            BorderColor = "#000000",
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
