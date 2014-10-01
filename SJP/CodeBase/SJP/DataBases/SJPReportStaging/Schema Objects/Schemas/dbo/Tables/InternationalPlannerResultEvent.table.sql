CREATE TABLE [dbo].[InternationalPlannerResultEvent] (
    [Id]                            BIGINT       IDENTITY (1, 1) NOT NULL,
    [InternationalPlannerRequestId] VARCHAR (50) NULL,
    [ResponseCategory]              VARCHAR (50) NULL,
    [SessionId]                     VARCHAR (50) NULL,
    [UserLoggedOn]                  BIT          NULL,
    [TimeLogged]                    DATETIME     NULL
);

