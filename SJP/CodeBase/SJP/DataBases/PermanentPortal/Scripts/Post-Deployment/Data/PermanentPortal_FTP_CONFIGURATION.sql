﻿-- =============================================
-- Script Template
-- =============================================

IF NOT EXISTS(SELECT * FROM [dbo].[FTP_CONFIGURATION] WHERE DATA_FEED='ply010')
BEGIN
	INSERT INTO [dbo].[FTP_CONFIGURATION] VALUES(1,'ply010','LocalHost','TDP28Nov',
		'sI1732#3-','d:/Gateway/dat/Incoming/ply010','../ply010','*.zip',
		0, 1, '2009-04-03 02:30:00.000', 'IF138_098_201103181200.zip',1)
END

IF NOT EXISTS(SELECT * FROM [dbo].[FTP_CONFIGURATION] WHERE DATA_FEED='zse435')
BEGIN
	INSERT INTO [dbo].[FTP_CONFIGURATION] VALUES(1,'zse435','LocalHost','TDP28Nov',
		'qH12T56m-','d:/Gateway/dat/Incoming/zse435','../zse435','*.zip',
		0, 1, '2009-04-03 02:30:00.000', 'IF137_098_201103181200.zip',1)
END

IF NOT EXISTS(SELECT * FROM [dbo].[FTP_CONFIGURATION] WHERE DATA_FEED='gqv678')
BEGIN
	INSERT INTO [dbo].[FTP_CONFIGURATION] VALUES(1,'gqv678','LocalHost','TDP28Nov',
		'sV634LKY-','d:/Gateway/dat/Incoming/gqv678','../gqv678','*.zip',
		0, 1, '2009-04-03 02:30:00.000', 'IF139_098_201103181200.zip',1)
END

IF NOT EXISTS(SELECT * FROM [dbo].[FTP_CONFIGURATION] WHERE DATA_FEED='tgb987')
BEGIN
	INSERT INTO [dbo].[FTP_CONFIGURATION] VALUES(1,'tgb987','LocalHost','TDP28Nov',
		'cS7GFT89-','d:/Gateway/dat/Incoming/tgb987','../tgb987','*.zip',
		0, 1, '2009-04-03 02:30:00.000', 'IF147_098_201103181200.zip',1)
END

IF NOT EXISTS(SELECT * FROM [dbo].[FTP_CONFIGURATION] WHERE DATA_FEED='sul834')
BEGIN
	INSERT INTO [dbo].[FTP_CONFIGURATION] VALUES(1,'sul834','LocalHost','TDP28Nov',
		'oZ65lk38-','d:/Gateway/dat/Incoming/sul834','../sul834','*.zip',
		0, 1, '2009-04-03 02:30:00.000', 'IF148_098_201103181200.zip',1)
END

IF NOT EXISTS(SELECT * FROM [dbo].[FTP_CONFIGURATION] WHERE DATA_FEED='mkw489')
BEGIN
	INSERT INTO [dbo].[FTP_CONFIGURATION] VALUES(1,'mkw489','LocalHost','TDP28Nov',
		'cfGG58k7-','d:/Gateway/dat/Incoming/mkw489','../mkw489','*.zip',
		0, 1, '2009-04-03 02:30:00.000', 'IF136_098_201103181200.zip',1)
END

IF NOT EXISTS(SELECT * FROM [dbo].[FTP_CONFIGURATION] WHERE DATA_FEED='mnb765')
BEGIN
	INSERT INTO [dbo].[FTP_CONFIGURATION] VALUES(1,'mnb765','LocalHost','TDP28Nov',
		'tTt444wF-','d:/Gateway/dat/Incoming/mnb765','../mnb765','*.zip',
		0, 1, '2009-04-03 02:30:00.000', 'IF144_098_201103181200.zip',1)
END

IF NOT EXISTS(SELECT * FROM [dbo].[FTP_CONFIGURATION] WHERE DATA_FEED='mbt156')
BEGIN
	INSERT INTO [dbo].[FTP_CONFIGURATION] VALUES(1,'mbt156','LocalHost','TDP28Nov',
		'poTT88m4-','d:/Gateway/dat/Incoming/mbt156','../mbt156','*.zip',
		0, 1, '2009-04-03 02:30:00.000', 'IF149_098_201103181200.zip',1)
END

IF NOT EXISTS(SELECT * FROM [dbo].[FTP_CONFIGURATION] WHERE DATA_FEED='cft439')
BEGIN
	INSERT INTO [dbo].[FTP_CONFIGURATION] VALUES(1,'cft439','LocalHost','TDP28Nov',
		'pP987rR2-','d:/Gateway/dat/Incoming/cft439','../cft439','*.zip',
		0, 1, '2009-04-03 02:30:00.000', 'IF146_098_201103181200.zip',1)
END

IF NOT EXISTS(SELECT * FROM [dbo].[FTP_CONFIGURATION] WHERE DATA_FEED='zdf451')
BEGIN
	INSERT INTO [dbo].[FTP_CONFIGURATION] VALUES(1,'zdf451','LocalHost','TDP28Nov',
		'yD1256-2-','d:/Gateway/dat/Incoming/zdf451','../zdf451','*.zip',
		0, 1, '2009-04-03 02:30:00.000', 'IF142_098_201103181200.zip',1)
END

IF NOT EXISTS(SELECT * FROM [dbo].[FTP_CONFIGURATION] WHERE DATA_FEED='omg654')
BEGIN
	INSERT INTO [dbo].[FTP_CONFIGURATION] VALUES(1,'omg654','LocalHost','TDP28Nov',
		'sI1732#3-','d:/Gateway/dat/Incoming/omg654','omg654','*.zip',
		0, 1, '2009-04-03 02:30:00.000', 'IF151_098_201103181200.zip',1)
END