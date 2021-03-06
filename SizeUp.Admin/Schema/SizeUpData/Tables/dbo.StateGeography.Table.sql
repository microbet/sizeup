/****** Object:  Table [dbo].[StateGeography]    Script Date: 11/06/2012 15:28:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StateGeography](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[StateId] [bigint] NOT NULL,
	[GeographyId] [bigint] NOT NULL,
	[ClassId] [bigint] NOT NULL,
 CONSTRAINT [PK_StateGeography] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[StateGeography]  WITH CHECK ADD  CONSTRAINT [FK_StateGeography_Geography] FOREIGN KEY([GeographyId])
REFERENCES [dbo].[Geography] ([Id])
GO
ALTER TABLE [dbo].[StateGeography] CHECK CONSTRAINT [FK_StateGeography_Geography]
GO
ALTER TABLE [dbo].[StateGeography]  WITH CHECK ADD  CONSTRAINT [FK_StateGeography_GeographyClass] FOREIGN KEY([ClassId])
REFERENCES [dbo].[GeographyClass] ([Id])
GO
ALTER TABLE [dbo].[StateGeography] CHECK CONSTRAINT [FK_StateGeography_GeographyClass]
GO
ALTER TABLE [dbo].[StateGeography]  WITH CHECK ADD  CONSTRAINT [FK_StateGeography_State] FOREIGN KEY([StateId])
REFERENCES [dbo].[State] ([Id])
GO
ALTER TABLE [dbo].[StateGeography] CHECK CONSTRAINT [FK_StateGeography_State]
GO
