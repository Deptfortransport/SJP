CREATE TABLE [dbo].[SJPGNATLocations] (
    [StopNaPTAN]        VARCHAR (20)		NOT NULL,
    [StopName]          NVARCHAR (150)		NOT NULL,
    [StopAreaNaPTAN]    NVARCHAR (20)		NULL,
    [StopOperator]      NVARCHAR (8)		NULL,
    [WheelchairAccess]  BIT					NOT NULL,
    [AssistanceService] BIT					NOT NULL,
    [WEFDate]           DATE				NOT NULL,
    [WEUDate]           DATE				NOT NULL,
    [MOFRStartTime]     TIME (0)			NOT NULL,
    [MOFREndTime]       TIME (0)			NOT NULL,
    [SatStartTime]      TIME (0)			NOT NULL,
    [SatEndTime]        TIME (0)			NOT NULL,
    [SunStartTime]      TIME (0)			NOT NULL,
    [SunEndTime]        TIME (0)			NOT NULL,
	[StopCountry]		NVARCHAR(20)		NOT NULL,
	[AdministrativeAreaCode] INT	    	NOT NULL,
	[NPTGDistrictCode]  INT		    		NOT NULL,
	[StopType]			NVARCHAR(20)		NOT NULL
);






 