/****** Object:  Table [dbo].[IndustryDataByState]    Script Date: 11/06/2012 15:28:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IndustryDataByState](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Year] [int] NULL,
	[Quarter] [int] NULL,
	[StateId] [bigint] NULL,
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
	[WorkersCompRank] [int] NULL,
	[WorkersComp] [float] NULL,
	[HealthcareByState] [bigint] NULL,
	[Healthcare0To9Employees] [bigint] NULL,
	[Healthcare10To24Employees] [bigint] NULL,
	[Healthcare25To99Employees] [bigint] NULL,
	[Healthcare100To999Employees] [bigint] NULL,
	[Healthcare1000orMoreEmployees] [bigint] NULL,
	[HealthcareByIndustry] [bigint] NULL,
	[HealthcareByStateRank] [int] NULL,
	[Healthcare0To9EmployeesRank] [int] NULL,
	[Healthcare10To24EmployeesRank] [int] NULL,
	[Healthcare25To99EmployeesRank] [int] NULL,
	[Healthcare100To999EmployeesRank] [int] NULL,
	[Healthcare1000orMoreEmployeesRank] [int] NULL,
	[HealthcareByIndustryRank] [int] NULL,
 CONSTRAINT [PK_IndustryDataByState] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByState_YearQuarterIndustryIdAverageAnnualSalary] ON [dbo].[IndustryDataByState] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[AverageAnnualSalary] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByState_YearQuarterIndustryIdStateIdAverageAnnualSalary] ON [dbo].[IndustryDataByState] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[StateId] ASC,
	[AverageAnnualSalary] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByState_YearQuarterStateIdIndustryIdCostEffectiveness] ON [dbo].[IndustryDataByState] 
(
	[Year] ASC,
	[Quarter] ASC,
	[StateId] ASC,
	[IndustryId] ASC,
	[CostEffectiveness] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[IndustryDataByState]  WITH CHECK ADD  CONSTRAINT [FK_IndustryDataByState_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByState] CHECK CONSTRAINT [FK_IndustryDataByState_Industry]
GO
ALTER TABLE [dbo].[IndustryDataByState]  WITH CHECK ADD  CONSTRAINT [FK_IndustryDataByState_State] FOREIGN KEY([StateId])
REFERENCES [dbo].[State] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByState] CHECK CONSTRAINT [FK_IndustryDataByState_State]
GO
