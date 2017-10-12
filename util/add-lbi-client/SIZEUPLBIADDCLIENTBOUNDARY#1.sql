declare @user nvarchar(150)
declare @organization nvarchar(200)
declare @defaultcity nvarchar(200)

set @user='Kim.Lofgreen@mesaaz.gov'
set @organization='City of Mesa'
set @defaultcity='e.g. Mesa, AZ'
--change GEOENTITY

Insert into ClientBoundaries (ClientBoundary, username, ClientName,byAddress_placeholder,community_placeholder,industry_placeholder,dynamic_iframe,show_resources,ClientID)
SELECT Geography, @user,@organization,'Please enter a street address', @defaultcity,'e.g. Coffee Shops',0,1,NEWID()
  FROM [ZPE_Shared].[dbo].[GeographicEntityGeography] where GeoEntityID='610FBB5B-27B8-4FF2-B642-E21BCB7CDACE'