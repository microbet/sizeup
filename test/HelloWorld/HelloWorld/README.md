This is a hello-world app that should always work. It's useful for testing whether your ASP.NET MVC 4 web app container is working sanely.

It's hard to tell what code came with the Visual Studio template, and what code I actually wrote, so here's an outline of what I did:
- Looked at https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-aspnet-mvc4/
- Started with template ASP.NET MVC 4 Web Application / Internet Application
- Modified Controllers/HomeController.cs
- Modified Views/Home/Index.cshtml
- Modified Views/Shared/_Layout.cshtml
- Modified packages.config (via NuGet console: Install-Package MvcDiagnostics -Version 5.2.3 -ProjectName HelloWorld)
- Added <customErrors mode="Off" /> to Web.config/<configuration>/<system.web> (or IIS will reset this every time you publish)
- Changed CopyLocal to True on the following references:
-- System.Net.Http
-- System.Net.Http.Formatting
-- System.Net.Http.WebRequest
-- System.Net.Http.Formatting
-- System.Web.Http
-- System.Web.Http.WebHost

