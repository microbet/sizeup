



select 
ind.*,
d.*
 from 

(
select top 20
idn.industryid
from industrydatabynation idn
where idn.year = 2013 and idn.quarter = 1 
order by idn.totalrevenue desc
) ind

cross apply
(
select top 20
c.name + ', ' + s.abbreviation as place,
i.Name,
idc.revenuePercapita
from
city c
inner join state s
on c.stateid = s.id
inner join industrydatabycity idc
on c.id = idc.cityid and idc.year = 2013 and idc.quarter = 1
inner join demographicsbycity dbc
on c.id = dbc.cityid and dbc.year = 2012 and dbc.quarter = 1
inner join industry i
on i.id = idc.industryid
where dbc.totalPopulation > 100000 and i.id = ind.industryid
order by revenuePercapita desc
) d



















