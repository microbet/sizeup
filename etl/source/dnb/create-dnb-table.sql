/****** Object:  Table [dbo].[DnB]    Script Date: 9/27/2017 9:17:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DnB](
	[DUNSNumber] [varchar](11) NULL,
	[BusinessName] [varchar](90) NULL,
	[TradestyleName] [varchar](90) NULL,
	[RegisteredAddressIndicator] [varchar](1) NULL,
	[StreetAddress] [varchar](64) NULL,
	[StreetAddress2] [varchar](64) NULL,
	[City] [varchar](30) NULL,
	[State] [varchar](30) NULL,
	[Country] [varchar](20) NULL,
	[CityCode] [varchar](6) NULL,
	[CountyCode] [varchar](3) NULL,
	[StateCode] [varchar](3) NULL,
	[StateAbbreviation] [varchar](4) NULL,
	[CountryCode] [varchar](3) NULL,
	[PostalCode] [varchar](9) NULL,
	[ContinentCode] [varchar](1) NULL,
	[MailingAddress] [varchar](32) NULL,
	[MailingCity] [varchar](30) NULL,
	[MailingCounty] [varchar](30) NULL,
	[MailingState] [varchar](30) NULL,
	[MailingCountry] [varchar](20) NULL,
	[MailingCityCode] [varchar](6) NULL,
	[MailingCountyCode] [varchar](3) NULL,
	[MailingStateCode] [varchar](3) NULL,
	[MailingStateAbbreviation] [varchar](4) NULL,
	[MailingCountryCode] [varchar](3) NULL,
	[MailingPostalCode] [varchar](9) NULL,
	[MailingContinentCode] [varchar](1) NULL,
	[NationalIdentificationNumber] [varchar](16) NULL,
	[NationalIdentificationTypeCode] [varchar](5) NULL,
	[CountryAccessCode] [varchar](4) NULL,
	[TelephoneNumber] [varchar](16) NULL,
	[CableTelexNumber] [varchar](16) NULL,
	[FacsimileNumber] [varchar](16) NULL,
	[CEOName] [varchar](60) NULL,
	[CEOTitle] [varchar](60) NULL,
	[LineOfBusiness] [varchar](41) NULL,
	[SIC1] [varchar](4) NULL,
	[SIC2] [varchar](4) NULL,
	[SIC3] [varchar](4) NULL,
	[SIC4] [varchar](4) NULL,
	[SIC5] [varchar](4) NULL,
	[SIC6] [varchar](4) NULL,
	[PrimaryLocalActivityCode] [varchar](8) NULL,
	[LocalActivityTypeCode] [varchar](3) NULL,
	[YearStarted] [varchar](4) NULL,
	[SalesVolumeLocalCurrency] [varchar](18) NULL,
	[SalesVolumeReliabilityCode] [varchar](1) NULL,
	[SalesVolumeUSDollars] [varchar](15) NULL,
	[CurrencyCode] [varchar](4) NULL,
	[EmployeesHere] [varchar](7) NULL,
	[EmployeesHereReliabilityCode] [varchar](1) NULL,
	[EmployeesTotal] [varchar](7) NULL,
	[EmployeesTotalReliabilityCode] [varchar](1) NULL,
	[PrincipalsIncludedIndicator] [varchar](1) NULL,
	[ImportExportAgentCode] [varchar](1) NULL,
	[LegalStatusCode] [varchar](3) NULL,
	[Filler] [varchar](1) NULL,
	[StatusCode] [varchar](1) NULL,
	[SubsidiaryIndicator] [varchar](1) NULL,
	[PreviousDUNSNumber] [varchar](11) NULL,
	[FullReportDate] [varchar](8) NULL,
	[ParentDUNSNumber] [varchar](11) NULL,
	[ParentBusinessName] [varchar](90) NULL,
	[ParentStreetAddress] [varchar](64) NULL,
	[ParentCityName] [varchar](30) NULL,
	[ParentStateName] [varchar](30) NULL,
	[ParentCountryName] [varchar](20) NULL,
	[ParentCityCode] [varchar](6) NULL,
	[ParentCountyCode] [varchar](3) NULL,
	[ParentStateAbbreviation] [varchar](4) NULL,
	[ParentCountryCode] [varchar](3) NULL,
	[ParentPostalCode] [varchar](9) NULL,
	[ParentContinentCode] [varchar](1) NULL,
	[DomesticUltimateDUNSNumber] [varchar](11) NULL,
	[DomesticUltimateBusinessName] [varchar](90) NULL,
	[DomesticUltimateStreetAddress] [varchar](64) NULL,
	[DomesticUltimateCityName] [varchar](30) NULL,
	[DomesticUltimateStateName] [varchar](30) NULL,
	[DomesticUltimateCityCode] [varchar](6) NULL,
	[DomesticUltimateCountryCode] [varchar](3) NULL,
	[DomesticUltimateStateAbbreviation] [varchar](4) NULL,
	[DomesticUltimatePostalCode] [varchar](9) NULL,
	[GlobalUltimateIndicator] [varchar](1) NULL,
	[GlobalUltimateDUNSNumber] [varchar](11) NULL,
	[GlobalUltimateBusinessName] [varchar](90) NULL,
	[GlobalUltimateStreetAddress] [varchar](64) NULL,
	[GlobalUltimateCityName] [varchar](30) NULL,
	[GlobalUltimateStateName] [varchar](30) NULL,
	[GlobalUltimateCountryName] [varchar](20) NULL,
	[GlobalUltimateCityCode] [varchar](6) NULL,
	[GlobalUltimateCountyCode] [varchar](3) NULL,
	[GlobalUltimateStateAbbreviation] [varchar](4) NULL,
	[GlobalUltimateCountryCode] [varchar](3) NULL,
	[GlobalUltimatePostalCode] [varchar](9) NULL,
	[GlobalUltimateContinentCode] [varchar](1) NULL,
	[NumberOfFamilyMembers] [varchar](5) NULL,
	[DIASCode] [varchar](9) NULL,
	[HierarchyCode] [varchar](2) NULL,
	[Latitude] [varchar](10) NULL,
	[Longitude] [varchar](17) NULL
) ON [PRIMARY]

CREATE INDEX postal_code_index
ON [dbo].[DnB] (PostalCode)

CREATE INDEX state_code_index
ON [dbo].[DnB] (StateCode)

CREATE INDEX county_code_index
ON [dbo].[DnB] (CountyCode)

CREATE INDEX city_code_index
ON [dbo].[DnB] (CityCode)

GO


