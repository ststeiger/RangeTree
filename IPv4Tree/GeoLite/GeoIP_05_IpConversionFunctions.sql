
-- https://stackoverflow.com/questions/15662805/convert-ip-address-in-postgresql-to-integer
-- PostgreSQL: 
-- SELECT ('127.0.0.1'::inet - '0.0.0.0'::inet) as ip_integer





-- Microsoft SQL: 

ALTER TABLE geoip.geoip_blocks_temp 
	ADD lower_boundary_int bigint NULL,
		upper_boundary_int bigint NULL 

UPDATE geoip.geoip_blocks_temp 
   SET lower_boundary_int = dbo.IPAddressToInteger(lower_boundary)
      ,upper_boundary_int = dbo.IPAddressToInteger(upper_boundary)
; 





IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IPAddressToInteger]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[IPAddressToInteger]
GO



CREATE FUNCTION [dbo].[IPAddressToInteger] (@IP AS varchar(15))
	RETURNS bigint
AS
BEGIN
	RETURN 
	(
		  CONVERT(bigint, PARSENAME(@IP,1)) 
		+ CONVERT(bigint, PARSENAME(@IP,2)) * 256 
		+ CONVERT(bigint, PARSENAME(@IP,3)) * 65536 
		+ CONVERT(bigint, PARSENAME(@IP,4)) * 16777216
	) 
END


GO





IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IntegerToIPAddress]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[IntegerToIPAddress]
GO





CREATE FUNCTION [dbo].[IntegerToIPAddress](@IP AS bigint)
	RETURNS varchar(15)
AS
BEGIN
	DECLARE @Octet1 tinyint
	DECLARE @Octet2 tinyint
	DECLARE @Octet3 tinyint
	DECLARE @Octet4 tinyint
	DECLARE @RestOfIP bigint 
	
	SET @Octet1 = @IP / 16777216
	SET @RestOfIP = @IP - (@Octet1 * 16777216)
	SET @Octet2 = @RestOfIP / 65536
	SET @RestOfIP = @RestOfIP - (@Octet2 * 65536)
	SET @Octet3 = @RestOfIP / 256
	SET @Octet4 = @RestOfIP - (@Octet3 * 256) 
	
	RETURN
	(
		  CONVERT(varchar, @Octet1) + '.' 
		+ CONVERT(varchar, @Octet2) + '.' 
		+ CONVERT(varchar, @Octet3) + '.' 
		+ CONVERT(varchar, @Octet4)
	); 
END 


GO

