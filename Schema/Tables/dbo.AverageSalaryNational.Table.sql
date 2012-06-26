USE [SizeUp2]
GO
/****** Object:  Table [dbo].[AverageSalaryNational]    Script Date: 06/19/2012 22:47:21 ******/
ALTER TABLE [dbo].[AverageSalaryNational] DROP CONSTRAINT [FK_AverageSalaryNational_NAICS]
GO
ALTER TABLE [dbo].[AverageSalaryNational] DROP CONSTRAINT [FK_AverageSalaryNational_NAICS]
GO
DROP TABLE [dbo].[AverageSalaryNational]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AverageSalaryNational](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Year] [bigint] NULL,
	[NAICSId] [bigint] NULL,
	[AverageSalary] [bigint] NULL,
 CONSTRAINT [PK_AverageSalaryNational] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AverageSalaryNational]  WITH NOCHECK ADD  CONSTRAINT [FK_AverageSalaryNational_NAICS] FOREIGN KEY([NAICSId])
REFERENCES [dbo].[NAICS] ([Id])
GO
ALTER TABLE [dbo].[AverageSalaryNational] CHECK CONSTRAINT [FK_AverageSalaryNational_NAICS]
GO
