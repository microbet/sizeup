== To fetch Infogroup source files ==

Use Linux platform.

bin/get-infogroup.sh (work in progress; TODO convert to makefile and document)

== To extract data from Infogroup files ==

Use Windows (w/ SQL) platform.

set DTEXEC="C:\Program Files (x86)\Microsoft SQL Server\100\DTS\Binn\DTExec.exe"
set dbname=[name of the raw database]

cd sizeup/etl

sqlcmd -U etl -d %dbname% -i source\infogroup\bin\create-tables.sql

# This step should happen in Powershell. First copy your AWS user credentials to ~/.aws/credentials
# Then, to give AWS Powershell permission to execute scripts, either:
# - start it with `powershell -noprofile -executionpolicy bypass`
# - within the shell, run `Set-ExecutionPolicy unrestricted`
#
# TODO find much better cmd-line unzip than Powershell's which requires absolute path.
# TODO fix Infogroup US market file in fetch so it is not multiple files here.

Add-Type -A System.IO.Compression.FileSystem

foreach ($i in 1,2,3,4,5) {
  Read-S3Object -BucketName sizeup-datasources -Key infogroup/201710/SIZEUP_201710$i.ZIP -File SIZEUP_201710$i.ZIP
  [IO.Compression.ZipFile]::ExtractToDirectory("d:\db\source\infogroup\SIZEUP_201710$i.ZIP", "d:\db\source\infogroup\SIZEUP_201710$i")
}
# or just fetch Austin.csv

# okay, back to DOS prompt. Repeat for each infogroup file.

set infogroup_source=d:\db\source\infogroup\SIZEUP_2017101\SIZEUP_2017101.TXT

# These DTEXEC lines were produced with the help of dtexecui, which is part of the MS SQL suite. More info at http://www.sqlservercentral.com/articles/Integration+Services+(SSIS)/126293/ and/or https://www.mssqltips.com/sqlservertutorial/218/command-line-tool-to-execute-ssis-packages/

%DTEXEC% /FILE "source\DataImport.Raw\BusinessDataSingle.dtsx" /CONNECTION FlatFile;"%infogroup_source%" /CONNECTION "localhost.RawData";"\"Data Source=localhost;Initial Catalog=%dbname%;Provider=SQLNCLI10.1;Integrated Security=SSPI;Auto Translate=False;Application Name=SSIS-InfoGroup-{59353381-C5C1-474B-A9C2-FA376CD801A8}localhost.%dbname%.imsuser;\""  /WARNASERROR  /CHECKPOINTING OFF  /REPORTING EW

Note that invoking "execute" a second time inserts another set of rows in the database without deleting the first set.

"C:\Program Files (x86)\Microsoft SQL Server\100\DTS\Binn\DTExec.exe" /FILE "C:\Users\Administrator\sizeup\etl\source\DataImport.Raw\Industry.dtsx" /CONNECTION codedall;"D:\db\source\codedall.TXT" /CONNECTION industry;"D:\db\source\industry.TXT" /CONNECTION "localhost.RawData";"\"Data Source=localhost;Initial Catalog=%dbname%;Provider=SQLNCLI10.1;Integrated Security=SSPI;Auto Translate=False;Application Name=SSIS-Industry-{56A9E0F4-BB24-4B65-9616-81EE7C62FCC5}localhost.%dbname%;\""  /WARNASERROR  /CHECKPOINTING OFF  /REPORTING EW

TODO clean up for script, e.g.: %DTEXEC% /FILE Industry.dtsx /CONNECTION codedall;"%INFOGROUP%\codedall.TXT" /CONNECTION industry;"%INFOGROUP%\industry.TXT" /CONNECTION "localhost.RawData";"%RAWDATA%" %OPTIONS%

TODO: keep using windows logins so auth is cleaner
