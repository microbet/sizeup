
/****** Object:  Table [dbo].[RelatedCompetitor]    Script Date: 11/15/2012 17:27:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RelatedCompetitor](
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
	[UserId] [uniqueidentifier] NULL,
	[PlaceId] [bigint] NOT NULL,
	[PrimaryIndustryId] [bigint] NOT NULL,
	[RelatedIndustryId] [bigint] NOT NULL,
 CONSTRAINT [PK_RelatedCompetitor] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[RelatedCompetitor] ADD  DEFAULT (getutcdate()) FOR [Timestamp]
GO

