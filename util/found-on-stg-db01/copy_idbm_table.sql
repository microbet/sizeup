DBCC TRACEON(610);
GO

declare @tableName nvarchar(128)
set @tableName = 'IndustryDataByMetro'


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

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByMetroBandMapping_IndustryDataByMetro]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByMetroBandMapping]'))
ALTER TABLE [dbo].[IndustryDataByMetroBandMapping] DROP CONSTRAINT [FK_IndustryDataByMetroBandMapping_IndustryDataByMetro]


IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[IndustryDataByMetro]') AND name = N'PK_IndustryDataByMetro')
ALTER TABLE [dbo].[IndustryDataByMetro] DROP CONSTRAINT [PK_IndustryDataByMetro]
GO

TRUNCATE TABLE LBISizeUpData.dbo.IndustryDataByMetro
GO

SET IDENTITY_INSERT [LBISizeUpData].[dbo].[IndustryDataByMetro] ON 
INSERT INTO [LBISizeUpData].[dbo].[IndustryDataByMetro]
           ([Id]
           ,[Year]
           ,[Quarter]
           ,[MetroId]
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
           ,[MetroId]
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
FROM [GISP-CALC-DB02].[LBINewData].[dbo].[IndustryDataByMetro]
SET IDENTITY_INSERT [LBISizeUpData].[dbo].[IndustryDataByMetro] OFF
GO

SET NOCOUNT ON 

ALTER TABLE [dbo].[IndustryDataByMetro] ADD  CONSTRAINT [PK_IndustryDataByMetro] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]

ALTER TABLE [dbo].[IndustryDataByMetroBandMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByMetroBandMapping_IndustryDataByMetro] FOREIGN KEY([IndustryDataByMetroId])
REFERENCES [dbo].[IndustryDataByMetro] ([Id])

ALTER TABLE [dbo].[IndustryDataByMetroBandMapping] NOCHECK CONSTRAINT [FK_IndustryDataByMetroBandMapping_IndustryDataByMetro]

GO

SET NOCOUNT ON 

declare @tableName nvarchar(128)
set @tableName = 'IndustryDataByMetro'

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