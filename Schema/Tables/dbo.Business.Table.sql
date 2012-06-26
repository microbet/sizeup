USE [SizeUp2]
GO
/****** Object:  Table [dbo].[Business]    Script Date: 06/19/2012 22:47:22 ******/
ALTER TABLE [dbo].[Business] DROP CONSTRAINT [FK_Business_County]
GO
ALTER TABLE [dbo].[Business] DROP CONSTRAINT [FK_Business_Industry]
GO
ALTER TABLE [dbo].[Business] DROP CONSTRAINT [FK_Business_Metro]
GO
ALTER TABLE [dbo].[Business] DROP CONSTRAINT [FK_Business_State]
GO
ALTER TABLE [dbo].[Business] DROP CONSTRAINT [FK_Business_ZipCode]
GO
ALTER TABLE [dbo].[Business] DROP CONSTRAINT [FK_Business_County]
GO
ALTER TABLE [dbo].[Business] DROP CONSTRAINT [FK_Business_Industry]
GO
ALTER TABLE [dbo].[Business] DROP CONSTRAINT [FK_Business_Metro]
GO
ALTER TABLE [dbo].[Business] DROP CONSTRAINT [FK_Business_State]
GO
ALTER TABLE [dbo].[Business] DROP CONSTRAINT [FK_Business_ZipCode]
GO
DROP TABLE [dbo].[Business]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Business](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](30) NULL,
	[Address] [nvarchar](30) NULL,
	[City] [nvarchar](16) NULL,
	[ZipPlus4] [nvarchar](4) NULL,
	[PrimaryWebURL] [nvarchar](40) NULL,
	[Phone] [nvarchar](10) NULL,
	[BusinessStatusCode] [nvarchar](1) NULL,
	[WorkAtHomeFlag] [nvarchar](1) NULL,
	[PublicCompanyIndicator] [nvarchar](1) NULL,
	[FirmCode] [nvarchar](1) NULL,
	[YearEstablished] [int] NULL,
	[YearAppeared] [int] NULL,
	[InfoGroupId] [bigint] NULL,
	[Lat] [decimal](10, 6) NULL,
	[Long] [decimal](10, 6) NULL,
	[MatchLevel] [nvarchar](1) NULL,
	[IndustryId] [bigint] NULL,
	[ZipCodeId] [bigint] NULL,
	[CountyId] [bigint] NULL,
	[MetroId] [bigint] NULL,
	[StateId] [bigint] NULL,
	[Geography] [geography] NULL,
 CONSTRAINT [PK_Business] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE SPATIAL INDEX [IX_Business_Geography_1H_2H_3H_4H_1] ON [dbo].[Business] 
(
	[Geography]
)USING  GEOGRAPHY_GRID 
WITH (
GRIDS =(LEVEL_1 = HIGH,LEVEL_2 = HIGH,LEVEL_3 = HIGH,LEVEL_4 = HIGH), 
CELLS_PER_OBJECT = 1, PAD_INDEX  = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE SPATIAL INDEX [IX_Business_Geography_1H_2H_3M_4L_4] ON [dbo].[Business] 
(
	[Geography]
)USING  GEOGRAPHY_GRID 
WITH (
GRIDS =(LEVEL_1 = HIGH,LEVEL_2 = HIGH,LEVEL_3 = MEDIUM,LEVEL_4 = LOW), 
CELLS_PER_OBJECT = 4, PAD_INDEX  = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE SPATIAL INDEX [IX_Business_Geography_1H_2M_3L_4L_4] ON [dbo].[Business] 
(
	[Geography]
)USING  GEOGRAPHY_GRID 
WITH (
GRIDS =(LEVEL_1 = HIGH,LEVEL_2 = MEDIUM,LEVEL_3 = LOW,LEVEL_4 = LOW), 
CELLS_PER_OBJECT = 4, PAD_INDEX  = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Business_IndustryId] ON [dbo].[Business] 
(
	[IndustryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Business_IndustryIdBusinessStatusCode_LatLong] ON [dbo].[Business] 
(
	[IndustryId] ASC,
	[BusinessStatusCode] ASC
)
INCLUDE ( [Lat],
[Long]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Business_IndustryIdBusinessStatusCodeLatLong_Geography] ON [dbo].[Business] 
(
	[IndustryId] ASC,
	[BusinessStatusCode] ASC,
	[Lat] ASC,
	[Long] ASC
)
INCLUDE ( [Geography]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Business_IndustryIdBusinessStauscode_AllOtherBusinessColumns] ON [dbo].[Business] 
(
	[IndustryId] ASC,
	[BusinessStatusCode] ASC
)
INCLUDE ( [Id],
[Name],
[Address],
[City],
[PrimaryWebURL],
[Phone],
[WorkAtHomeFlag],
[PublicCompanyIndicator],
[FirmCode],
[YearEstablished],
[YearAppeared],
[Lat],
[Long],
[ZipCodeId],
[StateId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Business]  WITH NOCHECK ADD  CONSTRAINT [FK_Business_County] FOREIGN KEY([CountyId])
REFERENCES [dbo].[County] ([Id])
GO
ALTER TABLE [dbo].[Business] CHECK CONSTRAINT [FK_Business_County]
GO
ALTER TABLE [dbo].[Business]  WITH NOCHECK ADD  CONSTRAINT [FK_Business_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[Business] CHECK CONSTRAINT [FK_Business_Industry]
GO
ALTER TABLE [dbo].[Business]  WITH NOCHECK ADD  CONSTRAINT [FK_Business_Metro] FOREIGN KEY([MetroId])
REFERENCES [dbo].[Metro] ([Id])
GO
ALTER TABLE [dbo].[Business] CHECK CONSTRAINT [FK_Business_Metro]
GO
ALTER TABLE [dbo].[Business]  WITH NOCHECK ADD  CONSTRAINT [FK_Business_State] FOREIGN KEY([StateId])
REFERENCES [dbo].[State] ([Id])
GO
ALTER TABLE [dbo].[Business] CHECK CONSTRAINT [FK_Business_State]
GO
ALTER TABLE [dbo].[Business]  WITH NOCHECK ADD  CONSTRAINT [FK_Business_ZipCode] FOREIGN KEY([ZipCodeId])
REFERENCES [dbo].[ZipCode] ([Id])
GO
ALTER TABLE [dbo].[Business] CHECK CONSTRAINT [FK_Business_ZipCode]
GO
