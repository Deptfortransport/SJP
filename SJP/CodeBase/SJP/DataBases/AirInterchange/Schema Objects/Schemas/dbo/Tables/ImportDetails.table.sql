CREATE TABLE [dbo].[ImportDetails] (
    [ID]    INT           IDENTITY (1, 1) NOT NULL,
    [Field] VARCHAR (50)  COLLATE Latin1_General_CI_AS NOT NULL,
    [Value] VARCHAR (250) COLLATE Latin1_General_CI_AS NOT NULL
);

