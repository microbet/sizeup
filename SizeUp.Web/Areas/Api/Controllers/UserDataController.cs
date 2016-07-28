using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace SizeUp.Web.Controllers
{
    public class UserDataController : ApiController
    {
        public HttpResponseMessage Register(string email, string name, string password)
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public HttpResponseMessage ForgotPassword()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
