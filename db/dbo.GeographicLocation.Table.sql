USE [NewData]
GO
/****** Object:  Table [dbo].[GeographicLocation]    Script Date: 1/25/2018 2:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GeographicLocation](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GranularityId] [bigint] NOT NULL,
	[ShortName] [nvarchar](128) NULL,
	[LongName] [nvarchar](256) NULL,
 CONSTRAINT [PK_GeographicLocation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [FK_Unique_GeographicLocation_Granularity] UNIQUE NONCLUSTERED 
(
	[Id] ASC,
	[GranularityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Index [IX_GeographicLocation_GranularityId_Id]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_GeographicLocation_GranularityId_Id] ON [dbo].[GeographicLocation]
(
	[GranularityId] ASC
)
INCLUDE ( 	[Id]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[GeographicLocation]  WITH CHECK ADD  CONSTRAINT [FK_GeographicLocation_Granularity] FOREIGN KEY([GranularityId])
REFERENCES [dbo].[Granularity] ([Id])
GO
ALTER TABLE [dbo].[GeographicLocation] CHECK CONSTRAINT [FK_GeographicLocation_Granularity]
GO
