declare @placeid bigint
declare @industryid bigint

set @placeid = 3051
set @industryid = 11268


select 
revenue,
year, 
quarter,
b.*
 from businessdatabycounty bdc
inner join county co
on co.id = bdc.countyid
left outer join metro m
on m.id = co.metroid
inner join business b
on b.id = bdc.businessid
inner join citycountymapping ccm
on ccm.countyid = co.id
	

where ccm.id = @placeid and bdc.industryid = @industryid

order by  year desc, quarter







select 
count(*) as cnt,
year, 
quarter
 from businessdatabycounty bdc
inner join county co
on co.id = bdc.countyid
left outer join metro m
on m.id = co.metroid
inner join business b
on b.id = bdc.businessid
inner join citycountymapping ccm
on ccm.countyid = co.id
	

where ccm.id = @placeid and bdc.industryid = @industryid 

group by year, quarter

