/****** Object:  Table [dbo].[PlaceIndustrySearches]    Script Date: 11/06/2012 15:33:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlaceIndustrySearches](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[Minute] [int] NOT NULL,
	[Hour] [int] NOT NULL,
	[Day] [int] NOT NULL,
	[Week] [int] NOT NULL,
	[Month] [int] NOT NULL,
	[Quarter] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[APIKeyId] [bigint] NULL,
	[PlaceId] [bigint] NULL,
	[IndustryId] [bigint] NULL,
	[UserId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_PlaceIndustrySearches] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PlaceIndustrySearches] ADD  DEFAULT (getutcdate()) FOR [Timestamp]
GO
