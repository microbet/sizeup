using SizeUp.Core.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer
{
    public class Customer
    {
        /**
         * This function asks for two database connections that are open
         * simultaneously. It uses them one at a time, but this seems like
         * the only way to accept both contexts and let the consumer share
         * the contexts with other functions.
         */
        public static Models.Customer GetCustomerByKey(
            SizeUp.Data.API.APIContext apiContext,
            SizeUp.Data.SizeUpContext sizeupContext,
            Guid key)
        {
            Models.Customer customer;
            Int64[] serviceAreaReferences;

//            using (var context = ContextFactory.APIContext)
//            {
                apiContext.ContextOptions.LazyLoadingEnabled = false;
                customer = apiContext.APIKeys.Where(i => i.KeyValue == key && i.IsActive)
                    .Select(k => new Models.Customer
                    {
                        Id = k.Id,
                        Name = k.Name
                    })
                    .FirstOrDefault();
                if (customer == null)
                {
                    throw new System.Data.ObjectNotFoundException(key.ToString());
                }

                customer.Domains = apiContext.APIKeyDomains.Where(d => d.APIKeyId == customer.Id)
                    .Select(d => d.Domain).ToArray();
                customer.IdentityProviders = apiContext.IdentityProviders.Where(d => d.APIKeyId == customer.Id)
                    .Select(idp => new Models.IdentityProvider
                    {
                        EntryPoint = idp.EntryPoint,
                        Name = idp.Name
                    }).ToArray();
                serviceAreaReferences = apiContext.ServiceAreas.Where(area => area.APIKeyId == customer.Id)
                    .Select(area => area.GeographicLocationId).ToArray();
//            }

//            using (var context = ContextFactory.SizeUpContext)
//            {
                customer.ServiceAreas = sizeupContext.GeographicLocations
                    .Join(sizeupContext.Granularities,
                        loc => loc.GranularityId,
                        gran => gran.Id,
                        (loc, gran) => new Models.ServiceArea { Granularity = gran.Name, Id = loc.Id, Name = loc.LongName })
                    .Where(svcArea => serviceAreaReferences.Contains(svcArea.Id))
                    .ToArray();
//            }
                return customer;
        }
    }
}
