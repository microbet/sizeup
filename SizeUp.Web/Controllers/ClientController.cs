using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Web.Models;
using SizeUp.Core.Serialization;
using SizeUp.Core.Web;
using SizeUp.Data;
using Api = SizeUp.Web.Areas.Api;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using SizeUp.Data;


namespace SizeUp.Web.Controllers
{
    public class ClientController : Controller
    {

        protected bool isValidUser = false;
        protected Guid? ClientID;

        //
        // GET: /client/
        [HttpGet]
        public ActionResult Index()
        {
            var reset = false;
            if (TempData["PasswordReset"] != null)
                reset = true;
            ViewBag.PasswordReset = reset;
            ViewBag.PasswordResetSent = false;
            return View();
        }

        [HttpPost]
        public ActionResult Index(Models.User user)
        {
            ViewBag.PasswordReset = false;
            ViewBag.PasswordResetSent = false;
            if (ModelState.IsValid)
            {
                if (user.IsValid(user.Email, user.Password))
                {
                    FormsAuthentication.SetAuthCookie(user.Email, user.RememberMe);


                    using (var context = ContextFactory.APIContext)
                    {

                        var result = context.APIKeys.Where(i => i.UserName == user.Email).FirstOrDefault();
                        if (result != null)
                        {
                            this.isValidUser = true;
                            this.ClientID = result.KeyValue;

                            Response.Redirect("/client/dashboard/" + ClientID + "?section=revenue");
                        }
                        else
                            ModelState.AddModelError("", "User not found!");

                    }

                }
                else
                    ModelState.AddModelError("", string.Format("Incorrect password. Try again or <a href='/client/beginresetpassword/?email={0}'>reset your password</a>", Server.UrlEncode(user.Email)));
            }

            return View("Index", user);
        }


        [Authorize]
        [HttpGet]
        public ActionResult Dashboard(Guid? id = null, string section = "")
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("/client");

            var ClientId = id;
            this.isValidUser = true;
            if (!this.isValidUser || (ClientId == Guid.Empty || !ClientId.HasValue) || string.IsNullOrEmpty(section))
            {
                return Index();
            }
            else
            {
                ViewBag.ClientId = ClientId;

                using (var context = ContextFactory.SizeUpContext)
                {
                    var result = context.ClientResourceStrings.Where(x => x.ClientID == ClientId).FirstOrDefault();
                    string html = string.Empty;
                    if (result != null)
                    {

                        switch (section)
                        {
                            case "revenue":
                                html = result.DashboardRevenue;
                                break;
                            case "salary":
                                html = result.DashboardSalary;
                                break;
                            case "employees":
                                html = result.DashboardAverageEmployees;
                                break;
                            case "costeffectiveness":
                                html = result.DashboardCostEffectiveness;
                                break;
                            case "turnover":
                                html = result.DashboardTurnover;
                                break;
                            case "healthcare":
                                html = result.DashboardHealthcare;
                                break;
                            case "workerscomp":
                                html = result.DashboardWorkersComp;
                                break;
                            default:
                                html = result.DashboardRevenue;
                                break;
                        }
                        ViewBag.Strings = html;
                    }
                    if (string.IsNullOrEmpty(html))
                    {
                        string sectionId = string.Empty;
                        switch (section)
                        {
                            case "revenue":
                                sectionId = "Dashboard.Revenue.Resources";
                                break;
                            case "salary":
                                sectionId = "Dashboard.Salary.Resources";
                                break;
                            case "employees":
                                sectionId = "Dashboard.AverageEmployees.Resources";
                                break;
                            case "costeffectiveness":
                                sectionId = "Dashboard.CostEffectiveness.Resources";
                                break;
                            case "turnover":
                                sectionId = "Dashboard.Turnover.Resources";
                                break;
                            case "healthcare":
                                sectionId = "Dashboard.Healthcare.Resources";
                                break;
                            case "workerscomp":
                                sectionId = "Dashboard.WorkersComp.Resources";
                                break;
                            default:
                                sectionId = "Dashboard.Revenue.Resources";
                                break;
                        }
                        var resource = context.ResourceStrings.Where(i => i.Name == sectionId).FirstOrDefault();
                        ViewBag.Strings = resource.Value;
                    }
                }
            }
            ViewBag.Section = section;
            return View("Dashboard");

        }

        [Authorize]
        [HttpPost]
        public ActionResult Dashboard(Models.ResourceModel resource)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("/client");

            // extract img srcs from the thingy
            //string pattern = @"<img.*?src=""(?<url>.*?)"".*?>";
            string pattern = @"<img.+?src=[\""'](.+?)[\""'].*?>";
            Regex r = new Regex(pattern);

            string domain = "GISPLANNING";
            string username = "sroeder";
            string password = "g1$ppass";
            WrapperImpersonationContext impContext = new WrapperImpersonationContext(domain, username, password);
            impContext.Enter();
            // extract image tags
            foreach (Match match in r.Matches(resource.ResourceHtml))
            {
                // get the src data
                string base64ImageString = match.Groups[1].Value;

                var isJpeg = false;
                var isPng = false;
                var isTiff = false;
                var isGif = false;
                string replaceString = string.Empty;

                // check to see which format it is...jpeg,png,tiff,gif
                if (base64ImageString.Contains("data:image/jpeg;base64,"))
                {
                    isJpeg = true;
                    replaceString = "data:image/jpeg;base64,";
                }
                if (base64ImageString.Contains("data:image/png;base64,"))
                {
                    isPng = true;
                    replaceString = "data:image/png;base64,";
                }
                if (base64ImageString.Contains("data:image/tiff;base64,"))
                {
                    isTiff = true;
                    replaceString = "data:image/tiff;base64,";
                }
                if (base64ImageString.Contains("data:image/gif;base64,"))
                {
                    isGif = true;
                    replaceString = "data:image/gif;base64,";
                }

                if (!string.IsNullOrEmpty(replaceString))
                {
                    // remove the data:image/png;base64, from it
                    base64ImageString = base64ImageString.Replace(replaceString, "");

                    // save the img to disk
                    Bitmap bmpFromString = null;
                    byte[] byteBuffer = Convert.FromBase64String(base64ImageString);
                    //byte[] byteBuffer = Serializer.FromBase64(base64ImageString);
                    var myUniqueFileName = string.Format(@"{0}.jpg", Guid.NewGuid());
                    string imageServerPath = "http://images.lbi.sizeup.com";
                    string imageFilePath = "\\\\images-lbi-www01.gisplanning.net\\d$\\images\\";
                    string path = string.Empty;
                    using (MemoryStream memoryStream = new MemoryStream(byteBuffer))
                    {

                        memoryStream.Position = 0;

                        bmpFromString = (Bitmap)Bitmap.FromStream(memoryStream);

                        while (System.IO.File.Exists(myUniqueFileName))
                        {
                            myUniqueFileName = string.Format(@"{0}.jpg", Guid.NewGuid());

                        }


                        var context = System.Web.HttpContext.Current;

                        path = string.Format(@"{0}/{1}", imageServerPath, myUniqueFileName);

                        //System.IO.FileStream fs
                        //    = new System.IO.FileStream(path, System.IO.FileMode.Create
                        //    , System.IO.FileAccess.ReadWrite);

                        string filePath = string.Format(@"{0}{1}", imageFilePath, myUniqueFileName);
                        if (isJpeg == true)
                            bmpFromString.Save(filePath, ImageFormat.Jpeg);
                        if (isPng == true)
                            bmpFromString.Save(filePath, ImageFormat.Png);
                        if (isTiff == true)
                            bmpFromString.Save(filePath, ImageFormat.Tiff);
                        if (isGif == true)
                            bmpFromString.Save(filePath, ImageFormat.Gif);


                        //byte[] matriz = memoryStream.ToArray();
                        //fs.Write(matriz, 0, matriz.Length);

                        memoryStream.Close();
                        //fs.Close();

                    }
                    byteBuffer = null;


                    // replace base64 in html with new path
                    resource.ResourceHtml = resource.ResourceHtml.Replace(string.Format("{0}{1}", replaceString, base64ImageString), path);
                }
            }
            impContext.Leave();

            if (resource.SaveHtml == true)
            {
                Guid g_ClientID;
                Guid.TryParse(resource.ClientId, out g_ClientID);

                if (resource.ClientId != "")
                {

                    using (var context = ContextFactory.SizeUpContext)
                    {
                        var result = context.ClientResourceStrings.Where(x => x.ClientID == g_ClientID).FirstOrDefault();
                        if (result != null)
                        {
                            switch (resource.Section)
                            {
                                case "revenue":
                                    result.DashboardRevenue = resource.ResourceHtml;
                                    break;
                                case "salary":
                                    result.DashboardSalary = resource.ResourceHtml;
                                    break;
                                case "employees":
                                    result.DashboardAverageEmployees = resource.ResourceHtml;
                                    break;
                                case "costeffectiveness":
                                    result.DashboardCostEffectiveness = resource.ResourceHtml;
                                    break;
                                case "turnover":
                                    result.DashboardTurnover = resource.ResourceHtml;
                                    break;
                                case "healthcare":
                                    result.DashboardHealthcare = resource.ResourceHtml;
                                    break;
                                case "workerscomp":
                                    result.DashboardWorkersComp = resource.ResourceHtml;
                                    break;
                                default:
                                    result.DashboardRevenue = resource.ResourceHtml;
                                    break;
                            }

                            context.SaveChanges();
                        }

                    }
                }

            }

            // save all the resource to the database
            return Content(resource.ResourceHtml);
            //return View();
        }

    }
}
