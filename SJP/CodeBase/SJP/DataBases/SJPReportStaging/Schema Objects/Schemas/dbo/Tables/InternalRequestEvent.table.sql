CREATE TABLE [dbo].[InternalRequestEvent] (
    [Id]                  BIGINT        IDENTITY (1, 1) NOT NULL,
    [InternalRequestId]   VARCHAR (50)  NULL,
    [SessionId]           NVARCHAR (88) NULL,
    [Submitted]           DATETIME      NULL,
    [InternalRequestType] VARCHAR (50)  NULL,
    [Success]             BIT           NULL,
    [RefTransaction]      BIT           NULL,
    [TimeLogged]          DATETIME      NULL,
    [FunctionType]        CHAR (2)      NOT NULL
);

