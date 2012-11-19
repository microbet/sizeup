
/****** Object:  Table [dbo].[Region]    Script Date: 11/16/2012 11:47:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Region](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Region] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].Division(
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	RegionId bigint not null,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Division] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]



ALTER TABLE [dbo].Division  WITH NOCHECK ADD  CONSTRAINT [FK_Division_Region] FOREIGN KEY(RegionId)
REFERENCES [dbo].Region ([Id])
GO
ALTER TABLE [dbo].Division CHECK CONSTRAINT [FK_Division_Region]




insert into Region (name) values('Northeast')
insert into Region (name) values('Midwest')
insert into Region (name) values('South')
insert into Region (name) values('West')




insert into Division (name, regionid) values('New England',1)
insert into Division (name, regionid) values('Middle Atlantic',1)
insert into Division (name, regionid) values('East North Central',2)
insert into Division (name, regionid) values('West North Central',2)
insert into Division (name, regionid) values('South Atlantic',3)
insert into Division (name, regionid) values('East South Central',3)
insert into Division (name, regionid) values('West South Central',3)
insert into Division (name, regionid) values('Pacific',4)
insert into Division (name, regionid) values('Mountain',4)

       


alter table State add DivisionId bigint
ALTER TABLE [dbo].State  WITH NOCHECK ADD  CONSTRAINT [FK_State_Division] FOREIGN KEY(DivisionId)
REFERENCES [dbo].Division ([Id])
GO
ALTER TABLE [dbo].State CHECK CONSTRAINT [FK_State_Division]



update s
set divisionId = (SELECT id from division where name = 'New England')
FROM
state s
where s.fips in ('09','23','25','33','44','50')

update s
set divisionId = (SELECT id from division where name = 'Middle Atlantic')
FROM
state s
where s.fips in ('34','36','42')

update s
set divisionId = (SELECT id from division where name = 'East North Central')
FROM
state s
where s.fips in ('18','17','26','39','55')

update s
set divisionId = (SELECT id from division where name = 'West North Central')
FROM
state s
where s.fips in ('19','31','20','38','27','46','29')

update s
set divisionId = (SELECT id from division where name = 'South Atlantic')
FROM
state s
where s.fips in ('10','11','12','13','24','37','45','51','54')

update s
set divisionId = (SELECT id from division where name = 'East South Central')
FROM
state s
where s.fips in ('01','21','28','47')

update s
set divisionId = (SELECT id from division where name = 'West South Central')
FROM
state s
where s.fips in ('05','22','40','48')

update s
set divisionId = (SELECT id from division where name = 'Pacific')
FROM
state s
where s.fips in ('02','06','15','41','53')

update s
set divisionId = (SELECT id from division where name = 'Mountain')
FROM
state s
where s.fips in ('04','08','16','35','30','49','32','56')








