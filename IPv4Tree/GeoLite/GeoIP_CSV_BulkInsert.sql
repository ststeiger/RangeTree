
DELETE FROM geoip.geoip_blocks_temp 
DELETE FROM geoip.geoip_locations_temp 

-- https://connect.microsoft.com/SQLServer/feedback/details/370419/bulk-insert-and-bcp-does-not-recognize-codepage-65001
-- Hello, to address a few comments from the community, BCP and BULK INSERT in SQL Server 2016 and upcoming SQL Server 2014 SP2 support UTF-8.
-- SQL-Server (< 2014 SP2) does not support UTF8...  
-- This is just gross...

BULK INSERT geoip.geoip_locations_temp
FROM 'D:\stefan.steiger\Documents\visual studio 2013\Projects\Captcha\Captcha\IP\GeoLite\GeoLite2-Country-Locations-de.csv'
WITH
(
	CODEPAGE = '65001', -- UTF8  
	-- CODEPAGE = '28591', -- ISO-8859-1
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',  --CSV field delimiter
    -- ROWTERMINATOR = '\n',   --Use to shift the control to next row
    -- ROWTERMINATOR = '\r',   --Use to shift the control to next row
    -- ROWTERMINATOR = '\r\n',   --Use to shift the control to next row
    ROWTERMINATOR = '0x0a', 
    TABLOCK
);


BULK INSERT geoip.geoip_blocks_temp
FROM 'D:\username\Documents\visual studio 2013\Projects\Captcha\Captcha\IP\GeoLite\GeoLite2-Country-Blocks-IPv4.csv'
WITH
(
	CODEPAGE = '65001', -- UTF-8  
	-- CODEPAGE = '28591', -- ISO-8859-1
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',  --CSV field delimiter
    ROWTERMINATOR = '0x0a',
    TABLOCK
);



-- ----------------------------------------------------------

  
SELECT 
	 --t1.*,t2.*
	t2.country_name
	,COUNT(*) as cnt 
FROM geoip.geoip_blocks_temp AS t1
LEFT JOIN geoip.geoip_locations_temp AS t2
	ON t2.geoname_id = t1.geoname_id

WHERE represented_country_geoname_id IS NOT NULL 

GROUP BY t2.country_name 
ORDER BY cnt DESC 

