CREATE TABLE [dbo].[LandingPageEvent] (
    [Id]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [LPPCode]      VARCHAR (50)  NOT NULL,
    [LPSCode]      VARCHAR (50)  NOT NULL,
    [TimeLogged]   DATETIME      NOT NULL,
    [SessionId]    NVARCHAR (88) NULL,
    [UserLoggedOn] BIT           NULL
);

