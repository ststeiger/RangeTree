
DECLARE @address bigint 
SET @address = dbo.IPAddressToInteger('88.84.21.77')


SELECT 
	 network
	,registered_country_geoname_id
	,represented_country_geoname_id
	,is_anonymous_proxy
	,is_satellite_provider
	,geoip.geoip_locations_temp.* 
FROM geoip.geoip_blocks_temp

LEFT JOIN geoip.geoip_locations_temp
	ON geoip.geoip_locations_temp.geoname_id = geoip.geoip_blocks_temp.geoname_id
	

WHERE @address BETWEEN lower_boundary_int AND upper_boundary_int 
