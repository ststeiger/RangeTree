
-- http://doatt.com/2015/03/15/postgres-minmax-for-the-cidr-data-type/
-- https://dev.maxmind.com/geoip/geoip2/whats-new-in-geoip2/
-- https://dev.maxmind.com/geoip/geoip2/geolite2/
-- https://www.maxmind.com/en/high-risk-ip-sample-list
-- https://www.wordfence.com/blog/2016/02/wordpress-password-security/



-- CREATE SCHEMA geoip 
CREATE SCHEMA IF NOT EXISTS geoip; 

-- CREATE TABLE geoip.geoip_locations_temp  
CREATE TABLE IF NOT EXISTS geoip.geoip_locations_temp  
(
  geoname_id                           bigint NOT NULL,
  locale_code                          character varying(2) NOT NULL,
  continent_code                       character varying(2),
  continent_name                       character varying(15),
  country_iso_code                     character varying(2),
  --country_name                         character varying(45),
  country_name                         character varying(50), -- for incorrect SQL-Server import
  CONSTRAINT PK_geoip_locations_temp PRIMARY KEY (geoname_id)
);


-- CREATE TABLE geoip.geoip_blocks_temp  
CREATE TABLE IF NOT EXISTS geoip.geoip_blocks_temp  
(
  --network                         cidr NOT NULL,
  network                           character varying(32) NOT NULL CONSTRAINT DF_geoip_blocks_temp_network DEFAULT '',
  geoname_id                        bigint CONSTRAINT FK_geoip_locations_temp REFERENCES geoip.geoip_locations_temp (geoname_id),
  registered_country_geoname_id     bigint,
  represented_country_geoname_id    bigint,
  is_anonymous_proxy                int NOT NULL,
  is_satellite_provider             int NOT NULL,
  CONSTRAINT PK_geoip_blocks_temp PRIMARY KEY (network) 
);

--CREATE UNIQUE INDEX index_geoip_locations_geoname_id_temp       ON geoip.geoip_locations_temp (geoname_id);  
--CREATE        INDEX index_geoip_locations_locale_code_temp      ON geoip.geoip_locations_temp (locale_code);  
--CREATE        INDEX index_geoip_locations_country_iso_code_temp ON geoip.geoip_locations_temp (country_iso_code);  
--CREATE UNIQUE INDEX index_geoip_blocks_network_temp             ON geoip.geoip_blocks_temp (network);  
--CREATE        INDEX index_geoip_blocks_geoname_id_temp          ON geoip.geoip_blocks_temp (geoname_id);  
--CREATE        INDEX index_geoip_blocks_is_anonymous_proxy_temp  ON geoip.geoip_blocks_temp (is_anonymous_proxy);

-- First the locations table due to the foreign key constraint in blocks
-- \COPY geoip.geoip_locations_temp FROM '/usrdeploy/IPByCountry/GeoLite2-Country-Locations-en.csv' WITH CSV HEADER;
-- Then the blocks table
-- \COPY geoip.geoip_blocks_temp FROM '/usrdeploy/IPByCountry/GeoLite2-Country-Blocks-IPv4.csv' WITH CSV HEADER;

-- SQL: 
-- -- COPY zip_codes FROM '/path/to/csv/ZIP_CODES.txt' DELIMITER ',' CSV;
-- COPY geoip.geoip_locations_temp FROM 'D:\username\Documents\visual studio 2013\Projects\Captcha\Captcha\IP\GeoLite\GeoLite2-Country-Locations-de.csv' DELIMITER ',' CSV HEADER;
-- COPY geoip.geoip_blocks_temp FROM 'D:\username\Documents\visual studio 2013\Projects\Captcha\Captcha\IP\GeoLite\GeoLite2-Country-Blocks-IPv4.csv' DELIMITER ',' CSV HEADER;





-- http://www.siafoo.net/article/53
-- http://davidkane.net/installing-new-geoip-database-sql-database/
-- http://doatt.com/2015/03/15/postgres-minmax-for-the-cidr-data-type/
-- https://www.proxynova.com/proxy-articles/install-maxmind-geoip2/
-- https://github.com/ouspg/iot-gateway/blob/master/utils/geoip_table_creation_script.sql


/*
-- IPv4: 15, IPv6: 39
ALTER TABLE geoip.geoip_blocks_temp
	ADD lower_boundary character varying(39) NOT NULL CONSTRAINT DF_lower_geoip_blocks_temp_lower_boundary DEFAULT '',
		upper_boundary character varying(39) NOT NULL CONSTRAINT DF_lower_geoip_blocks_temp_upper_boundary DEFAULT '' 
	

UPDATE geoip.geoip_blocks_temp SET lower_boundary = host(  network::inet )
UPDATE geoip.geoip_blocks_temp SET upper_boundary = host( broadcast( network::inet ) )
*/
