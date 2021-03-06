USE [NewData]
GO
/****** Object:  UserDefinedFunction [dbo].[SEOIfy]    Script Date: 1/25/2018 2:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[SEOIfy]
(
	-- Add the parameters for the function here
	@name nvarchar(max)
)
RETURNS nvarchar(max)
AS
BEGIN
	declare @temp nvarchar(max)
	set @temp = @name
	set @temp = replace(@temp, '.','')
	set @temp = replace(@temp, ' ','-')
	set @temp = replace(@temp, '''','')
	set @temp = replace(@temp, '+','')
	set @temp = replace(@temp, ':','-')
	set @temp = replace(@temp, '%','')
	set @temp = replace(@temp, '&','-')
	set @temp = replace(@temp, ';','')
	set @temp = replace(@temp, '?','')
	set @temp = replace(@temp, ',','')
	set @temp = replace(@temp, '!','')
	set @temp = replace(@temp, '(','')
	set @temp = replace(@temp, ')','')
	set @temp = replace(@temp, '\','')
	set @temp = replace(@temp, '/','')
	set @temp = replace(@temp, '#','')
	
	
	set @temp = replace(@temp, '------','-')
	set @temp = replace(@temp, '-----','-')
	set @temp = replace(@temp, '----','-')
	set @temp = replace(@temp, '---','-')
	set @temp = replace(@temp, '--','-')
	
	RETURN lower(@temp)

END

GO
