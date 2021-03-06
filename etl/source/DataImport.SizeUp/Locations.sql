-- to undo: --
-- delete from NewData.dbo.ZipCode
-- delete from NewData.dbo.County
-- delete from NewData.dbo.Metro
-- delete from NewData.dbo.State
-- delete from NewData.dbo.Division
-- delete from NewData.dbo.Region
-- delete from NewData.dbo.Nation
-- delete from NewData.dbo.GeographicLocation

declare @granularity_id bigint

SET IDENTITY_INSERT NewData.dbo.GeographicLocation ON

select @granularity_id = Id from NewData.dbo.Granularity where Name = 'Nation'
insert into NewData.dbo.GeographicLocation
  (Id, GranularityId, ShortName, LongName)
  select Id, @granularity_id, ShortName, LongName from RawData.dbo.Nation
insert into NewData.dbo.Nation
  select Name, SEOKey, IsActive, Id from RawData.dbo.Nation

select @granularity_id = Id from NewData.dbo.Granularity where Name = 'Region'
insert into NewData.dbo.GeographicLocation
  (Id, GranularityId, ShortName, LongName)
  select Id, @granularity_id, ShortName, LongName from RawData.dbo.Region
insert into NewData.dbo.Region
  select Name, Id from RawData.dbo.Region

select @granularity_id = Id from NewData.dbo.Granularity where Name = 'Division'
insert into NewData.dbo.GeographicLocation
  (Id, GranularityId, ShortName, LongName)
  select Id, @granularity_id, ShortName, LongName from RawData.dbo.Division
insert into NewData.dbo.Division
  select RegionId, Name, Id from RawData.dbo.Division

select @granularity_id = Id from NewData.dbo.Granularity where Name = 'State'
insert into NewData.dbo.GeographicLocation
  (Id, GranularityId, ShortName, LongName)
  select Id, @granularity_id, ShortName, LongName from RawData.dbo.State
insert into NewData.dbo.State
  select FIPS, Name, Abbreviation, SEOKey, DivisionId, Id, NationId from RawData.dbo.State

select @granularity_id = Id from NewData.dbo.Granularity where Name = 'Metro'
insert into NewData.dbo.GeographicLocation
  (Id, GranularityId, ShortName, LongName)
  select Id, @granularity_id, ShortName, LongName from RawData.dbo.Metro
insert into NewData.dbo.Metro
  select FIPS, Name, SEOKey, Id from RawData.dbo.Metro

select @granularity_id = Id from NewData.dbo.Granularity where Name = 'County'
insert into NewData.dbo.GeographicLocation
  (Id, GranularityId, ShortName, LongName)
  select Id, @granularity_id, ShortName, LongName from RawData.dbo.County
insert into NewData.dbo.County
  select Name, SEOKey, FIPS, StateId, MetroId, Id from RawData.dbo.County
  
select @granularity_id = Id from NewData.dbo.Granularity where Name = 'ZipCode'
insert into NewData.dbo.GeographicLocation
  (Id, GranularityId, ShortName, LongName)
  select Id, @granularity_id, ShortName, LongName from RawData.dbo.ZipCode
insert into NewData.dbo.ZipCode
  select Zip, Name, Id from RawData.dbo.ZipCode

SET IDENTITY_INSERT NewData.dbo.GeographicLocation OFF
