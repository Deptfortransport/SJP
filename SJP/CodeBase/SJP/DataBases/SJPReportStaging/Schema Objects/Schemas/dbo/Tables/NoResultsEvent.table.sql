CREATE TABLE [dbo].[NoResultsEvent] (
    [Id]					BIGINT       IDENTITY (1, 1) NOT NULL,
    [Submitted]             DATETIME     NOT NULL,
    [SessionId]             VARCHAR (50) NULL,
	[UserLoggedOn]          BIT          NULL,
    [TimeLogged]            DATETIME     NULL
);