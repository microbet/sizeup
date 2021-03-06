/****** Object:  Table [dbo].[MetroGeography]    Script Date: 11/06/2012 15:28:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MetroGeography](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[MetroId] [bigint] NOT NULL,
	[GeographyId] [bigint] NOT NULL,
	[ClassId] [bigint] NOT NULL,
 CONSTRAINT [PK_MetroGeography] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[MetroGeography]  WITH CHECK ADD  CONSTRAINT [FK_MetroGeography_Geography] FOREIGN KEY([GeographyId])
REFERENCES [dbo].[Geography] ([Id])
GO
ALTER TABLE [dbo].[MetroGeography] CHECK CONSTRAINT [FK_MetroGeography_Geography]
GO
ALTER TABLE [dbo].[MetroGeography]  WITH CHECK ADD  CONSTRAINT [FK_MetroGeography_GeographyClass] FOREIGN KEY([ClassId])
REFERENCES [dbo].[GeographyClass] ([Id])
GO
ALTER TABLE [dbo].[MetroGeography] CHECK CONSTRAINT [FK_MetroGeography_GeographyClass]
GO
ALTER TABLE [dbo].[MetroGeography]  WITH CHECK ADD  CONSTRAINT [FK_MetroGeography_Metro] FOREIGN KEY([MetroId])
REFERENCES [dbo].[Metro] ([Id])
GO
ALTER TABLE [dbo].[MetroGeography] CHECK CONSTRAINT [FK_MetroGeography_Metro]
GO
