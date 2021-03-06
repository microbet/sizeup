/****** Object:  Table [dbo].[IndustryDataByCity]    Script Date: 11/06/2012 15:28:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IndustryDataByCity](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Year] [bigint] NOT NULL,
	[Quarter] [bigint] NOT NULL,
	[CityId] [bigint] NULL,
	[IndustryId] [bigint] NULL,
	[AverageEmployees] [bigint] NULL,
	[EmployeesPerCapita] [float] NULL,
	[TotalRevenue] [bigint] NULL,
	[AverageRevenue] [bigint] NULL,
	[TotalEmployees] [bigint] NULL,
	[RevenuePerCapita] [bigint] NULL,
 CONSTRAINT [PK_IndustryDataByCity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByCity_CityId_IndustryId] ON [dbo].[IndustryDataByCity] 
(
	[CityId] ASC
)
INCLUDE ( [IndustryId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByCity_YearQuarterCityIdIndustryId] ON [dbo].[IndustryDataByCity] 
(
	[Year] ASC,
	[Quarter] ASC,
	[CityId] ASC,
	[IndustryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByCity_YearQuarterCityIdIndustryId_AverageEmployees] ON [dbo].[IndustryDataByCity] 
(
	[Year] ASC,
	[Quarter] ASC,
	[CityId] ASC,
	[IndustryId] ASC
)
INCLUDE ( [AverageEmployees]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByCity_YearQuarterCityIdIndustryId_AverageRevenue] ON [dbo].[IndustryDataByCity] 
(
	[Year] ASC,
	[Quarter] ASC,
	[CityId] ASC,
	[IndustryId] ASC
)
INCLUDE ( [AverageRevenue]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByCity_YearQuarterCityIdIndustryId_EmployeesPerCapita] ON [dbo].[IndustryDataByCity] 
(
	[Year] ASC,
	[Quarter] ASC,
	[CityId] ASC,
	[IndustryId] ASC
)
INCLUDE ( [EmployeesPerCapita]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByCity_YearQuarterCityIdIndustryId_RevenuePerCapita] ON [dbo].[IndustryDataByCity] 
(
	[Year] ASC,
	[Quarter] ASC,
	[CityId] ASC,
	[IndustryId] ASC
)
INCLUDE ( [RevenuePerCapita]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByCity_YearQuarterIndustryId] ON [dbo].[IndustryDataByCity] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByCity_YearQuarterIndustryId_CityId] ON [dbo].[IndustryDataByCity] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC
)
INCLUDE ( [CityId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByCity_YearQuarterIndustryIdEmployeesPerCapita] ON [dbo].[IndustryDataByCity] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[EmployeesPerCapita] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByCity_YearQuarterIndustryIdEmployeesPerCapita_CityId] ON [dbo].[IndustryDataByCity] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[EmployeesPerCapita] ASC
)
INCLUDE ( [CityId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByCity_YearQuarterIndustryIdRevenuePerCapita] ON [dbo].[IndustryDataByCity] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[RevenuePerCapita] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[IndustryDataByCity]  WITH CHECK ADD  CONSTRAINT [FK_IndustryDataByCity_City] FOREIGN KEY([CityId])
REFERENCES [dbo].[City] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByCity] CHECK CONSTRAINT [FK_IndustryDataByCity_City]
GO
ALTER TABLE [dbo].[IndustryDataByCity]  WITH CHECK ADD  CONSTRAINT [FK_IndustryDataByCity_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByCity] CHECK CONSTRAINT [FK_IndustryDataByCity_Industry]
GO
