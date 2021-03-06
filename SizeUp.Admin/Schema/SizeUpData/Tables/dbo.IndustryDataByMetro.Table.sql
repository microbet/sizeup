/****** Object:  Table [dbo].[IndustryDataByMetro]    Script Date: 11/06/2012 15:28:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IndustryDataByMetro](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Year] [int] NULL,
	[Quarter] [int] NULL,
	[MetroId] [bigint] NULL,
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
 CONSTRAINT [PK_IndustryDataByMetro] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByMetro_YearQuarterMetroIdIndustryIdAverageAnnualSalary] ON [dbo].[IndustryDataByMetro] 
(
	[Year] ASC,
	[Quarter] ASC,
	[MetroId] ASC,
	[IndustryId] ASC,
	[AverageAnnualSalary] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByMetro_YearQuarterMetroIdIndustryIdCostEffectiveness] ON [dbo].[IndustryDataByMetro] 
(
	[Year] ASC,
	[Quarter] ASC,
	[MetroId] ASC,
	[IndustryId] ASC,
	[CostEffectiveness] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[IndustryDataByMetro]  WITH CHECK ADD  CONSTRAINT [FK_IndustryDataByMetro_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByMetro] CHECK CONSTRAINT [FK_IndustryDataByMetro_Industry]
GO
ALTER TABLE [dbo].[IndustryDataByMetro]  WITH CHECK ADD  CONSTRAINT [FK_IndustryDataByMetro_Metro] FOREIGN KEY([MetroId])
REFERENCES [dbo].[Metro] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByMetro] CHECK CONSTRAINT [FK_IndustryDataByMetro_Metro]
GO
