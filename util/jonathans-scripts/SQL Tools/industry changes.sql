


SELECT 
i.name,
i.siccode,
i.id,
case when new.isactive = 0 then 'Went inactive' else 'Went Active' end as wahappen,
new.cnt as businesses,
new.revCnt as busnessesWithRev
FROM
(
select 
bd.industryid,
count(*) as cnt,
count(bd.revenue) as revCnt,
case when count(*) >= 1000 and count(bd.revenue) >=100 then 1 else 0 end as isActive
 from
 businessdata bd
inner join business b
on b.id = bd.businessid
inner join nation n
on n.id = bd.geographiclocationid
where bd.year = 2013 and bd.quarter = 1 and b.isactive = 1 and b.inbusiness =1
group by bd.industryid
) old
inner join
(
select 
bd.industryid,
count(*) as cnt,
count(bd.revenue) as revCnt,
case when count(*) >= 1000 and count(bd.revenue) >=100 then 1 else 0 end as isActive
 from
 businessdata bd
inner join business b
on b.id = bd.businessid
inner join nation n
on n.id = bd.geographiclocationid
where bd.year = 2013 and bd.quarter = 3 and b.isactive = 1 and b.inbusiness =1
group by bd.industryid
) new
on old.industryid = new.industryid
inner join industry i
on i.id= old.industryid
where old.isactive != new.isactive


























