CREATE TABLE [dbo].[GradientProfileEvent] (
    [Id]              BIGINT       IDENTITY (1, 1) NOT NULL,
    [DisplayCategory] VARCHAR (50) NULL,
    [Submitted]       DATETIME     NULL,
    [SessionId]       VARCHAR (50) NULL,
    [UserLoggedOn]    BIT          NULL,
    [TimeLogged]      DATETIME     NULL
);

