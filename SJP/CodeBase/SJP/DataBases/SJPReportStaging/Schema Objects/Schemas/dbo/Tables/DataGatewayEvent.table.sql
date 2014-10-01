CREATE TABLE [dbo].[DataGatewayEvent] (
    [Id]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [FeedId]       VARCHAR (50)  NULL,
    [SessionId]    NVARCHAR (88) NULL,
    [FileName]     VARCHAR (100) NULL,
    [TimeStarted]  DATETIME      NULL,
    [TimeFinished] DATETIME      NULL,
    [SuccessFlag]  BIT           NULL,
    [ErrorCode]    INT           NULL,
    [UserLoggedOn] BIT           NULL,
    [TimeLogged]   DATETIME      NULL
);

