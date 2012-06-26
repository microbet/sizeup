USE [SizeUp2]
GO
/****** Object:  Table [dbo].[LaborDynamicsByCounty]    Script Date: 06/19/2012 22:47:35 ******/
ALTER TABLE [dbo].[LaborDynamicsByCounty] DROP CONSTRAINT [FK_LaborDynamicsByCounty_County]
GO
ALTER TABLE [dbo].[LaborDynamicsByCounty] DROP CONSTRAINT [FK_LaborDynamicsByCounty_Metro]
GO
ALTER TABLE [dbo].[LaborDynamicsByCounty] DROP CONSTRAINT [FK_LaborDynamicsByCounty_NAICS]
GO
ALTER TABLE [dbo].[LaborDynamicsByCounty] DROP CONSTRAINT [FK_LaborDynamicsByCounty_State]
GO
ALTER TABLE [dbo].[LaborDynamicsByCounty] DROP CONSTRAINT [FK_LaborDynamicsByCounty_County]
GO
ALTER TABLE [dbo].[LaborDynamicsByCounty] DROP CONSTRAINT [FK_LaborDynamicsByCounty_Metro]
GO
ALTER TABLE [dbo].[LaborDynamicsByCounty] DROP CONSTRAINT [FK_LaborDynamicsByCounty_NAICS]
GO
ALTER TABLE [dbo].[LaborDynamicsByCounty] DROP CONSTRAINT [FK_LaborDynamicsByCounty_State]
GO
DROP TABLE [dbo].[LaborDynamicsByCounty]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LaborDynamicsByCounty](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Year] [smallint] NULL,
	[Quarter] [smallint] NULL,
	[CountyId] [bigint] NULL,
	[StateId] [bigint] NULL,
	[MetroId] [bigint] NULL,
	[NAICSId] [bigint] NULL,
	[JobGains] [bigint] NULL,
	[JobLosses] [bigint] NULL,
	[NetJobChange] [bigint] NULL,
	[Hires] [bigint] NULL,
	[Separations] [bigint] NULL,
	[Turnover] [float] NULL,
 CONSTRAINT [PK_LaborDynamicsByCounty] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_LaborDynamicsByCounty_NAICSId_YearQuarterCountyIdTurnover] ON [dbo].[LaborDynamicsByCounty] 
(
	[NAICSId] ASC
)
INCLUDE ( [Year],
[Quarter],
[CountyId],
[Turnover]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_LaborDynamicsByCounty_YearQuarter] ON [dbo].[LaborDynamicsByCounty] 
(
	[Year] ASC,
	[Quarter] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[LaborDynamicsByCounty]  WITH NOCHECK ADD  CONSTRAINT [FK_LaborDynamicsByCounty_County] FOREIGN KEY([CountyId])
REFERENCES [dbo].[County] ([Id])
GO
ALTER TABLE [dbo].[LaborDynamicsByCounty] CHECK CONSTRAINT [FK_LaborDynamicsByCounty_County]
GO
ALTER TABLE [dbo].[LaborDynamicsByCounty]  WITH NOCHECK ADD  CONSTRAINT [FK_LaborDynamicsByCounty_Metro] FOREIGN KEY([MetroId])
REFERENCES [dbo].[Metro] ([Id])
GO
ALTER TABLE [dbo].[LaborDynamicsByCounty] CHECK CONSTRAINT [FK_LaborDynamicsByCounty_Metro]
GO
ALTER TABLE [dbo].[LaborDynamicsByCounty]  WITH NOCHECK ADD  CONSTRAINT [FK_LaborDynamicsByCounty_NAICS] FOREIGN KEY([NAICSId])
REFERENCES [dbo].[NAICS] ([Id])
GO
ALTER TABLE [dbo].[LaborDynamicsByCounty] CHECK CONSTRAINT [FK_LaborDynamicsByCounty_NAICS]
GO
ALTER TABLE [dbo].[LaborDynamicsByCounty]  WITH NOCHECK ADD  CONSTRAINT [FK_LaborDynamicsByCounty_State] FOREIGN KEY([StateId])
REFERENCES [dbo].[State] ([Id])
GO
ALTER TABLE [dbo].[LaborDynamicsByCounty] CHECK CONSTRAINT [FK_LaborDynamicsByCounty_State]
GO
