CREATE TABLE [dbo].[TravelNews]
(
	[UID] [varchar](25) NOT NULL,
	[SeverityLevel] [tinyint] NOT NULL,
	[SeverityLevelOlympic] [tinyint] NOT NULL,
	[PublicTransportOperator] [varchar](100) NULL,
	[ModeOfTransport] [varchar](50) NOT NULL,
	[Regions] [varchar](200) NULL,
	[Location] [varchar](100) NOT NULL,
	[IncidentType] [varchar](60) NOT NULL,
	[HeadlineText] [varchar](150) NOT NULL,
	[DetailText] [varchar](350) NOT NULL,
	[TravelAdviceOlympicText] [varchar](350) NOT NULL,
	[IncidentStatus] [varchar](1) NOT NULL,
	[Easting] [decimal](18, 6) NOT NULL,
	[Northing] [decimal](18, 6) NOT NULL,
	[ReportedDateTime] [datetime] NOT NULL,
	[StartDateTime] [datetime] NOT NULL,
	[LastModifiedDateTime] [datetime] NOT NULL,
	[ClearedDateTime] [datetime] NULL,
	[ExpiryDateTime] [datetime] NULL,
	[PlannedIncident] [bit] NOT NULL,
	[RoadType] [nvarchar](10) NULL,
	[IncidentParent] [varchar](25) NULL,
	[CarriagewayDirection] [varchar](15) NULL,
	[RoadNumber] [varchar](25) NULL,
	[DayMask] [varchar](14) NULL,
	[DailyStartTime] [time](7) NULL,
	[DailyEndTime] [time](7) NULL,
	[ItemChangeStatus] [varchar](9) NULL,
 CONSTRAINT [PK_TravelNews] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
