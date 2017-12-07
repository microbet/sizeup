SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BusinessData](
	[Name] [varchar](30) NULL,
	[Address] [varchar](30) NULL,
	[City] [varchar](16) NULL,
	[State] [varchar](2) NULL,
	[ZipCode] [varchar](5) NULL,
	[ZipPlus4] [varchar](4) NULL,
	[CountyFIPS] [varchar](3) NULL,
	[MetroFIPS] [varchar](5) NULL,
	[Filler] [varchar](16) NULL,
	[MetroLevel] [varchar](1) NULL,
	[Phone] [varchar](10) NULL,
	[ContactLastName] [varchar](14) NULL,
	[ContactFirstName] [varchar](11) NULL,
	[ContactProfessionalTitle] [varchar](3) NULL,
	[TitleCode] [varchar](1) NULL,
	[ProfessionalFlag] [varchar](1) NULL,
	[PrimarySicCode] [varchar](6) NULL,
	[YellowPagesCode] [varchar](5) NULL,
	[FranchiseSpecialtyCodes] [varchar](18) NULL,
	[IndustrySpecificCode] [varchar](1) NULL,
	[SecondarySicCode1] [varchar](6) NULL,
	[YellowPageCode1] [varchar](5) NULL,
	[FranchiseSpecialtyCode1] [varchar](18) NULL,
	[IndustrySpecificCode1] [varchar](1) NULL,
	[SecondarySicCode2] [varchar](6) NULL,
	[YellowPageCode2] [varchar](5) NULL,
	[FranchiseSpecialtyCode2] [varchar](18) NULL,
	[IndustrySpecificCode2] [varchar](1) NULL,
	[SecondarySicCode3] [varchar](6) NULL,
	[YellowPageCode3] [varchar](5) NULL,
	[FranchiseSpecialtyCode3] [varchar](18) NULL,
	[IndustrySpecificCode3] [varchar](1) NULL,
	[SecondarySicCode4] [varchar](6) NULL,
	[YellowPageCode4] [varchar](5) NULL,
	[FranchiseSpecialtyCode4] [varchar](18) NULL,
	[IndustrySpecificCode4] [varchar](1) NULL,
	[InfoGroupId] [varchar](9) NULL,
	[Lat] [varchar](9) NULL,
	[Long] [varchar](9) NULL,
	[CensusTract] [varchar](6) NULL,
	[BlockGroup] [varchar](1) NULL,
	[MatchLevel] [varchar](1) NULL,
	[SiteNumber] [varchar](9) NULL,
	[FirmCode] [varchar](1) NULL,
	[TertAdd] [varchar](30) NULL,
	[WorkAtHomeFlag] [varchar](1) NULL,
	[NAICSCode] [varchar](8) NULL,
	[NAICSDescription] [varchar](50) NULL,
	[EmployeeSizeCode] [varchar](1) NULL,
	[ActualEmployees] [varchar](6) NULL,
	[CorperateEmployeeSizeCode] [varchar](1) NULL,
	[TotalEmployeesCorp] [varchar](6) NULL,
	[SalesVolumeCode] [varchar](1) NULL,
	[EstimatedSalesVolume] [varchar](9) NULL,
	[CoperateSalesVolumeCode] [varchar](1) NULL,
	[TotalSalesVolumeCorp] [varchar](9) NULL,
	[BusinessStatusCode] [varchar](1) NULL,
	[PrimaryWebURL] [varchar](40) NULL,
	[YearEstablished] [varchar](4) NULL,
	[OfficeSizeCode] [varchar](1) NULL,
	[EmployementDirFlag] [varchar](1) NULL,
	[SubsitadryParentNumber] [varchar](9) NULL,
	[UltimateParentNumber] [varchar](9) NULL,
	[AssetFlag] [varchar](1) NULL,
	[PublicCompanyIndicator] [varchar](1) NULL,
	[YearAppeared] [int] NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[CodedAll](
	[Shared] [varchar](3) NULL,
	[Prefered] [varchar](3) NULL,
	[SicCode] [varchar](8) NULL,
	[Name] [varchar](50) NULL,
	[YPCode] [varchar](8) NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Industry](
	[SicCode] [varchar](8) NULL,
	[IndustrySpecificCode] [varchar](2) NULL,
	[Order] [varchar](3) NULL,
	[Name] [varchar](80) NULL
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO
