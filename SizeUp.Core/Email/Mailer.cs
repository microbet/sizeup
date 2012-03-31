using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core.Identity;

namespace SizeUp.Core.Email
{
    public class Mailer
    {
        public static void SendRegistrationEmail(Identity.Identity user)
        {
            //create a strings database and a templating system (mustache perhaps) 
            //make the emails part of those strings
        }

        public static void SendResetPasswordEmail(Identity.Identity user)
        {

        }
    }
}
