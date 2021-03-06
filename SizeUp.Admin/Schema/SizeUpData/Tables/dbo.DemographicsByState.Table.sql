/****** Object:  Table [dbo].[DemographicsByState]    Script Date: 11/06/2012 15:28:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DemographicsByState](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[Quarter] [int] NOT NULL,
	[StateId] [bigint] NOT NULL,
	[PersonalIncomeTax] [float] NULL,
	[PersonalCapitalGainsTax] [float] NULL,
	[CorporateIncomeTax] [float] NULL,
	[CorporateCapitalGainsTax] [float] NULL,
	[SalesTax] [float] NULL,
 CONSTRAINT [PK_DemographicsByState] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DemographicsByState]  WITH CHECK ADD  CONSTRAINT [FK_DemographicsByState_State] FOREIGN KEY([StateId])
REFERENCES [dbo].[State] ([Id])
GO
ALTER TABLE [dbo].[DemographicsByState] CHECK CONSTRAINT [FK_DemographicsByState_State]
GO
