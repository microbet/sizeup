SELECT * FROM
(
SELECT 
email,
sizeupmembership.dbo.GetProfilePropertyValue('FullName',propertynames,propertyvaluesstring) as fullname,
sizeupmembership.dbo.GetProfilePropertyValue('OptOut',propertynames,propertyvaluesstring) as optout,
cityname as city,
state,
industry,
(case 
when oldPercentile > newPercentile then 'lower'
when oldPercentile < newPercentile then 'higher'
else  'same'
end) as change

 FROM
(
SELECT 
email,
propertynames,
propertyvaluesstring,
cityname,
state,
industry,
case when oldTotal != 0 then
round((cast(oldLess as decimal) / cast(oldTotal as decimal))*100, 0)
else null end as oldPercentile,
case when newTotal != 0 then
round((cast(newLess as decimal) / cast(newTotal as decimal))*100,0)
else null end as newPercentile
FROM
(
SELECT 
m.email,
p.propertynames,
p.propertyvaluesstring,
c.Name as cityName,
s.Abbreviation as state,
i.name as industry,
revenue,
i.id as industryid,
c.id as cityid,

(
select count(*) from businessdata bdc
inner join county co
on co.id = bdc.geographiclocationid
where bdc.industryid = i.id   and bdc.year = 2013 and bdc.quarter = 1 
) as oldTotal,
(
select count(*) from businessdata bdc
inner join county co
on co.id = bdc.geographiclocationid
where bdc.industryid = i.id   and bdc.year = 2013 and bdc.quarter = 3
) as newTotal,


(
select count(*) from businessdata bdc
inner join county co
on co.id = bdc.geographiclocationid
where bdc.industryid = i.id   and bdc.year = 2013 and bdc.quarter = 1 
and bdc.revenue is not null and bdc.revenue < ba.revenue
) as oldLess,
(
select count(*) from businessdata bdc
inner join county co
on co.id = bdc.geographiclocationid
where bdc.industryid = i.id  and bdc.year = 2013 and bdc.quarter = 3
and bdc.revenue is not null and bdc.revenue < ba.revenue
) as newLess
 from 
sizeupmembership.dbo.aspnet_Membership m
inner join sizeupmembership.dbo.aspnet_Profile p
on p.userid = m.userid

outer APPLY
        (
        SELECT  TOP 1 placeId, industryId, revenue
        FROM    sizeupanalytics.dbo.BusinessAttributes aba
        WHERE   m.userid = aba.userid and revenue is not null
        ORDER BY aba.timestamp desc
        ) ba
        
left outer join sizeupdata.dbo.industry i
on i.id = ba.industryid
left outer join sizeupdata.dbo.place pl
on pl.id = ba.placeid
left outer join sizeupdata.dbo.city c
on c.id = pl.cityid
left outer join sizeupdata.dbo.state s
on s.id = c.stateid


where m.isapproved = 1

and 
revenue is not null 
and revenue > 25000


) x

) x

) x
where optout != 'False' and change != 'same'

















