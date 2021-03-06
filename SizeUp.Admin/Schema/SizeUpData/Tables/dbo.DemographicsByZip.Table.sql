/****** Object:  Table [dbo].[DemographicsByZip]    Script Date: 11/06/2012 15:28:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DemographicsByZip](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[Quarter] [int] NOT NULL,
	[ZipCodeId] [bigint] NOT NULL,
	[BachelorsOrHigherPercentage] [float] NULL,
	[HighSchoolOrHigherPercentage] [float] NULL,
	[WhiteCollarWorkersPercentage] [float] NULL,
	[BlueCollarWorkersPercentage] [float] NULL,
	[TotalPopulation] [bigint] NULL,
	[PopulationAge25PlusBachelorsDegree] [bigint] NULL,
	[PopulationAge25PlusGraduateDegree] [bigint] NULL,
	[PopulationAge25Plus] [bigint] NULL,
	[PopulationAge25PlusHighSchoolGrad] [bigint] NULL,
	[PopulationAge25PlusCollegeNoDiploma] [bigint] NULL,
	[PopulationAge25PlusAssociatesDegree] [bigint] NULL,
	[PopulationWhite] [bigint] NULL,
	[PopulationBlack] [bigint] NULL,
	[PopulationNativeAmerican] [bigint] NULL,
	[PopulationAsian] [bigint] NULL,
	[PopulationHawaiian] [bigint] NULL,
	[PopulationOtherRace] [bigint] NULL,
	[PopulationMixedRace] [bigint] NULL,
	[PopulationHispanic] [bigint] NULL,
	[PopulationNonHispanic] [bigint] NULL,
	[MedianHouseholdIncome] [bigint] NULL,
	[TotalHouseholdExpenditure] [bigint] NULL,
	[AverageHouseholdExpenditure] [float] NULL,
	[MuseumsZoos] [bigint] NULL,
	[Bars] [bigint] NULL,
	[Restaurants] [bigint] NULL,
	[WhiteCollarEmployees] [bigint] NULL,
	[BlueCollarEmployees] [bigint] NULL,
	[TotalEstablishments] [bigint] NULL,
	[EstablishmentsEmployees1to4] [bigint] NULL,
	[EstablishmentsEmployees5to9] [bigint] NULL,
	[EstablishmentsEmployees10to19] [bigint] NULL,
	[MedianAge] [float] NULL,
	[MedianCommuteTime] [float] NULL,
	[AverageCommuteTime] [float] NULL,
	[AveragePropertyTax] [bigint] NULL,
	[AverageAnnualTemperature] [float] NULL,
	[AverageAnnualHighTemperature] [float] NULL,
	[AverageAnnualLowTemperature] [float] NULL,
	[AnnualRainfall] [float] NULL,
	[AnnualSnowfall] [float] NULL,
	[LaborForce] [bigint] NULL,
	[LaborForceAge16Plus] [bigint] NULL,
	[PopulationEmployed] [bigint] NULL,
	[PopulationEmployedCurrentYear] [bigint] NULL,
 CONSTRAINT [PK_DemographicsByZip] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_DemographicsByZip_YearQuarter_Advertising] ON [dbo].[DemographicsByZip] 
(
	[Year] ASC,
	[Quarter] ASC
)
INCLUDE ( [ZipCodeId],
[BachelorsOrHigherPercentage],
[HighSchoolOrHigherPercentage],
[WhiteCollarWorkersPercentage],
[TotalPopulation],
[MedianHouseholdIncome],
[AverageHouseholdExpenditure],
[MedianAge]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_DemographicsByZip_YearQuarter_ZipCodeId] ON [dbo].[DemographicsByZip] 
(
	[Year] ASC,
	[Quarter] ASC
)
INCLUDE ( [ZipCodeId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DemographicsByZip]  WITH CHECK ADD  CONSTRAINT [FK_DemographicsByZip_Zip] FOREIGN KEY([ZipCodeId])
REFERENCES [dbo].[ZipCode] ([Id])
GO
ALTER TABLE [dbo].[DemographicsByZip] CHECK CONSTRAINT [FK_DemographicsByZip_Zip]
GO
