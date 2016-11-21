
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[geoip].[geoip_blocks_temp]') AND name = N'IX_geoip_blocks_temp_boundaries')
DROP INDEX [IX_geoip_blocks_temp_boundaries] ON [geoip].[geoip_blocks_temp] WITH ( ONLINE = OFF )
GO


CREATE NONCLUSTERED INDEX [IX_geoip_blocks_temp_boundaries]
ON [geoip].[geoip_blocks_temp] ([lower_boundary_int],[upper_boundary_int])
INCLUDE ([network],[geoname_id],[registered_country_geoname_id],[represented_country_geoname_id],[is_anonymous_proxy],[is_satellite_provider])
GO
