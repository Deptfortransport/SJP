CREATE TABLE [dbo].[StopEventRequestEvent] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [Submitted]   DATETIME     NULL,
    [RequestId]   VARCHAR (50) NULL,
    [RequestType] VARCHAR (50) NULL,
    [Successful]  BIT          NOT NULL,
    [TimeLogged]  DATETIME     NOT NULL
);

