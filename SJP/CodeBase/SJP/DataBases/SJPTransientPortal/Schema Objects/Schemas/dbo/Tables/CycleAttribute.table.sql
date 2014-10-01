CREATE TABLE [dbo].[CycleAttribute](
	[CycleAttributeId] [int] NOT NULL,
	[Description] [varchar](50) NULL,
	[Type] [varchar](10) NOT NULL,
	[Group] [varchar](10) NOT NULL,
	[Category] [varchar](25) NULL,
	[ResourceName] [varchar](50) NULL,
	[Mask] [varchar](10) NULL,
	[CycleInfrastructure] [bit] NOT NULL,
	[CycleRecommendedRoute] [bit] NOT NULL,
	[ShowAttribute] [bit] NOT NULL,
 CONSTRAINT [PK_CycleAttribute] PRIMARY KEY CLUSTERED 
(
	[CycleAttributeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]