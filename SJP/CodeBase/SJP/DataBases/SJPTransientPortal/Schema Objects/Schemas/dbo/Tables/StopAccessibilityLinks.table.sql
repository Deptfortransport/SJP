CREATE TABLE [dbo].[StopAccessibilityLinks]
(
	[StopNaPTAN] NVARCHAR (20) NOT NULL,
	[StopName] NVARCHAR (100) NULL,
	[StopOperator] [char](10) NOT NULL,
	[LinkUrl] NVARCHAR (300) NOT NULL,
	[WEFDate] DATE NOT NULL,
    [WEUDate] DATE NOT NULL
)
