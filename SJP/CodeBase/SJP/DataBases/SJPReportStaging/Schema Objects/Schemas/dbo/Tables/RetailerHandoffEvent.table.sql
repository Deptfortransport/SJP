CREATE TABLE [dbo].[RetailerHandoffEvent] (
    [Id]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [RetailerId]   VARCHAR (50)  NULL,
    [SessionId]    NVARCHAR (88) NULL,
    [UserLoggedOn] BIT           NULL,
    [TimeLogged]   DATETIME      NULL
);

