/****** Script for SelectTopNRows command from SSMS  ******/
SELECT *
  FROM [ZPE_Shared].[dbo].[City] where Name like 'las cruces'
  
  SELECT '{"label":"' + Name + ', ' + 'NM' + '", "value":"' + CONVERT(varchar(36), GeoEntityId) + '"},'
FROM ZPE_Shared.dbo.city WHERE fipsclassid='C' and CityID=3539380
For XML PATH ('')

update citylistings 
set citylisting='[{"label":"Las Cruces, NM", "value":"F379ED80-58F8-4D68-8C4A-D4FF2455CC27"}]'
from citylistings where clientid='C07BE341-68D4-4605-A020-CEC7E9AB03EC'

SELECT '{"label":"' + Name + ' County, ' + 'NM' + '", "value":"' + CONVERT(varchar(36), GeoEntityId) + '"},' 
FROM ZPE_Shared.dbo.county WHERE CountyID='35013'

update citylistings 
set countylisting='[{"label":"Dona Ana County, NM", "value":"F536D2B6-1B91-4957-8204-E0653E06A617"}]'
from citylistings where clientid='C07BE341-68D4-4605-A020-CEC7E9AB03EC'


SELECT '{"label":"' + Name +  '", "value":"' + CONVERT(varchar(36), GeoEntityId) + '"},' 
FROM ZPE_Shared.dbo.msa WHERE MSAid='29740'

update citylistings 
set metrolisting='[{"label":"Las Cruces, NM", "value":"AE9C65A0-20FC-4D16-83D2-4539273ACF79"}]'
from citylistings where clientid='C07BE341-68D4-4605-A020-CEC7E9AB03EC'