/*
given a city this query
finds the best ranked sectors (2 digit sics) for that city 
then takes the top X sectors where X is the number of sectors it takes to make up 20% of that cities economy
then finds the best ranked industries in those sectors for that city
*/
--sf 2377
--napa 2141
--la 2313
--boston 11438
--detroit 11713
--santa cruz  2380
--las vegas 15611
--emeryville 2371
--salem,ma 11442
-- oakland 2366
-- new york 16916

declare @industryCount int
declare @cityid bigint
declare @percentageOfEconomy decimal

set @percentageOfEconomy = .3
set @industryCount = 100000
set @cityid = 3589



/*

SELECT count(*) +1
from
(
select top 100 percent
a.id
from
TEST_sectorPercentages a,
TEST_sectorPercentages b
where b.id <=a.id and a.cityid = b.cityid and a.cityid = 2141
group by a.cityid,a.id,a.percentage
having sum(b.percentage) < .2
order by a.cityid,a.id
) x

*/







SELECT 
c.name,
s.abbreviation,
data.*
FROM
city c
inner join 
state s
on s.id = c.stateid
cross apply
(
select top(@industryCount)
i.Name,
s.rank as sectorRank,
d.Rank,
d.totalRevenue
 from
(
select top(
select max(v)
from(
SELECT count(*) +1 as v
from
(
select top 100 percent
a.id
from
TEST_sectorPercentages a,
TEST_sectorPercentages b
where b.id <=a.id and a.cityid = b.cityid and a.cityid = c.id
group by a.cityid,a.id,a.percentage
having sum(b.percentage) < @percentageOfEconomy
order by a.cityid,a.id
) x
union all
select 3 as v
)x
)
industryid,
rank
from 
test_sectors s
where s.cityid = c.id
order by s.rank, s.totalRevenue desc
) s
inner join 
(
select 
i2.id as parentId,
i6.*
from
industry i6
inner join industry i2
on left(i6.siccode, 2) = i2.siccode and len(i6.siccode)=6
where i6.isactive = 1
) i
on i.parentid = s.industryid

inner join
test_industries d
on d.industryid = i.id and d.cityid = c.id
order by d.rank, d.totalRevenue desc
) data
where c.id = @cityid

order by c.id










