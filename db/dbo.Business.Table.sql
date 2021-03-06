USE [NewData]
GO
/****** Object:  Table [dbo].[Business]    Script Date: 1/25/2018 2:41:04 PM ******/
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
	[IsActive] [bit] NOT NULL DEFAULT ((0)),
	[InBusiness] [bit] NOT NULL DEFAULT ((1)),
 CONSTRAINT [PK_Business] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Index [IX_Business_InBusinessIsActive_Id]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Business_InBusinessIsActive_Id] ON [dbo].[Business]
(
	[InBusiness] ASC,
	[IsActive] ASC
)
INCLUDE ( 	[Id]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Index [IX_Business_IndustryId]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Business_IndustryId] ON [dbo].[Business]
(
	[IndustryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Business_IndustryIdBusinessStatusCodeLatLong]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Business_IndustryIdBusinessStatusCodeLatLong] ON [dbo].[Business]
(
	[IndustryId] ASC,
	[BusinessStatusCode] ASC,
	[Lat] ASC,
	[Long] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Index [IX_Business_IndustryIdInBusinessIsActive_Id]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Business_IndustryIdInBusinessIsActive_Id] ON [dbo].[Business]
(
	[IndustryId] ASC,
	[InBusiness] ASC,
	[IsActive] ASC
)
INCLUDE ( 	[Id]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Index [IX_Business_InfoGroupId]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Business_InfoGroupId] ON [dbo].[Business]
(
	[InfoGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Index [IX_Business_IsActiveInBusiness_Id]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Business_IsActiveInBusiness_Id] ON [dbo].[Business]
(
	[IsActive] ASC,
	[InBusiness] ASC
)
INCLUDE ( 	[Id]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Business_LatLng]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Business_LatLng] ON [dbo].[Business]
(
	[Lat] ASC,
	[Long] ASC
)
INCLUDE ( 	[Id]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Business_MatchLevelIndustryIdIsActive_CompetitionData]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Business_MatchLevelIndustryIdIsActive_CompetitionData] ON [dbo].[Business]
(
	[MatchLevel] ASC,
	[IndustryId] ASC,
	[IsActive] ASC
)
INCLUDE ( 	[Id],
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
	[StateId],
	[SEOKey]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Business_MatchLevelIndustryIdIsActiveInBusiness]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Business_MatchLevelIndustryIdIsActiveInBusiness] ON [dbo].[Business]
(
	[MatchLevel] ASC,
	[IndustryId] ASC,
	[IsActive] ASC,
	[InBusiness] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Business_MatchLevelIndustryIdIsActiveInBusinessLatLong]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Business_MatchLevelIndustryIdIsActiveInBusinessLatLong] ON [dbo].[Business]
(
	[MatchLevel] ASC,
	[IndustryId] ASC,
	[IsActive] ASC,
	[InBusiness] ASC,
	[Lat] ASC,
	[Long] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Business_Name]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Business_Name] ON [dbo].[Business]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Index [ui_ukBusinessName]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [ui_ukBusinessName] ON [dbo].[Business]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Business]  WITH CHECK ADD  CONSTRAINT [FK_Business_County] FOREIGN KEY([CountyId])
REFERENCES [dbo].[County] ([Id])
GO
ALTER TABLE [dbo].[Business] CHECK CONSTRAINT [FK_Business_County]
GO
ALTER TABLE [dbo].[Business]  WITH NOCHECK ADD  CONSTRAINT [FK_Business_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[Business] CHECK CONSTRAINT [FK_Business_Industry]
GO
ALTER TABLE [dbo].[Business]  WITH CHECK ADD  CONSTRAINT [FK_Business_Metro] FOREIGN KEY([MetroId])
REFERENCES [dbo].[Metro] ([Id])
GO
ALTER TABLE [dbo].[Business] CHECK CONSTRAINT [FK_Business_Metro]
GO
ALTER TABLE [dbo].[Business]  WITH CHECK ADD  CONSTRAINT [FK_Business_State] FOREIGN KEY([StateId])
REFERENCES [dbo].[State] ([Id])
GO
ALTER TABLE [dbo].[Business] CHECK CONSTRAINT [FK_Business_State]
GO
ALTER TABLE [dbo].[Business]  WITH CHECK ADD  CONSTRAINT [FK_Business_ZipCode] FOREIGN KEY([ZipCodeId])
REFERENCES [dbo].[ZipCode] ([Id])
GO
ALTER TABLE [dbo].[Business] CHECK CONSTRAINT [FK_Business_ZipCode]
GO
/****** Object:  Trigger [dbo].[BusinessTrigger]    Script Date: 1/25/2018 2:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[BusinessTrigger] ON  [dbo].[Business]
   AFTER delete 
AS 
BEGIN

declare @number sysname;


DECLARE @sqlstr nvarchar(MAX)
SET @sqlstr = 'Record with id: (' + (SELECT id FROM deleted) + ') was deleted from dbo.[Business]';
DECLARE @s_sub nvarchar(1000)
EXEC msdb.dbo.sp_send_dbmail
      @profile_name = 'ProdProfile',
      @recipients = 'agopinath@gisplanning.com',
      @copy_recipients='sroeder@sizeup.com; monzon@gisplanning.com',
      @subject = 'Record Deleted from dbo.[Business]',
      @body =  @sqlstr
END

GO
/****** Object:  Trigger [dbo].[BusinessUpdateTrigger]    Script Date: 1/25/2018 2:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create trigger [dbo].[BusinessUpdateTrigger] on [dbo].[Business]
after update
	as
Begin
	set nocount on
	declare @Busid varchar;
	declare @BusName nvarchar(MAX);
	declare @sqlstr nvarchar(MAX)

	--create temp table
	declare @updates table (busID varchar, busName nvarchar(max))
	
	--insert changed values into temp table
	insert into @updates(busID,busName) 
	select i.Id, i.Name
	from inserted i
	left join Deleted D on D.id =(select busID from @updates)

	while exists (select 1 from @updates) begin
	select top 1 @Busid = (select busID from @updates),
	@sqlstr = 'Record with id: (' +  (select busID from @updates) + ') and name: ' + (select busName from @updates) +  ' was updated in dbo.[Business]';
	EXEC msdb.dbo.sp_send_dbmail
      @profile_name = 'ProdProfile',
      @recipients = 'agopinath@gisplanning.com',
      @copy_recipients='sroeder@sizeup.com; monzon@gisplanning.com',
      @subject = 'Record Updated in dbo.[Business]',
      @body =  @sqlstr
      delete from @updates where @Busid = busID
	end
end

GO
