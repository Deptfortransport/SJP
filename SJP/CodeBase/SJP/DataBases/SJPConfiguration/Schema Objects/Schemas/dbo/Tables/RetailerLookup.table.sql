CREATE TABLE [dbo].[RetailerLookup]
(
	[OperatorCode] [char](10) NOT NULL,
	[Mode] [char](10) NOT NULL,
	[RetailerId] [char](20) NOT NULL,
	CONSTRAINT [PK_RetailerLookup] PRIMARY KEY CLUSTERED 
	(
		[OperatorCode] ASC,
		[Mode] ASC,
		[RetailerId] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
