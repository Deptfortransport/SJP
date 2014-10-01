CREATE TABLE [dbo].[CyclePlannerResultEvent] (
    [Id]                    BIGINT       IDENTITY (1, 1) NOT NULL,
    [CyclePlannerRequestId] VARCHAR (50) NULL,
    [ResponseCategory]      VARCHAR (50) NULL,
    [SessionId]             VARCHAR (50) NULL,
    [UserLoggedOn]          BIT          NULL,
    [TimeLogged]            DATETIME     NULL
);

