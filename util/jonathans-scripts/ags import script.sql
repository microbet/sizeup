

CREATE TABLE #output
(

  [Year] [int] NOT NULL,
	[GeographicLocationId] [bigint] NOT NULL,
	[VariableId] [bigint] NOT NULL,
	[Value] [decimal](38, 12) NULL
)

DECLARE @script TABLE
(

  [KEY] nvarchar(128),
  cols nvarchar(1000),
  formula nvarchar(1000)
)

insert into @script ([key],cols,formula) values ('population', '[POPCY]','[POPCY]')
insert into @script ([key],cols,formula) values ('male', '[SEXCYMAL]','[SEXCYMAL]')
insert into @script ([key],cols,formula) values ('female', '[SEXCYFEM]','[SEXCYFEM]')
insert into @script ([key],cols,formula) values ('zeroToTwenty', '[AGECY0004],[AGECY0509],[AGECY1019]','[AGECY0004]+[AGECY0509]+[AGECY1019]')
insert into @script ([key],cols,formula) values ('twentyToFourty', '[AGECY2029],[AGECY3039]','[AGECY2029]+[AGECY3039]')
insert into @script ([key],cols,formula) values ('fourtyToSixtyFive', '[AGECY4049],[AGECY5059]','[AGECY4049]+[AGECY5059]')
insert into @script ([key],cols,formula) values ('sixtyFivePlus', '[AGECYGT65]','[AGECYGT65]')

insert into @script ([key],cols,formula) values ('white', '[RACCYWHITE]','[RACCYWHITE]')
insert into @script ([key],cols,formula) values ('black', '[RACCYBLACK]','[RACCYBLACK]')
insert into @script ([key],cols,formula) values ('americanIndian', '[RACCYAMIND]','[RACCYAMIND]')
insert into @script ([key],cols,formula) values ('asian', '[RACCYASIAN]','[RACCYASIAN]')
insert into @script ([key],cols,formula) values ('pacificIslander', '[RACCYHAWAI]','[RACCYHAWAI]')
insert into @script ([key],cols,formula) values ('other', '[RACCYOTHER]','[RACCYOTHER]')
insert into @script ([key],cols,formula) values ('multirace', '[RACCYMULT]','[RACCYMULT]')
insert into @script ([key],cols,formula) values ('hispanic', '[HISCYHISP]','[HISCYHISP]')

insert into @script ([key],cols,formula) values ('populationAge25Plus', '[AGECYGT25]','[AGECYGT25]')
insert into @script ([key],cols,formula) values ('lessThanHighSchool', '[EDUCYLTGR9],[EDUCYSHSCH]','[EDUCYLTGR9] + [EDUCYSHSCH]')
insert into @script ([key],cols,formula) values ('highSchoolDegree', '[EDUCYHSCH]','[EDUCYHSCH]')
insert into @script ([key],cols,formula) values ('someCollege', '[EDUCYSCOLL],[EDUCYASSOC]','[EDUCYSCOLL] + [EDUCYASSOC]')
insert into @script ([key],cols,formula) values ('bachelorsDegreeOrHigher', '[EDUCYBACH],[EDUCYGRAD]','[EDUCYBACH] + [EDUCYGRAD]')

insert into @script ([key],cols,formula) values ('householdExpenditureAverage', '[TCYCC],[TCYPI],[TCYAP],[TCYED],[TCYEN],[TCYFB],[TCYHC],[TCYHF],[TCYHH],[TCYHO],[TCYMI],[TCYPC],[TCYRD],[TCYTB],[TCYTR],[TCYUT],[TCYGI],[HHDCY]','([TCYCC]+[TCYPI]+[TCYAP]+[TCYED]+[TCYEN]+[TCYFB]+[TCYHC]+[TCYHF]+[TCYHH]+[TCYHO]+[TCYMI]+[TCYPC]+[TCYRD]+[TCYTB]+[TCYTR]+[TCYUT]+[TCYGI]) / nullif([HHDCY],0)')
insert into @script ([key],cols,formula) values ('householdIncomeMedian', '[INCCYMEDHH]','[INCCYMEDHH]')



insert into @script ([key],cols,formula) values ('lessThanTwentyK', '[HINCYLT10],[HINCY1020]','[HINCYLT10] + [HINCY1020]')
insert into @script ([key],cols,formula) values ('twentyToFourtyK', '[HINCY2030],[HINCY3040]','[HINCY2030] + [HINCY3040]')
insert into @script ([key],cols,formula) values ('fourtyToSixtyK', '[HINCY4050],[HINCY5060]','[HINCY4050] + [HINCY5060]')
insert into @script ([key],cols,formula) values ('sixtyToOneHundredK', '[HINCY6075], [HINCY75100]','[HINCY6075] + [HINCY75100]')
insert into @script ([key],cols,formula) values ('greaterThanOneHundredK', '[HINCYGT100]','[HINCYGT100]')


insert into @script ([key],cols,formula) values ('apparel', '[TCYAP]','[TCYAP]')
insert into @script ([key],cols,formula) values ('education', '[TCYED]','[TCYED]')
insert into @script ([key],cols,formula) values ('entertainment', '[TCYEN]','[TCYEN]')
insert into @script ([key],cols,formula) values ('foodAndBeverages', '[TCYFB]','[TCYFB]')
insert into @script ([key],cols,formula) values ('healthcare', '[TCYHC]','[TCYHC]')
insert into @script ([key],cols,formula) values ('householdFurnishings', '[TCYHF]','[TCYHF]')
insert into @script ([key],cols,formula) values ('shelter', '[TCYHH]','[TCYHH]')
insert into @script ([key],cols,formula) values ('householdOperations', '[TCYHO]','[TCYHO]')
insert into @script ([key],cols,formula) values ('miscellaneousExpenses', '[TCYMI]','[TCYMI]')
insert into @script ([key],cols,formula) values ('personalCare', '[TCYPC]','[TCYPC]')
insert into @script ([key],cols,formula) values ('reading', '[TCYRD]','[TCYRD]')
insert into @script ([key],cols,formula) values ('tobacco', '[TCYTB]','[TCYTB]')
insert into @script ([key],cols,formula) values ('transportation', '[TCYTR]','[TCYTR]')
insert into @script ([key],cols,formula) values ('utilities', '[TCYUT]','[TCYUT]')
insert into @script ([key],cols,formula) values ('gifts', '[TCYGI]','[TCYGI]')
insert into @script ([key],cols,formula) values ('personalInsurance', '[TCYPI]','[TCYPI]')
insert into @script ([key],cols,formula) values ('contributions', '[TCYCC]','[TCYCC]')

insert into @script ([key],cols,formula) values ('totalDwellings', '[DWLCY]','[DWLCY]')
insert into @script ([key],cols,formula) values ('ownerOccupiedDwellings', '[DWLCYOWNED]','[DWLCYOWNED]')
insert into @script ([key],cols,formula) values ('renterOccupiedDwellings', '[DWLCYRENT]','[DWLCYRENT]')
insert into @script ([key],cols,formula) values ('housingUnitsOccupied', '[DWLCYOCCUP]','[DWLCYOCCUP]')

insert into @script ([key],cols,formula) values ('oneToTwoPerson', '[HHDCYS1],[HHDCYS2]','[HHDCYS1] + [HHDCYS2]')
insert into @script ([key],cols,formula) values ('threeToFourPerson', '[HHDCYS3],[HHDCYS4]','[HHDCYS3] + [HHDCYS4]')
insert into @script ([key],cols,formula) values ('fivePlusPerson', '[HHDCYS5],[HHDCYS6]','[HHDCYS5] + [HHDCYS6]')


insert into @script ([key],cols,formula) values ('laborForce', '[LBFCYLBF2]','[LBFCYLBF2]')


insert into @script ([key],cols,formula) values ('whiteCollarWorkers', '[BCCCYWHTCL],[BUSCYEMP]','[BCCCYWHTCL]/nullif([BUSCYEMP],0)')
insert into @script ([key],cols,formula) values ('blueCollarWorkers', '[BUSCYEMP],[BCCCYWHTCL]','([BUSCYEMP]-[BCCCYWHTCL])/nullif([BUSCYEMP],0)')


insert into @script ([key],cols,formula) values ('oneToNineteenEmployees', '[ESZCYA],[ESZCYB] ,[ESZCYC]','[ESZCYA] + [ESZCYB] + [ESZCYC]')
insert into @script ([key],cols,formula) values ('twentyToNintyNineEmployees', '[ESZCYD],[ESZCYE]','[ESZCYD] + [ESZCYE]')
insert into @script ([key],cols,formula) values ('oneHundredPlusEmployees', '[ESZCYF],[ESZCYG],[ESZCYH],[ESZCYI]','[ESZCYF] + [ESZCYG] + [ESZCYH] + [ESZCYI]')





insert into @script ([key],cols,formula) values ('agriculturalForestryFishingEmployees', '[EMPCYAGRIC]','[EMPCYAGRIC]')
insert into @script ([key],cols,formula) values ('miningEmployees', '[EMPCYMINE]','[EMPCYMINE]')
insert into @script ([key],cols,formula) values ('constructionEmployees', '[EMPCYCONST]','[EMPCYCONST]')
insert into @script ([key],cols,formula) values ('manufacturingEmployees', '[EMPCYMANUF]','[EMPCYMANUF]')
insert into @script ([key],cols,formula) values ('transportationAndCommunicationsEmployees', '[EMPCYTRANS]','[EMPCYTRANS]')
insert into @script ([key],cols,formula) values ('wholesaleTradeEmployees', '[EMPCYWTRAD]','[EMPCYWTRAD]')
insert into @script ([key],cols,formula) values ('retailTradeEmployees', '[EMPCYRTRAD]','[EMPCYRTRAD]')
insert into @script ([key],cols,formula) values ('financeInsuranceAndRealEstateEmployees', '[EMPCYFIRE]','[EMPCYFIRE]')
insert into @script ([key],cols,formula) values ('servicesEmployees', '[EMPCYSRV]','[EMPCYSRV]')
insert into @script ([key],cols,formula) values ('publicAdministrationEmployees', '[EMPCYPUBAD]','[EMPCYPUBAD]')
insert into @script ([key],cols,formula) values ('unclassifiedEmployees', '[EMPCYUNCLA]','[EMPCYUNCLA]')



insert into @script ([key],cols,formula) values ('executiveManagersAndAdministrators', '[BCCCYEXEC]','[BCCCYEXEC]')
insert into @script ([key],cols,formula) values ('businessAndFinancialOperations', '[BCCCYBFIN]','[BCCCYBFIN]')
insert into @script ([key],cols,formula) values ('computerAndMathematicalOccupations', '[BCCCYCOMP]','[BCCCYCOMP]')
insert into @script ([key],cols,formula) values ('architectureAndEngineering', '[BCCCYENGR]','[BCCCYENGR]')
insert into @script ([key],cols,formula) values ('lifePhysicalSocialScienceOccupations', '[BCCCYSCNC]','[BCCCYSCNC]')
insert into @script ([key],cols,formula) values ('communityAndSocialServices', '[BCCCYCSRV]','[BCCCYCSRV]')
insert into @script ([key],cols,formula) values ('legal', '[BCCCYLEGL]','[BCCCYLEGL]')
insert into @script ([key],cols,formula) values ('educationTrainingLibrary', '[BCCCYEDUC]','[BCCCYEDUC]')
insert into @script ([key],cols,formula) values ('artsDesignEntertainmentSportsMedia', '[BCCCYARTS]','[BCCCYARTS]')
insert into @script ([key],cols,formula) values ('healthDiagnosingAndTreatingPractitioners', '[BCCCYHLTH]','[BCCCYHLTH]')
insert into @script ([key],cols,formula) values ('healthTechnologistsTechnicians', '[BCCCYHTCH]','[BCCCYHTCH]')
insert into @script ([key],cols,formula) values ('healthcareSupport', '[BCCCYHCSP]','[BCCCYHCSP]')
insert into @script ([key],cols,formula) values ('protectiveServices', '[BCCCYPROT]','[BCCCYPROT]')
insert into @script ([key],cols,formula) values ('foodPreparationServing', '[BCCCYFOOD]','[BCCCYFOOD]')
insert into @script ([key],cols,formula) values ('buildingAndGroundsMaintenance', '[BCCCYBGMT]','[BCCCYBGMT]')
insert into @script ([key],cols,formula) values ('personalCareAndService', '[BCCCYPCAR]','[BCCCYPCAR]')
insert into @script ([key],cols,formula) values ('sales', '[BCCCYSALE]','[BCCCYSALE]')
insert into @script ([key],cols,formula) values ('officeAndAdministrativeSupport', '[BCCCYOFFC]','[BCCCYOFFC]')
insert into @script ([key],cols,formula) values ('farmingFishingForestry', '[BCCCYPRIM]','[BCCCYPRIM]')
insert into @script ([key],cols,formula) values ('constructionAndExtraction', '[BCCCYCONS]','[BCCCYCONS]')
insert into @script ([key],cols,formula) values ('installationMaintenanceAndRepairWorkers', '[BCCCYMREP]','[BCCCYMREP]')
insert into @script ([key],cols,formula) values ('productionWorkers', '[BCCCYPROD]','[BCCCYPROD]')
insert into @script ([key],cols,formula) values ('transportationWorkers', '[BCCCYTRAN]','[BCCCYTRAN]')
insert into @script ([key],cols,formula) values ('materialMoving', '[BCCCYMMOV]','[BCCCYMMOV]')


insert into @script ([key],cols,formula) values ('restaurants', '[ESTCYRRST]','[ESTCYRRST]')
insert into @script ([key],cols,formula) values ('museumsAndZoos', '[ESTCYMUSMS]','[ESTCYMUSMS]')
insert into @script ([key],cols,formula) values ('hospitals', '[ESTCYHOSP]','[ESTCYHOSP]')






declare @key nvarchar(128)
declare @sql nvarchar(4000)
declare @cols nvarchar(4000)
declare @formula nvarchar(4000)



DECLARE db_cursor CURSOR FOR  
SELECT [KEY]
FROM @script

OPEN db_cursor   
FETCH NEXT FROM db_cursor INTO @key   

WHILE @@FETCH_STATUS = 0   
BEGIN   
		SELECT
		@cols = cols,
		@formula = formula
		FROM
		@script
		where [KEY] = @key
		
		
		
		set @sql = '
		insert into #output (year,geographiclocationid,variableid, value)
		select 
		demo.year,
		gl.id as geographiclocationid,
		v.id as variableid,
		demo.value

		FROM
		(
		SELECT year, fips, (' + @formula + ') as value
		from 
		(SELECT year, fips, variable, value 
			FROM rawdata.dbo.demographics) AS SourceTable
		PIVOT
		(
		max(value)
		FOR Variable IN (' + @cols + ')
		) piv
		) demo
		inner join  demographics.dbo.geographiclocation gl
		on gl.fips = demo.fips
		inner join  [GISP-STG-DB01.GISPLANNING.NET].zpdcdata.dbo.variable v
		on v.[key] = ''' + @key + ''''

		exec(@sql)

       FETCH NEXT FROM db_cursor INTO @key 
END   

CLOSE db_cursor   
DEALLOCATE db_cursor





insert into [GISP-STG-DB01.GISPLANNING.NET].zpdcdata.dbo.demographics (year, geographiclocationid, variableid, value)
select 
[Year] ,
	[GeographicLocationId],
	[VariableId] ,
	[Value]
	 from #output where Value is not null

drop table #output















