use master
ALTER DATABASE SizeUpData SET SINGLE_USER WITH ROLLBACK IMMEDIATE 


	ALTER DATABASE SizeUpData SET OFFLINE
	GO


	ALTER DATABASE SizeUpData MODIFY FILE (NAME =SizeUpData, FILENAME = 'D:\Data\SizeUp\SizeUpData-2013-09-30.mdf')
	GO

	--if changing log file name
	ALTER DATABASE  SizeUpData MODIFY FILE (NAME = SizeupData_log, FILENAME ='D:\Logs\SizeUp\SizeUpData-2013-09-30.ldf')
	GO

	ALTER DATABASE SizeUpData SET ONLINE
	GO

	/*drop user SizeUp
	CREATE USER SizeUp FOR LOGIN SizeUp;
	EXEC sp_addrolemember N'db_owner', N'SizeUp'*/

ALTER DATABASE SizeUpData SET MULTI_USER WITH ROLLBACK IMMEDIATE 






