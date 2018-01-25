

SELeCT 
co.name + ', ' + s.abbreviation,
i.name,
x.*
 from
(
select
idc1.countyid,
idc1.industryid,
idc1.totalrevenue as oldTRev,
idc2.totalrevenue as newTRev,
idc1.averageRevenue as oldARev,
idc2.averageRevenue as newARev,
(cast(idc2.totalrevenue - idc1.totalrevenue as decimal) / cast(idc2.totalRevenue as decimal)) * 100 as percentChangeTR,
(cast(idc2.averageRevenue - idc1.averageRevenue as decimal) / cast(idc2.averageRevenue as decimal)) * 100 as percentChangeAR
 from industrydatabycounty idc1
inner join industrydatabycounty idc2
on idc1.countyid = idc2.countyid and idc1.industryid = idc2.industryid

where idc1.year = 2012 and idc1.quarter = 1 and idc2.year = 2013 and idc2.quarter = 1

) x

inner join county co
on co.id = x.countyid
inner join state s
on s.id = co.stateid
inner join industry i
on i.id = x.industryid

where  co.id = 3011
order by abs(percentChangeTR) desc