CREATE TABLE [dbo].[JourneyWebRequestEvent] (
    [Id]                  BIGINT        IDENTITY (1, 1) NOT NULL,
    [JourneyWebRequestId] VARCHAR (50)  NULL,
    [SessionId]           NVARCHAR (88) NULL,
    [Submitted]           DATETIME      NULL,
    [RegionCode]          VARCHAR (50)  NULL,
    [Success]             BIT           NULL,
    [RefTransaction]      BIT           NULL,
    [TimeLogged]          DATETIME      NULL,
    [RequestType]         VARCHAR (50)  NOT NULL
);

