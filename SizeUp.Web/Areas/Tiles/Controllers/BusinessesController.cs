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

        public ActionResult Index(int x, int y, int zoom, long cityId, int radius, List<long> competitorIndustryIds = null, List<long> buyerIndustryIds = null, List<long> supplierIndustryIds = null)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                Businesses tile = new Businesses(256, 256, x, y, zoom);
                var boundingBox = tile.GetBoundingGeography();
                var boundingSpatial = DbGeography.FromText((string)boundingBox.STAsText().ToSqlString());


                var city = context.Cities.Where(i => i.Id == cityId).Select(i => i.Geography).FirstOrDefault();
                var geo = SqlGeography.Parse(city.AsText());
                var geom = SqlGeometry.STGeomFromWKB(geo.STAsBinary(), (int)geo.STSrid);
                geom = geom.STCentroid();
                geo = SqlGeography.Parse(geom.STAsText().ToSqlString());
                var lat = (double)geo.STPointN(1).Lat;
                var lng = (double)geo.STPointN(1).Long;
                var scalar = 69.1 * Math.Cos(lat / 57.3);

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


                var cGeos = competitors.Select(i => new
                {
                    Distance = Math.Pow(Math.Pow(((double)i.Lat.Value - lat) * 69.1, 2) + Math.Pow(((double)i.Long.Value - lng) * scalar, 2), 0.5),
                    Geography = i.Geography
                }).Where(i => i.Distance < radius && i.Geography.Intersects(boundingSpatial))
                .ToList()
                .Select(i => SqlGeography.Parse(i.Geography.AsText()))
                .ToList();


                GeographyCollection comp = new GeographyCollection();
                comp.Color = "#ff5522";
                comp.Opacity = 255;
                comp.Geographies.AddRange(cGeos);
                //comp. = 255;
                //comp.Opacity = 255;

                geoCollection.Add(comp);



                tile.Draw(geoCollection);
                var stream = new System.IO.MemoryStream();
                tile.Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return File(stream.GetBuffer(), "image/png");
            }
        }

    }
}
