USE [NewData]

/****** Object:  Index [PK_IndustryDataBand]    Script Date: 09/18/2015 20:01:01 ******/
ALTER TABLE [dbo].[IndustryDataBand] ADD  CONSTRAINT [PK_IndustryDataBand] PRIMARY KEY CLUSTERED 
(
	[IndustryDataId] ASC,
	[BandId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO


ALTER TABLE [dbo].[IndustryDataBand]  WITH CHECK ADD  CONSTRAINT [FK_IndustryDataBand_Band] FOREIGN KEY([BandId])
REFERENCES [dbo].[Band] ([Id])
GO

ALTER TABLE [dbo].[IndustryDataBand] CHECK CONSTRAINT [FK_IndustryDataBand_Band]
GO


ALTER TABLE [dbo].[IndustryDataBand]  WITH CHECK ADD  CONSTRAINT [FK_IndustryDataBand_IndustryData] FOREIGN KEY([IndustryDataId])
REFERENCES [dbo].[IndustryData] ([Id])
GO

ALTER TABLE [dbo].[IndustryDataBand] CHECK CONSTRAINT [FK_IndustryDataBand_IndustryData]
GO


