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
    // Originally used as the type for serviceAreaReferences, but I backed
    // away from that because I didn't know how entity comparisons would work.
    public class ServiceAreaReference
    {
        public Int64 GeographicLocationId;
        public Int64 GranularityId;
    }
    public class ServiceArea {
        public string Granularity;
        public Int64 Id;
        public string Name;
    }
    public class CustomerPublicRecord
    {
        public Int64 Id;
        public String Name;
        public ServiceArea[] ServiceAreas;
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

        public ActionResult Get(string key)
        {
            Guid _key;
            try
            {
                _key = new Guid(key);
            }
            catch (ArgumentNullException) { return InvalidApikeyArg(key); }
            catch (FormatException) { return InvalidApikeyArg(key); }
            catch (OverflowException) { return InvalidApikeyArg(key); }

            Nation nation;
            using (var context = ContextFactory.SizeUpContext) {
                context.ContextOptions.LazyLoadingEnabled = false;
                nation = context.Nations.FirstOrDefault();
            }

            CustomerPublicRecord customer;
            Int64[] serviceAreaReferences;

            using (var context = ContextFactory.APIContext)
            {
                context.ContextOptions.LazyLoadingEnabled = false;
                customer = context.APIKeys.Where(i => i.KeyValue == _key && i.IsActive)
                    .Select(k => new CustomerPublicRecord {
                        Id = k.Id,
                        Name = k.Name
                    })
                    .FirstOrDefault();
                if (customer == null)
                {
                    Response.StatusCode = 401;
                    return Content("The product key (\"key\") is invalid. Please see https://api.sizeup.com/documentation/ for help.", "text/plain");
                }

                customer.Domains = context.APIKeyDomains.Where(d => d.APIKeyId == customer.Id)
                    .Select(d => d.Domain).ToArray();
                customer.IdentityProviders = context.IdentityProviders.Where(d => d.APIKeyId == customer.Id)
                    .Select(idp => new IdentityProvider {
                        EntryPoint = idp.EntryPoint,
                        Name = idp.Name
                    }).ToArray();
                serviceAreaReferences = context.ServiceAreas.Where(area => area.APIKeyId == customer.Id)
                    .Select(area => area.GeographicLocationId).ToArray();
            }

            using (var context = ContextFactory.SizeUpContext)
            {
                customer.ServiceAreas = context.GeographicLocations
                    .Join(context.Granularities,
                        loc => loc.GranularityId,
                        gran => gran.Id,
                        (loc, gran) => new ServiceArea {Granularity=gran.Name, Id=loc.Id, Name=loc.LongName})
                    .Where(svcArea => serviceAreaReferences.Contains(svcArea.Id))
                    .ToArray();
            }

            return Json(customer, JsonRequestBehavior.AllowGet);
        }

    }
}
