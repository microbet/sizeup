alter table dnb add lat varchar(20) null
go
alter table dnb add long varchar(20) null
go

-- remove leading signs
update dnb set lat = stuff(latitude, 1, 1, '')
update dnb set long = stuff(longitude, 1, 1, '')

-- remove leading zeros
update dnb set lat = (select substring(lat, patindex('%[^0]%',lat), 10))
update dnb set long = (select substring(long, patindex('%[^0]%',long), 10))

-- convert to float
alter table dnb alter column lat float not null
go
alter table dnb alter column long float not null
go

-- make negative if necessary
update dnb set lat = (select iif(left(latitude, 1) = '-', lat*-1, lat))
update dnb set long = (select iif(left(longitude, 1) = '-', long*-1, long))