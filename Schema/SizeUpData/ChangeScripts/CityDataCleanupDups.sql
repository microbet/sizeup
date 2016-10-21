
begin transaction

delete data
from
industrydatabycity data
inner join
(
select 
ROW_NUMBER() OVER(PARTITION BY industryid, cityid ORDER BY industryid, cityid) AS Row, 
industryid, 
cityid,
id
 from industrydatabycity 

) x
on x.id = data.id 
where x.row > 1




--commit transaction

--rollback transaction






begin transaction

delete data
from
demographicsbycity data
inner join
(
select 
ROW_NUMBER() OVER(PARTITION BY  cityid ORDER BY  cityid) AS Row, 
cityid,
id
 from demographicsbycity 

) x
on x.id = data.id 
where x.row > 1




--commit transaction

--rollback transaction



















