using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core;
using SizeUp.Data.Analytics;
using SizeUp.Core.API;
using System.Web.Security;


namespace SizeUp.Web.Areas.Analytics.Controllers
{
    public class RelatedIndustryController : Controller
    {
        //
        // GET: /Analytics/RelatedIndustry/

        public ActionResult Competitor(long placeId, long primaryIndustryId, long relatedIndustryId)
        {
            APIToken token = APIToken.GetFromCookie();
            long? apikeyid = token != null ? token.APIKeyId : (long?)null;
            Guid? userid = null;

            if (User.Identity.IsAuthenticated)
            {
                userid = (Guid)Membership.GetUser().ProviderUserKey;
            }

            var item = new RelatedCompetitor()
            {
                PrimaryIndustryId = primaryIndustryId,
                RelatedIndustryId = relatedIndustryId,
                PlaceId = placeId,
                APIKeyId = apikeyid,
                UserId = userid
            };


            Singleton<Tracker>.Instance.RelatedCompetitor(item);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Buyer(long placeId, long primaryIndustryId, long relatedIndustryId)
        {
            APIToken token = APIToken.GetFromCookie();
            long? apikeyid = token != null ? token.APIKeyId : (long?)null;
            Guid? userid = null;

            if (User.Identity.IsAuthenticated)
            {
                userid = (Guid)Membership.GetUser().ProviderUserKey;
            }

            var item = new RelatedBuyer()
            {
                PrimaryIndustryId = primaryIndustryId,
                RelatedIndustryId = relatedIndustryId,
                PlaceId = placeId,
                APIKeyId = apikeyid,
                UserId = userid
            };


            Singleton<Tracker>.Instance.RelatedBuyer(item);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Supplier(long placeId, long primaryIndustryId, long relatedIndustryId)
        {
            APIToken token = APIToken.GetFromCookie();
            long? apikeyid = token != null ? token.APIKeyId : (long?)null;
            Guid? userid = null;

            if (User.Identity.IsAuthenticated)
            {
                userid = (Guid)Membership.GetUser().ProviderUserKey;
            }

            var item = new RelatedSupplier()
            {
                PrimaryIndustryId = primaryIndustryId,
                RelatedIndustryId = relatedIndustryId,
                PlaceId = placeId,
                APIKeyId = apikeyid,
                UserId = userid
            };


            Singleton<Tracker>.Instance.RelatedSupplier(item);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}
