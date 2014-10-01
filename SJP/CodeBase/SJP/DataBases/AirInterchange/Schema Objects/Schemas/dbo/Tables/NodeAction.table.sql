CREATE TABLE [dbo].[NodeAction] (
    [ID]         INT          NOT NULL,
    [LegNodeID]  INT          NOT NULL,
    [NodeAction] VARCHAR (80) COLLATE Latin1_General_CI_AS NOT NULL,
    [Duration]   INT          NOT NULL
);

