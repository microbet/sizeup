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

namespace SizeUp.Web.Areas.Tiles.Controllers
{
    public class BusinessesController : Controller
    {
        //
        // GET: /Tiles/Business/

        public ActionResult Index(int x, int y, int zoom, List<long> competitorIndustryIds = null, List<long> buyerIndustryIds = null, List<long> supplierIndustryIds = null)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                Businesses tile = new Businesses(256, 256, x, y, zoom);
                var boundingBox = tile.GetBoundingBox(0.1f);

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

                var competitors = context.Businesses.Where(i => competitorIndustryIds.Contains(i.IndustryId.Value));
                competitors = competitors.Where(i => i.BusinessStatusCode != "1" || i.BusinessStatusCode != "3");

                var buyers = context.Businesses.Where(i => buyerIndustryIds.Contains(i.IndustryId.Value));
                buyers = buyers.Where(i => i.BusinessStatusCode != "1" || i.BusinessStatusCode != "3");

                var suppliers = context.Businesses.Where(i => supplierIndustryIds.Contains(i.IndustryId.Value));
                suppliers = suppliers.Where(i => i.BusinessStatusCode != "1" || i.BusinessStatusCode != "3");


                List<GeographyCollection> geoCollection = new List<GeographyCollection>();

                
                var geos = context.Businesses.Select(g=> new {

                    competitors = competitors
                    .Where(i => i.Lat < (decimal)boundingBox.NorthEast.Y && i.Lat > (decimal)boundingBox.SouthWest.Y && i.Long > (decimal)boundingBox.SouthWest.X && i.Long < (decimal)boundingBox.NorthEast.X)
                    .Select(i => new
                    {
                        Lat = i.Lat,
                        Long = i.Long
                    }),

                    buyers = buyers
                    .Where(i => i.Lat < (decimal)boundingBox.NorthEast.Y && i.Lat > (decimal)boundingBox.SouthWest.Y && i.Long > (decimal)boundingBox.SouthWest.X && i.Long < (decimal)boundingBox.NorthEast.X)
                    .Select(i => new
                    {
                        Lat = i.Lat,
                        Long = i.Long
                    }),

                    suppliers = suppliers.Where(i => i.Lat < (decimal)boundingBox.NorthEast.Y && i.Lat > (decimal)boundingBox.SouthWest.Y && i.Long > (decimal)boundingBox.SouthWest.X && i.Long < (decimal)boundingBox.NorthEast.X)
                    .Select(i => new
                    {
                        Lat = i.Lat,
                        Long = i.Long
                    })
                }).FirstOrDefault();


                GeographyCollection competitorCollection = new GeographyCollection()
                {
                    Color = "#ff5522",
                    Opacity = Math.Max(128, Math.Min(255, (zoom - 5) * 25)),
                    BorderWidth = 1,
                    BorderColor = "#000000",
                    BorderOpacity = Math.Max(0, Math.Min(255, 25 * (zoom - 13) + 125)),
                };
                competitorCollection.Geographies.AddRange(geos.competitors.ToList().Select(i => SqlGeography.Parse(string.Format("POINT({0} {1})", i.Long, i.Lat))).ToList());
                geoCollection.Add(competitorCollection);

                GeographyCollection buyerCollection = new GeographyCollection()
                {
                    Color = "#66ee00",
                    Opacity = Math.Max(128, Math.Min(255, (zoom - 5) * 25)),
                    BorderWidth = 1,
                    BorderColor = "#000000",
                    BorderOpacity = Math.Max(0, Math.Min(255, 25 * (zoom - 13) + 125)),
                };
                buyerCollection.Geographies.AddRange(geos.buyers.ToList().Select(i => SqlGeography.Parse(string.Format("POINT({0} {1})", i.Long, i.Lat))).ToList());
                geoCollection.Add(buyerCollection);

                GeographyCollection supplierCollection = new GeographyCollection()
                {
                    Color = "#11aaff",
                    Opacity = Math.Max(128, Math.Min(255, (zoom - 5) * 25)),
                    BorderWidth = 1,
                    BorderColor = "#000000",
                    BorderOpacity = Math.Max(0, Math.Min(255, 25 * (zoom - 13) + 125)),
                };
                supplierCollection.Geographies.AddRange(geos.suppliers.ToList().Select(i => SqlGeography.Parse(string.Format("POINT({0} {1})", i.Long, i.Lat))).ToList());
                geoCollection.Add(supplierCollection);


                



                tile.Draw(geoCollection);
                var stream = new System.IO.MemoryStream();
                tile.Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return File(stream.GetBuffer(), "image/png");
            }
        }

    }
}
