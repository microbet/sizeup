using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SizeUp.Web.Areas.Api.Models.JobChange
{
    public class JobChangeDynamics
    {
        public long? JobGains { get; set; }
        public long? JobLosses { get; set; }
        public long? NetJobChange { get; set; }
    }
}