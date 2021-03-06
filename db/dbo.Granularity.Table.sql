USE [NewData]
GO
/****** Object:  Table [dbo].[Granularity]    Script Date: 1/25/2018 2:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Granularity](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_Granularity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET IDENTITY_INSERT [dbo].[Granularity] ON 

INSERT [dbo].[Granularity] ([Id], [Name]) VALUES (1, N'ZipCode')
INSERT [dbo].[Granularity] ([Id], [Name]) VALUES (2, N'City')
INSERT [dbo].[Granularity] ([Id], [Name]) VALUES (3, N'County')
INSERT [dbo].[Granularity] ([Id], [Name]) VALUES (4, N'Place')
INSERT [dbo].[Granularity] ([Id], [Name]) VALUES (5, N'Metro')
INSERT [dbo].[Granularity] ([Id], [Name]) VALUES (6, N'State')
INSERT [dbo].[Granularity] ([Id], [Name]) VALUES (7, N'Nation')
INSERT [dbo].[Granularity] ([Id], [Name]) VALUES (8, N'Division')
INSERT [dbo].[Granularity] ([Id], [Name]) VALUES (9, N'Region')
SET IDENTITY_INSERT [dbo].[Granularity] OFF
