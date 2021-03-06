/****** Object:  Table [dbo].[IndustryDataByZip]    Script Date: 11/06/2012 15:28:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IndustryDataByZip](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Year] [bigint] NOT NULL,
	[Quarter] [bigint] NOT NULL,
	[ZipCodeId] [bigint] NULL,
	[IndustryId] [bigint] NULL,
	[TotalEmployees] [bigint] NULL,
	[AverageEmployees] [bigint] NULL,
	[EmployeesPerCapita] [float] NULL,
	[TotalRevenue] [bigint] NULL,
	[AverageRevenue] [bigint] NULL,
	[RevenuePerCapita] [bigint] NULL,
 CONSTRAINT [PK_IndustryDataByZip] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByZip_AverageRevenue] ON [dbo].[IndustryDataByZip] 
(
	[AverageRevenue] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByZip_TotalRevenue] ON [dbo].[IndustryDataByZip] 
(
	[TotalRevenue] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByZip_YearQuarterIndustryId_ZipCodeIdAverageRevenue] ON [dbo].[IndustryDataByZip] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC
)
INCLUDE ( [ZipCodeId],
[AverageRevenue]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByZip_YearQuarterIndustryIdAverageEmployees_ZipCodeId] ON [dbo].[IndustryDataByZip] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[AverageEmployees] ASC
)
INCLUDE ( [ZipCodeId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByZip_YearQuarterIndustryIdAverageRevenue_ZipCodeId] ON [dbo].[IndustryDataByZip] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[AverageRevenue] ASC
)
INCLUDE ( [ZipCodeId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByZip_YearQuarterIndustryIdEmployeesPerCapita_ZipCodeId] ON [dbo].[IndustryDataByZip] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[EmployeesPerCapita] ASC
)
INCLUDE ( [ZipCodeId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByZip_YearQuarterZipCodeIdIndustryId_TotalEmployeesTotalRevenueAverageRevenueRevenuePerCapita] ON [dbo].[IndustryDataByZip] 
(
	[Year] ASC,
	[Quarter] ASC,
	[ZipCodeId] ASC,
	[IndustryId] ASC
)
INCLUDE ( [TotalEmployees],
[TotalRevenue],
[AverageRevenue],
[RevenuePerCapita]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryDataByZip_YearQuarterZipCodeIdIndustryId_TotalRevenue] ON [dbo].[IndustryDataByZip] 
(
	[Year] ASC,
	[Quarter] ASC,
	[ZipCodeId] ASC,
	[IndustryId] ASC
)
INCLUDE ( [TotalRevenue]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[IndustryDataByZip]  WITH CHECK ADD  CONSTRAINT [FK_IndustryDataByZip_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByZip] CHECK CONSTRAINT [FK_IndustryDataByZip_Industry]
GO
ALTER TABLE [dbo].[IndustryDataByZip]  WITH CHECK ADD  CONSTRAINT [FK_IndustryDataByZip_ZipCode] FOREIGN KEY([ZipCodeId])
REFERENCES [dbo].[ZipCode] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByZip] CHECK CONSTRAINT [FK_IndustryDataByZip_ZipCode]
GO
