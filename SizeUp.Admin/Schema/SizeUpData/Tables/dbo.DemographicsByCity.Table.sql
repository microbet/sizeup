/****** Object:  Table [dbo].[DemographicsByCity]    Script Date: 11/06/2012 15:28:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DemographicsByCity](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[Quarter] [int] NOT NULL,
	[CityId] [bigint] NOT NULL,
	[AirPortsWithinHalfMile] [int] NULL,
	[AirPortsWithin50Miles] [int] NULL,
	[TradeSchoolsWithinHalfMile] [int] NULL,
	[TradeSchoolsWithin50Miles] [int] NULL,
	[CommunityCollegesWithinHalfMile] [int] NULL,
	[CommunityCollegesWithin50Miles] [int] NULL,
	[UniversitiesWithinHalfMile] [int] NULL,
	[UniversitiesWithin50Miles] [int] NULL,
	[BachelorsOrHigherPercentage] [float] NULL,
	[HighSchoolOrHigherPercentage] [float] NULL,
	[WhiteCollarWorkersPercentage] [float] NULL,
	[BlueCollarWorkersPercentage] [float] NULL,
	[YoungEducatedPercentage] [float] NULL,
	[VeryCreativePercentage] [float] NULL,
	[CreativePercentage] [float] NULL,
	[HomeValue] [bigint] NULL,
	[PropertyTax] [float] NULL,
	[InternationalTalent] [float] NULL,
	[Unemployment] [float] NULL,
	[JobGrowth] [float] NULL,
	[CommuteTime] [float] NULL,
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
 CONSTRAINT [PK_DemographicsByCity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_DemographicsByCity_YearQuarterCityId_Stuff] ON [dbo].[DemographicsByCity] 
(
	[Year] ASC,
	[Quarter] ASC,
	[CityId] ASC
)
INCLUDE ( [UniversitiesWithinHalfMile],
[UniversitiesWithin50Miles],
[BachelorsOrHigherPercentage],
[HighSchoolOrHigherPercentage],
[WhiteCollarWorkersPercentage],
[BlueCollarWorkersPercentage],
[YoungEducatedPercentage],
[VeryCreativePercentage],
[CreativePercentage],
[HomeValue],
[PropertyTax],
[InternationalTalent],
[Unemployment],
[JobGrowth],
[CommuteTime],
[TotalPopulation],
[MedianHouseholdIncome],
[AverageHouseholdExpenditure],
[TotalEstablishments],
[MedianAge],
[LaborForce]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_DemographicsByCity_YearQuarterCityId_TotalPopulation] ON [dbo].[DemographicsByCity] 
(
	[Year] ASC,
	[Quarter] ASC,
	[CityId] ASC
)
INCLUDE ( [TotalPopulation]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DemographicsByCity]  WITH CHECK ADD  CONSTRAINT [FK_DemographicsByCity_City] FOREIGN KEY([CityId])
REFERENCES [dbo].[City] ([Id])
GO
ALTER TABLE [dbo].[DemographicsByCity] CHECK CONSTRAINT [FK_DemographicsByCity_City]
GO
