USE [SizeUp2]
GO
/****** Object:  Table [dbo].[LaborDynamicsByState]    Script Date: 06/19/2012 22:47:38 ******/
ALTER TABLE [dbo].[LaborDynamicsByState] DROP CONSTRAINT [FK_LaborDynamicsByState_NAICS]
GO
ALTER TABLE [dbo].[LaborDynamicsByState] DROP CONSTRAINT [FK_LaborDynamicsByState_State]
GO
ALTER TABLE [dbo].[LaborDynamicsByState] DROP CONSTRAINT [FK_LaborDynamicsByState_NAICS]
GO
ALTER TABLE [dbo].[LaborDynamicsByState] DROP CONSTRAINT [FK_LaborDynamicsByState_State]
GO
DROP TABLE [dbo].[LaborDynamicsByState]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LaborDynamicsByState](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Year] [smallint] NULL,
	[Quarter] [smallint] NULL,
	[StateId] [bigint] NULL,
	[NAICSId] [bigint] NULL,
	[JobGains] [bigint] NULL,
	[JobLosses] [bigint] NULL,
	[NetJobChange] [bigint] NULL,
	[Hires] [bigint] NULL,
	[Separations] [bigint] NULL,
	[Employment] [bigint] NULL,
	[Turnover] [float] NULL,
 CONSTRAINT [PK_LaborDynamicsByState] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_LaborDynamicsByState_NAICSId_YearQuarterHiresSeperationsEmploymment] ON [dbo].[LaborDynamicsByState] 
(
	[NAICSId] ASC
)
INCLUDE ( [Year],
[Quarter],
[Hires],
[Separations],
[Employment]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[LaborDynamicsByState]  WITH NOCHECK ADD  CONSTRAINT [FK_LaborDynamicsByState_NAICS] FOREIGN KEY([NAICSId])
REFERENCES [dbo].[NAICS] ([Id])
GO
ALTER TABLE [dbo].[LaborDynamicsByState] CHECK CONSTRAINT [FK_LaborDynamicsByState_NAICS]
GO
ALTER TABLE [dbo].[LaborDynamicsByState]  WITH NOCHECK ADD  CONSTRAINT [FK_LaborDynamicsByState_State] FOREIGN KEY([StateId])
REFERENCES [dbo].[State] ([Id])
GO
ALTER TABLE [dbo].[LaborDynamicsByState] CHECK CONSTRAINT [FK_LaborDynamicsByState_State]
GO
