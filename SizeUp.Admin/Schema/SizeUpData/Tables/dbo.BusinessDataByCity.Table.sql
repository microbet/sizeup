/****** Object:  Table [dbo].[BusinessDataByCity]    Script Date: 11/06/2012 15:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BusinessDataByCity](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[Quarter] [int] NOT NULL,
	[IndustryId] [bigint] NULL,
	[CityId] [bigint] NULL,
	[BusinessId] [bigint] NOT NULL,
	[Revenue] [bigint] NULL,
	[Employees] [bigint] NULL,
	[YearEstablished] [int] NULL,
	[YearAppeared] [int] NULL,
 CONSTRAINT [PK_BusinessDataByCity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_BusinessDataByCity_TempIndex] ON [dbo].[BusinessDataByCity] 
(
	[IndustryId] ASC
)
INCLUDE ( [CityId],
[YearEstablished],
[YearAppeared]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_BusinessDataByCity_TempIndex2] ON [dbo].[BusinessDataByCity] 
(
	[CityId] ASC,
	[IndustryId] ASC
)
INCLUDE ( [YearEstablished],
[YearAppeared]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_BusinessDataByCity_YearQuarterIndustryIdCityIdEmployees] ON [dbo].[BusinessDataByCity] 
(
	[Year] ASC,
	[Quarter] ASC,
	[IndustryId] ASC,
	[CityId] ASC,
	[Employees] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BusinessDataByCity]  WITH CHECK ADD  CONSTRAINT [FK_BusinessDataByCity_Business] FOREIGN KEY([BusinessId])
REFERENCES [dbo].[Business] ([Id])
GO
ALTER TABLE [dbo].[BusinessDataByCity] CHECK CONSTRAINT [FK_BusinessDataByCity_Business]
GO
ALTER TABLE [dbo].[BusinessDataByCity]  WITH CHECK ADD  CONSTRAINT [FK_BusinessDataByCity_City] FOREIGN KEY([CityId])
REFERENCES [dbo].[City] ([Id])
GO
ALTER TABLE [dbo].[BusinessDataByCity] CHECK CONSTRAINT [FK_BusinessDataByCity_City]
GO
ALTER TABLE [dbo].[BusinessDataByCity]  WITH CHECK ADD  CONSTRAINT [FK_BusinessDataByCity_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[BusinessDataByCity] CHECK CONSTRAINT [FK_BusinessDataByCity_Industry]
GO
