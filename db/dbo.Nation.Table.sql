USE [NewData]
GO
/****** Object:  Table [dbo].[Nation]    Script Date: 1/25/2018 2:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Nation](
	[Name] [nvarchar](100) NOT NULL,
	[SEOKey] [nvarchar](150) NULL,
	[IsActive] [bit] NOT NULL,
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_Nation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Nation]  WITH NOCHECK ADD  CONSTRAINT [FK_Nation_GeographicLocation] FOREIGN KEY([Id])
REFERENCES [dbo].[GeographicLocation] ([Id])
GO
ALTER TABLE [dbo].[Nation] CHECK CONSTRAINT [FK_Nation_GeographicLocation]
GO
