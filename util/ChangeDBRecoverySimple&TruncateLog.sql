USE <database_name>
GO

ALTER DATABASE <databse_name> SET RECOVERY SIMPLE
ALTER DATABASE <databse_name> SET RECOVERY FULL

SELECT name 
FROM sys.master_files
WHERE database_id = DB_ID('<databse_name>')

DBCC SHRINKFILE (N'<logical_file_name_of_the_log>' , 0, TRUNCATEONLY)

ALTER DATABASE <databse_name> SET RECOVERY SIMPLE