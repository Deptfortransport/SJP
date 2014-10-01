CREATE TABLE [dbo].[Accessibility] (
    [ID]            INT          NOT NULL,
    [LinkID]        INT          NOT NULL,
    [Hazard]        VARCHAR (24) COLLATE Latin1_General_CI_AS NOT NULL,
    [ChangeOfLevel] VARCHAR (12) COLLATE Latin1_General_CI_AS NULL
);

