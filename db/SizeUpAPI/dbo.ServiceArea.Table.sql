/** Note there are no FK constraints on the GeographicLocationId and
 * GranularityId references. Those are in a separate database, and
 * SQL Server won't do cross-database FK references.
 */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceArea](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[APIKeyId] [bigint] NOT NULL,
	[GeographicLocationId] [bigint] NOT NULL,
	[GranularityId] [bigint] NOT NULL,
  CONSTRAINT [PK_ServiceArea] PRIMARY KEY CLUSTERED
  (
    [Id] ASC
  ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ServiceArea]  WITH CHECK ADD  CONSTRAINT [FK_ServiceArea_APIKey] FOREIGN KEY([APIKeyId])
REFERENCES [dbo].[APIKey] ([Id])
GO
ALTER TABLE [dbo].[ServiceArea] CHECK CONSTRAINT [FK_ServiceArea_APIKey]
GO
