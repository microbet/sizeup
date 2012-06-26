USE [SizeUp2]
GO
/****** Object:  Table [dbo].[CityCountyMapping]    Script Date: 06/19/2012 22:47:25 ******/
ALTER TABLE [dbo].[CityCountyMapping] DROP CONSTRAINT [FK_CityCountyMapping_City]
GO
ALTER TABLE [dbo].[CityCountyMapping] DROP CONSTRAINT [FK_CityCountyMapping_County]
GO
ALTER TABLE [dbo].[CityCountyMapping] DROP CONSTRAINT [FK_CityCountyMapping_City]
GO
ALTER TABLE [dbo].[CityCountyMapping] DROP CONSTRAINT [FK_CityCountyMapping_County]
GO
DROP TABLE [dbo].[CityCountyMapping]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CityCountyMapping](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CityId] [bigint] NOT NULL,
	[CountyId] [bigint] NOT NULL,
 CONSTRAINT [PK_CityCountyMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CityCountyMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_CityCountyMapping_City] FOREIGN KEY([CityId])
REFERENCES [dbo].[City] ([Id])
GO
ALTER TABLE [dbo].[CityCountyMapping] CHECK CONSTRAINT [FK_CityCountyMapping_City]
GO
ALTER TABLE [dbo].[CityCountyMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_CityCountyMapping_County] FOREIGN KEY([CountyId])
REFERENCES [dbo].[County] ([Id])
GO
ALTER TABLE [dbo].[CityCountyMapping] CHECK CONSTRAINT [FK_CityCountyMapping_County]
GO
