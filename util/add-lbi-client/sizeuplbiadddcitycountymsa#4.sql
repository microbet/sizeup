/****** Script for SelectTopNRows command from SSMS  ******/
SELECT *
  FROM [ZPE_Shared].[dbo].[City] where Name like 'Santa cruz'
  
  SELECT '{"label":"' + Name + ', ' + 'KY' + '", "value":"' + CONVERT(varchar(36), GeoEntityId) + '"},'  as 
FROM ZPE_Shared.dbo.city WHERE fipsclassid='C' and CityID=0669112
For XML PATH ('')

update citylistings 
set citylisting='[{"label":"Santa Cruz, CA", "value":"8C57FB37-53A4-45B1-91FC-239131F393EC"}]'
from citylistings where clientid='BA9EB14B-7D01-4C2B-A6C4-A09402F004C3'

SELECT '{"label":"' + Name + ' County, ' + 'CA' + '", "value":"' + CONVERT(varchar(36), GeoEntityId) + '"},' 
FROM ZPE_Shared.dbo.county WHERE CountyID='06087'

update citylistings 
set countylisting='[{"label":"Santa Cruz County, CA", "value":"2BBF594C-E686-4DB8-BB82-78654D298D08"}]'
from citylistings where clientid='BA9EB14B-7D01-4C2B-A6C4-A09402F004C3'