/** Work in progress */

USE SizeUpAPI_Staging
GO

INSERT INTO APIKey (Name, KeyValue, IsActive, UserName) 
values
('Envision Greater Fond du Lac', '731A626C-2FD0-4295-8ADA-74011B3C7E06', 1, NULL),
('Tarrant County SBDC', '19F846DD-C67B-4633-BBC0-713494F26209', 1, NULL),
('Santa Rosa County, FL', '752A5F55-56B5-49F2-8842-D3709B7748F1', 1, NULL)

declare @apikey bigint
select @apikey = Id from APIKey where KeyValue = '731A626C-2FD0-4295-8ADA-74011B3C7E06'

INSERT INTO ServiceArea
(APIKeyId, GeographicLocationId, GranularityId)
values
(@apikey, 129006, 3),
(@apikey, 129009, 3),
(@apikey, 129021, 3),
(@apikey, 129046, 3),
(@apikey, 129072, 3)

