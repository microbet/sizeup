--truncate table industrydataband

insert into IndustryDataBand(industrydataid, bandid)
SELECT
IndustryDataId,
BandId
FROM
(
select
id.year,
id.quarter,
id.id as IndustryDataId,
bands.id as BandId
from 
dbo.IndustryData id
inner join 
(
SELECT 
b.id,
a.name,
b.min,
b.max
FROM
band b
inner join attribute a
on a.id = b.attributeid
) bands
on (id.TotalEmployees >= bands.min and id.TotalEmployees <= bands.max) and bands.name = 'TotalEmployees'



union all



select
id.year,
id.quarter,
id.id as IndustryDataId,
bands.id as BandId
from 
dbo.IndustryData id
inner join 
(
SELECT 
b.id,
a.name,
b.min,
b.max
FROM
band b
inner join attribute a
on a.id = b.attributeid
) bands
on id.AverageEmployees >= bands.min and id.AverageEmployees <= bands.max and bands.name = 'AverageEmployees'



union all



select
id.year,
id.quarter,
id.id as IndustryDataId,
bands.id as BandId
from 
dbo.IndustryData id
inner join 
(
SELECT 
b.id,
a.name,
b.min,
b.max
FROM
band b
inner join attribute a
on a.id = b.attributeid
) bands
on id.TotalOperatingCost > bands.min and id.TotalOperatingCost <=bands.max and bands.name = 'TotalOperatingCost'


union all



select
id.year,
id.quarter,
id.id as IndustryDataId,
bands.id as BandId
from 
dbo.IndustryData id
inner join 
(
SELECT 
b.id,
a.name,
b.min,
b.max
FROM
band b
inner join attribute a
on a.id = b.attributeid
) bands
on id.AverageOperatingCost > bands.min and id.AverageOperatingCost <=bands.max and bands.name = 'AverageOperatingCost'


union all



select
id.year,
id.quarter,
id.id as IndustryDataId,
bands.id as BandId
from 
dbo.IndustryData id
inner join 
(
SELECT 
b.id,
a.name,
b.min,
b.max
FROM
band b
inner join attribute a
on a.id = b.attributeid
) bands
on id.TotalNetProfit > bands.min and id.TotalNetProfit <=bands.max and bands.name = 'TotalNetProfit'


union all



select
id.year,
id.quarter,
id.id as IndustryDataId,
bands.id as BandId
from 
dbo.IndustryData id
inner join 
(
SELECT 
b.id,
a.name,
b.min,
b.max
FROM
band b
inner join attribute a
on a.id = b.attributeid
) bands
on id.AverageNetProfit > bands.min and id.AverageNetProfit <=bands.max and bands.name = 'AverageNetProfit'


union all



select
id.year,
id.quarter,
id.id as IndustryDataId,
bands.id as BandId
from 
dbo.IndustryData id
inner join 
(
SELECT 
b.id,
a.name,
b.min,
b.max
FROM
band b
inner join attribute a
on a.id = b.attributeid
) bands
on id.TotalDebtEquityRatio > bands.min and id.TotalDebtEquityRatio <=bands.max and bands.name = 'TotalDebtEquityRatio'


union all



select
id.year,
id.quarter,
id.id as IndustryDataId,
bands.id as BandId
from 
dbo.IndustryData id
inner join 
(
SELECT 
b.id,
a.name,
b.min,
b.max
FROM
band b
inner join attribute a
on a.id = b.attributeid
) bands
on id.AverageDebtEquityRatio > bands.min and id.AverageDebtEquityRatio <=bands.max and bands.name = 'AverageDebtEquityRatio'


union all



select
id.year,
id.quarter,
id.id as IndustryDataId,
bands.id as BandId
from 
dbo.IndustryData id
inner join 
(
SELECT 
b.id,
a.name,
b.min,
b.max
FROM
band b
inner join attribute a
on a.id = b.attributeid
) bands
on id.TotalNetWorth > bands.min and id.TotalNetWorth <=bands.max and bands.name = 'TotalNetWorth'



union all



select
id.year,
id.quarter,
id.id as IndustryDataId,
bands.id as BandId
from 
dbo.IndustryData id
inner join 
(
SELECT 
b.id,
a.name,
b.min,
b.max
FROM
band b
inner join attribute a
on a.id = b.attributeid
) bands
on id.AverageNetWorth > bands.min and id.AverageNetWorth <=bands.max and bands.name = 'AverageNetWorth'

union all



select
id.year,
id.quarter,
id.id as IndustryDataId,
bands.id as BandId
from 
dbo.IndustryData id
inner join 
(
SELECT 
b.id,
a.name,
b.min,
b.max
FROM
band b
inner join attribute a
on a.id = b.attributeid
) bands
on
ROUND(id.EmployeesPerCapita,  CAST( cast(3 as float(53)) - (FLOOR(LOG10(ABS(id.EmployeesPerCapita)))) AS int)) > bands.min and 
ROUND(id.EmployeesPerCapita,  CAST( cast(3 as float(53)) - (FLOOR(LOG10(ABS(id.EmployeesPerCapita)))) AS int)) <=bands.max and
 bands.name = 'EmployeesPerCapita'

where id.employeesPerCapita > 0 and id.employeesPErCapita is not null



union all



select
id.year,
id.quarter,
id.id as IndustryDataId,
bands.id as BandId
from 
dbo.IndustryData id
inner join 
(
SELECT 
b.id,
a.name,
b.min,
b.max
FROM
band b
inner join attribute a
on a.id = b.attributeid
) bands
on id.TotalRevenue > bands.min and id.TotalRevenue <=bands.max and bands.name = 'TotalRevenue'





union all



select
id.year,
id.quarter,
id.id as IndustryDataId,
bands.id as BandId
from 
dbo.IndustryData id
inner join 
(
SELECT 
b.id,
a.name,
b.min,
b.max
FROM
band b
inner join attribute a
on a.id = b.attributeid
) bands 
on id.AverageRevenue > bands.min and id.AverageRevenue <=bands.max and bands.name = 'AverageRevenue'
--CAST( ROUND( CAST( id.[AverageRevenue] AS float), -3) AS bigint) > bands.min and 
--CAST( ROUND( CAST( id.[AverageRevenue] AS float), -3) AS bigint) <=bands.max and
--bands.name = 'AverageRevenue'



union all



select
id.year,
id.quarter,
id.id as IndustryDataId,
bands.id as BandId
from 
dbo.IndustryData id
inner join 
(
SELECT 
b.id,
a.name,
b.min,
b.max
FROM
band b
inner join attribute a
on a.id = b.attributeid
) bands
on id.RevenuePerCapita >= bands.min and id.RevenuePerCapita <bands.max and bands.name = 'RevenuePerCapita'




union all

select
id.year,
id.quarter,
id.id as IndustryDataId,
bands.id as BandId
from 
dbo.IndustryData id
inner join 
(
SELECT 
b.id,
a.name,
b.min,
b.max
FROM
band b
inner join attribute a
on a.id = b.attributeid
) bands
on
ROUND(id.CostEffectiveness,  CAST( cast(3 as float(53)) - (FLOOR(LOG10(ABS(id.CostEffectiveness)))) AS int)) >= bands.min and 
ROUND(id.CostEffectiveness,  CAST( cast(3 as float(53)) - (FLOOR(LOG10(ABS(id.CostEffectiveness)))) AS int)) <=bands.max and
 bands.name = 'CostEffectiveness'

where id.CostEffectiveness > 0 and id.CostEffectiveness is not null





union all



select
id.year,
id.quarter,
id.id as IndustryDataId,
bands.id as BandId
from 
dbo.IndustryData id
inner join 
(
SELECT 
b.id,
a.name,
b.min,
b.max
FROM
band b
inner join attribute a
on a.id = b.attributeid
) bands
on id.AverageAnnualSalary > bands.min and id.AverageAnnualSalary <=bands.max and bands.name = 'AverageAnnualSalary'

) x
where year= 2015 and quarter = 1