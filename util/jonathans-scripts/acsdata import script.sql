--insert into [GISP-STG-DB01.GISPLANNING.NET].zpdcdata.dbo.demographics (year, geographiclocationid, variableid, value)
SELECT 
data.year,
p.id,
data.variableid,
data.value
 FROM
(
select
id,
fips
from
[gisp-stg-db01.gisplanning.net].zpdcdata.dbo.city

union all

select
id,
fips
from
[gisp-stg-db01.gisplanning.net].zpdcdata.dbo.county

union all

select
id,
fips
from
[gisp-stg-db01.gisplanning.net].zpdcdata.dbo.metro

union all

select
id,
fips
from
[gisp-stg-db01.gisplanning.net].zpdcdata.dbo.state

union all

select
id,
''
from
[gisp-stg-db01.gisplanning.net].zpdcdata.dbo.nation

) p
inner join
(
--young enducated
SELECT * FROM
(
select 
v.id as variableid,
piv.year,
piv.geoid,
(coalesce(piv.vd17,0) + coalesce(piv.vd18,0) + coalesce(piv.vd58,0) + coalesce(piv.vd59,0)) / nullif(piv.vd01,0) as value
 from 
 (select YEAR, variable, geoid, tableid, hd01  from ACSDAta ) as sourceDAta
 PIVOT
(
max(hd01)
FOR Variable IN (vd01 , vd17, vd18,vd58, vd59)
) piv
		 
inner join [gisp-stg-db01.gisplanning.net].zpdcdata.dbo.variable v
on v.[key] = 'youngAndEducated' and piv.tableid = 'B15001'

) x
where value is not null and value > 0


union all

--very creative percent

SELECT * FROM
(
select 
v.id as variableid,
piv.year,
piv.geoid,
(coalesce(piv.VD20,0) + coalesce(piv.VD27,0) + coalesce(piv.VD31,0) + coalesce(piv.VD43,0) + coalesce(piv.VD52,0) + coalesce(piv.VD171,0) + coalesce(piv.VD178,0) + coalesce(piv.VD182,0) + coalesce(piv.VD194,0) + coalesce(piv.VD203,0) ) / nullif(piv.vd01,0) as value
 
 from 
 (select YEAR, variable, geoid, tableid, hd01  from ACSDAta ) as sourceDAta
 PIVOT
(
max(hd01)
FOR Variable IN (vd01 , vd20, vd27,VD31, VD43, VD52, VD171, VD178,VD182, VD194, VD203)
) piv

inner join [gisp-stg-db01.gisplanning.net].zpdcdata.dbo.variable v
on v.[key] = 'veryCreativeProfessionals' and piv.tableid = 'B24010'
) x
where value is not null and value > 0


--creative percent


union all


SELECT * FROM
(
select 
v.id as variableid,
piv.year,
piv.geoid,
(coalesce(piv.VD05,0) + coalesce(piv.VD13,0) + coalesce(piv.VD39,0) + coalesce(piv.VD156,0) + coalesce(piv.VD164,0) + coalesce(piv.VD190,0)  ) / nullif(piv.vd01,0) as value
 
 from 
 (select YEAR, variable, geoid, tableid, hd01  from ACSDAta ) as sourceDAta
 PIVOT
(
max(hd01)
FOR Variable IN (vd01 , VD05, VD13,VD39, VD156, VD164, VD190)
) piv

inner join [gisp-stg-db01.gisplanning.net].zpdcdata.dbo.variable v
on v.[key] = 'creativeProfessionals' and piv.tableid = 'B24010'

) x
where value is not null and value > 0




union all 


SELECT * FROM
(
select 
v.id as variableid,
piv.year,
piv.geoid,
nullif(piv.vd01,0) as value
 
 from 
 (select YEAR, variable, geoid, tableid, hd01  from ACSDAta ) as sourceDAta
 PIVOT
(
max(hd01)
FOR Variable IN (vd01)
) piv

inner join [gisp-stg-db01.gisplanning.net].zpdcdata.dbo.variable v
on v.[key] = 'homeValueMedian' and piv.tableid = 'B25077'

) x
where value is not null and value > 0


union all


SELECT * FROM
(
select 
v.id as variableid,
piv.year,
piv.geoid,
(coalesce(piv.VD30,0) + coalesce(piv.VD31,0)  ) / nullif(piv.vd01,0) as value
 
 from 
 (select YEAR, variable, geoid, tableid, hd01  from ACSDAta ) as sourceDAta
 PIVOT
(
max(hd01)
FOR Variable IN (vd01 , VD30, VD31)
) piv

inner join [gisp-stg-db01.gisplanning.net].zpdcdata.dbo.variable v
on v.[key] = 'internationalTalent' and piv.tableid = 'B06009'

) x
where value is not null and value > 0


union all


	
SELECT * FROM
(
select 
v.id as variableid,
piv.year,
piv.geoid,
 nullif(piv.VC55,0) as value
 
 from 
 (select YEAR, variable, geoid, tableid, hd01  from ACSDAta ) as sourceDAta
 PIVOT
(
max(hd01)
FOR Variable IN (VC55 )
) piv

inner join [gisp-stg-db01.gisplanning.net].zpdcdata.dbo.variable v
on v.[key] = 'commuteTravelTime' and piv.tableid = 'S0801'

) x
where value is not null and value > 0



union all


	
	
SELECT * FROM
(
select 
v.id as variableid,
piv.year,
piv.geoid,
(coalesce(piv.VD02,0)   ) / nullif(piv2.vd01,0) as value
 
 from 
 (select YEAR, variable, geoid, tableid, hd01  from ACSDAta ) as sourceDAta
 PIVOT
(
max(hd01)
FOR Variable IN (VD02)
) piv
inner join

 (select YEAR, variable, geoid, tableid, hd01  from ACSDAta ) as sourceDAta
 PIVOT
(
max(hd01)
FOR Variable IN (VD01)
) piv2
on  piv.tableid = 'B25103' and piv2.tableid = 'B25077' and piv.year = piv2.year and piv.geoid = piv2.geoid
inner join [gisp-stg-db01.gisplanning.net].zpdcdata.dbo.variable v
on v.[key] = 'propertyTax' 

) x
where value is not null and value > 0


) data
on data.geoid = p.fips


