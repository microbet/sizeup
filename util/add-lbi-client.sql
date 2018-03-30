-- TODO set county and city names by XML; see https://weblogs.asp.net/jongalloway/passing-lists-to-sql-server-2005-with-xml-parameters

-- Inputs that should be set by the consumer of this script --

-- Required (please replace values in "set" statements with your own):
declare @user nvarchar(150) -- Contact email address of the customer.
declare @organization nvarchar(200) -- Name of the customer.
declare @city_name nvarchar(200) -- Common name of the city, e.g. "Los Angeles".
declare @state_abbr char(2) -- Two-letter state abbreviation, e.g. "CA".

set @user='mubalde@sizeup.com'
set @organization='StartItUp Technologies'
set @city_name='Long Beach'
set @state_abbr='CA'

-- Optional (this script provides reasonable defaults):
declare @byAddress_placeholder varchar(100) -- ?
declare @community_placeholder varchar(100) -- Text shown in unfilled "Location" form field
declare @industry_placeholder varchar(100) -- Text shown in unfilled "Industry" form field
declare @dynamic_iframe bit -- ?
declare @show_resources bit -- ?
declare @lockcountyPicker bit -- ?

set @byAddress_placeholder = 'Please enter a street address'
set @community_placeholder = 'e.g. ' + @city_name + ', ' + @state_abbr
set @industry_placeholder = 'e.g. Coffee Shops'
set @dynamic_iframe = 0
set @show_resources = 1
set @lockcountyPicker = 1

-- Runtime variables used by this script; consumer can ignore these.
declare @client_id uniqueidentifier
declare @city_id char(7)
declare @sizeup_city_id bigint
declare @county_id char(5)
declare @geo_entity_id uniqueidentifier
declare @fips nvarchar(10)
declare @seo_key nvarchar(150)
declare @state_id char(2)
declare @state_name nvarchar(50)

-- From SIZEUPLBIADDCLIENTBOUNDARY#1.sql

select @state_id = StateID, @state_name = Name from [LBI_Geo].[dbo].State where Abbreviation = @state_abbr;
print 'Using state #' + @state_id + ' (' + @state_name + ')'

select
  @city_id = CityID,
  @county_id = CountyID,
  @geo_entity_id = GeoEntityID
from [LBI_Geo].[dbo].City
  where Name = @city_name and StateID = @state_id;

select
  @sizeup_city_id = Id,
  @fips = FIPS,
  @seo_key = SEOKey
from LBISizeUp2.dbo.City
  where LEFT(FIPS,2) = @state_id and Name = @city_name and CityTypeid=15

print 'Found ' + @city_name + ' (FIPS: ' + @fips + '; SEOKey: ' + @seo_key
  + '; CityID: ' + @city_id + '; Sizeup ID: ' + convert(nvarchar(32), @sizeup_city_id) + '; CountyID: ' + @county_id + '; GeoEntityID: ' + convert(nvarchar(36), @geo_entity_id) + ')'

-- TODO some sanity check here. Is the LBI_Geo row in sync with the LBISizeUp2 row?
-- select GeoEntityID, * from LBI_Geo.dbo.City where CityID=@fips

select @client_id = NEWID()
print 'Creating new client with ID ' + convert(nvarchar(36), @client_id)

Insert into [LBISizeUp].[dbo].ClientBoundaries
  (
    ClientBoundary, username, ClientName, byAddress_placeholder, community_placeholder,
    industry_placeholder, dynamic_iframe, show_resources, ClientID
  )
  SELECT
    Geography, @user, @organization, @byAddress_placeholder, @community_placeholder,
    @industry_placeholder, @dynamic_iframe, @show_resources, @client_id
  FROM [LBI_Geo].[dbo].[GeographicEntityGeography] where GeoEntityID=@geo_entity_id;

select * from [LBISizeUp].[dbo].ClientBoundaries where ClientID=@client_id

-- From SIZEUPLBIADDCITYLISTING#2.sql and sizeuplbiadddefaulticty#3.sql

Insert into [LBISizeUp].[dbo].CityListings
  (ClientId, defaultCity, defaultCityId, defaultCityKey, lockcountyPicker)
  VALUES (@client_id, @city_name + ', ' + @state_abbr, @geo_entity_id, @seo_key, 1)

-- From: sizeuplbiaddresourcesa#5.sql

INSERT INTO [LBISizeUp].[dbo].[ClientResourceString] (ClientID)
  VALUES (@client_id);

-- From: sizeuplbiadddcitycountymsa#4.sql

print 'Manual step: Trim trailing comma from the outputs of the two SELECTs below. Then paste the outputs into the UPDATE below.'

-- Also, @city_id is calculated differently. The original query fetched all
-- City rows `WHERE Name LIKE 'Santa Cruz'`. Not sure if wildcards were intended
-- (otherwise why LIKE?) nor what to do with results where state != @state_id.

SELECT '{"label":"' + Name + ', ' + @state_abbr + '", "value":"' + CONVERT(varchar(36), GeoEntityId) + '"},' 
FROM LBI_Geo.dbo.city WHERE fipsclassid='C' and CityID = @city_id
For XML PATH ('')

SELECT '{"label":"' + Name + ' County, ' + @state_abbr + '", "value":"' + CONVERT(varchar(36), GeoEntityId) + '"},' 
FROM LBI_Geo.dbo.county WHERE CountyID = @county_id

-- UPDATE LBISizeUp.dbo.CityListings set
--   citylisting='[{"label":"Long Beach, CA", "value":"976A7D48-B345-4FAD-BC8A-C6A316B3751C"}]',
--   countylisting='[{"label":"Los Angeles County, CA", "value":"94866E7D-93F3-4995-A88C-C2B196056BC3"}]'
-- from LBISizeUp.dbo.CityListings where clientid = @client_id

-- From sizeuplbiaddcitylbilisting#5or#3.5.sql

print 'Manual step: Copy the contents of http://public.lbi.sizeup.com/Api/ClientSetup/GetPlacesByCityIDs/?Id='
  + convert(nvarchar(32), @sizeup_city_id) + ' into the UPDATE below and execute that.'

-- UPDATE [LBISizeUp].[dbo].[CityListings] SET
--   lbicitylisting='[{"Id":2215,"DisplayName":"Long Beach, CA","City":{"Id":2314,"Name":"Long Beach","SEOKey":"long-beach-city","TypeName":"City","Counties":null},"County":{"Id":203,"Name":"Los Angeles","SEOKey":"los-angeles"},"Metro":{"Id":953,"Name":"Los Angeles-Long Beach-Santa Ana, CA","SEOKey":"los-angeles-long-beach-santa-ana-ca"},"State":{"Id":5,"Name":"California","Abbreviation":"CA","SEOKey":"california"},"Region":{"Id":8,"RegionName":"West","Name":"Pacific"}}]'
--   WHERE ClientID = @client_id

print 'Now the site should be available at http://public.lbi.sizeup.com/Home/Begin?myClientID='
  + convert(nvarchar(36), @client_id) + '&isWithoutCookie=True'
