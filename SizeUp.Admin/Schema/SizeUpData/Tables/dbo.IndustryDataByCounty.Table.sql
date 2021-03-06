/****** Object:  Table [dbo].[IndustryDataByCounty]    Script Date: 11/06/2012 15:28:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IndustryDataByCounty](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Year] [int] NULL,
	[Quarter] [int] NULL,
	[CountyId] [bigint] NULL,
	[IndustryId] [bigint] NULL,
	[AverageEmployees] [bigint] NULL,
	[EmployeesPerCapita] [float] NULL,
	[CostEffectiveness] [float] NULL,
	[TotalRevenue] [bigint] NULL,
	[AverageRevenue] [bigint] NULL,
	[TotalEmployees] [bigint] NULL,
	[RevenuePerCapita] [bigint] NULL,
	[AverageAnnualSalary] [bigint] NULL,
	[Hires] [bigint] NULL,
	[Separations] [bigint] NULL,
	[TurnoverRate] [float] NULL,
	[JobGains] [bigint] NULL,
	[JobLosses] [bigint] NULL,
	[NetJobChange] [bigint] NULL,
	[Employment] [bigint] NULL,
 CONSTRAINT [PK_IndustryDataByCounty] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByCounty_YearQuarterCountyIdIndustryId_HiresSeparationsTurnoverRate] ON [dbo].[IndustryDataByCounty] 
(
	[Year] ASC,
	[Quarter] ASC,
	[CountyId] ASC,
	[IndustryId] ASC
)
INCLUDE ( [Hires],
[Separations],
[TurnoverRate]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByCounty_YearQuarterCountyIdIndustryId_JobGainsJobLossesNetJobChange] ON [dbo].[IndustryDataByCounty] 
(
	[Year] ASC,
	[Quarter] ASC,
	[CountyId] ASC,
	[IndustryId] ASC
)
INCLUDE ( [JobGains],
[JobLosses],
[NetJobChange]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByCounty_YearQuarterCountyIdIndustryId_TurnoverRate] ON [dbo].[IndustryDataByCounty] 
(
	[Year] ASC,
	[Quarter] ASC,
	[CountyId] ASC,
	[IndustryId] ASC
)
INCLUDE ( [TurnoverRate]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByCounty_YearQuarterIndustryId] ON [dbo].[IndustryDataByCounty] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByCounty_YearQuarterIndustryIdAverageAnnualSalary_CountyId] ON [dbo].[IndustryDataByCounty] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[AverageAnnualSalary] ASC
)
INCLUDE ( [CountyId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByCounty_YearQuarterIndustryIdAverageEmployees_CountyId] ON [dbo].[IndustryDataByCounty] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[AverageEmployees] ASC
)
INCLUDE ( [CountyId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByCounty_YearQuarterIndustryIdAverageRevenue_CountyId] ON [dbo].[IndustryDataByCounty] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[AverageRevenue] ASC
)
INCLUDE ( [CountyId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByCounty_YearQuarterIndustryIdCostEffectiveness_CountyId] ON [dbo].[IndustryDataByCounty] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[CostEffectiveness] ASC
)
INCLUDE ( [CountyId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByCounty_YearQuarterIndustryIdEmployeesPerCapita_CountyId] ON [dbo].[IndustryDataByCounty] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[EmployeesPerCapita] ASC
)
INCLUDE ( [CountyId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByCounty_YearQuarterIndustryIdRevenuePerCapita_CountyId] ON [dbo].[IndustryDataByCounty] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[RevenuePerCapita] ASC
)
INCLUDE ( [CountyId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByCounty_YearQuarterIndustryIdTotalRevenue_CountyId] ON [dbo].[IndustryDataByCounty] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[TotalRevenue] ASC
)
INCLUDE ( [CountyId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByCounty_YearQuarterIndustryIdTurnoverRate] ON [dbo].[IndustryDataByCounty] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[TurnoverRate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[IndustryDataByCounty]  WITH CHECK ADD  CONSTRAINT [FK_IndustryDataByCounty_County] FOREIGN KEY([CountyId])
REFERENCES [dbo].[County] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByCounty] CHECK CONSTRAINT [FK_IndustryDataByCounty_County]
GO
ALTER TABLE [dbo].[IndustryDataByCounty]  WITH CHECK ADD  CONSTRAINT [FK_IndustryDataByCounty_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByCounty] CHECK CONSTRAINT [FK_IndustryDataByCounty_Industry]
GO
