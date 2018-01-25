DBCC TRACEON(610);
GO

declare @tableName nvarchar(128)
set @tableName = 'IndustryDataByCounty'


DECLARE @sql AS VARCHAR(MAX)='';
SELECT @sql = @sql + 
'ALTER INDEX ' + i.Name + ' ON  ' + t.name + ' DISABLE;' +CHAR(13)+CHAR(10)
FROM 
    sys.indexes i
JOIN 
    sys.objects t
    ON i.object_id = t.object_id
WHERE i.type_desc = 'NONCLUSTERED'
  AND t.type_desc = 'USER_TABLE'
  and t.name = @tableName

exec(@sql);
GO

SET NOCOUNT ON

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByCountyBandMapping_IndustryDataByCounty]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByCountyBandMapping]'))
ALTER TABLE [dbo].[IndustryDataByCountyBandMapping] DROP CONSTRAINT [FK_IndustryDataByCountyBandMapping_IndustryDataByCounty]

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[IndustryDataByCounty]') AND name = N'PK_IndustryDataByCounty')
ALTER TABLE [dbo].[IndustryDataByCounty] DROP CONSTRAINT [PK_IndustryDataByCounty]
GO

TRUNCATE TABLE LBISizeUpData.dbo.IndustryDataByCounty
GO

SET IDENTITY_INSERT [LBISizeUpData].[dbo].[IndustryDataByCounty] ON 
INSERT INTO [LBISizeUpData].[dbo].[IndustryDataByCounty]
           ([Id] 
           ,[Year]
           ,[Quarter]
           ,[CountyId]
           ,[IndustryId]
           ,[AverageEmployees]
           ,[EmployeesPerCapita]
           ,[CostEffectiveness]
           ,[TotalRevenue]
           ,[AverageRevenue]
           ,[TotalEmployees]
           ,[RevenuePerCapita]
           ,[AverageAnnualSalary]
           ,[Hires]
           ,[Separations]
           ,[TurnoverRate]
           ,[JobGains]
           ,[JobLosses]
           ,[NetJobChange]
           ,[Employment])
SELECT [Id],[Year]
           ,[Quarter]
           ,[CountyId]
           ,[IndustryId]
           ,[AverageEmployees]
           ,[EmployeesPerCapita]
           ,[CostEffectiveness]
           ,[TotalRevenue]
           ,[AverageRevenue]
           ,[TotalEmployees]
           ,[RevenuePerCapita]
           ,[AverageAnnualSalary]
           ,[Hires]
           ,[Separations]
           ,[TurnoverRate]
           ,[JobGains]
           ,[JobLosses]
           ,[NetJobChange]
           ,[Employment]
FROM [GISP-CALC-DB02].[LBINewData].[dbo].[IndustryDataByCounty]

SET IDENTITY_INSERT [LBISizeUpData].[dbo].[IndustryDataByCounty] OFF
GO


SET NOCOUNT ON 

ALTER TABLE [dbo].[IndustryDataByCounty] ADD  CONSTRAINT [PK_IndustryDataByCounty] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]


ALTER TABLE [dbo].[IndustryDataByCountyBandMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByCountyBandMapping_IndustryDataByCounty] FOREIGN KEY([IndustryDataByCountyId])
REFERENCES [dbo].[IndustryDataByCounty] ([Id])


ALTER TABLE [dbo].[IndustryDataByCountyBandMapping] NOCHECK CONSTRAINT [FK_IndustryDataByCountyBandMapping_IndustryDataByCounty]
GO

declare @tableName nvarchar(128)
set @tableName = 'IndustryDataByCounty'

DECLARE @sql AS VARCHAR(MAX)='';
SELECT @sql = @sql + 
'ALTER INDEX ' + i.Name + ' ON  ' + t.name + ' REBUILD;' +CHAR(13)+CHAR(10)
FROM 
    sys.indexes i
JOIN 
    sys.objects t
    ON i.object_id = t.object_id
WHERE i.type_desc = 'NONCLUSTERED'
  AND t.type_desc = 'USER_TABLE'
  and t.name = @tableName

exec(@sql);
GO
