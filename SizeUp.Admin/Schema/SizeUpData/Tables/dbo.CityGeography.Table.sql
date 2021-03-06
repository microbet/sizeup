/****** Object:  Table [dbo].[CityGeography]    Script Date: 11/06/2012 15:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CityGeography](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CityId] [bigint] NOT NULL,
	[GeographyId] [bigint] NOT NULL,
	[ClassId] [bigint] NOT NULL,
 CONSTRAINT [PK_CityGeography] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CityGeography_CityId] ON [dbo].[CityGeography] 
(
	[CityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CityGeography_CityId_GeographyIdClassId] ON [dbo].[CityGeography] 
(
	[CityId] ASC
)
INCLUDE ( [GeographyId],
[ClassId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CityGeography_CityIdClassId] ON [dbo].[CityGeography] 
(
	[CityId] ASC,
	[ClassId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CityGeography_CityIdClassId_GeographyId] ON [dbo].[CityGeography] 
(
	[CityId] ASC,
	[ClassId] ASC
)
INCLUDE ( [GeographyId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CityGeography]  WITH CHECK ADD  CONSTRAINT [FK_CityGeography_City] FOREIGN KEY([CityId])
REFERENCES [dbo].[City] ([Id])
GO
ALTER TABLE [dbo].[CityGeography] CHECK CONSTRAINT [FK_CityGeography_City]
GO
ALTER TABLE [dbo].[CityGeography]  WITH CHECK ADD  CONSTRAINT [FK_CityGeography_Geography] FOREIGN KEY([GeographyId])
REFERENCES [dbo].[Geography] ([Id])
GO
ALTER TABLE [dbo].[CityGeography] CHECK CONSTRAINT [FK_CityGeography_Geography]
GO
ALTER TABLE [dbo].[CityGeography]  WITH CHECK ADD  CONSTRAINT [FK_CityGeography_GeographyClass] FOREIGN KEY([ClassId])
REFERENCES [dbo].[GeographyClass] ([Id])
GO
ALTER TABLE [dbo].[CityGeography] CHECK CONSTRAINT [FK_CityGeography_GeographyClass]
GO
