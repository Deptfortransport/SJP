

CREATE TABLE [dbo].[ChecksumMonitoringItems] (
    [ItemID]				INT           IDENTITY (1, 1) NOT NULL,
    [ChecksumRootPath]		VARCHAR (300) NOT NULL,
    [CheckInterval]			INT           NOT NULL,
    [Enabled]				BIT           NOT NULL,
	[ExtensionsToIgnore]    VARCHAR (200) NOT NULL,
    [Description]			VARCHAR (200) NOT NULL,
    [RedCondition]			VARCHAR (200) NOT NULL
);


