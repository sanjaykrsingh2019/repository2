
-- for Windows cluster, LOCATION should be '/HdiSamples/SensorSampleData/'
SET sample_data_path = /HdiSamples/SensorSampleData;
-- for Linux cluster, LOCATION should be '/HdiSamples/HdiSamples/SensorSampleData/'
-- SET sample_data_path = /HdiSamples/HdiSamples/SensorSampleData;

--Part 1: Introduction 
-- Many personal and commercial devices now contain sensors, which collect information from the physical world. For example, most phones have a GPS, fitness devices track how many steps you've taken, and thermostats can monitor the temperature of a building. 
-- In this tutorial, you'll learn how HDInsight can be used to process historical data produced by heating, ventilation, and air conditioning (HVAC) systems to identify systems that are not able to reliably maintain a set temperature. You will learn how to: 
-- *Refine and enrich temperature data from buildings in several countries 
-- *Analyze the data to determine which buildings have problems maintaining comfortable temperatures (actual recorded temperature vs. temperature the thermostat was set to) 
-- *Infer reliability of HVAC systems used in the buildings 

--Part 2: Sensor Data Loaded Into Windows Azure Storage Blob 
-- The files contain temperature that the HVAC system was set to, as well as the actual recorded temperature. The files also contain building metadata, such as the location and HVAC system information. 
-- The data stored in Windows Azure Storage Blob can be accessed by expanding a HDInsight cluster and double clicking the default container of your default storage account. The data for this sample can be found under the /HdiSamples/SensorSampleData path in your default container. 

--Part 3: Creating Hive Tables to Query the Sensor Data in the Windows Azure Blob Storage 
-- The following Hive statements create external tables that allow Hive to query data stored in Azure Blob Storage. External tables preserve the data in the original file format, while allowing Hive to perform queries against the data within the file. In this case, the data is stored in the files as comma separated values (CSV). 
-- The Hive statements below create two new tables, named hvac and building, by describing the fields within the files, the delimiter (comma) between fields, and the location of the file in Azure Blob Storage. This will allow you to create Hive queries over your data. 
-- You could also create a table by right clicking on a certain database and select "Create Table". We will provide you a UI to help you to create such a table.
DROP TABLE IF EXISTS hvac;
-- create the hvac table on comma-separated sensor data. 
-- In this sample we will use the default container. You could also use 'wasb://[container]@[storage account].blob.core.windows.net/Path/To/Data/' to access the data in other containers.
CREATE EXTERNAL TABLE hvac(loggingdate STRING, time STRING, targettemp BIGINT,
			actualtemp BIGINT, system BIGINT, systemage BIGINT, buildingid BIGINT)
ROW FORMAT DELIMITED FIELDS TERMINATED BY ','
STORED AS TEXTFILE LOCATION '${hiveconf:sample_data_path}/hvac/';
DROP TABLE IF EXISTS building;
-- create the building table on comma-separated building data. 
-- In this sample we will use the default container. You could also use 'wasb://[container]@[storage account].blob.core.windows.net/Path/To/Data/' to access the data in other containers.
CREATE EXTERNAL TABLE building(buildingid BIGINT, buildingmgr STRING, 
			buildingage BIGINT, hvacproduct STRING, country STRING)
ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' 
STORED AS TEXTFILE LOCATION '${hiveconf:sample_data_path}/building/';

--Part 4: Creating Hive Queries over Sensor Data 
-- The following Hive queries create select temperatures from your HVAC data, looking for temperature variations (see the query below). 
-- Specifically, the difference between the target temperature the thermostat was set to and the recorded temperature. 
-- If the difference is greater than 5, the temp_diff column will be set to 'HOT',or 'COLD' and extremetemp will be set to 1; otherwise, temp_diff will be set to ‘NORMAL’ and extremetemp will be set to 0. 
-- The queries will write the results into two new tables: hvac_temperatures and hvac_building (see the CREATE TABLE statements below). 
-- The hvac_building table will contain building information such as the manager, building age, and the HVAC system for buildings, and will also be used to look up temperature data for the building through the JOIN with the hvac_temperatures table. 
