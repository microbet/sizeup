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

        public ActionResult Index(int x, int y, int zoom, List<long> competitorIndustryIds = null, List<long> buyerIndustryIds = null, List<long> supplierIndustryIds = null)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                Businesses tile = new Businesses(256, 256, x, y, zoom);
                var boundingBox = tile.GetBoundingBox(TileBuffer);

                if (competitorIndustryIds == null)
                {
                    competitorIndustryIds = new List<long>();
                }
                if (buyerIndustryIds == null)
                {
                    buyerIndustryIds = new List<long>();
                }
                if (supplierIndustryIds == null)
                {
                    supplierIndustryIds = new List<long>();
                }


                var competitor = Core.DataLayer.Base.Business.In(context, boundingBox)
                   .Where(i => competitorIndustryIds.Contains(i.IndustryId.Value))
                   .Select(i => new { Type = "Competitor", Lat = i.Lat, Lng = i.Long });
                   
                
                var supplier = Core.DataLayer.Base.Business.In(context, boundingBox)
                   .Where(i => supplierIndustryIds.Contains(i.IndustryId.Value))
                   .Select(i => new { Type = "Supplier", Lat = i.Lat, Lng = i.Long });

                var buyer = Core.DataLayer.Base.Business.In(context, boundingBox)
                   .Where(i => buyerIndustryIds.Contains(i.IndustryId.Value))
                   .Select(i => new { Type = "Buyer", Lat = i.Lat, Lng = i.Long });



            

                var opacity = Math.Max(128, Math.Min(255, (zoom - 5) * 25));
                var borderOpacity = Math.Max(0, Math.Min(255, 25 * (zoom - 13) + 125));


                var geos = competitor.Union(supplier).Union(buyer)
                    .ToList()
                    .Select(i => new GeographyEntity()
                    {
                        Geography = SqlGeography.Parse(string.Format("POINT({0} {1})", i.Lng, i.Lat)),
                        Color = i.Type == "Competitor" ? "#ff5522" : i.Type == "Supplier" ? "#11AAFF" : "#66EE00",
                        Opacity = opacity,
                        BorderWidth = 1,
                        BorderColor = "#000000",
                        BorderOpacity = borderOpacity
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
