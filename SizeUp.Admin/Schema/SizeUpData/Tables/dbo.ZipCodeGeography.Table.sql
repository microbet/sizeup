/****** Object:  Table [dbo].[ZipCodeGeography]    Script Date: 11/06/2012 15:28:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ZipCodeGeography](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ZipCodeId] [bigint] NOT NULL,
	[GeographyId] [bigint] NOT NULL,
	[ClassId] [bigint] NOT NULL,
 CONSTRAINT [PK_ZipCodeGeography] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ZipCodeGeography_ClassId_ZipCodeIdGeographyId] ON [dbo].[ZipCodeGeography] 
(
	[ClassId] ASC
)
INCLUDE ( [ZipCodeId],
[GeographyId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ZipCodeGeography_ZipCodeIdClassId_GeographyId] ON [dbo].[ZipCodeGeography] 
(
	[ZipCodeId] ASC,
	[ClassId] ASC
)
INCLUDE ( [GeographyId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ZipCodeGeography]  WITH CHECK ADD  CONSTRAINT [FK_ZipCodeGeography_Geography] FOREIGN KEY([GeographyId])
REFERENCES [dbo].[Geography] ([Id])
GO
ALTER TABLE [dbo].[ZipCodeGeography] CHECK CONSTRAINT [FK_ZipCodeGeography_Geography]
GO
ALTER TABLE [dbo].[ZipCodeGeography]  WITH CHECK ADD  CONSTRAINT [FK_ZipCodeGeography_GeographyClass] FOREIGN KEY([ClassId])
REFERENCES [dbo].[GeographyClass] ([Id])
GO
ALTER TABLE [dbo].[ZipCodeGeography] CHECK CONSTRAINT [FK_ZipCodeGeography_GeographyClass]
GO
ALTER TABLE [dbo].[ZipCodeGeography]  WITH CHECK ADD  CONSTRAINT [FK_ZipCodeGeography_ZipCode] FOREIGN KEY([ZipCodeId])
REFERENCES [dbo].[ZipCode] ([Id])
GO
ALTER TABLE [dbo].[ZipCodeGeography] CHECK CONSTRAINT [FK_ZipCodeGeography_ZipCode]
GO
