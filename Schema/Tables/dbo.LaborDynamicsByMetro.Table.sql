USE [SizeUp2]
GO
/****** Object:  Table [dbo].[LaborDynamicsByMetro]    Script Date: 06/19/2012 22:47:36 ******/
ALTER TABLE [dbo].[LaborDynamicsByMetro] DROP CONSTRAINT [FK_LaborDynamicsByMetro_Metro]
GO
ALTER TABLE [dbo].[LaborDynamicsByMetro] DROP CONSTRAINT [FK_LaborDynamicsByMetro_NAICS]
GO
ALTER TABLE [dbo].[LaborDynamicsByMetro] DROP CONSTRAINT [FK_LaborDynamicsByMetro_Metro]
GO
ALTER TABLE [dbo].[LaborDynamicsByMetro] DROP CONSTRAINT [FK_LaborDynamicsByMetro_NAICS]
GO
DROP TABLE [dbo].[LaborDynamicsByMetro]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LaborDynamicsByMetro](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Year] [smallint] NULL,
	[Quarter] [smallint] NULL,
	[MetroId] [bigint] NULL,
	[NAICSId] [bigint] NULL,
	[JobGains] [bigint] NULL,
	[JobLosses] [bigint] NULL,
	[NetJobChange] [bigint] NULL,
	[Hires] [bigint] NULL,
	[Separations] [bigint] NULL,
	[Turnover] [float] NULL,
 CONSTRAINT [PK_LaborDynamicsByMetro] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_LaborDynamicsByMetro_NAICSId_YearQuarterMetroIdHiresSeperationsTurnover] ON [dbo].[LaborDynamicsByMetro] 
(
	[NAICSId] ASC
)
INCLUDE ( [Year],
[Quarter],
[MetroId],
[Hires],
[Separations],
[Turnover]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_LaborDynamicsByMetro_Year_Quarter] ON [dbo].[LaborDynamicsByMetro] 
(
	[Year] ASC
)
INCLUDE ( [Quarter]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[LaborDynamicsByMetro]  WITH NOCHECK ADD  CONSTRAINT [FK_LaborDynamicsByMetro_Metro] FOREIGN KEY([MetroId])
REFERENCES [dbo].[Metro] ([Id])
GO
ALTER TABLE [dbo].[LaborDynamicsByMetro] CHECK CONSTRAINT [FK_LaborDynamicsByMetro_Metro]
GO
ALTER TABLE [dbo].[LaborDynamicsByMetro]  WITH NOCHECK ADD  CONSTRAINT [FK_LaborDynamicsByMetro_NAICS] FOREIGN KEY([NAICSId])
REFERENCES [dbo].[NAICS] ([Id])
GO
ALTER TABLE [dbo].[LaborDynamicsByMetro] CHECK CONSTRAINT [FK_LaborDynamicsByMetro_NAICS]
GO
