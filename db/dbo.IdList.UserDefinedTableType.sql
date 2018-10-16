CREATE TYPE [dbo].[IdList] AS TABLE(
	[Item] [bigint] NULL
)
GO
-- FIXME this should be granted to db_datareader but I couldn't figure that out,
-- and I'm not sure "EXECUTE" is correct. Currently I have "CONTROL" granted instead.
GRANT EXECUTE ON TYPE::[dbo].[IdList] TO [SizeUp]
GO
