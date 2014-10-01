CREATE TABLE [dbo].[Retailers]
(
	[RetailerId] [char](20) NOT NULL,
	[Name] [char](50) NOT NULL,
	[WebsiteURL] [char](300) NULL,
	[HandoffURL] [char](300) NULL,
	[DisplayURL] [char](100) NULL,
	[PhoneNumber] [char](100) NULL,
	[PhoneNumberDisplay] [char](100) NULL,
	[ResourceKey] [char](100) NULL,
	
	CONSTRAINT [PK_Retailer] PRIMARY KEY CLUSTERED 
	(
		[RetailerId] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

) ON [PRIMARY]
