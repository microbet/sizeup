using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Core.Geo;
namespace SizeUp.Core.DataLayer
{
    public class Business
    {
        public static IQueryable<Data.Business> Get(SizeUpContext context)
        {
            return context.Businesses.Where(i => i.IsActive && i.InBusiness);
        }

        public static IQueryable<Models.DistanceEntity<Data.Business>> Near(SizeUpContext context, LatLng latLng)
        {
            var distanceFilter = new DistanceEntity<Data.Business>.DistanceEntityFilter(latLng);
            return Get(context)
                .Where(i => i.MatchLevel == "0")
                .Select(i => new KeyValue<Data.Business, Geo.LatLng>
                {
                    Key = i,
                    Value = new Geo.LatLng { Lat = (double)i.Lat, Lng = (double)i.Long }
                })
                .Select(distanceFilter.Projection);

        }

        public static IQueryable<Data.Business> In(SizeUpContext context, BoundingBox boundingBox)
        {
            return Get(context)
                .Where(i => i.MatchLevel == "0")
                .Select(i => new KeyValue<Data.Business, Geo.LatLng>
                {
                    Key = i,
                    Value = new Geo.LatLng { Lat = (double)i.Lat, Lng = (double)i.Long }
                })
                .Where(i=> i.Value.Lat < boundingBox.NorthEast.Lat && i.Value.Lat > boundingBox.SouthWest.Lat && i.Value.Lng > boundingBox.SouthWest.Lng && i.Value.Lng < boundingBox.NorthEast.Lng)
                .Select(i=>i.Key);

        }

        public static Models.Business Get(SizeUpContext context, long id)
        {
            var data = Get(context)
                .Where(i => i.Id == id)
                .Select(new Projections.Business.Default().Expression).FirstOrDefault();
            return data;
        }

        public static Models.Business GetLegacy(SizeUpContext context, string SEOKey)
        {
            var data = Get(context)
                .Where(i => i.LegacyBusinessSEOKeys.Any(l=>l.SEOKey == SEOKey))
                .Select(new Projections.Business.Default().Expression)
                .FirstOrDefault();
            return data;
        }

        public static Models.Business GetIn(SizeUpContext context, long id, long placeId)
        {
            var data = Get(context)
                .Where(i => i.Id == id && i.Cities.Any(bc=>bc.Places.Any(p=>p.Id == placeId)))
                .Select(new Projections.Business.Default().Expression)
                .FirstOrDefault();
            return data;
        }

        public static Models.Business GetAt(SizeUpContext context, LatLng latLng, List<long> industryIds)
        {
            var data = Near(context, latLng)
                .Where(i => industryIds.Contains(i.Entity.IndustryId.Value))
                .OrderBy(i => i.Distance)
                .Select(i=>i.Entity)
                .Select(new Projections.Business.Default().Expression)
                .FirstOrDefault();
            return data;
        }

        public static IQueryable<Models.DistanceEntity<Models.Business>> ListNear(SizeUpContext context, LatLng latLng, List<long> industryIds)
        {
            var data = Near(context, latLng)
                .Where(i => industryIds.Contains(i.Entity.IndustryId.Value))
                .Select(new Projections.Business.Distance().Expression);
            return data;
        }


        public static IQueryable<Models.Business> ListIn(SizeUpContext context, Core.Geo.BoundingBox boundingBox, List<long> industryIds)
        {
            var data = In(context, boundingBox)
                .Where(i => industryIds.Contains(i.IndustryId.Value))
                .Select(new Projections.Business.Default().Expression);
            return data;
        }

        public static IQueryable<Models.Business> ListIn(SizeUpContext context, long industryId, long placeId)
        {
            var data = Get(context)
                .Where(i => i.IndustryId == industryId && i.Cities.Any(bc=>bc.Places.Any(p=>p.Id == placeId)))
                .Select(new Projections.Business.Default().Expression);
            return data;
        }
    }
}
