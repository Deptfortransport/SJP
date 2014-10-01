CREATE TABLE [dbo].[CyclePlannerRequestEvent] (
    [Id]                    BIGINT       IDENTITY (1, 1) NOT NULL,
    [CyclePlannerRequestId] VARCHAR (50) NULL,
    [Cycle]                 BIT          NULL,
    [SessionId]             VARCHAR (50) NULL,
    [UserLoggedOn]          BIT          NULL,
    [TimeLogged]            DATETIME     NULL
);

