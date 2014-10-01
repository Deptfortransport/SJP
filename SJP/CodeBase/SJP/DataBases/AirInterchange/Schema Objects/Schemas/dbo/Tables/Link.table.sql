CREATE TABLE [dbo].[Link] (
    [ID]            INT           NOT NULL,
    [InterchangeID] INT           NOT NULL,
    [Duration]      INT           NOT NULL,
    [Frequency]     INT           NULL,
    [FrequencyMin]  INT           NULL,
    [FrequencyMax]  INT           NULL,
    [LinkAction]    VARCHAR (24)  COLLATE Latin1_General_CI_AS NULL,
    [Description]   VARCHAR (100) COLLATE Latin1_General_CI_AS NULL,
    [Order]         INT           NOT NULL
);

