--truncate table industrydata

insert into RawDataIt2.dbo.IndustryDataAggregated (
Year,
Quarter,
GeographicLocationId,
IndustryId,
TotalEmployees,
averageEmployees,
totalrevenue, 
averagerevenue,
businesscount,
totaloperatingcost, 
averageoperatingcost,
totalnetprofit,
averagenetprofit,
totaldebtequityratio, 
averagedebtequityratio,
totalnetworth,
averagenetworth, 
medianrevenue,
medianemployees, 
averageannualsalary,
employeespercapita,
revenuepercapita,
costeffectiveness
)
SELECT 
bd.year,
bd.quarter,
bd.geographiclocationid,
bd.industryid,
bd.totalEmployees,
bd.AverageEmployees,
bd.TotalRevenue,
bd.AverageRevenue,
bd.Businesscount,
bd.TotalOperatingCost,
bd.AverageOperatingCost,
bd.TotalNetProfit,
bd.AverageNetProfit,
bd.TotalDebtEquityRatio,
bd.AverageDebtEquityRatio,
bd.TotalNetWorth,
bd.AverageNetWorth,
medrev.MedianRevenue,
medemp.MedianEmployees,
bd.AverageAnnualSalary,
bd.EmployeesPerCapita,
bd.RevenuePercapita,

(case
when coalesce(bd.TotalEmployees * bd.AverageAnnualSalary ,0) = 0 then null
else  bd.TotalRevenue / cast(bd.TotalEmployees * bd.AverageAnnualSalary as float)
end) as CostEffectiveness

FROM
(
SELECT 
bd.year,
bd.quarter,
bd.geographiclocationid,
bd.industryid,
(select Sum(NULLIF(employees,0)) from BusinessData where geographiclocationid = bd.geographiclocationid and IndustryId in (select i.Id from Industry i cross join (select cervedateco from Industry where ID = bd.industryid) x where i.CervedAteco like x.CervedAteco + '%')) as TotalEmployees,
(select Avg(NULLIF(Employees,0)) from BusinessData where geographiclocationid = bd.geographiclocationid and IndustryId in (select i.Id from Industry i cross join (select cervedateco from Industry where ID = bd.industryid) x where i.CervedAteco like x.CervedAteco + '%')) as AverageEmployees,
(select Sum(NULLIF(Revenue,0)) from BusinessData where geographiclocationid = bd.geographiclocationid and IndustryId in (select i.Id from Industry i cross join (select cervedateco from Industry where ID = bd.industryid) x where i.CervedAteco like x.CervedAteco + '%')) as TotalRevenue,
(select Avg(NULLIF(Revenue,0)) from BusinessData where geographiclocationid = bd.geographiclocationid and IndustryId in (select i.Id from Industry i cross join (select cervedateco from Industry where ID = bd.industryid) x where i.CervedAteco like x.CervedAteco + '%')) as AverageRevenue,
(select Sum(NULLIF(operatingcost,0)) from BusinessData where geographiclocationid = bd.geographiclocationid and IndustryId in (select i.Id from Industry i cross join (select cervedateco from Industry where ID = bd.industryid) x where i.CervedAteco like x.CervedAteco + '%')) as TotalOperatingCost,
(select Avg(NULLIF(operatingcost,0)) from BusinessData where geographiclocationid = bd.geographiclocationid and IndustryId in (select i.Id from Industry i cross join (select cervedateco from Industry where ID = bd.industryid) x where i.CervedAteco like x.CervedAteco + '%')) as AverageOperatingCost,
(select Sum(NULLIF(netprofit,0)) from BusinessData where geographiclocationid = bd.geographiclocationid and IndustryId in (select i.Id from Industry i cross join (select cervedateco from Industry where ID = bd.industryid) x where i.CervedAteco like x.CervedAteco + '%')) as TotalNetProfit,
(select Avg(NULLIF(netprofit,0)) from BusinessData where geographiclocationid = bd.geographiclocationid and IndustryId in (select i.Id from Industry i cross join (select cervedateco from Industry where ID = bd.industryid) x where i.CervedAteco like x.CervedAteco + '%')) as AverageNetProfit,
(select Sum(NULLIF(debtequityratio/100,0)) from BusinessData where geographiclocationid = bd.geographiclocationid and IndustryId in (select i.Id from Industry i cross join (select cervedateco from Industry where ID = bd.industryid) x where i.CervedAteco like x.CervedAteco + '%')) as TotalDebtEquityRatio,
(select Avg(NULLIF(debtequityratio/100,0)) from BusinessData where geographiclocationid = bd.geographiclocationid and IndustryId in (select i.Id from Industry i cross join (select cervedateco from Industry where ID = bd.industryid) x where i.CervedAteco like x.CervedAteco + '%')) as AverageDebtEquityRatio,
(select Sum(NULLIF(networth,0)) from BusinessData where geographiclocationid = bd.geographiclocationid and IndustryId in (select i.Id from Industry i cross join (select cervedateco from Industry where ID = bd.industryid) x where i.CervedAteco like x.CervedAteco + '%')) as TotalNetWorth,
(select Avg(NULLIF(networth,0)) from BusinessData where geographiclocationid = bd.geographiclocationid and IndustryId in (select i.Id from Industry i cross join (select cervedateco from Industry where ID = bd.industryid) x where i.CervedAteco like x.CervedAteco + '%')) as AverageNetWorth,
(select AVG(costofpersonnel/employees) from BusinessData where geographiclocationid = bd.geographiclocationid and IndustryId in (select i.Id from Industry i cross join (select cervedateco from Industry where ID = bd.industryid) x where i.CervedAteco like x.CervedAteco + '%')) as AverageAnnualSalary,

count(*) as Businesscount,

(case
when coalesce(pop.Total,0) = 0 then null
else  cast((select Sum(employees) from BusinessData where geographiclocationid = bd.geographiclocationid and IndustryId in (select id from industry where id like CAST(bd.industryid as varchar) + '%')) as float) /cast(pop.Total as float)
end) as EmployeesPerCapita,

(case
when coalesce(pop.Total,0) = 0 then 0
else  cast((select Sum(Revenue) from BusinessData where geographiclocationid = bd.geographiclocationid and IndustryId in (select id from industry where id like CAST(bd.industryid as varchar) + '%')) /pop.Total as bigint)
end) as RevenuePerCapita




FROM
geographiclocation gl(NOLOCK)
inner join BusinessData bd(NOLOCK)
on bd.geographiclocationid = gl.id
outer apply
(
SELECT top 1 total
from Demographics d(NOLOCK)
where gl.id = d.geographiclocationid
order by YEAR, quarter
) pop

where bd.year = 2015 and bd.quarter = 1
group by bd.year,bd.quarter,bd.geographiclocationid,bd.industryid,pop.total
) bd

left outer join
(
SELECT
	YEAR,
	quarter,
   industryid,
   geographiclocationid,
   AVG(Revenue) as MedianRevenue
FROM
(
  SELECT
	YEAR,
	quarter,
      industryid,
      geographiclocationid,
      Revenue,
      ROW_NUMBER() OVER (
         PARTITION BY industryid, geographiclocationid, year, quarter 
         ORDER BY Revenue ASC, id asc) AS RowAsc,
      ROW_NUMBER() OVER (
         PARTITION BY industryid , geographiclocationid, year, quarter 
         ORDER BY Revenue DESC, id desc) AS RowDesc
   FROM 
   (
		SELECT
		b.id,
		b.YEAR,
		b.quarter,
		b.geographiclocationid,
		b.industryid,
		b.revenue
		FROM
		businessdata b (NOLOCK)
		inner join industry i (NOLOCK)
		on b.industryid = i.Id
		where b.industryid in (select i.Id from Industry i cross join (select cervedateco from Industry where ID = b.industryid) x where i.CervedAteco like x.CervedAteco + '%')
	) ind
) x
WHERE 
   RowAsc IN (RowDesc, RowDesc - 1, RowDesc + 1)
GROUP BY 
industryid, geographiclocationid, YEAR, quarter

) medrev
on medrev.year = bd.year and medrev.quarter = bd.quarter and medrev.industryid = bd.industryid and medrev.geographiclocationid = bd.geographiclocationid
left outer join
(
SELECT
	YEAR,
	quarter,
   industryid,
   geographiclocationid,
   AVG(Employees) as MedianEmployees
FROM
(
  SELECT
	YEAR,
	quarter,
      industryid,
      geographiclocationid,
      Employees,
      ROW_NUMBER() OVER (
         PARTITION BY industryid, geographiclocationid, year, quarter 
         ORDER BY Employees ASC, id asc) AS RowAsc,
      ROW_NUMBER() OVER (
         PARTITION BY industryid , geographiclocationid, year, quarter 
         ORDER BY Employees DESC, id desc) AS RowDesc
   FROM 
   (
		SELECT
		b.id,
		b.YEAR,
		b.quarter,
		b.geographiclocationid,
		b.industryid,
		b.Employees
		FROM
		businessdata b (NOLOCK)
		inner join industry i (NOLOCK)
		on b.industryid = i.Id
		where b.industryid in (select i.Id from Industry i cross join (select cervedateco from Industry where ID = b.industryid) x where i.CervedAteco like x.CervedAteco + '%')
	) ind
) x
WHERE 
   RowAsc IN (RowDesc, RowDesc - 1, RowDesc + 1)
GROUP BY 
industryid, geographiclocationid, YEAR, quarter

) medemp
on medemp.year = bd.year and medemp.quarter = bd.quarter and medemp.industryid = bd.industryid and medemp.geographiclocationid = bd.geographiclocationid

