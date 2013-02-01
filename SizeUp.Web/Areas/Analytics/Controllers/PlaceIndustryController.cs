﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core;
using SizeUp.Data.Analytics;
using SizeUp.Core.Web;
using System.Web.Security;

namespace SizeUp.Web.Areas.Analytics.Controllers
{
    public class PlaceIndustryController : Controller
    {
        //
        // GET: /Analytics/PlaceIndustry/

        public ActionResult Index(long placeId, long industryId)
        {
            long? apikeyid = WidgetToken.APIKeyId;
            Guid? userid = null;
            
            if(User.Identity.IsAuthenticated){
                userid = (Guid)Membership.GetUser().ProviderUserKey;
            }

            var item = new PlaceIndustrySearch(){
                IndustryId = industryId,
                PlaceId = placeId,
                APIKeyId = apikeyid,
                UserId = userid                
            };


            Singleton<Tracker>.Instance.PlaceIndustrySearch(item);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}
