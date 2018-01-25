DBCC TRACEON(610);
GO

declare @tableName nvarchar(128)
set @tableName = 'IndustryDataByCity'


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

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByCityBandMapping_IndustryDataByCity]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByCityBandMapping]'))
ALTER TABLE [dbo].[IndustryDataByCityBandMapping] DROP CONSTRAINT [FK_IndustryDataByCityBandMapping_IndustryDataByCity]

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[IndustryDataByCity]') AND name = N'PK_IndustryDataByCity')
ALTER TABLE [dbo].[IndustryDataByCity] DROP CONSTRAINT [PK_IndustryDataByCity]
GO

TRUNCATE TABLE LBISizeUpData.dbo.IndustryDataByCity
GO

SET IDENTITY_INSERT [LBISizeUpData].[dbo].[IndustryDataByCity] ON 
INSERT INTO [LBISizeUpData].[dbo].[IndustryDataByCity]
           ([Id] 
           ,[Year]
           ,[Quarter]
           ,[CityId]
           ,[IndustryId]
           ,[AverageEmployees]
           ,[EmployeesPerCapita]
           ,[TotalRevenue]
           ,[AverageRevenue]
           ,[TotalEmployees]
           ,[RevenuePerCapita])
SELECT [Id] 
           ,[Year]
           ,[Quarter]
           ,[CityId]
           ,[IndustryId]
           ,[AverageEmployees]
           ,[EmployeesPerCapita]
           ,[TotalRevenue]
           ,[AverageRevenue]
           ,[TotalEmployees]
           ,[RevenuePerCapita]
FROM [GISP-CALC-DB02].[LBINewData].[dbo].[IndustryDataByCity]

SET IDENTITY_INSERT [LBISizeUpData].[dbo].[IndustryDataByCity] OFF
GO


SET NOCOUNT ON 

ALTER TABLE [dbo].[IndustryDataByCity] ADD  CONSTRAINT [PK_IndustryDataByCity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]


ALTER TABLE [dbo].[IndustryDataByCityBandMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByCityBandMapping_IndustryDataByCity] FOREIGN KEY([IndustryDataByCityId])
REFERENCES [dbo].[IndustryDataByCity] ([Id])


ALTER TABLE [dbo].[IndustryDataByCityBandMapping] NOCHECK CONSTRAINT [FK_IndustryDataByCityBandMapping_IndustryDataByCity]
GO

declare @tableName nvarchar(128)
set @tableName = 'IndustryDataByCity'

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
