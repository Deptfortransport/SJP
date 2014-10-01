CREATE TABLE [dbo].[DropDownLists](
	[DataSet] [varchar](200) NOT NULL,
	[ResourceID] [varchar](200) NOT NULL,
	[ItemValue] [varchar](200) NOT NULL,
	[IsSelected] [int] NOT NULL,
	[SortOrder] [int] NOT NULL
 CONSTRAINT [pk_DropDownLists] PRIMARY KEY CLUSTERED 
(
	[DataSet] ASC,
	[ResourceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]