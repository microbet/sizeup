using System;
using System.Diagnostics;
using System.Web;

namespace SizeUp.Core.Web
{
    public class P3PHeader : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
        }

        void OnBeginRequest(object sender, System.EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("p3p", "CP=\"ALL ADM DEV PSAi COM OUR OTRo STP IND ONL\"");
        }  
    }
}
