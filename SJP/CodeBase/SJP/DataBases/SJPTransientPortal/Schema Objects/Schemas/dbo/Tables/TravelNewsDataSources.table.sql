CREATE TABLE [dbo].[TravelNewsDataSources]
(
	[DataSourceId] [varchar](50) NOT NULL,
	[DataSourceName] [varchar](500) NOT NULL,
	[Trusted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[DataSourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
