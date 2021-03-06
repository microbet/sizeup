USE [NewData]
GO
/****** Object:  Table [dbo].[Industry]    Script Date: 1/25/2018 2:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Industry](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SicCode] [nvarchar](10) NOT NULL,
	[IndustrySpecificCode] [nvarchar](2) NULL,
	[Name] [nvarchar](255) NOT NULL,
	[SEOKey] [nvarchar](255) NOT NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((0)),
	[IsDisabled] [bit] NOT NULL DEFAULT ((0)),
	[ParentId] [bigint] NULL,
	[NAICSId] [bigint] NULL,
	[NAICS2007Id] [bigint] NULL,
 CONSTRAINT [PK_Industry] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Index [IX_Industry_IsActive_Id]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Industry_IsActive_Id] ON [dbo].[Industry]
(
	[IsActive] ASC
)
INCLUDE ( 	[Id]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Industry_IsActiveIsDisabled_Id]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Industry_IsActiveIsDisabled_Id] ON [dbo].[Industry]
(
	[IsActive] ASC,
	[IsDisabled] ASC
)
INCLUDE ( 	[Id]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Industry_IsActiveName_IdSEOKey]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Industry_IsActiveName_IdSEOKey] ON [dbo].[Industry]
(
	[IsActive] ASC,
	[Name] ASC
)
INCLUDE ( 	[Id],
	[SEOKey]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Industry_NAICSId]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Industry_NAICSId] ON [dbo].[Industry]
(
	[NAICSId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Industry_Name]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Industry_Name] ON [dbo].[Industry]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Industry_ParentId]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Industry_ParentId] ON [dbo].[Industry]
(
	[ParentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Industry_SEOKey]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Industry_SEOKey] ON [dbo].[Industry]
(
	[SEOKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Industry_SEOKeyIsActive]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Industry_SEOKeyIsActive] ON [dbo].[Industry]
(
	[SEOKey] ASC,
	[IsActive] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Industry_SicCode]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Industry_SicCode] ON [dbo].[Industry]
(
	[SicCode] ASC
)
INCLUDE ( 	[IndustrySpecificCode]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Industry_SicCodeIsActive]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_Industry_SicCodeIsActive] ON [dbo].[Industry]
(
	[SicCode] ASC,
	[IsActive] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Industry]  WITH CHECK ADD  CONSTRAINT [FK_Industry_Industry] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[Industry] CHECK CONSTRAINT [FK_Industry_Industry]
GO
ALTER TABLE [dbo].[Industry]  WITH CHECK ADD  CONSTRAINT [FK_industry_NAICS] FOREIGN KEY([NAICSId])
REFERENCES [dbo].[NAICS] ([Id])
GO
ALTER TABLE [dbo].[Industry] CHECK CONSTRAINT [FK_industry_NAICS]
GO
/****** Object:  Trigger [dbo].[IndustryTrigger]    Script Date: 1/25/2018 2:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[IndustryTrigger] ON  [dbo].[Industry]
   AFTER delete 
AS 
BEGIN

declare @number sysname;


DECLARE @sqlstr nvarchar(MAX)
SET @sqlstr = 'Record with id: (' + (SELECT id FROM deleted) + ') was deleted from dbo.[Industry]';
DECLARE @s_sub nvarchar(1000)
EXEC msdb.dbo.sp_send_dbmail
      @profile_name = 'ProdProfile',
      @recipients = 'agopinath@gisplanning.com',
      @copy_recipients='sroeder@sizeup.com; monzon@gisplanning.com',
      @subject = 'Record Deleted from dbo.[Industry]',
      @body =  @sqlstr
END

GO
/****** Object:  Trigger [dbo].[IndustryUpdateTrigger]    Script Date: 1/25/2018 2:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create trigger [dbo].[IndustryUpdateTrigger] on [dbo].[Industry]
after update
	as
Begin
	set nocount on
	declare @Indid varchar;
	declare @IndName nvarchar(MAX);
	declare @sqlstr nvarchar(MAX)

	--create temp table
	declare @updates table (IndID varchar, IndName nvarchar(max))
	
	--insert changed values into temp table
	insert into @updates(IndID,IndName) 
	select i.Id, i.Name
	from inserted i
	left join Deleted D on D.id =(select IndID from @updates)

	while exists (select 1 from @updates) begin
	select top 1 @Indid = (select IndID from @updates),
	@sqlstr = 'Record with id: (' +  (select IndID from @updates) + ') and name: ' + (select IndName from @updates) +  ' was updated in dbo.[Industry]';
	EXEC msdb.dbo.sp_send_dbmail
      @profile_name = 'ProdProfile',
      @recipients = 'agopinath@gisplanning.com',
      @copy_recipients='sroeder@sizeup.com; monzon@gisplanning.com',
      @subject = 'Record Updated in dbo.[Industry]',
      @body =  @sqlstr
      delete from @updates where @Indid = IndID
	end
end

GO
