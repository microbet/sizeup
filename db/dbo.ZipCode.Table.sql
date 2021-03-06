USE [NewData]
GO
/****** Object:  Table [dbo].[ZipCode]    Script Date: 1/25/2018 2:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ZipCode](
	[Zip] [nvarchar](5) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_ZipCode] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_ZipCode_Zip]    Script Date: 1/25/2018 2:41:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_ZipCode_Zip] ON [dbo].[ZipCode]
(
	[Zip] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ZipCode]  WITH CHECK ADD  CONSTRAINT [FK_ZipCode_GeographicLocation] FOREIGN KEY([Id])
REFERENCES [dbo].[GeographicLocation] ([Id])
GO
ALTER TABLE [dbo].[ZipCode] CHECK CONSTRAINT [FK_ZipCode_GeographicLocation]
GO
