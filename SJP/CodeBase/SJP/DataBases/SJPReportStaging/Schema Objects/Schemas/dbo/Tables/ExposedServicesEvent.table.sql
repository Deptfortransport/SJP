CREATE TABLE [dbo].[ExposedServicesEvent] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [Submitted]  DATETIME     NULL,
    [Token]      VARCHAR (50) NULL,
    [Category]   VARCHAR (50) NULL,
    [Successful] BIT          NOT NULL,
    [TimeLogged] DATETIME     NOT NULL
);

