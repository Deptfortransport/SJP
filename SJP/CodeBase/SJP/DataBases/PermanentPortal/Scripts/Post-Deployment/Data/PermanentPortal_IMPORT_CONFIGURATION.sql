-- =============================================
-- Script Template
-- =============================================

IF NOT EXISTS(SELECT * FROM IMPORT_CONFIGURATION WHERE DATA_FEED='ply010')
BEGIN
	INSERT INTO IMPORT_CONFIGURATION VALUES('ply010','SJP.DataImporters.SJPCycleParkImporter','sjp.dataimporters.dll',
		' ',' ',' ','D:/Gateway/dat/Processing/ply010')
END

IF NOT EXISTS(SELECT * FROM IMPORT_CONFIGURATION WHERE DATA_FEED='zse435')
BEGIN
	INSERT INTO IMPORT_CONFIGURATION VALUES('zse435','SJP.DataImporters.SJPParkAndRideLocationsImporter','sjp.dataimporters.dll',
		' ',' ',' ','D:/Gateway/dat/Processing/zse435')
END

IF NOT EXISTS(SELECT * FROM IMPORT_CONFIGURATION WHERE DATA_FEED='gqv678')
BEGIN
	INSERT INTO IMPORT_CONFIGURATION VALUES('gqv678','SJP.DataImporters.SJPVenueAdditionalDataImporter','sjp.dataimporters.dll',
		' ',' ',' ','D:/Gateway/dat/Processing/gqv678')
END

IF NOT EXISTS(SELECT * FROM IMPORT_CONFIGURATION WHERE DATA_FEED='tgb987')
BEGIN
	INSERT INTO IMPORT_CONFIGURATION VALUES('tgb987','SJP.DataImporters.SJPVenueGateDataImporter','sjp.dataimporters.dll',
		' ',' ',' ','D:/Gateway/dat/Processing/tgb987')
END

IF NOT EXISTS(SELECT * FROM IMPORT_CONFIGURATION WHERE DATA_FEED='sul834')
BEGIN
	INSERT INTO IMPORT_CONFIGURATION VALUES('sul834','SJP.DataImporters.SJPGNATAdminAreasDataImporter','sjp.dataimporters.dll',
		' ',' ',' ','D:/Gateway/dat/Processing/sul834')
END

IF NOT EXISTS(SELECT * FROM IMPORT_CONFIGURATION WHERE DATA_FEED='mkw489')
BEGIN
	INSERT INTO IMPORT_CONFIGURATION VALUES('mkw489','SJP.DataImporters.SJPGNATLocationsDataImporter','sjp.dataimporters.dll',
		' ',' ',' ','D:/Gateway/dat/Processing/mkw489')
END

IF NOT EXISTS(SELECT * FROM IMPORT_CONFIGURATION WHERE DATA_FEED='mnb765')
BEGIN
	INSERT INTO IMPORT_CONFIGURATION VALUES('mnb765','SJP.DataImporters.SJPStopAccessibilityLinksDataImporter','sjp.dataimporters.dll',
		' ',' ',' ','D:/Gateway/dat/Processing/mnb765')
END

IF NOT EXISTS(SELECT * FROM IMPORT_CONFIGURATION WHERE DATA_FEED='mbt156')
BEGIN
	INSERT INTO IMPORT_CONFIGURATION VALUES('mbt156','SJP.DataImporters.SJPTravelcardDataImporter','sjp.dataimporters.dll',
		' ',' ',' ','D:/Gateway/dat/Processing/mbt156')
END

IF NOT EXISTS(SELECT * FROM IMPORT_CONFIGURATION WHERE DATA_FEED='cft439')
BEGIN
	INSERT INTO IMPORT_CONFIGURATION VALUES('cft439','SJP.DataImporters.SJPVenueAccessDataImporter','sjp.dataimporters.dll',
		' ',' ',' ','D:/Gateway/dat/Processing/cft439')
END

IF NOT EXISTS(SELECT * FROM IMPORT_CONFIGURATION WHERE DATA_FEED='zdf451')
BEGIN
	INSERT INTO IMPORT_CONFIGURATION VALUES('zdf451','SJP.DataImporters.SJPParkAndRideToidsImporter','sjp.dataimporters.dll',
		' ',' ',' ','D:/Gateway/dat/Processing/zdf451')
END

IF NOT EXISTS(SELECT * FROM IMPORT_CONFIGURATION WHERE DATA_FEED='omg654')
BEGIN
	INSERT INTO IMPORT_CONFIGURATION VALUES('omg654','SJP.DataImporters.SJPTravelNewsDataImporter','sjp.dataimporters.dll',
		'D:/Gateway/bat/SJPTravelNews.bat',' ',' ','D:/Gateway/dat/Processing/omg654')
END