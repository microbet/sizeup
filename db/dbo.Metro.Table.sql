USE [NewData]
GO
/****** Object:  Table [dbo].[Metro]    Script Date: 1/25/2018 2:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Metro](
	[FIPS] [nvarchar](5) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[SEOKey] [nvarchar](150) NULL,
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_Metro] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Metro_FIPS]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Metro_FIPS] ON [dbo].[Metro]
(
	[FIPS] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Metro]  WITH NOCHECK ADD  CONSTRAINT [FK_Metro_GeographicLocation] FOREIGN KEY([Id])
REFERENCES [dbo].[GeographicLocation] ([Id])
GO
ALTER TABLE [dbo].[Metro] CHECK CONSTRAINT [FK_Metro_GeographicLocation]
GO
