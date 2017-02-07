using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Web.Models;
using SizeUp.Data;
using SizeUp.Core.Web;




namespace SizeUp.Web.Controllers
{
    public class BestPlacesController : BaseController
    {
        //
        // GET: /Competition/
        string str = " declare @clmName varchar(100)  declare @divField varchar(50)  DECLARE @sql nvarchar(4000)  set @clmName = '0 as [Value], 0 as [Percentage]'  set @sql = if (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'census_BG' and  COLUMN_NAME = '+{0}+') > 0  BEGIN  select @divField = divideby from census_label where FIELD = '(0}'  set @clmName = 'd.[' +{0}+ '] as [Value], 0 as [Percentage]'  if ' + @divField +' is not null and @divField != ''  BEGIN\tset @clmName = 'd.['+  {0}+  '] as [Value], d.[' + @divField + '] as [Percentage]'  END  END\t SELECT ' + @clmName + ', bg.Geography, 0 as ValueRangeIndex, 0 as PercentageRangeIndex  FROM demographics.dbo.census_BG d join  demographics.dbo.census_blockgroups_geo bg on d.BLOCKGROUP = bg.BG where  bg.Geography.STIntersects('{1}') = 1 order by [Value] asc ";
        public ActionResult Index(string industry)
        {
            if (CurrentInfo.CurrentIndustry == null)
            {
                throw new HttpException(404, "Page Not Found");
            }
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Regions = Core.DataLayer.Place.List(context).Select(i => i.Region).Distinct().ToList();
                ViewBag.States = Core.DataLayer.Place.List(context).Select(i => i.State).Distinct().OrderBy(i => i.Name).ToList();
                return View();
            }
        }

        public ActionResult PickIndustry()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                return View();
            }
        }

    }
}
