Rough natural language description of how to get Dun and Bradstreet data into FDG schema.

Create ___GB volume (from snapshot TBD); attach it; format and name dnb-etl; map to E:
WinSCP ftp.dnb.com:gets/*WB* -> E:/WB ; extract (bash unzip is identical to windows unzip, and takes 42 minutes)
Execute create-dnb-table.sql (against RawData)
Launch Package.dtsx (requires VS SQL Server Data Tools 2015); execute task
- (note: 5M rows in 3-4 minutes)

create index on countrycode column with full db: 50 minutes (countrycode still varchar)

select distinct(CountryCode), count(*) as num_rows, max(Country) as name, count(distinct(Country))-1 as errors from DnB____ group by CountryCode: 67 minutes during index creation

select distinct(SIC1), count(*) as num_rows, max(LineOfBusiness) as name, count(distinct(LineOfBusiness))-1 as errors from DnB____ group by SIC1 order by num_rows desc

SELECT *
INTO DnB_UK
FROM DnB
WHERE CountryCode IN ('785','793','797','801');
-- 7 minutes

SELECT *
INTO DnB_Manchester
from DnB_UK
where CityCode='000515'
and SIC1 in (
  5812, 5813, 5411, 5921, 5461, 5499, 5421, 2099, 2051, 5182, 5147, 5149,
  5431, 2082, 5148, 3585, 3556, 2086, 0161, 2033, 2013, 2011, 2841, 2024,
  2026, 2091, 2095, 2015, 2098
);
-- 2 seconds

SELECT distinct(LineOfBusiness),
MAX(SIC1) as Primary_SIC,
COUNT(*) as Number_of_Businesses,
count(distinct(SIC2)) as Unique_2nd_SICs,
count(distinct(SIC3)) as Unique_3rd_SICs,
count(distinct(SIC4)) as Unique_4th_SICs,
count(distinct(SIC5)) as Unique_5th_SICs,
count(distinct(SIC6)) as Unique_6th_SICs
FROM [RawData].[dbo].[DnB_UK]
GROUP BY LineOfBusiness
: 18 seconds

Food service makes a nice industry subset:
select distinct(LineOfBusiness), max(SIC1) from DnB_UK
where SIC1 in (
  5812, 5813, 5411, 5921, 5461, 5499, 5421, 2099, 2051, 5182, 5147, 5149,
  5431, 2082, 5148, 3585, 3556, 2086, 0161, 2033, 2013, 2011, 2841, 2024,
  2026, 2091, 2095, 2015, 2098
)
group by LineOfBusiness



