
update r
set value = 'Your business generates {{Percentile}} of businesses in your industry in the nation.'
from resourcestring r where name = 'Dashboard.Revenue.Description'



update r
set value = 'Your business started in {{Year}}. In the same year, {{Counts.City}} businesses in your industry started in your city,  {{Counts.County}} started in your county, {{Counts.Metro}} started in your metro area, {{Counts.State}} started in your state,  and {{Counts.Nation}} started nationally.  <br />  <br />  In your industry, your business has been in operation {{Percentiles.City}} of businesses in your city,  {{Percentiles.County}} of businesses in your county, {{#Percentiles.Metro}}{{Percentiles.Metro}} of businesses in your metro area,{{/Percentiles.Metro}}  {{Percentiles.State}} of businesses in your state, and {{Percentiles.Nation}} of businesses in the nation. '
from resourcestring r where name = 'Dashboard.YearStarted.Description'



update r
set value = 'The average salary for all workers in your county for the closest corresponding industry category,  <strong>{{NAICS6.Name}}</strong>, is {{Salary}}. The average salary for your business is {{Percentage}} for your county.  '
from resourcestring r where name = 'Dashboard.Salary.Description'


update r
set value = '  In your industry, your business has {{Percentiles.City}} of businesses in your city,   {{Percentiles.County}} of businesses in your county, {{#Percentiles.Metro}}{{Percentiles.Metro}} of businesses in your metro area,{{/Percentiles.Metro}} {{Percentiles.State}} of businesses   in your state, and {{Percentiles.Nation}} of businesses in the nation.        '
from resourcestring r where name = 'Dashboard.AverageEmployees.Description'




update r
set value = '  Employees per capita measures how many people are employed in a community   for every person living in the community. The employees per capita measure for workers in your industry and in your city is {{Percentiles.County}}     of cities in your county, {{#Percentiles.Metro}}{{Percentiles.Metro}} of cities in your metro area,{{/Percentiles.Metro}} {{Percentiles.State}} of cities     in your state, and {{Percentiles.Nation}} of cities in the nation.  '
from resourcestring r where name = 'Dashboard.EmployeesPerCapita.Description'



update r
set value = '  Cost effectiveness is measured by dividing the annual revenue produced per employee by the average annual worker salary.  The cost effectiveness for your business is {{Percentage}} for your county. The salary data used in this calculation,  which is not available for <strong>{{Industry.Name}}</strong>, comes from the closest corresponding industry category,   <strong>{{NAICS6.Name}}</strong>.  '
from resourcestring r where name = 'Dashboard.CostEffectiveness.Description'



  
update r
set value = '  Revenue per capita is measured by the revenue generated for every person living in the community.  The revenue per capita in your industry in your city is {{Percentiles.County}} of cities  in your county, {{#Percentiles.Metro}}{{Percentiles.Metro}} of cities in your metro area,{{/Percentiles.Metro}} {{Percentiles.State}} of cities in your state,  and {{Percentiles.Nation}} of cities in the nation. '
from resourcestring r where name = 'Dashboard.RevenuePerCapita.Description'


