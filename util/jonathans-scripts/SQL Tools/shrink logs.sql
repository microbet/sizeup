

ALTER DATABASE SizeUpData SET RECOVERY SIMPLE WITH NO_WAIT
GO
ALTER DATABASE SizeUpData SET RECOVERY SIMPLE 
GO
USE SizeUpData 
GO
DBCC SHRINKFILE (N'SizeUpData_log' , 1, TRUNCATEONLY)
GO


ALTER DATABASE SizeUpData SET RECOVERY FULL WITH NO_WAIT
GO
ALTER DATABASE SizeUpData SET RECOVERY FULL 
GO
