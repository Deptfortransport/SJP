CREATE TABLE [dbo].[ContentOverride](
	[GroupId] [int] NOT NULL,
	[CultureCode] [char](2) NOT NULL DEFAULT('en'),
	[ControlName] [varchar](60) NOT NULL,
	[PropertyName] [varchar](100) NOT NULL,
	[ContentValue] [text] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL
 CONSTRAINT [PK_ContentOverride] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC,
	[CultureCode] ASC,
	[ControlName] ASC,
	[PropertyName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
