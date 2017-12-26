/** See https://sizeup.atlassian.net/browse/TECH-65 */
SELECT
  Country,
  Count(CASE WHEN
    (EmployeesHere > 0 and EmployeesHere < 10)
    or (EmployeesHere = 0 and EmployeesTotal > 0 and EmployeesTotal < 10)
    THEN 1 ELSE NULL END
  ) MicroEmployees,
  Count(CASE WHEN
    (EmployeesHere >= 10 and EmployeesHere < 50)
    or (EmployeesHere = 0 and EmployeesTotal >= 10 and EmployeesTotal < 50)
    THEN 1 ELSE NULL END
  ) SmallEmployees,
  Count(CASE WHEN
    (EmployeesHere >= 50 and EmployeesHere < 250)
    or (EmployeesHere = 0 and EmployeesTotal >= 50 and EmployeesTotal < 250)
    THEN 1 ELSE NULL END
  ) MediumEmployees,
  Count(CASE WHEN
    (EmployeesHere = 0 and EmployeesTotal = 0)
    THEN 1 ELSE NULL END
  ) UnknownEmployees,
  Count(CASE WHEN
    cast(SalesVolumeUSDollars as bigint) > 0 and
    cast(SalesVolumeUSDollars as bigint) < 1680000
    THEN 1 ELSE NULL END
  ) MicroRevenue,
  Count(CASE WHEN
    cast(SalesVolumeUSDollars as bigint) >= 1680000 and
    cast(SalesVolumeUSDollars as bigint) < 8400000
    THEN 1 ELSE NULL END
  ) SmallRevenue,
  Count(CASE WHEN
    cast(SalesVolumeUSDollars as bigint) >= 8400000 and
    cast(SalesVolumeUSDollars as bigint) < 42000000
    THEN 1 ELSE NULL END
  ) MediumRevenue,
  Count(CASE WHEN
    cast(SalesVolumeUSDollars as bigint) = 0
    THEN 1 ELSE NULL END
  ) ZeroKnownRevenue,
  Count(CASE WHEN
    (EmployeesHere >= 250)
    or (EmployeesHere = 0 and EmployeesTotal >= 250)
    THEN 1 ELSE NULL END
  ) OtherEmployees,
  Count(CASE WHEN
    cast(SalesVolumeUSDollars as bigint) >= 42000000
    THEN 1 ELSE NULL END
  ) OtherRevenue,
  Count(*) TotalAllBusinesses
FROM [RawData].[dbo].[DnB]
WHERE CountryCode in (
  '041', -- AUSTRIA
  '009', -- ALBANIA             
  '915', -- BELARUS             
  '910', -- BOSNIA-HERZEGOVINA  
  '100', -- BULGARIA            
  '179', -- CROATIA             
  '190', -- CZECH REPUBLIC      
  '325', -- HUNGARY             
  '392', -- KOSOVO              
  '597', -- POLAND              
  '620', -- ROMANIA             
  '622', -- RUSSIAN FEDERATION  
  '663', -- SERBIA              
  '680', -- SLOVAKIA            
  '771', -- UKRAINE             
  -- UK countries included for comparison with small data set results
  '785', -- ENGLAND
  '793', -- NORTHERN IRELAND
  '797', -- SCOTLAND
  '801' -- WALES
)
group by Country

