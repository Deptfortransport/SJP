CREATE TABLE [dbo].[InternationalPlannerRequestEvent] (
    [Id]                            BIGINT       IDENTITY (1, 1) NOT NULL,
    [InternationalPlannerRequestId] VARCHAR (50) NULL,
    [SessionId]                     VARCHAR (50) NULL,
    [UserLoggedOn]                  BIT          NULL,
    [TimeLogged]                    DATETIME     NULL
);

