CREATE TABLE [dbo].[Partner] (
    [PartnerId]       INT           NOT NULL,
    [HostName]        VARCHAR (50)  NOT NULL,
    [PartnerName]     VARCHAR (100) NOT NULL,
    [Channel]         VARCHAR (50)  NOT NULL,
    [PartnerPassword] VARCHAR (250) DEFAULT (null) NULL
);

