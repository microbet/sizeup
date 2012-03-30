using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SizeUp.Web.Models
{
    public enum NavItems
    {
        Dashboard,
        Competition,
        Advertising,
        TopPlaces
    }

    public class Header
    {
        public bool HideMenu { get; set; }
        public bool HideNavigation { get; set; }
        public NavItems ActiveTab { get; set; }
    }
}