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
using System.Linq.Expressions;

namespace SizeUp.Api.Areas.Tiles.Controllers
{
    public class ConsumerExpendituresController : BaseController
    {
        //
        // GET: /Tiles/Revenue/
        public ActionResult Index(int x, int y, int zoom, long variableId, long boundingGeographicLocationId, string startColor, string endColor, int bands, Core.DataLayer.Granularity granularity = Core.DataLayer.Granularity.State, int width = 256, int height = 256)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                Heatmap tile = new Heatmap(width, height, x, y, zoom);
                BoundingBox boundingBox = tile.GetBoundingBox(TileBuffer);
                double tolerance = GetPolygonTolerance(zoom);
                var boundingGeo = boundingBox.GetDbGeography();

                var variable = Core.DataLayer.ConsumerExpenditures.Variables(context).Where(i => i.Id == variableId).Select(i => i.Variable).FirstOrDefault();

                var gran = Enum.GetName(typeof(Core.DataLayer.Granularity), granularity);

                var geos = Core.DataLayer.GeographicLocation.Get(context)
                    .Where(i => i.Granularity.Name == gran)
                    .Where(i => i.GeographicLocations.Any(g => g.Id == boundingGeographicLocationId));

                var data = Core.DataLayer.ConsumerExpenditures.Get(context);
                    //.Where(i => i.GeographicLocation.Granularity.Name == gran)
                    //.Where(i => i.GeographicLocation.GeographicLocations.Any(g => g.Id == boundingGeographicLocationId));

                ConstantExpression constant = Expression.Constant(data); //empty set
                IQueryProvider provider = data.Provider; 
                Type dataType = typeof(ConsumerExpenditure);
                var param = Expression.Parameter(dataType, "c");

                var varSelector = Expression.Convert(Expression.Property(param, variable), typeof(long?)) as Expression;
                var idSelector = Expression.Property(param, "GeographicLocationId") as Expression;
                var transType = typeof(KeyValue<long, long?>);
                var constructor = transType.GetConstructor(new Type[] { typeof(long), typeof(long?) });
                var selector = Expression.New(constructor, new Expression[] { idSelector, varSelector }.AsEnumerable(), new System.Reflection.MemberInfo[] { transType.GetProperty("Key"), transType.GetProperty("Value") });
                var pred = Expression.Lambda(selector, param) as Expression;
                var expression = Expression.Call(typeof(Queryable), "Select", new Type[] { dataType, transType }, constant, pred);
                var transformedData = data.Provider.CreateQuery<KeyValue<long, long?>>(expression);

                var list = geos.GroupJoin(transformedData, i => i.Id, o => o.Key, (i, o) => new { Data = o, GeographicLocation = i })
                    .Select(i => new KeyValue<DbGeography, long?>
                    {
                        Key = i.GeographicLocation.Geographies.Where(g => g.GeographyClass.Name == Core.Geo.GeographyClass.Display)
                        .Select(g => SqlSpatialFunctions.Reduce(g.Polygon, tolerance).Intersection(boundingGeo)).FirstOrDefault(),
                        Value = i.Data.Select(d => d.Value).FirstOrDefault()
                    }).ToList();

                var quantiles = list
                    .Where(i => i.Value != null)
                    .NTileDescending(i => i.Value, bands);

                ColorBands colorBands = new Core.Tiles.ColorBands(System.Drawing.ColorTranslator.FromHtml("#" + startColor), System.Drawing.ColorTranslator.FromHtml("#" + endColor), quantiles.Count());
                string[] bandList = colorBands.GetColorBands().ToArray();

                var validValues = quantiles
                    .Select((i, index) => i.Where(g => g.Key != null).Select(g => new GeographyEntity() { Geography = SqlGeography.Parse(g.Key.AsText()), Color = bandList[index] }))
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
