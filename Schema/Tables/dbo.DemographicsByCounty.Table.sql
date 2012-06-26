USE [SizeUp2]
GO
/****** Object:  Table [dbo].[DemographicsByCounty]    Script Date: 06/19/2012 22:47:30 ******/
DROP TABLE [dbo].[DemographicsByCounty]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DemographicsByCounty](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[CountyId] [bigint] NULL,
	[MetroId] [bigint] NULL,
	[StateId] [bigint] NULL,
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
	[TotalEmployees] [bigint] NULL,
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
	[PopulationEmployed16Plus] [bigint] NULL,
 CONSTRAINT [PK_Demographics] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
