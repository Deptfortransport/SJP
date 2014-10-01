CREATE TABLE [dbo].[EBCCalculationEvent] (
    [Id]         BIGINT       IDENTITY (1, 1) NOT NULL,
    [Submitted]  DATETIME     NULL,
    [SessionId]  VARCHAR (50) NULL,
    [TimeLogged] DATETIME     NULL,
    [Success]    BIT          NULL
);

