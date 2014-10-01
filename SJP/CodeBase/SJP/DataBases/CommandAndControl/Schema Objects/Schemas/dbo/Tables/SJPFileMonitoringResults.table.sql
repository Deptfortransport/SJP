﻿CREATE TABLE [dbo].[SJPFileMonitoringResults] (
    [MonitoringItemID]     INT           NOT NULL,
    [SJP_Server]           VARCHAR (50)  NOT NULL,
    [Description]          VARCHAR (200) NOT NULL,
    [CheckTime]            DATETIME      NOT NULL,
    [ValueAtCheck]         VARCHAR (200) NOT NULL,
    [FullFilePath]         VARCHAR (300) NULL,
    [FileCreatedDateTime]  DATETIME      NULL,
    [FileModifiedDateTime] DATETIME      NULL,
    [FileSize]             VARCHAR (30)  NULL,
    [FileProductVersion]   VARCHAR (30)  NULL,
    [IsInRed]              BIT           NOT NULL
);

