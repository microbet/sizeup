USE [NewData]
GO
/****** Object:  Table [dbo].[Geography]    Script Date: 1/25/2018 2:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Geography](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Polygon] [geography] NOT NULL,
	[GeographySourceId] [bigint] NOT NULL,
	[GeographyClassId] [bigint] NOT NULL,
	[GeographicLocationId] [bigint] NOT NULL,
	[North] [float] NOT NULL DEFAULT ((0)),
	[South] [float] NOT NULL DEFAULT ((0)),
	[East] [float] NOT NULL DEFAULT ((0)),
	[West] [float] NOT NULL DEFAULT ((0)),
	[CenterLat] [float] NULL,
	[CenterLong] [float] NULL,
 CONSTRAINT [PK_Geography] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Index [IX_Generic_Geography_GeographicLocationId_GeographyClassId]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Generic_Geography_GeographicLocationId_GeographyClassId] ON [dbo].[Geography]
(
	[GeographicLocationId] ASC
)
INCLUDE ( 	[GeographyClassId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Generic_Geography_GeographicLocationIdGeographyClassId_CenterLatCenterLongNorthSouthEastWest]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Generic_Geography_GeographicLocationIdGeographyClassId_CenterLatCenterLongNorthSouthEastWest] ON [dbo].[Geography]
(
	[GeographicLocationId] ASC,
	[GeographyClassId] ASC
)
INCLUDE ( 	[CenterLat],
	[CenterLong],
	[North],
	[South],
	[East],
	[West]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Geography_GeographyClassId_GeographicLocationIdCenterLatCenterLng]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Geography_GeographyClassId_GeographicLocationIdCenterLatCenterLng] ON [dbo].[Geography]
(
	[GeographyClassId] ASC
)
INCLUDE ( 	[GeographicLocationId],
	[CenterLat],
	[CenterLong]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Geography_GeographyClassIdNorthSouthEastWest_IdGeographicLocationId]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Geography_GeographyClassIdNorthSouthEastWest_IdGeographicLocationId] ON [dbo].[Geography]
(
	[GeographyClassId] ASC,
	[North] ASC,
	[South] ASC,
	[East] ASC,
	[West] ASC
)
INCLUDE ( 	[Id],
	[GeographicLocationId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Geography]  WITH CHECK ADD  CONSTRAINT [FK_Geography_GeographicLocation] FOREIGN KEY([GeographicLocationId])
REFERENCES [dbo].[GeographicLocation] ([Id])
GO
ALTER TABLE [dbo].[Geography] CHECK CONSTRAINT [FK_Geography_GeographicLocation]
GO
ALTER TABLE [dbo].[Geography]  WITH CHECK ADD  CONSTRAINT [FK_Geography_GeographyClass] FOREIGN KEY([GeographyClassId])
REFERENCES [dbo].[GeographyClass] ([Id])
GO
ALTER TABLE [dbo].[Geography] CHECK CONSTRAINT [FK_Geography_GeographyClass]
GO
ALTER TABLE [dbo].[Geography]  WITH CHECK ADD  CONSTRAINT [FK_Geography_GeographySource] FOREIGN KEY([GeographySourceId])
REFERENCES [dbo].[GeographySource] ([Id])
GO
ALTER TABLE [dbo].[Geography] CHECK CONSTRAINT [FK_Geography_GeographySource]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF

GO
/****** Object:  Index [IX_Geography_Spatial_1H2H3H4H_256]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE SPATIAL INDEX [IX_Geography_Spatial_1H2H3H4H_256] ON [dbo].[Geography]
(
	[Polygon]
)USING  GEOGRAPHY_GRID 
WITH (GRIDS =(LEVEL_1 = HIGH,LEVEL_2 = HIGH,LEVEL_3 = HIGH,LEVEL_4 = HIGH), 
CELLS_PER_OBJECT = 256, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF

GO
/****** Object:  Index [IX_Geography_Spatial_1H2H3L4L_16]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE SPATIAL INDEX [IX_Geography_Spatial_1H2H3L4L_16] ON [dbo].[Geography]
(
	[Polygon]
)USING  GEOGRAPHY_GRID 
WITH (GRIDS =(LEVEL_1 = HIGH,LEVEL_2 = HIGH,LEVEL_3 = LOW,LEVEL_4 = LOW), 
CELLS_PER_OBJECT = 16, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF

GO
/****** Object:  Index [IX_Geography_Spatial_1L2L3L4H_512]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE SPATIAL INDEX [IX_Geography_Spatial_1L2L3L4H_512] ON [dbo].[Geography]
(
	[Polygon]
)USING  GEOGRAPHY_GRID 
WITH (GRIDS =(LEVEL_1 = LOW,LEVEL_2 = LOW,LEVEL_3 = LOW,LEVEL_4 = HIGH), 
CELLS_PER_OBJECT = 512, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF

GO
/****** Object:  Index [IX_Geography_Spatial_1M2M3M4M_16]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE SPATIAL INDEX [IX_Geography_Spatial_1M2M3M4M_16] ON [dbo].[Geography]
(
	[Polygon]
)USING  GEOGRAPHY_GRID 
WITH (GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
