
--FIX ORPHANED USERS
EXEC sp_change_users_login 'Report';

EXEC sp_change_users_login 'update_one', 'SizeUpLBI', 'SizeUpLBI';

EXEC sp_change_users_login 'Auto_Fix', 'user', 'login', 'password';