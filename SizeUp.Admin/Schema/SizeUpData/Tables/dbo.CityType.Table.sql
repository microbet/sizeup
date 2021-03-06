/****** Object:  Table [dbo].[CityType]    Script Date: 11/06/2012 15:28:44 ******/
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
	[IsActive] [bit] NOT NULL,
	[SEOKey] [nvarchar](25) NULL,
 CONSTRAINT [PK_CityType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CityType] ADD  DEFAULT ((0)) FOR [IsActive]
GO
