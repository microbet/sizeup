UPDATE NewData.dbo.Business
SET IsActive=0
WHERE InfoGroupId in (
  435141812, -- Royce Huang, attorney, removed at business's request
  710595891, -- Zaylore Stout, attorney, removed at business's request
  365412915, -- Rachel Ministries, abortion clinic, removed at customer's request
  711044176, -- Flying Spikes Baseball Club, athletic organization, removed at business's request
  715916269  -- Sadighi Layla, Psychologist, removed at business's request
  718131015  -- Anturio Marketing, removed at Sizeup request from Mario (??)
)

-- This is just to see the results in a test transaction before rolling it back.
-- We use SizeUp-assigned ID instead of InfoGroupId, just to check consistency,
-- but SizeUp ID may go away in the future.
SELECT Name, IsActive, InfoGroupId
FROM [NewData].[dbo].[Business]
WHERE ID in (
  19765420,
  23829656,
  4305618,
  25654347,
  23445256,
  27642007
)

UPDATE [NewData].[dbo].[Business]
set PrimaryWebURL = ''
WHERE ID=23579546 -- Home Comfort Care, URL was abandoned
