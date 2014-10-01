CREATE TABLE [dbo].[Interchange] (
    [ID]               INT           NOT NULL,
    [TotalTime]        INT           NOT NULL,
    [FromName]         VARCHAR (256) COLLATE Latin1_General_CI_AS NULL,
    [FromNodeID]       VARCHAR (24)  COLLATE Latin1_General_CI_AS NOT NULL,
    [FromNodeTypeID]   VARCHAR (12)  COLLATE Latin1_General_CI_AS NOT NULL,
    [FromGridType]     VARCHAR (12)  COLLATE Latin1_General_CI_AS NULL,
    [FromEasting]      INT           NULL,
    [FromNorthing]     INT           NULL,
    [FromBay]          VARCHAR (50)  COLLATE Latin1_General_CI_AS NULL,
    [FromOperatorCode] VARCHAR (12)  COLLATE Latin1_General_CI_AS NULL,
    [FromOperatorName] VARCHAR (50)  COLLATE Latin1_General_CI_AS NULL,
    [ToName]           VARCHAR (256) COLLATE Latin1_General_CI_AS NULL,
    [ToNodeID]         VARCHAR (24)  COLLATE Latin1_General_CI_AS NOT NULL,
    [ToNodeTypeID]     VARCHAR (12)  COLLATE Latin1_General_CI_AS NOT NULL,
    [ToGridType]       VARCHAR (12)  COLLATE Latin1_General_CI_AS NULL,
    [ToEasting]        INT           NULL,
    [ToNorthing]       INT           NULL,
    [ToBay]            VARCHAR (50)  COLLATE Latin1_General_CI_AS NULL,
    [ToOperatorCode]   VARCHAR (12)  COLLATE Latin1_General_CI_AS NULL,
    [ToOperatorName]   VARCHAR (50)  COLLATE Latin1_General_CI_AS NULL
);

