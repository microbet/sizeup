/*
This generates the small location data sets from location source data.

To use it:
- Make databases called "RawData" and "Locations.Austin".
- Import source data into RawData.
- Run this script.
- Export Locations.Austin, with either "Generate Scripts"
    or with mssql-scripter, which I haven't tried, but see
    https://blogs.technet.microsoft.com/dataplatforminsider/2017/05/17/try-new-sql-server-command-line-tools-to-generate-t-sql-scripts-and-monitor-dynamic-management-views/

Arguably this script could incorporate filters for other types of source
data too, but those are in text and easier to filter using easier languages.
Arguably we should be filtering location data with easier languages too;
after all, text INSERT statements can be grepped. TBD.
*/

-- Make the Austin small data set --

set identity_insert [Locations.Austin].dbo.Nation ON
insert into [Locations.Austin].dbo.Nation
(Id, ShortName, LongName, Name, SEOKey, IsActive)
SELECT Id, ShortName, LongName, Name, SEOKey, IsActive
  FROM RawData.dbo.Nation
set identity_insert [Locations.Austin].dbo.Nation OFF

set identity_insert [Locations.Austin].dbo.Region ON
insert into [Locations.Austin].dbo.Region
(Id, ShortName, LongName, Name)
SELECT Id, ShortName, LongName, Name
  FROM RawData.dbo.Region
set identity_insert [Locations.Austin].dbo.Region OFF

set identity_insert [Locations.Austin].dbo.Division ON
insert into [Locations.Austin].dbo.Division
(Id, ShortName, LongName, RegionId, Name)
SELECT Id, ShortName, LongName, RegionId, Name
  FROM RawData.dbo.Division
set identity_insert [Locations.Austin].dbo.Division OFF

set identity_insert [Locations.Austin].dbo.State ON
insert into [Locations.Austin].dbo.State
(Id, ShortName, LongName, FIPS, Name, Abbreviation, SEOKey, DivisionId, NationId)
SELECT Id, ShortName, LongName, FIPS, Name, Abbreviation, SEOKey, DivisionId, NationId
  FROM RawData.dbo.State
  where Abbreviation = 'TX'
set identity_insert [Locations.Austin].dbo.State OFF

set identity_insert [Locations.Austin].dbo.Metro ON
insert into [Locations.Austin].dbo.Metro
(Id, ShortName, LongName, FIPS, Name, SEOKey)
SELECT Id, ShortName, LongName, FIPS, Name, SEOKey
  FROM RawData.dbo.Metro
  where Id in (129158, 130038)
set identity_insert [Locations.Austin].dbo.Metro OFF

set identity_insert [Locations.Austin].dbo.County ON
insert into [Locations.Austin].dbo.County
(Id, ShortName, LongName, Name, SEOKey, FIPS, StateId, MetroId)
SELECT Id, ShortName, LongName, Name, SEOKey, FIPS, StateId, MetroId
  FROM RawData.dbo.County
  where Name in ('Travis','Williamson','Hays','Blanco','Burnet','Bastrop','Caldwell')
  and StateId = 130112
set identity_insert [Locations.Austin].dbo.County OFF

set identity_insert [Locations.Austin].dbo.ZipCode ON
insert into [Locations.Austin].dbo.ZipCode
(Id, ShortName, LongName, Zip, Name)
SELECT Id, ShortName, LongName, Zip, Name
  FROM RawData.[dbo].[ZipCode]
  where Zip like '786%' or Zip like '787%'
set identity_insert [Locations.Austin].dbo.ZipCode OFF

/*
-- undo --
delete from [Locations.Austin].dbo.ZipCode
delete from [Locations.Austin].dbo.County
delete from [Locations.Austin].dbo.Metro 
delete from [Locations.Austin].dbo.State
delete from [Locations.Austin].dbo.Division
delete from [Locations.Austin].dbo.Region
delete from [Locations.Austin].dbo.Nation

-- verify --
select count(*) from [Locations.Austin].dbo.ZipCode
select count(*) from [Locations.Austin].dbo.County
select count(*) from [Locations.Austin].dbo.Metro 
select count(*) from [Locations.Austin].dbo.State
select count(*) from [Locations.Austin].dbo.Division
select count(*) from [Locations.Austin].dbo.Region
select count(*) from [Locations.Austin].dbo.Nation
*/
