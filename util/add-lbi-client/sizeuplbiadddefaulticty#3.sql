Declare @defaultcity  nvarchar(30)
set @defaultcity='Moline'
Declare @fips nvarchar(30)
declare @seokey nvarchar(30)


set @fips=(select fips from City where LEFT(FIPS,2)=17 and Name=@defaultcity and CityTypeid=15)


select @fips
set @seokey=(select seokey from City where LEFT(FIPS,2)=17 and Name=@defaultcity and CityTypeid=15)

select GeoEntityID, * from ZPE_Shared.dbo.City where CityID=@fips

update citylistings
set defaultcityid=(select GeoEntityID from ZPE_Shared.dbo.City where CityID=@fips
), defaultcitykey=@seokey
from lbisizeup.dbo.Citylistings where defaultcity like '%' + @defaultcity +'%' and defaultcity like '%IL%'

select * from lbisizeup.dbo.Citylistings where defaultcity like '%' +  @defaultcity  +'%' and defaultcity like '%IL%'