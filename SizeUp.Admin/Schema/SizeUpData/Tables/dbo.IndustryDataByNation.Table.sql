/****** Object:  Table [dbo].[IndustryDataByNation]    Script Date: 11/06/2012 15:28:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IndustryDataByNation](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Year] [int] NULL,
	[Quarter] [int] NULL,
	[IndustryId] [bigint] NULL,
	[AverageEmployees] [bigint] NULL,
	[EmployeesPerCapita] [float] NULL,
	[CostEffectiveness] [float] NULL,
	[MedianRevenue] [bigint] NULL,
	[MedianEmployees] [bigint] NULL,
	[TotalRevenue] [bigint] NULL,
	[AverageRevenue] [bigint] NULL,
	[TotalEmployees] [bigint] NULL,
	[RevenuePerCapita] [bigint] NULL,
	[AverageAnnualSalary] [bigint] NULL,
	[Hires] [bigint] NULL,
	[Separations] [bigint] NULL,
	[TurnoverRate] [float] NULL,
	[JobGains] [bigint] NULL,
	[JobLosses] [bigint] NULL,
	[NetJobChange] [bigint] NULL,
	[Employment] [bigint] NULL,
 CONSTRAINT [PK_IndustryDataByNation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[IndustryDataByNation]  WITH CHECK ADD  CONSTRAINT [FK_IndustryDataByNation_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[IndustryDataByNation] CHECK CONSTRAINT [FK_IndustryDataByNation_Industry]
GO
