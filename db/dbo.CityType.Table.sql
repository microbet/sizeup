USE [NewData]
GO
/****** Object:  Table [dbo].[CityType]    Script Date: 1/25/2018 2:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CityType](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](40) NULL,
	[CensusClassCode] [nvarchar](3) NULL,
	[LegalStatisticalAreaDescriptionCode] [nvarchar](5) NULL,
	[SourceId] [nvarchar](1) NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((0)),
	[SEOKey] [nvarchar](25) NULL,
 CONSTRAINT [PK_CityType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET IDENTITY_INSERT [dbo].[CityType] ON 

INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (1, N'Village', N'C9', N'47', N'P', 1, N'village')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (2, N'Town', N'T1', N'43', N'C', 1, N'town')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (3, N'City', N'C1', N'UG', N'P', 1, N'city')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (4, N'Village', N'C5', N'47', N'P', 1, N'village')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (5, N'Town', N'T9', N'43', N'C', 1, N'town')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (6, N'Census Designated Place', N'M2', N'57', N'P', 1, N'cdp')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (7, N'City', N'C1', N'CN', N'P', 1, N'city')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (8, N'Census Designated Place', N'U2', N'57', N'P', 1, N'cdp')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (9, N'City', N'C1', N'MG', N'P', 1, N'city')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (10, N'Town', N'C5', N'43', N'P', 1, N'town')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (11, N'City', N'C1', N'53', N'P', 1, N'city')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (12, N'Town', N'C9', N'43', N'P', 1, N'town')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (13, N'City', N'C1', N'37', N'P', 1, N'city')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (14, N'Town', N'T5', N'43', N'C', 1, N'town')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (15, N'City', N'C1', N'25', N'P', 1, N'city')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (16, N'City', N'C5', N'37', N'P', 1, N'city')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (17, N'City', N'C2', N'25', N'P', 1, N'city')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (18, N'City', N'C1', N'00', N'P', 1, N'city')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (19, N'Plantation	', N'T1', N'39', N'C', 0, N'plantation')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (20, N'Village', N'C2', N'47', N'P', 1, N'village')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (21, N'City', N'C5', N'25', N'P', 1, N'city')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (22, N'Township', N'T9', N'45', N'C', 1, N'township')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (23, N'Town', N'C1', N'43', N'P', 1, N'town')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (24, N'City', N'C7', N'00', N'P', 1, N'city')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (25, N'Borough', N'C5', N'21', N'P', 1, N'borough')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (26, N'Borough', N'C1', N'21', N'P', 1, N'borough')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (27, N'Village', N'C1', N'47', N'P', 1, N'village')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (28, N'City', N'C8', N'00', N'P', 1, N'city')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (29, N'City', N'C9', N'25', N'P', 1, N'city')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (30, N'Township', N'T9', N'44', N'C', 1, N'township')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (31, N'Township', N'T1', N'44', N'C', 1, N'township')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (32, N'City', N'C1', N'UC', N'P', 1, N'city')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (33, N'Township', N'T1', N'45', N'C', 1, N'township')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (34, N'City', N'C6', N'25', N'P', 1, N'city')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (35, N'Township', N'T1', N'49', N'C', 1, N'township')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (36, N'Census Designated Place', N'U1', N'57', N'P', 1, N'cdp')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (37, N'Township', N'T5', N'44', N'C', 1, N'township')
INSERT [dbo].[CityType] ([Id], [Name], [CensusClassCode], [LegalStatisticalAreaDescriptionCode], [SourceId], [IsActive], [SEOKey]) VALUES (38, N'City', N'C7', N'25', N'P', 1, N'city')
SET IDENTITY_INSERT [dbo].[CityType] OFF
