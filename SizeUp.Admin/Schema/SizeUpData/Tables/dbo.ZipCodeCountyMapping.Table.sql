/****** Object:  Table [dbo].[ZipCodeCountyMapping]    Script Date: 11/06/2012 15:28:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ZipCodeCountyMapping](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ZipCodeId] [bigint] NOT NULL,
	[CountyId] [bigint] NOT NULL,
 CONSTRAINT [PK_ZipCodeCountyMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ZipCodeCountyMapping_CountyId] ON [dbo].[ZipCodeCountyMapping] 
(
	[CountyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ZipCodeCountyMapping_ZipCodeId] ON [dbo].[ZipCodeCountyMapping] 
(
	[ZipCodeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ZipCodeCountyMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_ZipCodeCountyMapping_County] FOREIGN KEY([CountyId])
REFERENCES [dbo].[County] ([Id])
GO
ALTER TABLE [dbo].[ZipCodeCountyMapping] CHECK CONSTRAINT [FK_ZipCodeCountyMapping_County]
GO
ALTER TABLE [dbo].[ZipCodeCountyMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_ZipCodeCountyMapping_ZipCode] FOREIGN KEY([ZipCodeId])
REFERENCES [dbo].[ZipCode] ([Id])
GO
ALTER TABLE [dbo].[ZipCodeCountyMapping] CHECK CONSTRAINT [FK_ZipCodeCountyMapping_ZipCode]
GO
