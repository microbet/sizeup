--BUSINESS IMPORT
USE [XXX_LBISizeUpData]
GO
ALTER DATABASE [XXX_LBISizeUpData] SET RECOVERY SIMPLE WITH NO_WAIT
GO
ALTER DATABASE [XXX_LBISizeUpData] SET RECOVERY SIMPLE 
GO

ALTER INDEX IX_Business_IndustryId ON [XXX_LBISizeUpData].dbo.Business DISABLE;
ALTER INDEX IX_Business_IndustryId_IsActive  ON [XXX_LBISizeUpData].dbo.Business DISABLE;
ALTER INDEX IX_Business_IndustryIdBusinessStatusCodeLatLong  ON [XXX_LBISizeUpData].dbo.Business DISABLE;
ALTER INDEX IX_Business_IndustryIdIsActive_Id  ON [XXX_LBISizeUpData].dbo.Business DISABLE;
ALTER INDEX IX_Business_InfoGroupId  ON [XXX_LBISizeUpData].dbo.Business DISABLE;
ALTER INDEX IX_Business_IsActive_Id  ON [XXX_LBISizeUpData].dbo.Business DISABLE;
ALTER INDEX IX_Business_IsActive_InfoGroupIdIndustryIdMetroId  ON [XXX_LBISizeUpData].dbo.Business DISABLE;
ALTER INDEX IX_Business_MatchLevelIndustryIdIsActive_CompetitionData  ON [XXX_LBISizeUpData].dbo.Business DISABLE;
ALTER INDEX IX_Business_MatchLevelIndustryIdIsActiveInBusiness  ON [XXX_LBISizeUpData].dbo.Business DISABLE;
ALTER INDEX IX_Business_MatchLevelIndustryIdIsActiveInBusinessLatLong  ON [XXX_LBISizeUpData].dbo.Business DISABLE;
ALTER INDEX IX_Business_Name  ON [XXX_LBISizeUpData].dbo.Business DISABLE;
ALTER INDEX PK_Business ON [XXX_LBISizeUpData].dbo.Business DISABLE;

ALTER INDEX IX_Generic_BusinessCityMapping_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessCityMapping DISABLE;
ALTER INDEX IX_Generic_BusinessCityMapping_CityId_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessCityMapping DISABLE;
ALTER INDEX PK_BusinessCityMapping ON [XXX_LBISizeUpData].dbo.BusinessCityMapping DISABLE;

ALTER INDEX IX_Generic_BusinessDataByCity_IndustryIdCityId_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCity DISABLE;
ALTER INDEX IX_Generic_BusinessDataByCity_YearQuarterCityId_IndustryIdBusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCity DISABLE;
ALTER INDEX IX_Generic_BusinessDataByCity_YearQuarterIndustryIdCityId_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCity DISABLE;
ALTER INDEX PK_BusinessDataByCity ON [XXX_LBISizeUpData].dbo.BusinessDataByCity DISABLE;

ALTER INDEX IX_BusinessDataByCounty_YearQuarterIndustryId_BusinessIdYearEstablishedYearAppeared ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty DISABLE;
ALTER INDEX IX_BusinessDataByCounty_YearQuarterIndustryIdCountyId_BusinessIdYearEstablishedYearAppeared ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty DISABLE;
ALTER INDEX IX_BusinessDataByCounty_YearQuarterIndustryIdCountyIdEmployees_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty DISABLE;
ALTER INDEX IX_BusinessDataByCounty_YearQuarterIndustryIdEmployees_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty DISABLE;
ALTER INDEX IX_BusinessDataByCounty_YearQuarterIndustryIdMetroId_BusinessIdYearEstablishedYearAppeared ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty DISABLE;
ALTER INDEX IX_BusinessDataByCounty_YearQuarterIndustryIdMetroIdEmployees_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty DISABLE;
ALTER INDEX IX_BusinessDataByCounty_YearQuarterIndustryIdRevenue_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty DISABLE;
ALTER INDEX IX_BusinessDataByCounty_YearQuarterIndustryIdRevenue_CountyIdBusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty DISABLE;
ALTER INDEX IX_BusinessDataByCounty_YearQuarterIndustryIdStateId_BusinessIdYearEstablishedYearAppeared ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty DISABLE;
ALTER INDEX IX_BusinessDataByCounty_YearQuarterIndustryIdStateIdEmployees ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty DISABLE;
ALTER INDEX IX_Generic_BusinessDataByCounty_IndustryIdCountyId_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty DISABLE;
ALTER INDEX IX_Generic_BusinessDataByCounty_IndustryIdMetroId_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty DISABLE;
ALTER INDEX IX_Generic_BusinessDataByCounty_IndustryIdStateId_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty DISABLE;
ALTER INDEX IX_Generic_BusinessDataByCounty_YearQuarterIndustryIdCountyId_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty DISABLE;
ALTER INDEX IX_Generic_BusinessDataByCounty_YearQuarterIndustryIdMetroId_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty DISABLE;
ALTER INDEX IX_Generic_BusinessDataByCounty_YearQuarterIndustryIdStateId_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty DISABLE;
ALTER INDEX PK_BusinessDataByCounty ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty DISABLE;

ALTER INDEX IX_Generic_BusinessDataByZip_YearQuarterIndustryIdZipCodeId_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByZip DISABLE;
ALTER INDEX PK_BusinessDataByZip ON [XXX_LBISizeUpData].dbo.BusinessDataByZip DISABLE;

ALTER INDEX IX_Industry_IsActive_Id ON [XXX_LBISizeUpData].dbo.Industry DISABLE;
ALTER INDEX IX_Industry_IsActiveName_IdSEOKey ON [XXX_LBISizeUpData].dbo.Industry DISABLE;
ALTER INDEX IX_Industry_Name ON [XXX_LBISizeUpData].dbo.Industry DISABLE;
ALTER INDEX IX_Industry_SEOKey ON [XXX_LBISizeUpData].dbo.Industry DISABLE;
ALTER INDEX IX_Industry_SEOKeyIsActive ON [XXX_LBISizeUpData].dbo.Industry DISABLE;
ALTER INDEX IX_Industry_SicCode ON [XXX_LBISizeUpData].dbo.Industry DISABLE;
ALTER INDEX IX_Industry_SicCodeIsActive ON [XXX_LBISizeUpData].dbo.Industry DISABLE;
ALTER INDEX PK_Industry ON [XXX_LBISizeUpData].dbo.Industry DISABLE;

ALTER INDEX IX_Generic_IndustryDataByCity_CityId_IndustryId ON [XXX_LBISizeUpData].dbo.IndustryDataByCity DISABLE;
ALTER INDEX IX_Generic_IndustryDataByCity_YearQuarterCityIdIndustryId ON [XXX_LBISizeUpData].dbo.IndustryDataByCity DISABLE;
ALTER INDEX IX_Generic_IndustryDataByCity_YearQuarterIndustryId ON [XXX_LBISizeUpData].dbo.IndustryDataByCity DISABLE;
ALTER INDEX IX_IndustryDataByCity_YearQuarterIndustryIdBestPlacesAttributes_CityId ON [XXX_LBISizeUpData].dbo.IndustryDataByCity DISABLE;
ALTER INDEX IX_IndustryDataByCity_YearQuarterIndustryIdEmployeesPerCapita ON [XXX_LBISizeUpData].dbo.IndustryDataByCity DISABLE;
ALTER INDEX IX_IndustryDataByCity_YearQuarterIndustryIdEmployeesPerCapita_CityId ON [XXX_LBISizeUpData].dbo.IndustryDataByCity DISABLE;
ALTER INDEX IX_IndustryDataByCity_YearQuarterIndustryIdEmployeesPerCapita_IdCityId ON [XXX_LBISizeUpData].dbo.IndustryDataByCity DISABLE;
ALTER INDEX IX_IndustryDataByCity_YearQuarterIndustryIdRevenuePerCapita ON [XXX_LBISizeUpData].dbo.IndustryDataByCity DISABLE;
ALTER INDEX PK_IndustryDataByCity ON [XXX_LBISizeUpData].dbo.IndustryDataByCity DISABLE;

ALTER INDEX IX_Generic_IndustryDataByCounty_YearQuarterCountyIdIndustryId ON [XXX_LBISizeUpData].dbo.IndustryDataByCounty DISABLE;
ALTER INDEX IX_Generic_IndustryDataByCounty_YearQuarterIndustryId ON [XXX_LBISizeUpData].dbo.IndustryDataByCounty DISABLE;
ALTER INDEX IX_IndustryDataByCounty_YearQuarterIndustryIdBestPlacesAttributes_CountyId ON [XXX_LBISizeUpData].dbo.IndustryDataByCounty DISABLE;
ALTER INDEX IX_IndustryDataByCounty_YearQuarterIndustryIdCountyIdAverageEmployees ON [XXX_LBISizeUpData].dbo.IndustryDataByCounty DISABLE;
ALTER INDEX IX_IndustryDataByCounty_YearQuarterIndustryIdCountyIdAverageRevenue ON [XXX_LBISizeUpData].dbo.IndustryDataByCounty DISABLE;
ALTER INDEX IX_IndustryDataByCounty_YearQuarterIndustryIdCountyIdEmployeesPerCapita ON [XXX_LBISizeUpData].dbo.IndustryDataByCounty DISABLE;
ALTER INDEX IX_IndustryDataByCounty_YearQuarterIndustryIdCountyIdTotalRevenue ON [XXX_LBISizeUpData].dbo.IndustryDataByCounty DISABLE;
ALTER INDEX IX_IndustryDataByCounty_YearQuarterIndustryIdTurnoverRate ON [XXX_LBISizeUpData].dbo.IndustryDataByCounty DISABLE;
ALTER INDEX IX_IndustryDataByCounty_YearQuarterIndustryIdTurnoverRate_CountyId ON [XXX_LBISizeUpData].dbo.IndustryDataByCounty DISABLE;
ALTER INDEX IX_IndustryDataByCounty_YearQuarterIndustryIdTurnoverRateNetJobChange ON [XXX_LBISizeUpData].dbo.IndustryDataByCounty DISABLE;
ALTER INDEX IX_IndustryDataByCounty_YearQuarterIndustryIdTurnoverRateNetJobChange_CountyId ON [XXX_LBISizeUpData].dbo.IndustryDataByCounty DISABLE;
ALTER INDEX PK_IndustryDataByCounty ON [XXX_LBISizeUpData].dbo.IndustryDataByCounty DISABLE;

ALTER INDEX IX_Generic_IndustryDataByMetro_YearQuarterIndustryId ON [XXX_LBISizeUpData].dbo.IndustryDataByMetro DISABLE;
ALTER INDEX IX_Generic_IndustryDataByMetro_YearQuarterMetroIdIndustryId ON [XXX_LBISizeUpData].dbo.IndustryDataByMetro DISABLE;
ALTER INDEX IX_IndustryDataByMetro_YearQuarterIndustryIdBestPlacesAttributes_MetroId ON [XXX_LBISizeUpData].dbo.IndustryDataByMetro DISABLE;
ALTER INDEX IX_IndustryDataByMetro_YearQuarterIndustryIdTurnoverRate ON [XXX_LBISizeUpData].dbo.IndustryDataByMetro DISABLE;
ALTER INDEX PK_IndustryDataByMetro ON [XXX_LBISizeUpData].dbo.IndustryDataByMetro DISABLE;

ALTER INDEX IX_Generic_IndustryDataByNation_YearQuarterIndustryId ON [XXX_LBISizeUpData].dbo.IndustryDataByNation DISABLE;
ALTER INDEX PK_IndustryDataByNation ON [XXX_LBISizeUpData].dbo.IndustryDataByNation DISABLE;

ALTER INDEX IX_Generic_IndustryDataByState_YearQuarterIndustryId ON [XXX_LBISizeUpData].dbo.IndustryDataByState DISABLE;
ALTER INDEX IX_Generic_IndustryDataByState_YearQuarterIndustryIdStateId ON [XXX_LBISizeUpData].dbo.IndustryDataByState DISABLE;
ALTER INDEX IX_IndustryDataByState_YearQuarterIndustryIdBestPlacesAttributes_StateId ON [XXX_LBISizeUpData].dbo.IndustryDataByState DISABLE;
ALTER INDEX PK_IndustryDataByState ON [XXX_LBISizeUpData].dbo.IndustryDataByState DISABLE;

ALTER INDEX IX_Generic_IndustryDataByZip_YearQuarterIndustryId ON [XXX_LBISizeUpData].dbo.IndustryDataByZip DISABLE;
ALTER INDEX IX_Generic_IndustryDataByZip_YearQuarterIndustryIdZipCodeId ON [XXX_LBISizeUpData].dbo.IndustryDataByZip DISABLE;
ALTER INDEX IX_IndustryDataByZip_YearQuarterIndustryIdAverageRevenue_ZipCodeId ON [XXX_LBISizeUpData].dbo.IndustryDataByZip DISABLE;
ALTER INDEX IX_IndustryDataByZip_YearQuarterIndustryIdAverageRevenue_ZipCodeIdTotalEmployeesTotalRevenueRevenuePerCapita ON [XXX_LBISizeUpData].dbo.IndustryDataByZip DISABLE;
ALTER INDEX IX_IndustryDataByZip_YearQuarterIndustryIdRevenuePerCapita_ZipCodeId ON [XXX_LBISizeUpData].dbo.IndustryDataByZip DISABLE;
ALTER INDEX IX_IndustryDataByZip_YearQuarterIndustryIdRevenuePerCapita_ZipCodeIdTotalEmployeesTotalRevenueAverageRevenue ON [XXX_LBISizeUpData].dbo.IndustryDataByZip DISABLE;
ALTER INDEX IX_IndustryDataByZip_YearQuarterIndustryIdTotalRevenue_ZipCodeId ON [XXX_LBISizeUpData].dbo.IndustryDataByZip DISABLE;
ALTER INDEX IX_IndustryDataByZip_YearQuarterIndustryIdTotalRevenue_ZipCodeIdTotalEmployeesAverageRevenueRevenuePerCapita ON [XXX_LBISizeUpData].dbo.IndustryDataByZip DISABLE;
ALTER INDEX IX_IndustryDataByZip_ZipCodeIdIndustryId_AdvertisingStuff ON [XXX_LBISizeUpData].dbo.IndustryDataByZip DISABLE;
ALTER INDEX PK_IndustryDataByZip ON [XXX_LBISizeUpData].dbo.IndustryDataByZip DISABLE;


IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Business]') AND name = N'PK_Business')
ALTER TABLE [dbo].[Business] DROP CONSTRAINT [PK_Business]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Business_County]') AND parent_object_id = OBJECT_ID(N'[dbo].[Business]'))
ALTER TABLE [dbo].[Business] DROP CONSTRAINT [FK_Business_County]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Business_Industry]') AND parent_object_id = OBJECT_ID(N'[dbo].[Business]'))
ALTER TABLE [dbo].[Business] DROP CONSTRAINT [FK_Business_Industry]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Business_Metro]') AND parent_object_id = OBJECT_ID(N'[dbo].[Business]'))
ALTER TABLE [dbo].[Business] DROP CONSTRAINT [FK_Business_Metro]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Business_State]') AND parent_object_id = OBJECT_ID(N'[dbo].[Business]'))
ALTER TABLE [dbo].[Business] DROP CONSTRAINT [FK_Business_State]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Business_ZipCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[Business]'))
ALTER TABLE [dbo].[Business] DROP CONSTRAINT [FK_Business_ZipCode]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessCityMapping]') AND name = N'PK_BusinessCityMapping')
ALTER TABLE [dbo].[BusinessCityMapping] DROP CONSTRAINT [PK_BusinessCityMapping]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BusinessCityMapping_Business]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessCityMapping]'))
ALTER TABLE [dbo].[BusinessCityMapping] DROP CONSTRAINT [FK_BusinessCityMapping_Business]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BusinessCityMapping_City]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessCityMapping]'))
ALTER TABLE [dbo].[BusinessCityMapping] DROP CONSTRAINT [FK_BusinessCityMapping_City]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessDataByCity]') AND name = N'PK_BusinessDataByCity')
ALTER TABLE [dbo].[BusinessDataByCity] DROP CONSTRAINT [PK_BusinessDataByCity]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BusinessDataByCity_Business]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessDataByCity]'))
ALTER TABLE [dbo].[BusinessDataByCity] DROP CONSTRAINT [FK_BusinessDataByCity_Business]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BusinessDataByCity_City]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessDataByCity]'))
ALTER TABLE [dbo].[BusinessDataByCity] DROP CONSTRAINT [FK_BusinessDataByCity_City]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BusinessDataByCity_Industry]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessDataByCity]'))
ALTER TABLE [dbo].[BusinessDataByCity] DROP CONSTRAINT [FK_BusinessDataByCity_Industry]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessDataByCounty]') AND name = N'PK_BusinessDataByCounty')
ALTER TABLE [dbo].[BusinessDataByCounty] DROP CONSTRAINT [PK_BusinessDataByCounty]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BusinessDataByCounty_Business]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessDataByCounty]'))
ALTER TABLE [dbo].[BusinessDataByCounty] DROP CONSTRAINT [FK_BusinessDataByCounty_Business]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BusinessDataByCounty_County]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessDataByCounty]'))
ALTER TABLE [dbo].[BusinessDataByCounty] DROP CONSTRAINT [FK_BusinessDataByCounty_County]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BusinessDataByCounty_Industry]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessDataByCounty]'))
ALTER TABLE [dbo].[BusinessDataByCounty] DROP CONSTRAINT [FK_BusinessDataByCounty_Industry]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BusinessDataByCounty_Metro]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessDataByCounty]'))
ALTER TABLE [dbo].[BusinessDataByCounty] DROP CONSTRAINT [FK_BusinessDataByCounty_Metro]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BusinessDataByCounty_State]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessDataByCounty]'))
ALTER TABLE [dbo].[BusinessDataByCounty] DROP CONSTRAINT [FK_BusinessDataByCounty_State]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessDataByZip]') AND name = N'PK_BusinessDataByZip')
ALTER TABLE [dbo].[BusinessDataByZip] DROP CONSTRAINT [PK_BusinessDataByZip]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BusinessDataByZip_Business]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessDataByZip]'))
ALTER TABLE [dbo].[BusinessDataByZip] DROP CONSTRAINT [FK_BusinessDataByZip_Business]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BusinessDataByZip_Industry]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessDataByZip]'))
ALTER TABLE [dbo].[BusinessDataByZip] DROP CONSTRAINT [FK_BusinessDataByZip_Industry]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BusinessDataByZip_ZipCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessDataByZip]'))
ALTER TABLE [dbo].[BusinessDataByZip] DROP CONSTRAINT [FK_BusinessDataByZip_ZipCode]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LegacyBusinessSEOKey_Business]') AND parent_object_id = OBJECT_ID(N'[dbo].[LegacyBusinessSEOKey]'))
ALTER TABLE [dbo].[LegacyBusinessSEOKey] DROP CONSTRAINT [FK_LegacyBusinessSEOKey_Business]
GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByCityBandMapping_Band]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByCityBandMapping]'))
ALTER TABLE [dbo].[IndustryDataByCityBandMapping] DROP CONSTRAINT [FK_IndustryDataByCityBandMapping_Band]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByCityBandMapping_IndustryDataByCity]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByCityBandMapping]'))
ALTER TABLE [dbo].[IndustryDataByCityBandMapping] DROP CONSTRAINT [FK_IndustryDataByCityBandMapping_IndustryDataByCity]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByCountyBandMapping_Band]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByCountyBandMapping]'))
ALTER TABLE [dbo].[IndustryDataByCountyBandMapping] DROP CONSTRAINT [FK_IndustryDataByCountyBandMapping_Band]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByCountyBandMapping_IndustryDataByCounty]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByCountyBandMapping]'))
ALTER TABLE [dbo].[IndustryDataByCountyBandMapping] DROP CONSTRAINT [FK_IndustryDataByCountyBandMapping_IndustryDataByCounty]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByMetroBandMapping_Band]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByMetroBandMapping]'))
ALTER TABLE [dbo].[IndustryDataByMetroBandMapping] DROP CONSTRAINT [FK_IndustryDataByMetroBandMapping_Band]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByMetroBandMapping_IndustryDataByMetro]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByMetroBandMapping]'))
ALTER TABLE [dbo].[IndustryDataByMetroBandMapping] DROP CONSTRAINT [FK_IndustryDataByMetroBandMapping_IndustryDataByMetro]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByNationBandMapping_Band]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByNationBandMapping]'))
ALTER TABLE [dbo].[IndustryDataByNationBandMapping] DROP CONSTRAINT [FK_IndustryDataByNationBandMapping_Band]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByNationBandMapping_IndustryDataByNation]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByNationBandMapping]'))
ALTER TABLE [dbo].[IndustryDataByNationBandMapping] DROP CONSTRAINT [FK_IndustryDataByNationBandMapping_IndustryDataByNation]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByStateBandMapping_Band]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByStateBandMapping]'))
ALTER TABLE [dbo].[IndustryDataByStateBandMapping] DROP CONSTRAINT [FK_IndustryDataByStateBandMapping_Band]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByStateBandMapping_IndustryDataByState]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByStateBandMapping]'))
ALTER TABLE [dbo].[IndustryDataByStateBandMapping] DROP CONSTRAINT [FK_IndustryDataByStateBandMapping_IndustryDataByState]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByZipBandMapping_Band]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByZipBandMapping]'))
ALTER TABLE [dbo].[IndustryDataByZipBandMapping] DROP CONSTRAINT [FK_IndustryDataByZipBandMapping_Band]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByZipBandMapping_IndustryDataByZip]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByZipBandMapping]'))
ALTER TABLE [dbo].[IndustryDataByZipBandMapping] DROP CONSTRAINT [FK_IndustryDataByZipBandMapping_IndustryDataByZip]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Industry]') AND name = N'PK_Industry')
ALTER TABLE [dbo].[Industry] DROP CONSTRAINT [PK_Industry]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[IndustryDataByCity]') AND name = N'PK_IndustryDataByCity')
ALTER TABLE [dbo].[IndustryDataByCity] DROP CONSTRAINT [PK_IndustryDataByCity]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByCity_City]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByCity]'))
ALTER TABLE [dbo].[IndustryDataByCity] DROP CONSTRAINT [FK_IndustryDataByCity_City]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByCity_Industry]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByCity]'))
ALTER TABLE [dbo].[IndustryDataByCity] DROP CONSTRAINT [FK_IndustryDataByCity_Industry]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[IndustryDataByCounty]') AND name = N'PK_IndustryDataByCounty')
ALTER TABLE [dbo].[IndustryDataByCounty] DROP CONSTRAINT [PK_IndustryDataByCounty]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByCounty_County]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByCounty]'))
ALTER TABLE [dbo].[IndustryDataByCounty] DROP CONSTRAINT [FK_IndustryDataByCounty_County]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByCounty_Industry]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByCounty]'))
ALTER TABLE [dbo].[IndustryDataByCounty] DROP CONSTRAINT [FK_IndustryDataByCounty_Industry]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[IndustryDataByMetro]') AND name = N'PK_IndustryDataByMetro')
ALTER TABLE [dbo].[IndustryDataByMetro] DROP CONSTRAINT [PK_IndustryDataByMetro]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByMetro_Industry]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByMetro]'))
ALTER TABLE [dbo].[IndustryDataByMetro] DROP CONSTRAINT [FK_IndustryDataByMetro_Industry]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByMetro_Metro]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByMetro]'))
ALTER TABLE [dbo].[IndustryDataByMetro] DROP CONSTRAINT [FK_IndustryDataByMetro_Metro]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[IndustryDataByNation]') AND name = N'PK_IndustryDataByNation')
ALTER TABLE [dbo].[IndustryDataByNation] DROP CONSTRAINT [PK_IndustryDataByNation]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByNation_Industry]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByNation]'))
ALTER TABLE [dbo].[IndustryDataByNation] DROP CONSTRAINT [FK_IndustryDataByNation_Industry]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[IndustryDataByState]') AND name = N'PK_IndustryDataByState')
ALTER TABLE [dbo].[IndustryDataByState] DROP CONSTRAINT [PK_IndustryDataByState]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByState_Industry]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByState]'))
ALTER TABLE [dbo].[IndustryDataByState] DROP CONSTRAINT [FK_IndustryDataByState_Industry]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByState_State]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByState]'))
ALTER TABLE [dbo].[IndustryDataByState] DROP CONSTRAINT [FK_IndustryDataByState_State]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[IndustryDataByZip]') AND name = N'PK_IndustryDataByZip')
ALTER TABLE [dbo].[IndustryDataByZip] DROP CONSTRAINT [PK_IndustryDataByZip]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByZip_Industry]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByZip]'))
ALTER TABLE [dbo].[IndustryDataByZip] DROP CONSTRAINT [FK_IndustryDataByZip_Industry]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryDataByZip_ZipCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryDataByZip]'))
ALTER TABLE [dbo].[IndustryDataByZip] DROP CONSTRAINT [FK_IndustryDataByZip_ZipCode]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IndustryKeyword_Industry]') AND parent_object_id = OBJECT_ID(N'[dbo].[IndustryKeyword]'))
ALTER TABLE [dbo].[IndustryKeyword] DROP CONSTRAINT [FK_IndustryKeyword_Industry]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LegacyIndustrySEOKey_Industry]') AND parent_object_id = OBJECT_ID(N'[dbo].[LegacyIndustrySEOKey]'))
ALTER TABLE [dbo].[LegacyIndustrySEOKey] DROP CONSTRAINT [FK_LegacyIndustrySEOKey_Industry]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SicToNAICSMapping_Industry]') AND parent_object_id = OBJECT_ID(N'[dbo].[SicToNAICSMapping]'))
ALTER TABLE [dbo].[SicToNAICSMapping] DROP CONSTRAINT [FK_SicToNAICSMapping_Industry]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SicToNAICSMapping_NAICS]') AND parent_object_id = OBJECT_ID(N'[dbo].[SicToNAICSMapping]'))
ALTER TABLE [dbo].[SicToNAICSMapping] DROP CONSTRAINT [FK_SicToNAICSMapping_NAICS]
GO

--TRUNCATE
truncate table Business
truncate table BusinessData
truncate table BusinessCityMapping
truncate table BusinessDataByCity
truncate table BusinessDataByCounty
truncate table BusinessDataByZip
--INDUSTRY
truncate table Industry
truncate table IndustryDataByCity
truncate table IndustryDataByCounty
truncate table IndustryDataByMetro
truncate table IndustryDataByNation
truncate table IndustryDataByState
truncate table IndustryDataByZip

DBCC SHRINKFILE (N'XXX_LBISizeUpData_log' , 1, TRUNCATEONLY)
GO

BULK INSERT [XXX_LBISizeUpData].dbo.Business 
    FROM 'H:\Temp\Business.DAT' 
   WITH (DATAFILETYPE='widenative', KEEPIDENTITY, formatfile='H:\Temp\BusinessFMT.Fmt', ROWS_PER_BATCH=10000); 
GO
BULK INSERT [XXX_LBISizeUpData].dbo.BusinessData
    FROM 'H:\Temp\BusinessData.DAT' 
   WITH (DATAFILETYPE='widenative', KEEPIDENTITY, formatfile='H:\Temp\BusinessDataFMT.Fmt', ROWS_PER_BATCH=10000); 
GO
BULK INSERT [XXX_LBISizeUpData].dbo.BusinessCityMapping 
    FROM 'H:\Temp\BusinessCityMapping.DAT' 
   WITH (DATAFILETYPE='widenative', KEEPIDENTITY, formatfile='H:\Temp\BusinessCityMappingFMT.Fmt', ROWS_PER_BATCH=10000); 
GO
BULK INSERT [XXX_LBISizeUpData].dbo.BusinessDataByCity 
    FROM 'H:\Temp\BusinessDataByCity.DAT' 
   WITH (DATAFILETYPE='widenative', KEEPIDENTITY, formatfile='H:\Temp\BusinessDataByCityFMT.Fmt', ROWS_PER_BATCH=10000); 
GO
BULK INSERT [XXX_LBISizeUpData].dbo.BusinessDataByCounty 
    FROM 'H:\Temp\BusinessDataByCounty.DAT' 
   WITH (DATAFILETYPE='widenative', KEEPIDENTITY, formatfile='H:\Temp\BusinessDataByCountyFMT.Fmt', ROWS_PER_BATCH=10000); 
GO
BULK INSERT [XXX_LBISizeUpData].dbo.BusinessDataByZip 
    FROM 'H:\Temp\BusinessDataByZip.DAT' 
   WITH (DATAFILETYPE='widenative', KEEPIDENTITY, formatfile='H:\Temp\BusinessDataByZipFMT.Fmt', ROWS_PER_BATCH=10000); 
GO
BULK INSERT [XXX_LBISizeUpData].dbo.Industry 
    FROM 'H:\Temp\Industry.DAT' 
   WITH (DATAFILETYPE='widenative', KEEPIDENTITY, formatfile='H:\Temp\IndustryFMT.Fmt', ROWS_PER_BATCH=10000); 
GO
BULK INSERT [XXX_LBISizeUpData].dbo.IndustryDataByCity 
    FROM 'H:\Temp\IndustryDataByCity.DAT' 
   WITH (DATAFILETYPE='widenative', KEEPIDENTITY, formatfile='H:\Temp\IndustryDataByCityFMT.Fmt', ROWS_PER_BATCH=10000); 
GO
BULK INSERT [XXX_LBISizeUpData].dbo.IndustryDataByCounty 
    FROM 'H:\Temp\IndustryDataByCounty.DAT' 
   WITH (DATAFILETYPE='widenative', KEEPIDENTITY, formatfile='H:\Temp\IndustryDataByCountyFMT.Fmt', ROWS_PER_BATCH=10000); 
GO
BULK INSERT [XXX_LBISizeUpData].dbo.IndustryDataByMetro 
    FROM 'H:\Temp\IndustryDataByMetro.DAT' 
   WITH (DATAFILETYPE='widenative', KEEPIDENTITY, formatfile='H:\Temp\IndustryDataByMetroFMT.Fmt', ROWS_PER_BATCH=10000); 
GO
BULK INSERT [XXX_LBISizeUpData].dbo.IndustryDataByNation 
    FROM 'H:\Temp\IndustryDataByNation.DAT' 
   WITH (DATAFILETYPE='widenative', KEEPIDENTITY, formatfile='H:\Temp\IndustryDataByNationFMT.Fmt', ROWS_PER_BATCH=10000); 
GO
BULK INSERT [XXX_LBISizeUpData].dbo.IndustryDataByState 
    FROM 'H:\Temp\IndustryDataByState.DAT' 
   WITH (DATAFILETYPE='widenative', KEEPIDENTITY, formatfile='H:\Temp\IndustryDataByStateFMT.Fmt', ROWS_PER_BATCH=10000); 
GO
BULK INSERT [XXX_LBISizeUpData].dbo.IndustryDataByZip 
    FROM 'H:\Temp\IndustryDataByZip.DAT' 
   WITH (DATAFILETYPE='widenative', KEEPIDENTITY, formatfile='H:\Temp\IndustryDataByZipFMT.Fmt', ROWS_PER_BATCH=10000); 
GO

ALTER INDEX PK_Business ON [XXX_LBISizeUpData].dbo.Business REBUILD;
ALTER INDEX IX_Business_IndustryId ON [XXX_LBISizeUpData].dbo.Business REBUILD;
ALTER INDEX IX_Business_IndustryId_IsActive  ON [XXX_LBISizeUpData].dbo.Business REBUILD;
ALTER INDEX IX_Business_IndustryIdBusinessStatusCodeLatLong  ON [XXX_LBISizeUpData].dbo.Business REBUILD;
ALTER INDEX IX_Business_IndustryIdIsActive_Id  ON [XXX_LBISizeUpData].dbo.Business REBUILD;
ALTER INDEX IX_Business_InfoGroupId  ON [XXX_LBISizeUpData].dbo.Business REBUILD;
ALTER INDEX IX_Business_IsActive_Id  ON [XXX_LBISizeUpData].dbo.Business REBUILD;
ALTER INDEX IX_Business_IsActive_InfoGroupIdIndustryIdMetroId  ON [XXX_LBISizeUpData].dbo.Business REBUILD;
ALTER INDEX IX_Business_MatchLevelIndustryIdIsActive_CompetitionData  ON [XXX_LBISizeUpData].dbo.Business REBUILD;
ALTER INDEX IX_Business_MatchLevelIndustryIdIsActiveInBusiness  ON [XXX_LBISizeUpData].dbo.Business REBUILD;
ALTER INDEX IX_Business_MatchLevelIndustryIdIsActiveInBusinessLatLong  ON [XXX_LBISizeUpData].dbo.Business REBUILD;
ALTER INDEX IX_Business_Name  ON [XXX_LBISizeUpData].dbo.Business REBUILD;

ALTER INDEX PK_BusinessCityMapping ON [XXX_LBISizeUpData].dbo.BusinessCityMapping REBUILD;
ALTER INDEX IX_Generic_BusinessCityMapping_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessCityMapping REBUILD;
ALTER INDEX IX_Generic_BusinessCityMapping_CityId_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessCityMapping REBUILD;

ALTER INDEX PK_BusinessDataByCity ON [XXX_LBISizeUpData].dbo.BusinessDataByCity REBUILD;
ALTER INDEX IX_Generic_BusinessDataByCity_IndustryIdCityId_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCity REBUILD;
ALTER INDEX IX_Generic_BusinessDataByCity_YearQuarterCityId_IndustryIdBusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCity REBUILD;
ALTER INDEX IX_Generic_BusinessDataByCity_YearQuarterIndustryIdCityId_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCity REBUILD;

ALTER INDEX PK_BusinessDataByCounty ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty REBUILD;
ALTER INDEX IX_BusinessDataByCounty_YearQuarterIndustryId_BusinessIdYearEstablishedYearAppeared ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty REBUILD;
ALTER INDEX IX_BusinessDataByCounty_YearQuarterIndustryIdCountyId_BusinessIdYearEstablishedYearAppeared ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty REBUILD;
ALTER INDEX IX_BusinessDataByCounty_YearQuarterIndustryIdCountyIdEmployees_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty REBUILD;
ALTER INDEX IX_BusinessDataByCounty_YearQuarterIndustryIdEmployees_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty REBUILD;
ALTER INDEX IX_BusinessDataByCounty_YearQuarterIndustryIdMetroId_BusinessIdYearEstablishedYearAppeared ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty REBUILD;
ALTER INDEX IX_BusinessDataByCounty_YearQuarterIndustryIdMetroIdEmployees_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty REBUILD;
ALTER INDEX IX_BusinessDataByCounty_YearQuarterIndustryIdRevenue_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty REBUILD;
ALTER INDEX IX_BusinessDataByCounty_YearQuarterIndustryIdRevenue_CountyIdBusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty REBUILD;
ALTER INDEX IX_BusinessDataByCounty_YearQuarterIndustryIdStateId_BusinessIdYearEstablishedYearAppeared ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty REBUILD;
ALTER INDEX IX_BusinessDataByCounty_YearQuarterIndustryIdStateIdEmployees ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty REBUILD;
ALTER INDEX IX_Generic_BusinessDataByCounty_IndustryIdCountyId_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty REBUILD;
ALTER INDEX IX_Generic_BusinessDataByCounty_IndustryIdMetroId_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty REBUILD;
ALTER INDEX IX_Generic_BusinessDataByCounty_IndustryIdStateId_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty REBUILD;
ALTER INDEX IX_Generic_BusinessDataByCounty_YearQuarterIndustryIdCountyId_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty REBUILD;
ALTER INDEX IX_Generic_BusinessDataByCounty_YearQuarterIndustryIdMetroId_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty REBUILD;
ALTER INDEX IX_Generic_BusinessDataByCounty_YearQuarterIndustryIdStateId_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByCounty REBUILD;

ALTER INDEX PK_BusinessDataByZip ON [XXX_LBISizeUpData].dbo.BusinessDataByZip REBUILD;
ALTER INDEX IX_Generic_BusinessDataByZip_YearQuarterIndustryIdZipCodeId_BusinessId ON [XXX_LBISizeUpData].dbo.BusinessDataByZip REBUILD;

ALTER INDEX PK_Industry ON [XXX_LBISizeUpData].dbo.Industry REBUILD;
ALTER INDEX IX_Industry_IsActive_Id ON [XXX_LBISizeUpData].dbo.Industry REBUILD;
ALTER INDEX IX_Industry_IsActiveName_IdSEOKey ON [XXX_LBISizeUpData].dbo.Industry REBUILD;
ALTER INDEX IX_Industry_Name ON [XXX_LBISizeUpData].dbo.Industry REBUILD;
ALTER INDEX IX_Industry_SEOKey ON [XXX_LBISizeUpData].dbo.Industry REBUILD;
ALTER INDEX IX_Industry_SEOKeyIsActive ON [XXX_LBISizeUpData].dbo.Industry REBUILD;
ALTER INDEX IX_Industry_SicCode ON [XXX_LBISizeUpData].dbo.Industry REBUILD;
ALTER INDEX IX_Industry_SicCodeIsActive ON [XXX_LBISizeUpData].dbo.Industry REBUILD;

ALTER INDEX PK_IndustryDataByCity ON [XXX_LBISizeUpData].dbo.IndustryDataByCity REBUILD;
ALTER INDEX IX_Generic_IndustryDataByCity_CityId_IndustryId ON [XXX_LBISizeUpData].dbo.IndustryDataByCity REBUILD;
ALTER INDEX IX_Generic_IndustryDataByCity_YearQuarterCityIdIndustryId ON [XXX_LBISizeUpData].dbo.IndustryDataByCity REBUILD;
ALTER INDEX IX_Generic_IndustryDataByCity_YearQuarterIndustryId ON [XXX_LBISizeUpData].dbo.IndustryDataByCity REBUILD;
ALTER INDEX IX_IndustryDataByCity_YearQuarterIndustryIdBestPlacesAttributes_CityId ON [XXX_LBISizeUpData].dbo.IndustryDataByCity REBUILD;
ALTER INDEX IX_IndustryDataByCity_YearQuarterIndustryIdEmployeesPerCapita ON [XXX_LBISizeUpData].dbo.IndustryDataByCity REBUILD;
ALTER INDEX IX_IndustryDataByCity_YearQuarterIndustryIdEmployeesPerCapita_CityId ON [XXX_LBISizeUpData].dbo.IndustryDataByCity REBUILD;
ALTER INDEX IX_IndustryDataByCity_YearQuarterIndustryIdEmployeesPerCapita_IdCityId ON [XXX_LBISizeUpData].dbo.IndustryDataByCity REBUILD;
ALTER INDEX IX_IndustryDataByCity_YearQuarterIndustryIdRevenuePerCapita ON [XXX_LBISizeUpData].dbo.IndustryDataByCity REBUILD;

ALTER INDEX PK_IndustryDataByCounty ON [XXX_LBISizeUpData].dbo.IndustryDataByCounty REBUILD;
ALTER INDEX IX_Generic_IndustryDataByCounty_YearQuarterCountyIdIndustryId ON [XXX_LBISizeUpData].dbo.IndustryDataByCounty REBUILD;
ALTER INDEX IX_Generic_IndustryDataByCounty_YearQuarterIndustryId ON [XXX_LBISizeUpData].dbo.IndustryDataByCounty REBUILD;
ALTER INDEX IX_IndustryDataByCounty_YearQuarterIndustryIdBestPlacesAttributes_CountyId ON [XXX_LBISizeUpData].dbo.IndustryDataByCounty REBUILD;
ALTER INDEX IX_IndustryDataByCounty_YearQuarterIndustryIdCountyIdAverageEmployees ON [XXX_LBISizeUpData].dbo.IndustryDataByCounty REBUILD;
ALTER INDEX IX_IndustryDataByCounty_YearQuarterIndustryIdCountyIdAverageRevenue ON [XXX_LBISizeUpData].dbo.IndustryDataByCounty REBUILD;
ALTER INDEX IX_IndustryDataByCounty_YearQuarterIndustryIdCountyIdEmployeesPerCapita ON [XXX_LBISizeUpData].dbo.IndustryDataByCounty REBUILD;
ALTER INDEX IX_IndustryDataByCounty_YearQuarterIndustryIdCountyIdTotalRevenue ON [XXX_LBISizeUpData].dbo.IndustryDataByCounty REBUILD;
ALTER INDEX IX_IndustryDataByCounty_YearQuarterIndustryIdTurnoverRate ON [XXX_LBISizeUpData].dbo.IndustryDataByCounty REBUILD;
ALTER INDEX IX_IndustryDataByCounty_YearQuarterIndustryIdTurnoverRate_CountyId ON [XXX_LBISizeUpData].dbo.IndustryDataByCounty REBUILD;
ALTER INDEX IX_IndustryDataByCounty_YearQuarterIndustryIdTurnoverRateNetJobChange ON [XXX_LBISizeUpData].dbo.IndustryDataByCounty REBUILD;
ALTER INDEX IX_IndustryDataByCounty_YearQuarterIndustryIdTurnoverRateNetJobChange_CountyId ON [XXX_LBISizeUpData].dbo.IndustryDataByCounty REBUILD;

ALTER INDEX PK_IndustryDataByMetro ON [XXX_LBISizeUpData].dbo.IndustryDataByMetro REBUILD;
ALTER INDEX IX_Generic_IndustryDataByMetro_YearQuarterIndustryId ON [XXX_LBISizeUpData].dbo.IndustryDataByMetro REBUILD;
ALTER INDEX IX_Generic_IndustryDataByMetro_YearQuarterMetroIdIndustryId ON [XXX_LBISizeUpData].dbo.IndustryDataByMetro REBUILD;
ALTER INDEX IX_IndustryDataByMetro_YearQuarterIndustryIdBestPlacesAttributes_MetroId ON [XXX_LBISizeUpData].dbo.IndustryDataByMetro REBUILD;
ALTER INDEX IX_IndustryDataByMetro_YearQuarterIndustryIdTurnoverRate ON [XXX_LBISizeUpData].dbo.IndustryDataByMetro REBUILD;

ALTER INDEX PK_IndustryDataByNation ON [XXX_LBISizeUpData].dbo.IndustryDataByNation REBUILD;
ALTER INDEX IX_Generic_IndustryDataByNation_YearQuarterIndustryId ON [XXX_LBISizeUpData].dbo.IndustryDataByNation REBUILD;

ALTER INDEX PK_IndustryDataByState ON [XXX_LBISizeUpData].dbo.IndustryDataByState REBUILD;
ALTER INDEX IX_Generic_IndustryDataByState_YearQuarterIndustryId ON [XXX_LBISizeUpData].dbo.IndustryDataByState REBUILD;
ALTER INDEX IX_Generic_IndustryDataByState_YearQuarterIndustryIdStateId ON [XXX_LBISizeUpData].dbo.IndustryDataByState REBUILD;
ALTER INDEX IX_IndustryDataByState_YearQuarterIndustryIdBestPlacesAttributes_StateId ON [XXX_LBISizeUpData].dbo.IndustryDataByState REBUILD;

ALTER INDEX PK_IndustryDataByZip ON [XXX_LBISizeUpData].dbo.IndustryDataByZip REBUILD;
ALTER INDEX IX_Generic_IndustryDataByZip_YearQuarterIndustryId ON [XXX_LBISizeUpData].dbo.IndustryDataByZip REBUILD;
ALTER INDEX IX_Generic_IndustryDataByZip_YearQuarterIndustryIdZipCodeId ON [XXX_LBISizeUpData].dbo.IndustryDataByZip REBUILD;
ALTER INDEX IX_IndustryDataByZip_YearQuarterIndustryIdAverageRevenue_ZipCodeId ON [XXX_LBISizeUpData].dbo.IndustryDataByZip REBUILD;
ALTER INDEX IX_IndustryDataByZip_YearQuarterIndustryIdAverageRevenue_ZipCodeIdTotalEmployeesTotalRevenueRevenuePerCapita ON [XXX_LBISizeUpData].dbo.IndustryDataByZip REBUILD;
ALTER INDEX IX_IndustryDataByZip_YearQuarterIndustryIdRevenuePerCapita_ZipCodeId ON [XXX_LBISizeUpData].dbo.IndustryDataByZip REBUILD;
ALTER INDEX IX_IndustryDataByZip_YearQuarterIndustryIdRevenuePerCapita_ZipCodeIdTotalEmployeesTotalRevenueAverageRevenue ON [XXX_LBISizeUpData].dbo.IndustryDataByZip REBUILD;
ALTER INDEX IX_IndustryDataByZip_YearQuarterIndustryIdTotalRevenue_ZipCodeId ON [XXX_LBISizeUpData].dbo.IndustryDataByZip REBUILD;
ALTER INDEX IX_IndustryDataByZip_YearQuarterIndustryIdTotalRevenue_ZipCodeIdTotalEmployeesAverageRevenueRevenuePerCapita ON [XXX_LBISizeUpData].dbo.IndustryDataByZip REBUILD;
ALTER INDEX IX_IndustryDataByZip_ZipCodeIdIndustryId_AdvertisingStuff ON [XXX_LBISizeUpData].dbo.IndustryDataByZip REBUILD;

ALTER TABLE [dbo].[Industry] ADD  CONSTRAINT [PK_Industry] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Business] ADD  CONSTRAINT [PK_Business] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Business]  WITH NOCHECK ADD  CONSTRAINT [FK_Business_ZipCode] FOREIGN KEY([ZipCodeId])
REFERENCES [dbo].[ZipCode] ([Id])
GO
ALTER TABLE [dbo].[Business] NOCHECK CONSTRAINT [FK_Business_ZipCode]
GO
ALTER TABLE [dbo].[Business]  WITH NOCHECK ADD  CONSTRAINT [FK_Business_State] FOREIGN KEY([StateId])
REFERENCES [dbo].[State] ([Id])
GO
ALTER TABLE [dbo].[Business] NOCHECK CONSTRAINT [FK_Business_State]
GO
ALTER TABLE [dbo].[Business]  WITH NOCHECK ADD  CONSTRAINT [FK_Business_Metro] FOREIGN KEY([MetroId])
REFERENCES [dbo].[Metro] ([Id])
GO
ALTER TABLE [dbo].[Business] NOCHECK CONSTRAINT [FK_Business_Metro]
GO
ALTER TABLE [dbo].[Business]  WITH NOCHECK ADD  CONSTRAINT [FK_Business_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[Business] NOCHECK CONSTRAINT [FK_Business_Industry]
GO
ALTER TABLE [dbo].[Business]  WITH NOCHECK ADD  CONSTRAINT [FK_Business_County] FOREIGN KEY([CountyId])
REFERENCES [dbo].[County] ([Id])
GO
ALTER TABLE [dbo].[Business] NOCHECK CONSTRAINT [FK_Business_County]
GO

ALTER TABLE [dbo].[BusinessCityMapping] ADD  CONSTRAINT [PK_BusinessCityMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BusinessCityMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_BusinessCityMapping_Business] FOREIGN KEY([BusinessId])
REFERENCES [dbo].[Business] ([Id])
GO
ALTER TABLE [dbo].[BusinessCityMapping] NOCHECK CONSTRAINT [FK_BusinessCityMapping_Business]
GO
ALTER TABLE [dbo].[BusinessCityMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_BusinessCityMapping_City] FOREIGN KEY([CityId])
REFERENCES [dbo].[City] ([Id])
GO
ALTER TABLE [dbo].[BusinessCityMapping] NOCHECK CONSTRAINT [FK_BusinessCityMapping_City]
GO

ALTER TABLE [dbo].[BusinessDataByCity] ADD  CONSTRAINT [PK_BusinessDataByCity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BusinessDataByCity]  WITH NOCHECK ADD  CONSTRAINT [FK_BusinessDataByCity_Business] FOREIGN KEY([BusinessId])
REFERENCES [dbo].[Business] ([Id])
GO
ALTER TABLE [dbo].[BusinessDataByCity] NOCHECK CONSTRAINT [FK_BusinessDataByCity_Business]
GO
ALTER TABLE [dbo].[BusinessDataByCity]  WITH NOCHECK ADD  CONSTRAINT [FK_BusinessDataByCity_City] FOREIGN KEY([CityId])
REFERENCES [dbo].[City] ([Id])
GO
ALTER TABLE [dbo].[BusinessDataByCity] NOCHECK CONSTRAINT [FK_BusinessDataByCity_City]
GO
ALTER TABLE [dbo].[BusinessDataByCity]  WITH NOCHECK ADD  CONSTRAINT [FK_BusinessDataByCity_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[BusinessDataByCity] NOCHECK CONSTRAINT [FK_BusinessDataByCity_Industry]
GO

ALTER TABLE [dbo].[BusinessDataByCounty] ADD  CONSTRAINT [PK_BusinessDataByCounty] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BusinessDataByCounty]  WITH NOCHECK ADD  CONSTRAINT [FK_BusinessDataByCounty_Business] FOREIGN KEY([BusinessId])
REFERENCES [dbo].[Business] ([Id])
GO
ALTER TABLE [dbo].[BusinessDataByCounty] NOCHECK CONSTRAINT [FK_BusinessDataByCounty_Business]
GO
ALTER TABLE [dbo].[BusinessDataByCounty]  WITH NOCHECK ADD  CONSTRAINT [FK_BusinessDataByCounty_County] FOREIGN KEY([CountyId])
REFERENCES [dbo].[County] ([Id])
GO
ALTER TABLE [dbo].[BusinessDataByCounty] NOCHECK CONSTRAINT [FK_BusinessDataByCounty_County]
GO
ALTER TABLE [dbo].[BusinessDataByCounty]  WITH NOCHECK ADD  CONSTRAINT [FK_BusinessDataByCounty_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[BusinessDataByCounty] NOCHECK CONSTRAINT [FK_BusinessDataByCounty_Industry]
GO
ALTER TABLE [dbo].[BusinessDataByCounty]  WITH NOCHECK ADD  CONSTRAINT [FK_BusinessDataByCounty_Metro] FOREIGN KEY([MetroId])
REFERENCES [dbo].[Metro] ([Id])
GO
ALTER TABLE [dbo].[BusinessDataByCounty] NOCHECK CONSTRAINT [FK_BusinessDataByCounty_Metro]
GO
ALTER TABLE [dbo].[BusinessDataByCounty]  WITH NOCHECK ADD  CONSTRAINT [FK_BusinessDataByCounty_State] FOREIGN KEY([StateId])
REFERENCES [dbo].[State] ([Id])
GO
ALTER TABLE [dbo].[BusinessDataByCounty] NOCHECK CONSTRAINT [FK_BusinessDataByCounty_State]
GO

ALTER TABLE [dbo].[BusinessDataByZip] ADD  CONSTRAINT [PK_BusinessDataByZip] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BusinessDataByZip]  WITH NOCHECK ADD  CONSTRAINT [FK_BusinessDataByZip_Business] FOREIGN KEY([BusinessId])
REFERENCES [dbo].[Business] ([Id])
GO
ALTER TABLE [dbo].[BusinessDataByZip] NOCHECK CONSTRAINT [FK_BusinessDataByZip_Business]
GO
ALTER TABLE [dbo].[BusinessDataByZip]  WITH NOCHECK ADD  CONSTRAINT [FK_BusinessDataByZip_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[BusinessDataByZip] NOCHECK CONSTRAINT [FK_BusinessDataByZip_Industry]
GO
ALTER TABLE [dbo].[BusinessDataByZip]  WITH NOCHECK ADD  CONSTRAINT [FK_BusinessDataByZip_ZipCode] FOREIGN KEY([ZipCodeId])
REFERENCES [dbo].[ZipCode] ([Id])
GO
ALTER TABLE [dbo].[BusinessDataByZip] NOCHECK CONSTRAINT [FK_BusinessDataByZip_ZipCode]
GO

ALTER TABLE [dbo].[LegacyBusinessSEOKey]  WITH NOCHECK ADD  CONSTRAINT [FK_LegacyBusinessSEOKey_Business] FOREIGN KEY([businessId])
REFERENCES [dbo].[Business] ([Id])
GO
ALTER TABLE [dbo].[LegacyBusinessSEOKey] NOCHECK CONSTRAINT [FK_LegacyBusinessSEOKey_Business]
GO




ALTER TABLE [dbo].[IndustryDataByCity] ADD  CONSTRAINT [PK_IndustryDataByCity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
ALTER TABLE [dbo].[IndustryDataByCity]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByCity_City] FOREIGN KEY([CityId])
REFERENCES [dbo].[City] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByCity] NOCHECK CONSTRAINT [FK_IndustryDataByCity_City]
GO
ALTER TABLE [dbo].[IndustryDataByCity]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByCity_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByCity] NOCHECK CONSTRAINT [FK_IndustryDataByCity_Industry]
GO

ALTER TABLE [dbo].[IndustryDataByCounty] ADD  CONSTRAINT [PK_IndustryDataByCounty] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
ALTER TABLE [dbo].[IndustryDataByCounty]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByCounty_County] FOREIGN KEY([CountyId])
REFERENCES [dbo].[County] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByCounty] NOCHECK CONSTRAINT [FK_IndustryDataByCounty_County]
GO
ALTER TABLE [dbo].[IndustryDataByCounty]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByCounty_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByCounty] NOCHECK CONSTRAINT [FK_IndustryDataByCounty_Industry]
GO

ALTER TABLE [dbo].[IndustryDataByMetro] ADD  CONSTRAINT [PK_IndustryDataByMetro] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
ALTER TABLE [dbo].[IndustryDataByMetro]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByMetro_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByMetro] NOCHECK CONSTRAINT [FK_IndustryDataByMetro_Industry]
GO
ALTER TABLE [dbo].[IndustryDataByMetro]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByMetro_Metro] FOREIGN KEY([MetroId])
REFERENCES [dbo].[Metro] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByMetro] NOCHECK CONSTRAINT [FK_IndustryDataByMetro_Metro]
GO

ALTER TABLE [dbo].[IndustryDataByNation] ADD  CONSTRAINT [PK_IndustryDataByNation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
ALTER TABLE [dbo].[IndustryDataByNation]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByNation_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByNation] NOCHECK CONSTRAINT [FK_IndustryDataByNation_Industry]
GO

ALTER TABLE [dbo].[IndustryDataByState] ADD  CONSTRAINT [PK_IndustryDataByState] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
ALTER TABLE [dbo].[IndustryDataByState]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByState_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByState] NOCHECK CONSTRAINT [FK_IndustryDataByState_Industry]
GO
ALTER TABLE [dbo].[IndustryDataByState]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByState_State] FOREIGN KEY([StateId])
REFERENCES [dbo].[State] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByState] NOCHECK CONSTRAINT [FK_IndustryDataByState_State]
GO

ALTER TABLE [dbo].[IndustryDataByZip] ADD  CONSTRAINT [PK_IndustryDataByZip] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
ALTER TABLE [dbo].[IndustryDataByZip]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByZip_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByZip] NOCHECK CONSTRAINT [FK_IndustryDataByZip_Industry]
GO
ALTER TABLE [dbo].[IndustryDataByZip]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByZip_ZipCode] FOREIGN KEY([ZipCodeId])
REFERENCES [dbo].[ZipCode] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByZip] NOCHECK CONSTRAINT [FK_IndustryDataByZip_ZipCode]
GO
ALTER TABLE [dbo].[IndustryKeyword]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryKeyword_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[IndustryKeyword] NOCHECK CONSTRAINT [FK_IndustryKeyword_Industry]
GO
ALTER TABLE [dbo].[LegacyIndustrySEOKey]  WITH NOCHECK ADD  CONSTRAINT [FK_LegacyIndustrySEOKey_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[LegacyIndustrySEOKey] NOCHECK CONSTRAINT [FK_LegacyIndustrySEOKey_Industry]
GO
ALTER TABLE [dbo].[SicToNAICSMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_SicToNAICSMapping_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[SicToNAICSMapping] NOCHECK CONSTRAINT [FK_SicToNAICSMapping_Industry]
GO
ALTER TABLE [dbo].[SicToNAICSMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_SicToNAICSMapping_NAICS] FOREIGN KEY([NAICSId])
REFERENCES [dbo].[NAICS] ([Id])
GO
ALTER TABLE [dbo].[SicToNAICSMapping] NOCHECK CONSTRAINT [FK_SicToNAICSMapping_NAICS]
GO
ALTER TABLE [dbo].[IndustryDataByCityBandMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByCityBandMapping_Band] FOREIGN KEY([BandId])
REFERENCES [dbo].[Band] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByCityBandMapping] NOCHECK CONSTRAINT [FK_IndustryDataByCityBandMapping_Band]
GO
ALTER TABLE [dbo].[IndustryDataByCityBandMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByCityBandMapping_IndustryDataByCity] FOREIGN KEY([IndustryDataByCityId])
REFERENCES [dbo].[IndustryDataByCity] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByCityBandMapping] NOCHECK CONSTRAINT [FK_IndustryDataByCityBandMapping_IndustryDataByCity]
GO
ALTER TABLE [dbo].[IndustryDataByCountyBandMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByCountyBandMapping_Band] FOREIGN KEY([BandId])
REFERENCES [dbo].[Band] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByCountyBandMapping] NOCHECK CONSTRAINT [FK_IndustryDataByCountyBandMapping_Band]
GO
ALTER TABLE [dbo].[IndustryDataByCountyBandMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByCountyBandMapping_IndustryDataByCounty] FOREIGN KEY([IndustryDataByCountyId])
REFERENCES [dbo].[IndustryDataByCounty] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByCountyBandMapping] NOCHECK CONSTRAINT [FK_IndustryDataByCountyBandMapping_IndustryDataByCounty]
GO
ALTER TABLE [dbo].[IndustryDataByMetroBandMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByMetroBandMapping_Band] FOREIGN KEY([BandId])
REFERENCES [dbo].[Band] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByMetroBandMapping] NOCHECK CONSTRAINT [FK_IndustryDataByMetroBandMapping_Band]
GO
ALTER TABLE [dbo].[IndustryDataByMetroBandMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByMetroBandMapping_IndustryDataByMetro] FOREIGN KEY([IndustryDataByMetroId])
REFERENCES [dbo].[IndustryDataByMetro] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByMetroBandMapping] NOCHECK CONSTRAINT [FK_IndustryDataByMetroBandMapping_IndustryDataByMetro]
GO
ALTER TABLE [dbo].[IndustryDataByNationBandMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByNationBandMapping_Band] FOREIGN KEY([BandId])
REFERENCES [dbo].[Band] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByNationBandMapping] NOCHECK CONSTRAINT [FK_IndustryDataByNationBandMapping_Band]
GO
ALTER TABLE [dbo].[IndustryDataByNationBandMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByNationBandMapping_IndustryDataByNation] FOREIGN KEY([IndustryDataByNationId])
REFERENCES [dbo].[IndustryDataByNation] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByNationBandMapping] NOCHECK CONSTRAINT [FK_IndustryDataByNationBandMapping_IndustryDataByNation]
GO
ALTER TABLE [dbo].[IndustryDataByStateBandMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByStateBandMapping_Band] FOREIGN KEY([BandId])
REFERENCES [dbo].[Band] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByStateBandMapping] NOCHECK CONSTRAINT [FK_IndustryDataByStateBandMapping_Band]
GO
ALTER TABLE [dbo].[IndustryDataByStateBandMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByStateBandMapping_IndustryDataByState] FOREIGN KEY([IndustryDataByStateId])
REFERENCES [dbo].[IndustryDataByState] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByStateBandMapping] NOCHECK CONSTRAINT [FK_IndustryDataByStateBandMapping_IndustryDataByState]
GO
ALTER TABLE [dbo].[IndustryDataByZipBandMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByZipBandMapping_Band] FOREIGN KEY([BandId])
REFERENCES [dbo].[Band] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByZipBandMapping] NOCHECK CONSTRAINT [FK_IndustryDataByZipBandMapping_Band]
GO
ALTER TABLE [dbo].[IndustryDataByZipBandMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryDataByZipBandMapping_IndustryDataByZip] FOREIGN KEY([IndustryDataByZipId])
REFERENCES [dbo].[IndustryDataByZip] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByZipBandMapping] NOCHECK CONSTRAINT [FK_IndustryDataByZipBandMapping_IndustryDataByZip]
GO


ALTER DATABASE [XXX_LBISizeUpData] SET RECOVERY FULL WITH NO_WAIT
GO
ALTER DATABASE [XXX_LBISizeUpData] SET RECOVERY FULL 
GO