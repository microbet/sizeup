/****** Object:  Table [dbo].[BusinessDataByZip]    Script Date: 11/06/2012 15:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BusinessDataByZip](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[Quarter] [int] NOT NULL,
	[ZipCodeId] [bigint] NULL,
	[IndustryId] [bigint] NULL,
	[BusinessId] [bigint] NOT NULL,
	[Revenue] [bigint] NULL,
	[Employees] [bigint] NULL,
	[YearEstablished] [int] NULL,
	[YearAppeared] [int] NULL,
 CONSTRAINT [PK_BusinessDataByZip] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BusinessDataByZip]  WITH CHECK ADD  CONSTRAINT [FK_BusinessDataByZip_Business] FOREIGN KEY([BusinessId])
REFERENCES [dbo].[Business] ([Id])
GO
ALTER TABLE [dbo].[BusinessDataByZip] CHECK CONSTRAINT [FK_BusinessDataByZip_Business]
GO
ALTER TABLE [dbo].[BusinessDataByZip]  WITH CHECK ADD  CONSTRAINT [FK_BusinessDataByZip_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])
GO
ALTER TABLE [dbo].[BusinessDataByZip] CHECK CONSTRAINT [FK_BusinessDataByZip_Industry]
GO
ALTER TABLE [dbo].[BusinessDataByZip]  WITH CHECK ADD  CONSTRAINT [FK_BusinessDataByZip_ZipCode] FOREIGN KEY([ZipCodeId])
REFERENCES [dbo].[ZipCode] ([Id])
GO
ALTER TABLE [dbo].[BusinessDataByZip] CHECK CONSTRAINT [FK_BusinessDataByZip_ZipCode]
GO
