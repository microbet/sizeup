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
using SizeUp.Core.DataLayer;
using SizeUp.Core.DataLayer.Base;
using SizeUp.Core.DataLayer.Models.Base;
using System.Data.Objects.SqlClient;
using System.Linq.Expressions;

namespace SizeUp.Web.Areas.Tiles.Controllers
{
    public class ConsumerExpendituresController : BaseController
    {
        //
        // GET: /Tiles/Revenue/
        public ActionResult Index(int x, int y, int zoom, long variableId, long placeId, string[] colors, Granularity granularity = Granularity.State, Granularity boundingGranularity = Granularity.Nation )
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                Heatmap tile = new Heatmap(256, 256, x, y, zoom);
                BoundingBox boundingBox = tile.GetBoundingBox(.2f);
                double tolerance = GetPolygonTolerance(zoom);
                var boundingGeo = boundingBox.GetDbGeography();

                var variable = Core.DataLayer.ConsumerExpenditures.Variable(context, variableId);

                IQueryable<KeyValue<DbGeography, long?>> values = new List<KeyValue<DbGeography,long?>>().AsQueryable();//empty set
                if (granularity == Granularity.ZipCode)
                {
                    var entities = Core.DataLayer.Base.ZipCode.In(context, placeId, boundingGranularity);
                    var data = Core.DataLayer.Base.ConsumerExpenditures.ZipCode(context);

                    var dataType = typeof(ConsumerExpendituresByZip);
                    var constant = Expression.Constant(data);
                    var param = Expression.Parameter(dataType, "c");
                    var varSelector = Expression.Convert(Expression.Property(param, variable.Variable), typeof(long?)) as Expression;
                    var idSelector = Expression.Property(param, "ZipCodeId") as Expression;
                    var transType = typeof(KeyValue<long, long?>);
                    var constructor = transType.GetConstructor(new Type[] {typeof(long), typeof(long?) });
                    var selector = Expression.New(constructor, new Expression[]{idSelector, varSelector}.AsEnumerable(), new System.Reflection.MemberInfo[] {transType.GetProperty("Key"), transType.GetProperty("Value")} );
                    var pred = Expression.Lambda(selector, param) as Expression;
                    var expression = Expression.Call(typeof(Queryable), "Select", new Type[] { dataType, transType }, constant, pred);
                    var transformedData = data.Provider.CreateQuery<KeyValue<long,long?>>(expression);
                    values = entities.GroupJoin(transformedData, i => i.Id, i => i.Key, (e, d) => new KeyValue<DbGeography, long?>
                    {
                        Key = e.ZipCodeGeographies.Where(g => g.GeographyClass.Name == Core.Geo.GeographyClass.Display)
                        .Select(g => SqlSpatialFunctions.Reduce(g.Geography.GeographyPolygon, tolerance)).FirstOrDefault(),
                        Value = d.Select(v=>v.Value).DefaultIfEmpty(null).FirstOrDefault()
                    });
                }
                else if (granularity == Granularity.County)
                {
                    var entities = Core.DataLayer.Base.County.In(context, placeId, boundingGranularity);
                    var data = Core.DataLayer.Base.ConsumerExpenditures.County(context);

                    var dataType = typeof(ConsumerExpendituresByCounty);
                    var constant = Expression.Constant(data);
                    var param = Expression.Parameter(dataType, "c");
                    var varSelector = Expression.Convert(Expression.Property(param, variable.Variable), typeof(long?)) as Expression;
                    var idSelector = Expression.Property(param, "CountyId") as Expression;
                    var transType = typeof(KeyValue<long, long?>);
                    var constructor = transType.GetConstructor(new Type[] { typeof(long), typeof(long?) });
                    var selector = Expression.New(constructor, new Expression[] { idSelector, varSelector }.AsEnumerable(), new System.Reflection.MemberInfo[] { transType.GetProperty("Key"), transType.GetProperty("Value") });
                    var pred = Expression.Lambda(selector, param) as Expression;
                    var expression = Expression.Call(typeof(Queryable), "Select", new Type[] { dataType, transType }, constant, pred);
                    var transformedData = data.Provider.CreateQuery<KeyValue<long, long?>>(expression);

                    values = entities.GroupJoin(transformedData, i => i.Id, i => i.Key, (e, d) => new KeyValue<DbGeography, long?>
                    {
                        Key = e.CountyGeographies.Where(g => g.GeographyClass.Name == Core.Geo.GeographyClass.Display)
                        .Select(g => SqlSpatialFunctions.Reduce(g.Geography.GeographyPolygon, tolerance)).FirstOrDefault(),
                        Value = d.Select(v => v.Value).DefaultIfEmpty(null).FirstOrDefault()
                    });
                }
                else if (granularity == Granularity.State)
                {
                    var entities = Core.DataLayer.Base.State.In(context, placeId, boundingGranularity);
                    var data = Core.DataLayer.Base.ConsumerExpenditures.State(context);

                    var dataType = typeof(ConsumerExpendituresByState);
                    var constant = Expression.Constant(data);
                    var param = Expression.Parameter(dataType, "c");
                    var varSelector = Expression.Convert(Expression.Property(param, variable.Variable), typeof(long?)) as Expression;
                    var idSelector = Expression.Property(param, "StateId") as Expression;
                    var transType = typeof(KeyValue<long, long?>);
                    var constructor = transType.GetConstructor(new Type[] { typeof(long), typeof(long?) });
                    var selector = Expression.New(constructor, new Expression[] { idSelector, varSelector }.AsEnumerable(), new System.Reflection.MemberInfo[] { transType.GetProperty("Key"), transType.GetProperty("Value") });
                    var pred = Expression.Lambda(selector, param) as Expression;
                    var expression = Expression.Call(typeof(Queryable), "Select", new Type[] { dataType, transType }, constant, pred);
                    var transformedData = data.Provider.CreateQuery<KeyValue<long, long?>>(expression);

                    values = entities.GroupJoin(transformedData, i => i.Id, i => i.Key, (e, d) => new KeyValue<DbGeography, long?>
                    {
                        Key = e.StateGeographies.Where(g => g.GeographyClass.Name == Core.Geo.GeographyClass.Display)
                        .Select(g => SqlSpatialFunctions.Reduce(g.Geography.GeographyPolygon, tolerance)).FirstOrDefault(),
                        Value = d.Select(v => v.Value).DefaultIfEmpty(null).FirstOrDefault()
                    });
                }

                var list = values.ToList();

                var validValues = list
                    .Where(i => i.Value != null && i.Value > 0)
                    .NTile(i => i.Value, colors.Length)
                    .Select((i, index) => i.Where(g=>g.Key!= null).Select(g => new GeographyEntity() { Geography = SqlGeography.Parse(g.Key.AsText()), Color = colors[index] }))
                    .SelectMany(i => i)
                    .ToList();

                var invalidValues = list
                    .Where(i => i.Value == null || i.Value <= 0)
                    .Where(i => i.Key != null)
                    .Select(g => new GeographyEntity() { Geography = SqlGeography.Parse(g.Key.AsText()) })
                    .ToList();

                var geos = validValues.Union(invalidValues).ToList();

                tile.Draw(geos);
                var stream = new System.IO.MemoryStream();
                tile.Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return File(stream.GetBuffer(), "image/png");
            }
        }
    }
}
