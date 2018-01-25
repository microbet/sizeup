DBCC TRACEON(610)
GO

declare @tableName nvarchar(128)
set @tableName = 'IndustryDataByNation'


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

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByNationBandMapping_IndustryDataByNation]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByNationBandMapping]'))
ALTER TABLE [dbo].[IndustryDataByNationBandMapping] DROP CONSTRAINT [FK_IndustryDataByNationBandMapping_IndustryDataByNation]

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[IndustryDataByNation]') AND name = N'PK_IndustryDataByNation')
ALTER TABLE [dbo].[IndustryDataByNation] DROP CONSTRAINT [PK_IndustryDataByNation]
GO

TRUNCATE TABLE LBISizeUpData.dbo.IndustryDataByNation
GO

SET IDENTITY_INSERT [LBISizeUpData].[dbo].[IndustryDataByNation] ON
INSERT INTO [LBISizeUpData].[dbo].[IndustryDataByNation]
           ([Id]
           ,[Year]
           ,[Quarter]
           ,[IndustryId]
           ,[AverageEmployees]
           ,[EmployeesPerCapita]
           ,[CostEffectiveness]
           ,[MedianRevenue]
           ,[MedianEmployees]
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
SELECT [Id]
      ,[Year]
      ,[Quarter]
      ,[IndustryId]
      ,[AverageEmployees]
      ,[EmployeesPerCapita]
      ,[CostEffectiveness]
      ,[MedianRevenue]
      ,[MedianEmployees]
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
  FROM [GISP-CALC-DB02].[LBINewData].[dbo].[IndustryDataByNation]
SET IDENTITY_INSERT [LBISizeUpData].[dbo].[IndustryDataByNation] OFF
GO

SET NOCOUNT ON 
 
ALTER TABLE [dbo].[IndustryDataByNation] ADD  CONSTRAINT [PK_IndustryDataByNation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]

ALTER TABLE [dbo].[IndustryDataByNationBandMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByNationBandMapping_IndustryDataByNation] FOREIGN KEY([IndustryDataByNationId])
REFERENCES [dbo].[IndustryDataByNation] ([Id])

ALTER TABLE [dbo].[IndustryDataByNationBandMapping] NOCHECK CONSTRAINT [FK_IndustryDataByNationBandMapping_IndustryDataByNation]
GO

declare @tableName nvarchar(128)
set @tableName = 'IndustryDataByNation'


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

