-- =============================================
-- Inserts TravelNewsDataSources
-- =============================================

-- **********************************************************************
-- IMPORTANT
-- The Insert statements are copied from TDP script MDS014_TraveNewsDataSources.sql
-- These should be copied from MDS014
-- **********************************************************************

USE [SJPTransientPortal]
GO

BEGIN TRANSACTION

-- Clear existing datasources so this script retains complete control 
DELETE [dbo].[TravelNewsDataSources]

INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (2)', 'Government Agency (2)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (5)', 'Eyewitness (5)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (6)', 'Eyewitness (6)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (7)', 'Eyewitness (7)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Operator (10)', 'Operator (10)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Operator (11)', 'Operator (11)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Emergency Services (12)', 'Emergency Services (12)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Local Authority (13)', 'Local Authority (13)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Operator (14)', 'Operator (14)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Camera (15)', 'Camera (15)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Camera (16)', 'Camera (16)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Emergency Services (17)', 'Emergency Services (17)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Local Authority (19)', 'Local Authority (19)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Utility Company (22)', 'Utility Company (22)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (23)', 'Government Agency (23)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Operator (25)', 'Operator (25)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Emergency Services (26)', 'Emergency Services (26)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (27)', 'Government Agency (27)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (28)', 'Eyewitness (28)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Local Authority (31)', 'Local Authority (31)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (34)', 'Government Agency (34)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Operator (36)', 'Operator (36)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (38)', 'Eyewitness (38)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (39)', 'Eyewitness (39)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Emergency Services (40)', 'Emergency Services (40)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Operator (45)', 'Operator (45)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (47)', 'Government Agency (47)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (50)', 'Eyewitness (50)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Operator (57)', 'Operator (57)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Operator (58)', 'Operator (58)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Operator (59)', 'Operator (59)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Operator (60)', 'Operator (60)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Operator (62)', 'Operator (62)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Utility Company (63)', 'Utility Company (63)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (64)', 'Eyewitness (64)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Operator (66)', 'Operator (66)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Local Authority (67)', 'Local Authority (67)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (68)', 'Eyewitness (68)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Sensor (69)', 'Sensor (69)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Local Authority (70)', 'Local Authority (70)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (71)', 'Government Agency (71)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (72)', 'Government Agency (72)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (73)', 'Government Agency (73)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (74)', 'Government Agency (74)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (75)', 'Eyewitness (75)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Camera (76)', 'Camera (76)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Sensor (77)', 'Sensor (77)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (78)', 'Eyewitness (78)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (79)', 'Eyewitness (79)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Sensor (80)', 'Sensor (80)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (81)', 'Eyewitness (81)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (82)', 'Eyewitness (82)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (82)', 'Government Agency (82)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (83)', 'Eyewitness (83)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (86)', 'Government Agency (86)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (88)', 'Government Agency (88)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Local Authority (90)', 'Local Authority (90)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Emergency Services (92)', 'Emergency Services (92)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (93)', 'Government Agency (93)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Local Authority (94)', 'Local Authority (94)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (95)', 'Government Agency (95)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (96)', 'Government Agency (96)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (97)', 'Government Agency (97)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (98)', 'Government Agency (98)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (100)', 'Eyewitness (100)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (102)', 'Government Agency (102)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (103)', 'Government Agency (103)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (108)', 'Eyewitness (108)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Bus Company (109)', 'Bus Company (109)', 1)

COMMIT TRANSACTION
