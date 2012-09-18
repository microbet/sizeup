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
using SizeUp.Core.Geo;
using SizeUp.Core.DataAccess.Models;

namespace SizeUp.Web.Areas.Tiles.Controllers
{
    public class GeographyBoundaryController : Controller
    {
        //
        // GET: /Tiles/GeographyBoundary/

        public ActionResult Index(int x, int y, int zoom, string entityId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                GeographyBoundary tile = new GeographyBoundary(256, 256, x, y, zoom);
                var boundingEntity = new BoundingEntity(entityId);
             
                IQueryable<DisplayGeography> geos = null;
               if(boundingEntity.EntityType == BoundingEntity.BoundingEntityType.Zip){
                   geos = context.ZipCodes.Where(i=>i.Id == boundingEntity.EntityId)
                       .Select(i=>i.ZipCodeGeographies.Where(g=>g.GeographyClass.Name == "Display").Select(g=> new DisplayGeography(){ Geography = g.Geography.GeographyPolygon, Id = i.Id}).FirstOrDefault());
               }
               else if(boundingEntity.EntityType == BoundingEntity.BoundingEntityType.City){
                   geos = context.Cities.Where(i=>i.Id == boundingEntity.EntityId)
                       .Select(i=>i.CityGeographies.Where(g=>g.GeographyClass.Name == "Display").Select(g=> new DisplayGeography(){ Geography = g.Geography.GeographyPolygon, Id = i.Id}).FirstOrDefault());
               }
               else if(boundingEntity.EntityType == BoundingEntity.BoundingEntityType.County){
                   geos = context.Counties.Where(i=>i.Id == boundingEntity.EntityId)
                       .Select(i=>i.CountyGeographies.Where(g=>g.GeographyClass.Name == "Display").Select(g=> new DisplayGeography(){ Geography = g.Geography.GeographyPolygon, Id = i.Id}).FirstOrDefault());
               }
               else if (boundingEntity.EntityType == BoundingEntity.BoundingEntityType.Metro){
                   geos = context.Metroes.Where(i=>i.Id == boundingEntity.EntityId)
                       .Select(i=>i.MetroGeographies.Where(g=>g.GeographyClass.Name == "Display").Select(g=> new DisplayGeography(){ Geography = g.Geography.GeographyPolygon, Id = i.Id}).FirstOrDefault());
               }
               else if(boundingEntity.EntityType == BoundingEntity.BoundingEntityType.State){
                   geos = context.States.Where(i=>i.Id == boundingEntity.EntityId)
                       .Select(i=>i.StateGeographies.Where(g=>g.GeographyClass.Name == "Display").Select(g=> new DisplayGeography(){ Geography = g.Geography.GeographyPolygon, Id = i.Id}).FirstOrDefault());
               }

               List<GeographyCollection> geoCollection = GeographyBoundary.CreateCollections("#6495ED", geos);

                tile.Draw(geoCollection);
                var stream = new System.IO.MemoryStream();
                tile.Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return File(stream.GetBuffer(), "image/png");
            }
        }

    }
}
