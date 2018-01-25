MERGE Industry i
USING (
select 
tot.industryid
from
(
SELECT 
b.industryid,
COUNT(*) as cnt
from
Business b (nolock)
where b.IsActive = 1 and b.InBusiness = 1
group by b.industryid
) tot
inner join
(
SELECT 
b.industryid,
COUNT(*) as cnt
from
Business b (nolock)
inner join businessdata bd (nolock)
on bd.industryid = b.industryid and bd.businessid = b.id
inner join nation n (nolock)
on n.id = bd.geographiclocationid
where b.IsActive = 1 and b.InBusiness = 1 and bd.year = 2013 and bd.quarter = 1 and bd.revenue is not null
group by b.industryid
) rev
on tot.industryid = rev.industryid

where tot.cnt > 1000 and rev.cnt > 100


)x
ON i.id = x.industryid
WHEN MATCHED THEN
	update
	set i.isactive = 1

WHEN NOT MATCHED BY SOURCE THEN
  update  
  set i.isactive = 0
;

























