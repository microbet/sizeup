using SizeUp.Api.Controllers;
using SizeUp.Data;
using SizeUp.Data.API;
using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace SizeUp.Api.Areas.Customer.Controllers
{
    public class IdentityProvider {
        public string EntryPoint;
    }
    public class Area
    {
        public Int64 Id;
        public String SEOKey;
        public String Name;
    }
    public class CustomerPublicRecord
    {
        public Int64 Id;
        public String Name;
        public Area ServiceArea;
        public string[] Domains;
        public IdentityProvider IdentityProvider;
    }

    public class AccountController : BaseController
    {
        // GET: /customer/get?key=KEY

        public ActionResult Get(Guid key)
        {
            Nation nation;
            using (var context = ContextFactory.SizeUpContext) {
                context.ContextOptions.LazyLoadingEnabled = false;
                nation = context.Nations.FirstOrDefault();
            }

            using (var context = ContextFactory.APIContext)
            {
                context.ContextOptions.LazyLoadingEnabled = false;
                var customer = context.APIKeys.Where(i => i.KeyValue == key && i.IsActive)
                    .Select(k => new CustomerPublicRecord {
                        Id = k.Id,
                        Name = k.Name
                    })
                    .FirstOrDefault();
                if (customer == null)
                {
                    Response.StatusCode = 401;
                    return Content("The product key (\"apikey\") is invalid. Please see https://www.sizeup.com/developers/documentation for help.", "text/plain");
                }
                customer.ServiceArea = new Area
                {
                    Id = nation.Id,
                    Name = nation.Name,
                    SEOKey = nation.SEOKey
                };
                customer.Domains = context.APIKeyDomains.Where(d => d.APIKeyId == customer.Id).Select(d => d.Domain).ToArray();
                customer.IdentityProvider = new IdentityProvider { EntryPoint = "https://login.sizeup.com/saml2/idp/SSOService" };
                return Json(customer, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
