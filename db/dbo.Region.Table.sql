USE [NewData]
GO
/****** Object:  Table [dbo].[Region]    Script Date: 1/25/2018 2:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Region](
	[Name] [nvarchar](50) NOT NULL,
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_Region] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Region]  WITH NOCHECK ADD  CONSTRAINT [FK_Region_GeographicLocation] FOREIGN KEY([Id])
REFERENCES [dbo].[GeographicLocation] ([Id])
GO
ALTER TABLE [dbo].[Region] CHECK CONSTRAINT [FK_Region_GeographicLocation]
GO
