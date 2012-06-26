USE [SizeUp2]
GO
/****** Object:  Table [dbo].[IndustryKeyword]    Script Date: 06/19/2012 22:47:34 ******/
ALTER TABLE [dbo].[IndustryKeyword] DROP CONSTRAINT [FK_IndustryKeyword_Industry]
GO
ALTER TABLE [dbo].[IndustryKeyword] DROP CONSTRAINT [FK_IndustryKeyword_Industry]
GO
DROP TABLE [dbo].[IndustryKeyword]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IndustryKeyword](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IndustryId] [bigint] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[SortOrder] [int] NOT NULL,
 CONSTRAINT [PK_IndustryKeyword] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IndustryKeyword_Name] ON [dbo].[IndustryKeyword] 
(
	[IndustryId] ASC,
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[IndustryKeyword]  WITH NOCHECK ADD  CONSTRAINT [FK_IndustryKeyword_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[IndustryKeyword] CHECK CONSTRAINT [FK_IndustryKeyword_Industry]
GO
