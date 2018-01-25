


/*


update gl
set gl.seokey = x.seokey
FROM
geographiclocation gl
inner join 
(
select 
c.id,
sizeupdata.dbo.SEOIfy(c.name + ' ' +  ct.seokey + ' ' + s.abbreviation) as seokey
from city c
inner join state s
on s.id = c.stateid
inner join citytype ct
on ct.id = c.citytypeid
inner join citycounty cc
on cc.cityid = c.id

group by c.name, s.abbreviation, ct.seokey, c.id
having count(*) > 1



union all

select 
c.id,
sizeupdata.dbo.SEOIfy(c.name + ' ' +  ct.seokey + ' ' + s.abbreviation + ' ' + co.name) as seokey
from city c
inner join state s
on s.id = c.stateid
inner join citytype ct
on ct.id = c.citytypeid
inner join citycounty cc
on cc.cityid = c.id
inner join  county co
on co.id = cc.countyid

where (select count(*) from citycounty where cityid = c.id) = 1
) x
on gl.id = x.id



*/


select count(*), gl.longname
from geographiclocation gl
inner join granularity g
on g.id = gl.granularityid
where g.name in ('City','County','Metro','State')
group by gl.longname
having count(*) > 1




select * from city c
inner join citytype ct
on ct.id = c.citytypeid

where c.id in (85498,
95716)



update gl
set gl.LongName = gl.longName + ' (City)'
from
geographiclocation gl
where gl.id in(85498,
95716)



/*
if total state = 1 then just city, state
if total state > 1 and totalStateType = 1 then city, state (type)
if total state > 1 and total state type > 1 and totalState == totalStateType then city, state, (county)
if total state > 1 and totalStateType > 1 and totalState > totalStatetype then city, state (type - county)


*/


update gl
set gl.LongName =  
(case 
when byName.cnt = 1 and byNameType.cnt = 1 then c.name + ', ' + s.abbreviation
when byName.cnt > 1 and byNameType.cnt = 1 then c.name + ', ' + s.abbreviation + ' (' + ct.name + ')'
when byName.cnt > 1 and byNameType.cnt > 1 and byName.cnt = byNameType.cnt then c.name + ', ' + s.abbreviation + ' (' + glco.shortname + ')'
when byName.cnt > 1 and byNameType.cnt > 1 and byName.cnt > byNameType.cnt then c.name + ', ' + s.abbreviation + ' (' + ct.name + ' - ' + glco.shortname + ')'
end)

 FROM
 
city c
inner join geographiclocation gl 
on gl.id = c.id
inner join citycounty cc
on cc.cityid = c.id
inner join county co
on co.id = cc.countyid
inner join geographiclocation glco
on glco.id = co.id
inner join citytype ct
on ct.id = c.citytypeid
inner join state s
on s.id = c.stateid
inner join
(
select 
s.name, 
s.stateid,
count(*) as cnt
from geographiclocation gl
inner join city s
on s.id = gl.id
group by s.name, s.stateid
) byName
on byName.name = c.name and byName.stateid = c.stateid
inner join 
(
select 
s.name, 
s.stateid,
ct.name as typename,
count(*) as cnt
from geographiclocation gl
inner join city s
on s.id = gl.id
inner join citytype ct
on ct.id = s.citytypeid
group by s.name, s.stateid,ct.name
) byNameType
on byNameType.name = c.name and byNameType.stateid = c.Stateid and byNameType.typename = ct.name

order by c.name,c.stateid,ct.name

























