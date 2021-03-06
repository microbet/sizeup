/****** Object:  Table [dbo].[BusinessCityMapping]    Script Date: 11/06/2012 15:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BusinessCityMapping](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CityId] [bigint] NOT NULL,
	[BusinessId] [bigint] NOT NULL,
 CONSTRAINT [PK_BusinessCityMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [BusinessCityMapping_CityId_BusinessId] ON [dbo].[BusinessCityMapping] 
(
	[CityId] ASC
)
INCLUDE ( [BusinessId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_BusinessCityMapping_BusinessId_CityId] ON [dbo].[BusinessCityMapping] 
(
	[BusinessId] ASC
)
INCLUDE ( [CityId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BusinessCityMapping]  WITH CHECK ADD  CONSTRAINT [FK_BusinessCityMapping_Business] FOREIGN KEY([BusinessId])
REFERENCES [dbo].[Business] ([Id])
GO
ALTER TABLE [dbo].[BusinessCityMapping] CHECK CONSTRAINT [FK_BusinessCityMapping_Business]
GO
ALTER TABLE [dbo].[BusinessCityMapping]  WITH CHECK ADD  CONSTRAINT [FK_BusinessCityMapping_City] FOREIGN KEY([CityId])
REFERENCES [dbo].[City] ([Id])
GO
ALTER TABLE [dbo].[BusinessCityMapping] CHECK CONSTRAINT [FK_BusinessCityMapping_City]
GO
