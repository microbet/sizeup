-- Index Read/Write stats for a single table
    SELECT OBJECT_NAME(s.[object_id]) AS [TableName], 
    i.name AS [IndexName], i.index_id,
    SUM(user_seeks) AS [User Seeks], SUM(user_scans) AS [User Scans], 
    SUM(user_lookups)AS [User Lookups],
    SUM(user_seeks + user_scans + user_lookups)AS [Total Reads], 
    SUM(user_updates) AS [Total Writes]     
    FROM sys.dm_db_index_usage_stats AS s
    INNER JOIN sys.indexes AS i
    ON s.[object_id] = i.[object_id]
    AND i.index_id = s.index_id
    WHERE OBJECTPROPERTY(s.[object_id],'IsUserTable') = 1
    AND s.database_id = DB_ID()
    AND OBJECT_NAME(s.[object_id]) = N'BusinessData'
    GROUP BY OBJECT_NAME(s.[object_id]), i.name, i.index_id
    ORDER BY [Total Writes] DESC, [Total Reads] DESC;
    
    
    --STATEMENT I/O RESOURCE USE
    
    SELECT TOP 50 
	SUBSTRING(qt.TEXT, (qs.statement_start_offset/2)+1,
		((
			CASE qs.statement_end_offset
				WHEN -1 THEN DATALENGTH(qt.TEXT)
				ELSE qs.statement_end_offset
			END - qs.statement_start_offset)/2)+1),
	qs.execution_count,
	(qs.total_logical_reads + qs.total_logical_writes) / qs.execution_count as [Avg IO],
	qp.query_plan,
	qs.total_logical_reads, qs.last_logical_reads,
	qs.total_logical_writes, qs.last_logical_writes,
	qs.total_worker_time,
	qs.last_worker_time,
	qs.total_elapsed_time/1000 total_elapsed_time_in_ms,
	qs.last_elapsed_time/1000 last_elapsed_time_in_ms,
	qs.last_execution_time
FROM 
	sys.dm_exec_query_stats qs
		CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) qt
		OUTER APPLY sys.dm_exec_query_plan(qs.plan_handle) qp
ORDER BY 
	qs.last_worker_time,
	[Avg IO] DESC