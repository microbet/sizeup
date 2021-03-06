USE [NewData]
GO
/****** Object:  Table [dbo].[GeographyClass]    Script Date: 1/25/2018 2:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GeographyClass](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_GeographyClass] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET IDENTITY_INSERT [dbo].[GeographyClass] ON 

INSERT [dbo].[GeographyClass] ([Id], [Name]) VALUES (1, N'Display')
INSERT [dbo].[GeographyClass] ([Id], [Name]) VALUES (2, N'Calculation')
SET IDENTITY_INSERT [dbo].[GeographyClass] OFF
