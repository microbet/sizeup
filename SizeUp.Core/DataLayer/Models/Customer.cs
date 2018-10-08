using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Models
{
    public class Customer
    {
        public Int64 Id;
        public String Name;
        public ServiceArea[] ServiceAreas;
        public string[] Domains;
        public IdentityProvider[] IdentityProviders;
    }
}
