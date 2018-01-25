select 
bd.year,
bd.quarter,
count(*) as cnt
 from
businessdata bd
inner join business b
on b.id = bd.businessid
inner join industry i
on i.id = bd.industryid
inner join nation n
on n.id = bd.geographiclocationid
where  b.isactive = 1 and i.isactive =1
group by bd.year, bd.quarter

order by bd.year, bd.quarter












