CREATE TABLE [dbo].[SJPChecksumMonitoringResults] (
    [MonitoringItemID] INT           NOT NULL,
    [SJP_Server]       VARCHAR (50)  NOT NULL,
    [Description]      VARCHAR (200) NOT NULL,
    [CheckTime]        DATETIME      NOT NULL,
    [ValueAtCheck]     VARCHAR (200),
    [IsInRed]          BIT           NOT NULL
);

