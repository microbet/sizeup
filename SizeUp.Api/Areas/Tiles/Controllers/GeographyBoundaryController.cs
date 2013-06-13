﻿using System;
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

namespace SizeUp.Api.Areas.Tiles.Controllers
{
    public class GeographyBoundaryController : BaseController
    {
        //
        // GET: /Tiles/GeographyBoundary/

        public ActionResult Index(int x, int y, int zoom, long id, Granularity granularity = Granularity.State, int width = 256, int height = 256)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                GeographyBoundary tile = new GeographyBoundary(width, height, x, y, zoom);
                BoundingBox boundingBox = tile.GetBoundingBox(TileBuffer);
                double tolerance = GetPolygonTolerance(zoom);
                var boundingGeo = boundingBox.GetDbGeography();


                IQueryable<KeyValue<DbGeography, long?>> entity = new List<KeyValue<DbGeography, long?>>().AsQueryable();
                if (granularity == Granularity.City)
                {
                    entity = Core.DataLayer.Base.City.Get(context).Where(i=>i.Id == id)
                        .Select(i=> new KeyValue<DbGeography, long?>
                        {
                            Key = i.CityGeographies.Where(g=>g.GeographyClass.Name == Core.Geo.GeographyClass.Display)
                            .Select(g => SqlSpatialFunctions.Reduce(g.Geography.GeographyPolygon, tolerance).Intersection(boundingGeo)).FirstOrDefault(),
                            Value = i.Id
                        });
                }
                else if (granularity == Granularity.County)
                {
                    entity = context.Counties.Where(i => i.Id == id)
                        .Select(i => new KeyValue<DbGeography, long?>
                        {
                            Key = i.CountyGeographies.Where(g => g.GeographyClass.Name == Core.Geo.GeographyClass.Display)
                            .Select(g => SqlSpatialFunctions.Reduce(g.Geography.GeographyPolygon, tolerance).Intersection(boundingGeo)).FirstOrDefault(),
                            Value = i.Id
                        });
                }
                else if (granularity == Granularity.Metro)
                {
                    entity = context.Metroes.Where(i => i.Id == id)
                        .Select(i => new KeyValue<DbGeography, long?>
                        {
                            Key = i.MetroGeographies.Where(g => g.GeographyClass.Name == Core.Geo.GeographyClass.Display)
                            .Select(g => SqlSpatialFunctions.Reduce(g.Geography.GeographyPolygon, tolerance).Intersection(boundingGeo)).FirstOrDefault(),
                            Value = i.Id
                        });
                }
                else if (granularity == Granularity.State)
                {
                    entity = context.States.Where(i => i.Id == id)
                        .Select(i => new KeyValue<DbGeography, long?>
                        {
                            Key = i.StateGeographies.Where(g => g.GeographyClass.Name == Core.Geo.GeographyClass.Display)
                            .Select(g => SqlSpatialFunctions.Reduce(g.Geography.GeographyPolygon, tolerance)).FirstOrDefault(),
                            Value = i.Id
                        });
                }


                var geos = entity.Where(i => i.Key != null)
                    .ToList()
                    .Select(g => new GeographyEntity()
                    { 
                        Geography = SqlGeography.Parse(g.Key.AsText()),
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