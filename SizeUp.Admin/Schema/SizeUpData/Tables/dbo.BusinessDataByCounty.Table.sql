/****** Object:  Table [dbo].[BusinessDataByCounty]    Script Date: 11/06/2012 15:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BusinessDataByCounty](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[Quarter] [int] NOT NULL,
	[IndustryId] [bigint] NULL,
	[CountyId] [bigint] NULL,
	[MetroId] [bigint] NULL,
	[StateId] [bigint] NOT NULL,
	[BusinessId] [bigint] NOT NULL,
	[Revenue] [bigint] NULL,
	[Employees] [bigint] NULL,
	[YearEstablished] [int] NULL,
	[YearAppeared] [int] NULL,
 CONSTRAINT [PK_BusinessDataByCounty] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_BusinessDataByCounty_YearQuarterIndustryId_YearEstablishedYearAppeared] ON [dbo].[BusinessDataByCounty] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC
)
INCLUDE ( [YearEstablished],
[YearAppeared]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_BusinessDataByCounty_YearQuarterIndustryIdCountyId_YearEstablishedYearAppeared] ON [dbo].[BusinessDataByCounty] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[CountyId] ASC
)
INCLUDE ( [YearEstablished],
[YearAppeared]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_BusinessDataByCounty_YearQuarterIndustryIdCountyIdEmployees] ON [dbo].[BusinessDataByCounty] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[CountyId] ASC,
	[Employees] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_BusinessDataByCounty_YearQuarterIndustryIdEmployees] ON [dbo].[BusinessDataByCounty] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[Employees] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_BusinessDataByCounty_YearQuarterIndustryIdMetroId_YearEstablishedYearAppeared] ON [dbo].[BusinessDataByCounty] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[MetroId] ASC
)
INCLUDE ( [YearEstablished],
[YearAppeared]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_BusinessDataByCounty_YearQuarterIndustryIdMetroIdEmployees] ON [dbo].[BusinessDataByCounty] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[MetroId] ASC,
	[Employees] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_BusinessDataByCounty_YearQuarterIndustryIdRevenue] ON [dbo].[BusinessDataByCounty] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[Revenue] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_BusinessDataByCounty_YearQuarterIndustryIdStateId_YearEstablishedYearAppeared] ON [dbo].[BusinessDataByCounty] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[StateId] ASC
)
INCLUDE ( [YearEstablished],
[YearAppeared]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_BusinessDataByCounty_YearQuarterIndustryIdStateIdEmployees] ON [dbo].[BusinessDataByCounty] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[StateId] ASC,
	[Employees] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BusinessDataByCounty]  WITH CHECK ADD  CONSTRAINT [FK_BusinessDataByCounty_Business] FOREIGN KEY([BusinessId])
REFERENCES [dbo].[Business] ([Id])
GO
ALTER TABLE [dbo].[BusinessDataByCounty] CHECK CONSTRAINT [FK_BusinessDataByCounty_Business]
GO
ALTER TABLE [dbo].[BusinessDataByCounty]  WITH CHECK ADD  CONSTRAINT [FK_BusinessDataByCounty_County] FOREIGN KEY([CountyId])
REFERENCES [dbo].[County] ([Id])
GO
ALTER TABLE [dbo].[BusinessDataByCounty] CHECK CONSTRAINT [FK_BusinessDataByCounty_County]
GO
ALTER TABLE [dbo].[BusinessDataByCounty]  WITH CHECK ADD  CONSTRAINT [FK_BusinessDataByCounty_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[BusinessDataByCounty] CHECK CONSTRAINT [FK_BusinessDataByCounty_Industry]
GO
ALTER TABLE [dbo].[BusinessDataByCounty]  WITH CHECK ADD  CONSTRAINT [FK_BusinessDataByCounty_Metro] FOREIGN KEY([MetroId])
REFERENCES [dbo].[Metro] ([Id])
GO
ALTER TABLE [dbo].[BusinessDataByCounty] CHECK CONSTRAINT [FK_BusinessDataByCounty_Metro]
GO
ALTER TABLE [dbo].[BusinessDataByCounty]  WITH CHECK ADD  CONSTRAINT [FK_BusinessDataByCounty_State] FOREIGN KEY([StateId])
REFERENCES [dbo].[State] ([Id])
GO
ALTER TABLE [dbo].[BusinessDataByCounty] CHECK CONSTRAINT [FK_BusinessDataByCounty_State]
GO
