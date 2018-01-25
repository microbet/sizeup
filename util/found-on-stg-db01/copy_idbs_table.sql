DBCC TRACEON(610)
GO

declare @tableName nvarchar(128)
set @tableName = 'IndustryDataByState'


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

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByStateBandMapping_IndustryDataByState]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByStateBandMapping]'))
ALTER TABLE [dbo].[IndustryDataByStateBandMapping] DROP CONSTRAINT [FK_IndustryDataByStateBandMapping_IndustryDataByState]

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[IndustryDataByState]') AND name = N'PK_IndustryDataByState')
ALTER TABLE [dbo].[IndustryDataByState] DROP CONSTRAINT [PK_IndustryDataByState]
GO

TRUNCATE TABLE LBISizeUpData.dbo.IndustryDataByState
GO

SET IDENTITY_INSERT [LBISizeUpData].[dbo].[IndustryDataByState] ON
INSERT INTO [LBISizeUpData].[dbo].[IndustryDataByState]
           ([Id]
           ,[Year]
           ,[Quarter]
           ,[StateId]
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
           ,[WorkersCompRank]
           ,[WorkersComp]
           ,[HealthcareByState]
           ,[Healthcare0To9Employees]
           ,[Healthcare10To24Employees]
           ,[Healthcare25To99Employees]
           ,[Healthcare100To999Employees]
           ,[Healthcare1000orMoreEmployees]
           ,[HealthcareByIndustry]
           ,[HealthcareByStateRank]
           ,[Healthcare0To9EmployeesRank]
           ,[Healthcare10To24EmployeesRank]
           ,[Healthcare25To99EmployeesRank]
           ,[Healthcare100To999EmployeesRank]
           ,[Healthcare1000orMoreEmployeesRank]
           ,[HealthcareByIndustryRank])
SELECT [Id],[Year]
           ,[Quarter]
           ,[StateId]
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
           ,[WorkersCompRank]
           ,[WorkersComp]
           ,[HealthcareByState]
           ,[Healthcare0To9Employees]
           ,[Healthcare10To24Employees]
           ,[Healthcare25To99Employees]
           ,[Healthcare100To999Employees]
           ,[Healthcare1000orMoreEmployees]
           ,[HealthcareByIndustry]
           ,[HealthcareByStateRank]
           ,[Healthcare0To9EmployeesRank]
           ,[Healthcare10To24EmployeesRank]
           ,[Healthcare25To99EmployeesRank]
           ,[Healthcare100To999EmployeesRank]
           ,[Healthcare1000orMoreEmployeesRank]
           ,[HealthcareByIndustryRank]
FROM [GISP-CALC-DB02].[LBINewData].[dbo].[IndustryDataByState]
SET IDENTITY_INSERT [LBISizeUpData].[dbo].[IndustryDataByState] OFF         
GO


SET NOCOUNT ON 

ALTER TABLE [dbo].[IndustryDataByState] ADD  CONSTRAINT [PK_IndustryDataByState] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]

ALTER TABLE [dbo].[IndustryDataByStateBandMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByStateBandMapping_IndustryDataByState] FOREIGN KEY([IndustryDataByStateId])
REFERENCES [dbo].[IndustryDataByState] ([Id])

ALTER TABLE [dbo].[IndustryDataByStateBandMapping] NOCHECK CONSTRAINT [FK_IndustryDataByStateBandMapping_IndustryDataByState]
GO

declare @tableName nvarchar(128)
set @tableName = 'IndustryDataByState'


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
 