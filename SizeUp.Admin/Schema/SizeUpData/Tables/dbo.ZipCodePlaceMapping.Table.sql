/****** Object:  Table [dbo].[ZipCodePlaceMapping]    Script Date: 11/06/2012 15:28:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ZipCodePlaceMapping](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[PlaceId] [bigint] NOT NULL,
	[ZipCodeId] [bigint] NOT NULL,
 CONSTRAINT [PK_ZipCodePlaceMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ZipCodePlaceMapping_ZipCodeId] ON [dbo].[ZipCodePlaceMapping] 
(
	[ZipCodeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ZipCodePlaceMapping]  WITH CHECK ADD  CONSTRAINT [FK_ZipCodePlaceMapping_CityCountyMapping] FOREIGN KEY([PlaceId])
REFERENCES [dbo].[CityCountyMapping] ([Id])
GO
ALTER TABLE [dbo].[ZipCodePlaceMapping] CHECK CONSTRAINT [FK_ZipCodePlaceMapping_CityCountyMapping]
GO
ALTER TABLE [dbo].[ZipCodePlaceMapping]  WITH CHECK ADD  CONSTRAINT [FK_ZipCodePlaceMapping_ZipCode] FOREIGN KEY([ZipCodeId])
REFERENCES [dbo].[ZipCode] ([Id])
GO
ALTER TABLE [dbo].[ZipCodePlaceMapping] CHECK CONSTRAINT [FK_ZipCodePlaceMapping_ZipCode]
GO
