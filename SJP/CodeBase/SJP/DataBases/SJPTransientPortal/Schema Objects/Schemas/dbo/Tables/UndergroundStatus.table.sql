CREATE TABLE [dbo].[UndergroundStatus]
(
	[LineStatusId] [varchar](10) NOT NULL, 
	[LineStatusDetails] [varchar](300) NULL,
	[LineId] [varchar](10) NOT NULL, 
	[LineName] [varchar](30) NOT NULL, 
	[StatusId] [varchar](10) NOT NULL, 
	[StatusDescription] [varchar](30) NULL, 
	[StatusIsActive] bit NOT NULL, 
	[StatusCssClass] [varchar](30) NOT NULL
)
