USE [SizeUp2]
GO
/****** Object:  Table [dbo].[AverageSalaryByCounty]    Script Date: 06/19/2012 22:47:17 ******/
ALTER TABLE [dbo].[AverageSalaryByCounty] DROP CONSTRAINT [FK_AverageSalaryByCounty_County]
GO
ALTER TABLE [dbo].[AverageSalaryByCounty] DROP CONSTRAINT [FK_AverageSalaryByCounty_NAICS]
GO
ALTER TABLE [dbo].[AverageSalaryByCounty] DROP CONSTRAINT [FK_AverageSalaryByCounty_County]
GO
ALTER TABLE [dbo].[AverageSalaryByCounty] DROP CONSTRAINT [FK_AverageSalaryByCounty_NAICS]
GO
DROP TABLE [dbo].[AverageSalaryByCounty]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AverageSalaryByCounty](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Year] [bigint] NULL,
	[NAICSId] [bigint] NULL,
	[CountyId] [bigint] NULL,
	[AverageSalary] [bigint] NULL,
 CONSTRAINT [PK_AverageSalaryByCounty] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_AverageSalary_AverageSalary_YearNAICSId] ON [dbo].[AverageSalaryByCounty] 
(
	[AverageSalary] ASC
)
INCLUDE ( [Year],
[NAICSId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_AverageSalaryByCounty_CountyId_YearNAICSIdAverageSalary] ON [dbo].[AverageSalaryByCounty] 
(
	[CountyId] ASC
)
INCLUDE ( [Year],
[NAICSId],
[AverageSalary]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_AverageSalaryByCounty_NAICSIdAverageSalary_Year] ON [dbo].[AverageSalaryByCounty] 
(
	[NAICSId] ASC,
	[AverageSalary] ASC
)
INCLUDE ( [Year]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AverageSalaryByCounty]  WITH NOCHECK ADD  CONSTRAINT [FK_AverageSalaryByCounty_County] FOREIGN KEY([CountyId])
REFERENCES [dbo].[County] ([Id])
GO
ALTER TABLE [dbo].[AverageSalaryByCounty] CHECK CONSTRAINT [FK_AverageSalaryByCounty_County]
GO
ALTER TABLE [dbo].[AverageSalaryByCounty]  WITH NOCHECK ADD  CONSTRAINT [FK_AverageSalaryByCounty_NAICS] FOREIGN KEY([NAICSId])
REFERENCES [dbo].[NAICS] ([Id])
GO
ALTER TABLE [dbo].[AverageSalaryByCounty] CHECK CONSTRAINT [FK_AverageSalaryByCounty_NAICS]
GO
