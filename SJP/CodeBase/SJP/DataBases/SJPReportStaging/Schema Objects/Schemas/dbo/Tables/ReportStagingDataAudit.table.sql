CREATE TABLE [dbo].[ReportStagingDataAudit] (
    [RSDAID]  INT      IDENTITY (1, 1) NOT NULL,
    [RSDTID]  INT      NOT NULL,
    [RSDATID] INT      NOT NULL,
    [Event]   DATETIME NOT NULL
);

