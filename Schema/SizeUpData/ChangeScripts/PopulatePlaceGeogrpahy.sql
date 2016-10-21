declare @geoId bigint
declare @placeId bigint

DECLARE db_cursor CURSOR FOR  
SELECT placeid 
FROM scratch.dbo.placegeography
 order by placeid

OPEN db_cursor   
FETCH NEXT FROM db_cursor INTO @placeId   

WHILE @@FETCH_STATUS = 0   
BEGIN   


		insert into sizeup2.dbo.geography (sourceid,geography, centerlat, centerlong, north,south,east,west)
       select 
       1 as sourceid,
       geography,
       centerLat,
       centerLong,
       north,
       south,
       east,
       west
       from scratch.dbo.placegeography where placeid = @placeid
       
       select @geoId = @@IDENTITY 
       
       insert into sizeup2.dbo.placegeography (placeid, geographyid,classid)
       select 
       @placeId as placeId,
       @geoId as geographyId,
       2 as classId
       
       print(cast(@geoId as nvarchar) + '    placeid= ' + cast(@placeId as nvarchar))

       FETCH NEXT FROM db_cursor INTO @placeId   
END   

CLOSE db_cursor   
DEALLOCATE db_cursor
