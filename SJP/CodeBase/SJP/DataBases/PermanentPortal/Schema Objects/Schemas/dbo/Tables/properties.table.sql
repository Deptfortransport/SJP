CREATE TABLE [dbo].[properties] (
    [pName]     VARCHAR (255)  NOT NULL,
    [pValue]    VARCHAR (2000) NOT NULL,
    [AID]       VARCHAR (50)   NOT NULL,
    [GID]       VARCHAR (50)   NOT NULL,
    [PartnerId] INT            DEFAULT (0) NOT NULL,
    [ThemeId]   INT            DEFAULT (1) NOT NULL
);

