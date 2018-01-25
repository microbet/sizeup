DBCC TRACEON(610)
GO

TRUNCATE TABLE LBISizeUpData.dbo.IndustryDataByCityBandMapping
GO

INSERT INTO [LBISizeUpData].[dbo].[IndustryDataByCityBandMapping]
           ([IndustryDataByCityId]
           ,[BandId])
SELECT [IndustryDataByCityId]
      ,[BandId]
FROM [GISP-CALC-DB02].[LBINewData].[dbo].[IndustryDataByCityBandMapping]
GO

TRUNCATE TABLE LBISizeUpData.dbo.IndustryDataByCountyBandMapping
GO

INSERT INTO [LBISizeUpData].[dbo].[IndustryDataByCountyBandMapping]
           ([IndustryDataByCountyId]
           ,[BandId])
SELECT [IndustryDataByCountyId]
      ,[BandId]
FROM [GISP-CALC-DB02].[LBINewData].[dbo].[IndustryDataByCountyBandMapping]
GO

TRUNCATE TABLE LBISizeUpData.dbo.IndustryDataByMetroBandMapping
GO

INSERT INTO [LBISizeUpData].[dbo].[IndustryDataByMetroBandMapping]
           ([IndustryDataByMetroId]
           ,[BandId])
SELECT [IndustryDataByMetroId]
      ,[BandId]
FROM [GISP-CALC-DB02].[LBINewData].[dbo].[IndustryDataByMetroBandMapping]
GO

TRUNCATE TABLE LBISizeUpData.dbo.IndustryDataByStateBandMapping
GO

INSERT INTO [LBISizeUpData].[dbo].[IndustryDataByStateBandMapping]
           ([IndustryDataByStateId]
           ,[BandId])
SELECT [IndustryDataByStateId]
      ,[BandId]
FROM [GISP-CALC-DB02].[LBINewData].[dbo].[IndustryDataByStateBandMapping]
GO

TRUNCATE TABLE LBISizeUpData.dbo.IndustryDataByNationBandMapping
GO

INSERT INTO [LBISizeUpData].[dbo].[IndustryDataByNationBandMapping]
           ([IndustryDataByNationId]
           ,[BandId])
SELECT [IndustryDataByNationId]
      ,[BandId]
FROM [GISP-CALC-DB02].[LBINewData].[dbo].[IndustryDataByNationBandMapping]
GO
           