USE [NewData]
GO
/****** Object:  Table [dbo].[BusinessCity]    Script Date: 1/25/2018 2:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BusinessCity](
	[CityId] [bigint] NOT NULL,
	[BusinessId] [bigint] NOT NULL,
 CONSTRAINT [PK_BusinessCity] PRIMARY KEY CLUSTERED 
(
	[BusinessId] ASC,
	[CityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Index [IX_BusinessCity_CityId_BusinessId]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_BusinessCity_CityId_BusinessId] ON [dbo].[BusinessCity]
(
	[CityId] ASC
)
INCLUDE ( 	[BusinessId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BusinessCity]  WITH CHECK ADD  CONSTRAINT [FK_BusinessCity_Business] FOREIGN KEY([BusinessId])
REFERENCES [dbo].[Business] ([Id])
GO
ALTER TABLE [dbo].[BusinessCity] CHECK CONSTRAINT [FK_BusinessCity_Business]
GO
ALTER TABLE [dbo].[BusinessCity]  WITH CHECK ADD  CONSTRAINT [FK_BusinessCity_City] FOREIGN KEY([CityId])
REFERENCES [dbo].[City] ([Id])
GO
ALTER TABLE [dbo].[BusinessCity] CHECK CONSTRAINT [FK_BusinessCity_City]
GO
