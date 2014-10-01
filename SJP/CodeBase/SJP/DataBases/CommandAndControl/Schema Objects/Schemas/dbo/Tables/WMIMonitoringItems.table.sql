CREATE TABLE [dbo].[WMIMonitoringItems] (
    [ItemID]        INT           IDENTITY (1, 1) NOT NULL,
    [CheckInterval] INT           NOT NULL,
    [Enabled]       BIT           NOT NULL,
    [Description]   VARCHAR (200) NOT NULL,
    [WQLQuery]      VARCHAR (300) NOT NULL,
    [RedCondition]  VARCHAR (200) NOT NULL
);

