USE [NewData]
GO
/****** Object:  Table [dbo].[BusinessData]    Script Date: 1/25/2018 2:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BusinessData](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[Quarter] [int] NOT NULL,
	[IndustryId] [bigint] NULL,
	[GeographicLocationId] [bigint] NOT NULL,
	[BusinessId] [bigint] NOT NULL,
	[Revenue] [bigint] NULL,
	[Employees] [bigint] NULL,
	[YearStarted] [int] NULL,
 CONSTRAINT [PK_BusinessData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Index [IX_BusinessData_Generic_YearQuarterBusinessId]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_BusinessData_Generic_YearQuarterBusinessId] ON [dbo].[BusinessData]
(
	[Year] ASC,
	[Quarter] ASC,
	[BusinessId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessData_Generic_YearQuarterIndustryId]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_BusinessData_Generic_YearQuarterIndustryId] ON [dbo].[BusinessData]
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessData_Generic_YearQuarterIndustryIdGeographicLocationId]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_BusinessData_Generic_YearQuarterIndustryIdGeographicLocationId] ON [dbo].[BusinessData]
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[GeographicLocationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessData_GeographicLocationId]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_BusinessData_GeographicLocationId] ON [dbo].[BusinessData]
(
	[GeographicLocationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessData_GeographicLocationId_YearQuarterIndustryIdBusinessIdRevenueEmployeesYearStarted]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_BusinessData_GeographicLocationId_YearQuarterIndustryIdBusinessIdRevenueEmployeesYearStarted] ON [dbo].[BusinessData]
(
	[GeographicLocationId] ASC
)
INCLUDE ( 	[Year],
	[Quarter],
	[IndustryId],
	[BusinessId],
	[Revenue],
	[Employees],
	[YearStarted]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessData_GeographicLocationId_YearQuarterIndustryIdRevenueEmployees]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_BusinessData_GeographicLocationId_YearQuarterIndustryIdRevenueEmployees] ON [dbo].[BusinessData]
(
	[GeographicLocationId] ASC
)
INCLUDE ( 	[Year],
	[Quarter],
	[IndustryId],
	[Revenue],
	[Employees]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessData_IndustryId_GeographicLocationIdBusinessId]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_BusinessData_IndustryId_GeographicLocationIdBusinessId] ON [dbo].[BusinessData]
(
	[IndustryId] ASC
)
INCLUDE ( 	[GeographicLocationId],
	[BusinessId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessData_IndustryId_YearQuarterGeographicLocationIdRevenueEmployees]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_BusinessData_IndustryId_YearQuarterGeographicLocationIdRevenueEmployees] ON [dbo].[BusinessData]
(
	[IndustryId] ASC
)
INCLUDE ( 	[Year],
	[Quarter],
	[GeographicLocationId],
	[Revenue],
	[Employees]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessData_IndustryIdGeographicLocationId_BusinessId]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_BusinessData_IndustryIdGeographicLocationId_BusinessId] ON [dbo].[BusinessData]
(
	[IndustryId] ASC,
	[GeographicLocationId] ASC
)
INCLUDE ( 	[BusinessId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessData_IndustryIdGeographicLocationId_YearQuarterEmployees]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_BusinessData_IndustryIdGeographicLocationId_YearQuarterEmployees] ON [dbo].[BusinessData]
(
	[IndustryId] ASC,
	[GeographicLocationId] ASC
)
INCLUDE ( 	[Year],
	[Quarter],
	[Employees]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessData_IndustryIdGeographicLocationId_YearQuarterRevenue]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_BusinessData_IndustryIdGeographicLocationId_YearQuarterRevenue] ON [dbo].[BusinessData]
(
	[IndustryId] ASC,
	[GeographicLocationId] ASC
)
INCLUDE ( 	[Year],
	[Quarter],
	[Revenue]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessData_IndustryIdGeographicLocationId_YearStarted]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_BusinessData_IndustryIdGeographicLocationId_YearStarted] ON [dbo].[BusinessData]
(
	[IndustryId] ASC,
	[GeographicLocationId] ASC
)
INCLUDE ( 	[YearStarted]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessData_YearQuarter_IndustryIdGeographicLocationIdBusinessId]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_BusinessData_YearQuarter_IndustryIdGeographicLocationIdBusinessId] ON [dbo].[BusinessData]
(
	[Year] ASC,
	[Quarter] ASC
)
INCLUDE ( 	[IndustryId],
	[GeographicLocationId],
	[BusinessId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessData_YearQuarterGeographicLocationId_IndustryIdRevenue]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_BusinessData_YearQuarterGeographicLocationId_IndustryIdRevenue] ON [dbo].[BusinessData]
(
	[Year] ASC,
	[Quarter] ASC,
	[GeographicLocationId] ASC
)
INCLUDE ( 	[IndustryId],
	[Revenue]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessData_YearQuarterIndustryId_GeographicLocationIdBusinessId]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_BusinessData_YearQuarterIndustryId_GeographicLocationIdBusinessId] ON [dbo].[BusinessData]
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC
)
INCLUDE ( 	[GeographicLocationId],
	[BusinessId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessData_YearQuarterIndustryIdGeographicLocationId_BusinessIdYearStarted]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_BusinessData_YearQuarterIndustryIdGeographicLocationId_BusinessIdYearStarted] ON [dbo].[BusinessData]
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[GeographicLocationId] ASC
)
INCLUDE ( 	[BusinessId],
	[YearStarted]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessData_YearQuarterIndustryIdGeographicLocationIdRevenue_BusinessId]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_BusinessData_YearQuarterIndustryIdGeographicLocationIdRevenue_BusinessId] ON [dbo].[BusinessData]
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[GeographicLocationId] ASC,
	[Revenue] ASC
)
INCLUDE ( 	[BusinessId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessData_YearQuarterYearStarted_IndustryIdGeographicLocationIdBusinessId]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_BusinessData_YearQuarterYearStarted_IndustryIdGeographicLocationIdBusinessId] ON [dbo].[BusinessData]
(
	[Year] ASC,
	[Quarter] ASC,
	[YearStarted] ASC
)
INCLUDE ( 	[IndustryId],
	[GeographicLocationId],
	[BusinessId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BusinessData]  WITH CHECK ADD  CONSTRAINT [FK_BusinessData_Business] FOREIGN KEY([BusinessId])
REFERENCES [dbo].[Business] ([Id])
GO
ALTER TABLE [dbo].[BusinessData] CHECK CONSTRAINT [FK_BusinessData_Business]
GO
ALTER TABLE [dbo].[BusinessData]  WITH CHECK ADD  CONSTRAINT [FK_BusinessData_GeographicLocation] FOREIGN KEY([GeographicLocationId])
REFERENCES [dbo].[GeographicLocation] ([Id])
GO
ALTER TABLE [dbo].[BusinessData] CHECK CONSTRAINT [FK_BusinessData_GeographicLocation]
GO
ALTER TABLE [dbo].[BusinessData]  WITH CHECK ADD  CONSTRAINT [FK_BusinessData_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[BusinessData] CHECK CONSTRAINT [FK_BusinessData_Industry]
GO
/****** Object:  Trigger [dbo].[BusinessDataTrigger]    Script Date: 1/25/2018 2:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[BusinessDataTrigger] ON  [dbo].[BusinessData]
   AFTER delete 
AS 
BEGIN

declare @number sysname;


DECLARE @sqlstr nvarchar(MAX)
SET @sqlstr = 'Record with id: (' + (SELECT id FROM deleted) + ') was deleted from dbo.[BusinessData]';
DECLARE @s_sub nvarchar(1000)
EXEC msdb.dbo.sp_send_dbmail
      @profile_name = 'ProdProfile',
      @recipients = 'agopinath@gisplanning.com',
      @copy_recipients='sroeder@sizeup.com; monzon@gisplanning.com',
      @subject = 'Record Deleted from dbo.[BusinessData]',
      @body =  @sqlstr
END

GO
/****** Object:  Trigger [dbo].[BusinessDataUpdateTrigger]    Script Date: 1/25/2018 2:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create trigger [dbo].[BusinessDataUpdateTrigger] on [dbo].[BusinessData]
after update
	as
Begin
	set nocount on
	declare @Busid varchar;
	declare @BusName nvarchar(MAX);
	declare @sqlstr nvarchar(MAX)

	--create temp table
	declare @updates table (busID varchar)
	
	--insert changed values into temp table
	insert into @updates(busID) 
	select i.Id
	from inserted i
	left join Deleted D on D.id =(select busID from @updates)

	while exists (select 1 from @updates) begin
	select top 1 @Busid = (select busID from @updates),
	@sqlstr = 'Record with id: (' +  (select busID from @updates) +  ' was updated in dbo.[BusinessData]';
	EXEC msdb.dbo.sp_send_dbmail
      @profile_name = 'ProdProfile',
      @recipients = 'agopinath@gisplanning.com',
      @copy_recipients='sroeder@sizeup.com; monzon@gisplanning.com',
      @subject = 'Record Updated in dbo.[BusinessData]',
      @body =  @sqlstr
      delete from @updates where @Busid = busID
	end
end

GO
