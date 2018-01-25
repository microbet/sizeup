--truncate table rawdatait2.dbo.businessdata2
--BD
insert into rawdatait2.dbo.businessdata2 (Year,Quarter,
businessId,
BranchTypeCode,
geographicLocationId,
industryId,
Employees,
Revenue,
YearStarted,
OperatingCost,
NetProfit,
DebtEquityRatio,
NetWorth,
CostOfPersonnel,
TotalBranchesNumber)

SELECT 
2015 as Year,
1 as Quarter,
businessId,
BranchTypeCode,
geographicLocationId,
industryId,
Employees,
Revenue,
YearStarted,
OperatingCost,
NetProfit,
DebtEquityRatio,
NetWorth,
CostOfPersonnel,
TotalBranchesNumber

FROM
(
--nation
select
b.id as businessId,
s.nationid as geographicLocationId,
b.industryid,
convert(int,convert(varchar(max),bd.TIPO_SEDE)) as BranchTypeCode,
case 
when coalesce(convert(bigint,convert(varchar(max),bd.ADDETTI)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.ADDETTI))
end as Employees,

coalesce(convert(bigint,convert(varchar(max),bd.FATTURATO)),convert(bigint,convert(varchar(max),bd.FATTURATO_UL)))  as Revenue,


--2
case 
when coalesce(LEFT(CONVERT(VARCHAR(MAX),bd.DATA_ISCRIZIONE_CCIAA),4) ,'') = '' then null
else  LEFT(CONVERT(VARCHAR(MAX),bd.DATA_ISCRIZIONE_CCIAA),4) 
end as YearStarted,

case 
when coalesce(convert(bigint,convert(varchar(max),bd.COSTI_OPERATIVI)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.COSTI_OPERATIVI))
end as OperatingCost,

case 
when coalesce(convert(bigint,convert(varchar(max),bd.UTILE)),'') = '' then null 
else  convert(bigint,convert(varchar(max),bd.UTILE))
end  as NetProfit,

case 
when coalesce(convert(float,replace(convert(varchar(max),[DEBITI_SU_PATRIMONIO_NETTO]),',','.')),'') = '' then null
else  convert(float,replace(convert(varchar(max),[DEBITI_SU_PATRIMONIO_NETTO]),',','.'))
end  as DebtEquityRatio,


case 
when coalesce(convert(bigint,convert(varchar(max),bd.PATRIMONIO_NETTO)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.PATRIMONIO_NETTO))
end as NetWorth,

--3
case 
when coalesce(convert(bigint,convert(varchar(max),bd.COSTO_DEL_PERSONALE)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.COSTO_DEL_PERSONALE))
end as CostOfPersonnel,

case 
when coalesce(convert(bigint,convert(varchar(max),bd.NUMERO_UL)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.NUMERO_UL))
end as TotalBranchesNumber


 from
  rawdatait2.dbo.business b  (NOLOCK)
 inner join 
 --rawdatait2.dbo.deutsche_output bd  (NOLOCK)
 [ZPE-PROD-DB01.GISPLANNING.NET].[Sandbox].[dbo].[DeutscheBankCerved] bd  (NOLOCK)
on b.cervedid = convert(bigint,convert(varchar(max),bd.ID_CERVEDGROUP))
inner join  rawdatait2.dbo.state s
on s.id = b.stateid
where
b.isactive = 1 
and b.Id = bd.ID


union all

--state
SELECT
b.id as businessId,
s.id as geographicLocationId,
b.industryid,
convert(int,convert(varchar(max),bd.TIPO_SEDE)) as BranchTypeCode,
case 
when coalesce(convert(bigint,convert(varchar(max),bd.ADDETTI)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.ADDETTI))
end as Employees,

coalesce(convert(bigint,convert(varchar(max),bd.FATTURATO)),convert(bigint,convert(varchar(max),bd.FATTURATO_UL)))  as Revenue,


--2
case 
when coalesce(LEFT(CONVERT(VARCHAR(MAX),bd.DATA_ISCRIZIONE_CCIAA),4) ,'') = '' then null
else  LEFT(CONVERT(VARCHAR(MAX),bd.DATA_ISCRIZIONE_CCIAA),4) 
end as YearStarted,

case 
when coalesce(convert(bigint,convert(varchar(max),bd.COSTI_OPERATIVI)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.COSTI_OPERATIVI))
end as OperatingCost,

case 
when coalesce(convert(bigint,convert(varchar(max),bd.UTILE)),'') = '' then null 
else  convert(bigint,convert(varchar(max),bd.UTILE))
end  as NetProfit,

case 
when coalesce(convert(float,replace(convert(varchar(max),[DEBITI_SU_PATRIMONIO_NETTO]),',','.')),'') = '' then null
else  convert(float,replace(convert(varchar(max),[DEBITI_SU_PATRIMONIO_NETTO]),',','.'))
end  as DebtEquityRatio,


case 
when coalesce(convert(bigint,convert(varchar(max),bd.PATRIMONIO_NETTO)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.PATRIMONIO_NETTO))
end as NetWorth,

--3
case 
when coalesce(convert(bigint,convert(varchar(max),bd.COSTO_DEL_PERSONALE)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.COSTO_DEL_PERSONALE))
end as CostOfPersonnel,

case 
when coalesce(convert(bigint,convert(varchar(max),bd.NUMERO_UL)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.NUMERO_UL))
end as TotalBranchesNumber

 from
  rawdatait2.dbo.business b  (NOLOCK)
 inner join 
 --rawdatait2.dbo.deutsche_output bd  (NOLOCK)
  [ZPE-PROD-DB01.GISPLANNING.NET].[Sandbox].[dbo].[DeutscheBankCerved] bd  (NOLOCK)
on b.cervedid = convert(bigint,convert(varchar(max),bd.ID_CERVEDGROUP))
inner join  rawdatait2.dbo.State s (NOLOCK)
on s.Id = b.stateid

where
b.isactive = 1
and b.Id = bd.ID

union all
--county

SELECT
b.id as businessId,
co.id as geographicLocationId,
b.industryid,
convert(int,convert(varchar(max),bd.TIPO_SEDE)) as BranchTypeCode,
case 
when coalesce(convert(bigint,convert(varchar(max),bd.ADDETTI)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.ADDETTI))
end as Employees,

coalesce(convert(bigint,convert(varchar(max),bd.FATTURATO)),convert(bigint,convert(varchar(max),bd.FATTURATO_UL)))  as Revenue,


--2
case 
when coalesce(LEFT(CONVERT(VARCHAR(MAX),bd.DATA_ISCRIZIONE_CCIAA),4) ,'') = '' then null
else  LEFT(CONVERT(VARCHAR(MAX),bd.DATA_ISCRIZIONE_CCIAA),4) 
end as YearStarted,

case 
when coalesce(convert(bigint,convert(varchar(max),bd.COSTI_OPERATIVI)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.COSTI_OPERATIVI))
end as OperatingCost,

case 
when coalesce(convert(bigint,convert(varchar(max),bd.UTILE)),'') = '' then null 
else  convert(bigint,convert(varchar(max),bd.UTILE))
end  as NetProfit,

case 
when coalesce(convert(float,replace(convert(varchar(max),[DEBITI_SU_PATRIMONIO_NETTO]),',','.')),'') = '' then null
else  convert(float,replace(convert(varchar(max),[DEBITI_SU_PATRIMONIO_NETTO]),',','.'))
end  as DebtEquityRatio,


case 
when coalesce(convert(bigint,convert(varchar(max),bd.PATRIMONIO_NETTO)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.PATRIMONIO_NETTO))
end as NetWorth,

--3
case 
when coalesce(convert(bigint,convert(varchar(max),bd.COSTO_DEL_PERSONALE)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.COSTO_DEL_PERSONALE))
end as CostOfPersonnel,

case 
when coalesce(convert(bigint,convert(varchar(max),bd.NUMERO_UL)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.NUMERO_UL))
end as TotalBranchesNumber

 from
  rawdatait2.dbo.business b  (NOLOCK)
 inner join 
 --rawdatait2.dbo.deutsche_output bd  (NOLOCK)
  [ZPE-PROD-DB01.GISPLANNING.NET].[Sandbox].[dbo].[DeutscheBankCerved] bd  (NOLOCK)
on b.cervedid = convert(bigint,convert(varchar(max),bd.ID_CERVEDGROUP))
inner join  rawdatait2.dbo.County co(NOLOCK)
on co.Id = b.CountyId 


where
b.isactive = 1 
and b.Id = bd.ID


union all
-- city


SELECT
b.id as businessId,
bc.cityid as geographiclocationid,
b.industryid,
convert(int,convert(varchar(max),bd.TIPO_SEDE)) as BranchTypeCode,
case 
when coalesce(convert(bigint,convert(varchar(max),bd.ADDETTI)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.ADDETTI))
end as Employees,

coalesce(convert(bigint,convert(varchar(max),bd.FATTURATO)),convert(bigint,convert(varchar(max),bd.FATTURATO_UL)))  as Revenue,


--2
case 
when coalesce(LEFT(CONVERT(VARCHAR(MAX),bd.DATA_ISCRIZIONE_CCIAA),4) ,'') = '' then null
else  LEFT(CONVERT(VARCHAR(MAX),bd.DATA_ISCRIZIONE_CCIAA),4) 
end as YearStarted,

case 
when coalesce(convert(bigint,convert(varchar(max),bd.COSTI_OPERATIVI)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.COSTI_OPERATIVI))
end as OperatingCost,

case 
when coalesce(convert(bigint,convert(varchar(max),bd.UTILE)),'') = '' then null 
else  convert(bigint,convert(varchar(max),bd.UTILE))
end  as NetProfit,

case 
when coalesce(convert(float,replace(convert(varchar(max),[DEBITI_SU_PATRIMONIO_NETTO]),',','.')),'') = '' then null
else  convert(float,replace(convert(varchar(max),[DEBITI_SU_PATRIMONIO_NETTO]),',','.'))
end  as DebtEquityRatio,


case 
when coalesce(convert(bigint,convert(varchar(max),bd.PATRIMONIO_NETTO)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.PATRIMONIO_NETTO))
end as NetWorth,

--3
case 
when coalesce(convert(bigint,convert(varchar(max),bd.COSTO_DEL_PERSONALE)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.COSTO_DEL_PERSONALE))
end as CostOfPersonnel,

case 
when coalesce(convert(bigint,convert(varchar(max),bd.NUMERO_UL)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.NUMERO_UL))
end as TotalBranchesNumber

 from
  rawdatait2.dbo.business b  (NOLOCK)
 inner join 
  rawdatait2.dbo.businesscity bc (NOLOCK)
 on b.id = bc.businessid
 inner join
 --rawdatait2.dbo.deutsche_output bd  (NOLOCK)
  [ZPE-PROD-DB01.GISPLANNING.NET].[Sandbox].[dbo].[DeutscheBankCerved] bd  (NOLOCK)
on b.cervedid = convert(bigint,convert(varchar(max),bd.ID_CERVEDGROUP))
where
b.isactive = 1
and b.Id = bd.ID


union all
-- zip


SELECT
b.id as businessId,
z.id as geographicLocationId,
b.industryid,
convert(int,convert(varchar(max),bd.TIPO_SEDE)) as BranchTypeCode,
case 
when coalesce(convert(bigint,convert(varchar(max),bd.ADDETTI)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.ADDETTI))
end as Employees,


coalesce(convert(bigint,convert(varchar(max),bd.FATTURATO)),convert(bigint,convert(varchar(max),bd.FATTURATO_UL)))  as Revenue,

--2
case 
when coalesce(LEFT(CONVERT(VARCHAR(MAX),bd.DATA_ISCRIZIONE_CCIAA),4) ,'') = '' then null
else  LEFT(CONVERT(VARCHAR(MAX),bd.DATA_ISCRIZIONE_CCIAA),4) 
end as YearStarted,

case 
when coalesce(convert(bigint,convert(varchar(max),bd.COSTI_OPERATIVI)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.COSTI_OPERATIVI))
end as OperatingCost,

case 
when coalesce(convert(bigint,convert(varchar(max),bd.UTILE)),'') = '' then null 
else  convert(bigint,convert(varchar(max),bd.UTILE))
end  as NetProfit,

case 
when coalesce(convert(float,replace(convert(varchar(max),[DEBITI_SU_PATRIMONIO_NETTO]),',','.')),'') = '' then null
else  convert(float,replace(convert(varchar(max),[DEBITI_SU_PATRIMONIO_NETTO]),',','.'))
end  as DebtEquityRatio,


case 
when coalesce(convert(bigint,convert(varchar(max),bd.PATRIMONIO_NETTO)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.PATRIMONIO_NETTO))
end as NetWorth,

--3
case 
when coalesce(convert(bigint,convert(varchar(max),bd.COSTO_DEL_PERSONALE)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.COSTO_DEL_PERSONALE))
end as CostOfPersonnel,


case 
when coalesce(convert(bigint,convert(varchar(max),bd.NUMERO_UL)),'') = '' then null
else  convert(bigint,convert(varchar(max),bd.NUMERO_UL))
end as TotalBranchesNumber

 from
  rawdatait2.dbo.business b  (NOLOCK)
 inner join 
 --rawdatait2.dbo.deutsche_output bd  (NOLOCK)
  [ZPE-PROD-DB01.GISPLANNING.NET].[Sandbox].[dbo].[DeutscheBankCerved] bd  (NOLOCK)
on b.cervedid = convert(bigint,convert(varchar(max),bd.ID_CERVEDGROUP))
inner join  rawdatait2.dbo.ZipCode z (NOLOCK)
on z.Zip = b.zipcode -- this might fuck shit up
where
b.isactive = 1 
and b.Id = bd.ID




) x