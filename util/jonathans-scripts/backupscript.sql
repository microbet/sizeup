DECLARE @dbname VARCHAR(50) -- database name  
DECLARE @path VARCHAR(256) -- path for backup files  
DECLARE @workingDir VARCHAR(256) -- filename for backup  
DECLARE @fileDate VARCHAR(20) -- used for file name
DECLARE @amazonKey VARCHAR(256) -- filename for backup 
DECLARE @amazonSecret VARCHAR(256) -- filename for backup 
DECLARE @zipCmd VARCHAR(1024)
DECLARE @copyCmd VARCHAR(1024)
DECLARE @cleanCmd VARCHAR(1024)
DECLARE @createDirCmd VARCHAR(1024)
 
-- specify database backup directory
SELECT @path = 'E:\Backup\'

SET @amazonKey = 'AKIAJLPNJDM7T5ITCNTA' 
SET @amazonSecret = 'yjHP1QmMLbrfaAG40dckCiqHQEuZKNSQmF0My9Cf'  

 
-- specify filename format
SELECT @fileDate = CONVERT(VARCHAR(20),GETDATE(),112) + '-' + REPLACE(CONVERT(VARCHAR(20),GETDATE(),108),':','')

 
DECLARE db_cursor CURSOR FOR  
SELECT name 
FROM master.dbo.sysdatabases 
WHERE name NOT IN ('master','model','msdb','tempdb')  -- exclude these databases


EXEC sp_configure 'xp_cmdshell', 1
RECONFIGURE

 
OPEN db_cursor   
FETCH NEXT FROM db_cursor INTO @dbname   

 
WHILE @@FETCH_STATUS = 0   
BEGIN   
       
       SET @workingDir = @path + cast(SERVERPROPERTY('MachineName') as varchar) + '\' + @dbname + '\' + @filedate + '\'
       
       SET @createDirCmd = 'mkdir ' + @workingDir
       exec master..xp_CMDShell @createDirCmd, 'NO_OUTPUT'
       
       EXEC('BACKUP DATABASE ' + @dbname + ' TO DISK = ''' + @workingDir + @dbname + '_' + @filedate + '.bak''')
       
       SET @zipCmd = '"C:\program files\winrar\winrar"  a  -df -ep -v5g ' + @workingDir + @dbname + ' ' + @workingDir + @dbname + '_' + @filedate + '.bak'
       SET @copyCmd = 'C:\s3 put gispbackups' + ' ' + @path + ' /sub  /key:'  + @amazonKey + ' /secret:' + @amazonSecret 
       SET @cleanCmd = 'rmdir /S /Q ' + @path + cast(SERVERPROPERTY('MachineName') as varchar) + '\' + @dbname + '\'
              
       print @zipCmd
       print @copyCmd
       print @cleanCmd
       
       exec master..xp_CMDShell @zipCmd, 'NO_OUTPUT'
	   exec master..xp_CMDShell @copyCmd, 'NO_OUTPUT'
	   exec master..xp_CMDShell @cleanCmd, 'NO_OUTPUT'
	   
 
       FETCH NEXT FROM db_cursor INTO @dbname   
END   

 
CLOSE db_cursor   
DEALLOCATE db_cursor


EXEC sp_configure 'xp_cmdshell', 0
RECONFIGURE




