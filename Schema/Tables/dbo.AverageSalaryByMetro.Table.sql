USE [SizeUp2]
GO
/****** Object:  Table [dbo].[AverageSalaryByMetro]    Script Date: 06/19/2012 22:47:18 ******/
ALTER TABLE [dbo].[AverageSalaryByMetro] DROP CONSTRAINT [FK_AverageSalaryByMetro_Metro]
GO
ALTER TABLE [dbo].[AverageSalaryByMetro] DROP CONSTRAINT [FK_AverageSalaryByMetro_NAICS]
GO
ALTER TABLE [dbo].[AverageSalaryByMetro] DROP CONSTRAINT [FK_AverageSalaryByMetro_Metro]
GO
ALTER TABLE [dbo].[AverageSalaryByMetro] DROP CONSTRAINT [FK_AverageSalaryByMetro_NAICS]
GO
DROP TABLE [dbo].[AverageSalaryByMetro]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AverageSalaryByMetro](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Year] [bigint] NULL,
	[NAICSId] [bigint] NULL,
	[MetroId] [bigint] NULL,
	[AverageSalary] [bigint] NULL,
 CONSTRAINT [PK_AverageSalaryByMetro] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_AverageSalaryByMetro_AverageSalary_YearNAICSIdMetroId] ON [dbo].[AverageSalaryByMetro] 
(
	[AverageSalary] ASC
)
INCLUDE ( [Year],
[NAICSId],
[MetroId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_AverageSalaryByMetro_NAICSIdAverageSalary_YearMetroId] ON [dbo].[AverageSalaryByMetro] 
(
	[NAICSId] ASC,
	[AverageSalary] ASC
)
INCLUDE ( [Year],
[MetroId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AverageSalaryByMetro]  WITH NOCHECK ADD  CONSTRAINT [FK_AverageSalaryByMetro_Metro] FOREIGN KEY([MetroId])
REFERENCES [dbo].[Metro] ([Id])
GO
ALTER TABLE [dbo].[AverageSalaryByMetro] CHECK CONSTRAINT [FK_AverageSalaryByMetro_Metro]
GO
ALTER TABLE [dbo].[AverageSalaryByMetro]  WITH NOCHECK ADD  CONSTRAINT [FK_AverageSalaryByMetro_NAICS] FOREIGN KEY([NAICSId])
REFERENCES [dbo].[NAICS] ([Id])
GO
ALTER TABLE [dbo].[AverageSalaryByMetro] CHECK CONSTRAINT [FK_AverageSalaryByMetro_NAICS]
GO
