/****** Object:  Table [dbo].[Business]    Script Date: 11/06/2012 15:28:43 ******/
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
	[SEOKey] [nvarchar](255) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Business] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Business_BusinessSTatusCodeIndustryIdZipCodeIdCountyId_NameAddressCityStateId] ON [dbo].[Business] 
(
	[BusinessStatusCode] ASC,
	[IndustryId] ASC,
	[ZipCodeId] ASC,
	[CountyId] ASC
)
INCLUDE ( [Name],
[Address],
[City],
[StateId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Business_IndustryId] ON [dbo].[Business] 
(
	[IndustryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Business_IndustryIdBusinessStatusCode_ExtraStuff] ON [dbo].[Business] 
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
CREATE NONCLUSTERED INDEX [IX_Business_IndustryIdBusinessStatusCodeLatLong] ON [dbo].[Business] 
(
	[IndustryId] ASC,
	[BusinessStatusCode] ASC,
	[Lat] ASC,
	[Long] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Business_IndustryIdBusinessStatusCodeZipCodeIdCountyID] ON [dbo].[Business] 
(
	[IndustryId] ASC,
	[BusinessStatusCode] ASC,
	[ZipCodeId] ASC,
	[CountyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Business_IndustryIdIsActive_ExtraStuff] ON [dbo].[Business] 
(
	[IndustryId] ASC,
	[IsActive] ASC
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
CREATE NONCLUSTERED INDEX [IX_Business_IndustryIdIsActive_Id] ON [dbo].[Business] 
(
	[IndustryId] ASC,
	[IsActive] ASC
)
INCLUDE ( [Id]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Business_IndustryIdIsActive_LatLong] ON [dbo].[Business] 
(
	[IndustryId] ASC,
	[IsActive] ASC
)
INCLUDE ( [Lat],
[Long]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Business_IndustryIdZipCodeIdCountyIdBusinessStatusCode] ON [dbo].[Business] 
(
	[ZipCodeId] ASC,
	[CountyId] ASC,
	[IndustryId] ASC,
	[BusinessStatusCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Business_InfoGroupId] ON [dbo].[Business] 
(
	[InfoGroupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Business_IsActive] ON [dbo].[Business] 
(
	[IsActive] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Business_IsActive_IdCountyId] ON [dbo].[Business] 
(
	[IsActive] ASC
)
INCLUDE ( [Id],
[CountyId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Business_Name] ON [dbo].[Business] 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
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
ALTER TABLE [dbo].[Business] ADD  DEFAULT ((0)) FOR [IsActive]
GO
