/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [Id]
      ,[ClientID]
      ,[DashboardSalary]
      ,[DashboardTurnover]
      ,[DashboardRevenue]
      ,[DashboardWorkersComp]
      ,[DashboardAverageEmployees]
      ,[DashboardCostEffectiveness]
      ,[DashboardHealthcare]
  FROM [LBISizeUp].[dbo].[ClientResourceString]
  
  Insert Into  clientresourcestring (clientid)
  values ('DADED6D5-5CB2-44C0-9CEA-1CBD8E01F1D4')
  
 