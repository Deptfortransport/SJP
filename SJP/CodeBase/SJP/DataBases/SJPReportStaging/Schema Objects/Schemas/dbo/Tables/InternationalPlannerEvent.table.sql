CREATE TABLE [dbo].[InternationalPlannerEvent] (
    [Id]                       BIGINT       IDENTITY (1, 1) NOT NULL,
    [InternationalPlannerType] VARCHAR (50) NULL,
    [SessionId]                VARCHAR (50) NULL,
    [UserLoggedOn]             BIT          NULL,
    [TimeLogged]               DATETIME     NULL
);

