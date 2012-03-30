using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SizeUp.Core.Identity
{
    public class Identity
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }
    }
}