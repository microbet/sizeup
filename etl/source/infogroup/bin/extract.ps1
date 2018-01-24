Param(
  [Parameter(Mandatory=$True)] [string] $version,
  [string] $source_dir="D:\db\source\infogroup",
  [string] $dbname="RawData"
)

$ErrorActionPreference = "Stop"

If (!($(Get-Location).Path.EndsWith("\etl"))) {
  throw "This script is meant to be run from the sizeup/etl directory."
}

echo "Creating database tables. Enter etl user's password."
sqlcmd -U etl -d "$dbname" -i source\infogroup\bin\create-tables.sql
If ($LastExitCode=1) { throw "Create tables failed." }

If(!(test-path "$source_dir")) { mkdir "$source_dir" }

Add-Type -A System.IO.Compression.FileSystem

foreach ($seq in 1,2,3,4,5) {
  $name="SIZEUP_$version$seq"
  echo "Downloading $name ..."
  Read-S3Object -BucketName sizeup-datasources -Key "infogroup/$version/$name.ZIP" -File "$source_dir/$name.ZIP"
  [IO.Compression.ZipFile]::ExtractToDirectory("$source_dir/$name.ZIP", "$source_dir")
  $infogroup_source="$source_dir\$name.TXT"
  echo "Extracting $infogroup_source ..."
  #
  # Invoke the DTSX file to extract. This command was supplied by SSIS and is intended
  # for regular MS-DOS shell. (A lot of its punctation is reserved as special characters
  # in Powershell.) So we create an MS-DOS shell for it. I only escaped the double quotes.
  #
  cmd /C "dtexec /FILE `"source\DataImport.Raw\BusinessDataSingle.dtsx`" /CONNECTION `"FlatFile;$infogroup_source`" /CONNECTION `"localhost.RawData`";`"\`"Data Source=localhost;Initial Catalog=$dbname;Provider=SQLNCLI10.1;Integrated Security=SSPI;Auto Translate=False;Application Name=SSIS-InfoGroup-{59353381-C5C1-474B-A9C2-FA376CD801A8}localhost.$dbname.imsuser;`"\`"  /WARNASERROR  /CHECKPOINTING OFF  /REPORTING EW"
}

Read-S3Object -BucketName sizeup-datasources -Key "infogroup/$version/codedall.ZIP" -File "$source_dir/codedall.ZIP"
[IO.Compression.ZipFile]::ExtractToDirectory("$source_dir/codedall.ZIP", "$source_dir")
Read-S3Object -BucketName sizeup-datasources -Key "infogroup/$version/industry.ZIP" -File "$source_dir/industry.ZIP"
[IO.Compression.ZipFile]::ExtractToDirectory("$source_dir/industry.ZIP", "$source_dir")
echo "Extracting codedall and industry data ... "
#
# Invoke DTSX file to extract "codedall" and "industry" tables from Infogroup.
#
cmd /C "dtexec /FILE `"source\DataImport.Raw\Industry.dtsx`" /CONNECTION codedall;`"$source_dir\codedall.TXT`" /CONNECTION industry;`"$source_dir\industry.TXT`" /CONNECTION `"localhost.RawData`";`"\`"Data Source=localhost;Initial Catalog=$dbname;Provider=SQLNCLI10.1;Integrated Security=SSPI;Auto Translate=False;Application Name=SSIS-Industry-{56A9E0F4-BB24-4B65-9616-81EE7C62FCC5}localhost.$dbname;\`"`"  /WARNASERROR  /CHECKPOINTING OFF  /REPORTING EW"

