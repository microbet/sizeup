/****** Object:  Table [dbo].[CountyGeography]    Script Date: 11/06/2012 15:28:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CountyGeography](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CountyId] [bigint] NOT NULL,
	[GeographyId] [bigint] NOT NULL,
	[ClassId] [bigint] NOT NULL,
 CONSTRAINT [PK_CountyGeography] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CountyGeography]  WITH CHECK ADD  CONSTRAINT [FK_CountyGeography_County] FOREIGN KEY([CountyId])
REFERENCES [dbo].[County] ([Id])
GO
ALTER TABLE [dbo].[CountyGeography] CHECK CONSTRAINT [FK_CountyGeography_County]
GO
ALTER TABLE [dbo].[CountyGeography]  WITH CHECK ADD  CONSTRAINT [FK_CountyGeography_Geography] FOREIGN KEY([GeographyId])
REFERENCES [dbo].[Geography] ([Id])
GO
ALTER TABLE [dbo].[CountyGeography] CHECK CONSTRAINT [FK_CountyGeography_Geography]
GO
ALTER TABLE [dbo].[CountyGeography]  WITH CHECK ADD  CONSTRAINT [FK_CountyGeography_GeographyClass] FOREIGN KEY([ClassId])
REFERENCES [dbo].[GeographyClass] ([Id])
GO
ALTER TABLE [dbo].[CountyGeography] CHECK CONSTRAINT [FK_CountyGeography_GeographyClass]
GO
