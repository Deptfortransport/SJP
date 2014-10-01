CREATE TABLE [dbo].[LegNode] (
    [ID]                  INT           NOT NULL,
    [InterchangeID]       INT           NOT NULL,
    [GeoCodeRef]          VARCHAR (12)  COLLATE Latin1_General_CI_AS NULL,
    [InterchangeNodeType] VARCHAR (12)  COLLATE Latin1_General_CI_AS NOT NULL,
    [Description]         VARCHAR (256) COLLATE Latin1_General_CI_AS NULL,
    [NodeID]              VARCHAR (24)  COLLATE Latin1_General_CI_AS NULL,
    [NodeTypeID]          VARCHAR (12)  COLLATE Latin1_General_CI_AS NULL,
    [Order]               INT           NOT NULL
);



