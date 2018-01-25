SET IDENTITY_INSERT Business2 ON

insert into [rawdatait2].[dbo].[Business2] 
(Id,Name,Address,City,Phone, CervedId,
YearAppeared,Lat,Long,IndustryId,ZipCode,StateId,CountyId,SEOKey,
IsActive)

select 
bd.ID,
cast([rawdatait2].[dbo].ProperCase(bd.Denominazione) as nvarchar(250)) as Name,
cast([rawdatait2].[dbo].ProperCase(bd.Indirizzo)  as nvarchar(60)) as Address,
cast([rawdatait2].[dbo].ProperCase(bd.Comune) as nvarchar(50))  as City,

CONVERT(VARCHAR(MAX),bd.TELEFONO_I) as Phone,
convert(bigint,convert(varchar(max),ID_CERVEDGROUP)) as CervedId,
(
case CONVERT(VARCHAR(MAX),bd.DATA_ISCRIZIONE_CCIAA)
when '' then null
else 
LEFT(CONVERT(VARCHAR(MAX),bd.DATA_ISCRIZIONE_CCIAA),4) 
end) as YearAppeared,

(
case convert(varchar(max),PRECISIONE)
when '' then null
when 'COM' then convert(float,REPLACE(convert(varchar(max),LATITUDINE),',','.'))
else convert(float,convert(varchar(max),LATITUDINE))
end) as Lat,

(
case convert(varchar(max),PRECISIONE)
when '' then null
when 'COM' then convert(float,REPLACE(convert(varchar(max),LONGITUDINE),',','.'))
else convert(float,convert(varchar(max),LONGITUDINE))
end) as Long,

i.id as Industryid,
CONVERT(VARCHAR(MAX),bd.Cap)  as ZipCode,
co.StateId as StateId,
co.Id as CountyId,
[rawdatait2].[dbo].SEOIfy(bd.DENOMINAZIONE) as SEOKEy,


(case  convert(varchar(max),bd.STATO_ATTIVITA)
when 'I' then 0
else 1
end
) as IsActive

FROM
[zpe-prod-db01.gisplanning.net].[sandbox].[dbo].[DeutscheBankCerved] bd

left outer join rawdatait2.dbo.industry i (NOLOCK)
on convert(varchar,bd.CODICE_ATECO_PRIMARIO) = convert(varchar,i.CervedAteco)

left outer join rawdatait2.dbo.county co (NOLOCK)
on convert(varchar,bd.PROVINCIA) = convert(varchar,co.Abbreviation)

-- left outer join rawdatait2.dbo.city c (NOLOCK)
--on convert(varchar,bd.Comune) = convert(varchar,c.Name)



SET IDENTITY_INSERT Business2 OFF

