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
        public string Name;
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
        public IdentityProvider[] IdentityProviders;
    }

    public class AccountController : BaseController
    {
        // GET: /customer/get?key=KEY

        protected ActionResult InvalidApikeyArg(string apikey)
        {
            Response.StatusCode = 400;
            return Content("Argument \"apikey\" is missing or misformatted.\nReceived: " + apikey + "\nExpected: A valid GUID, in the format apikey=xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx, where 'x' is a hexadecimal digit.", "text/plain");
        }

        public ActionResult Get(string apikey)
        {
            Guid key;
            try
            {
                key = new Guid(apikey);
            }
            catch (ArgumentNullException) { return InvalidApikeyArg(apikey); }
            catch (FormatException) { return InvalidApikeyArg(apikey); }
            catch (OverflowException) { return InvalidApikeyArg(apikey); }

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
                customer.Domains = context.APIKeyDomains.Where(d => d.APIKeyId == customer.Id)
                    .Select(d => d.Domain).ToArray();
                customer.IdentityProviders = context.IdentityProviders.Where(d => d.APIKeyId == customer.Id)
                    .Select(idp => new IdentityProvider {
                        EntryPoint = idp.EntryPoint,
                        Name = idp.Name
                    }).ToArray();
                return Json(customer, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
