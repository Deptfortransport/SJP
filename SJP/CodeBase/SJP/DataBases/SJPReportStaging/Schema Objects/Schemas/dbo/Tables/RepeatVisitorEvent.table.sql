CREATE TABLE [dbo].[RepeatVisitorEvent]
(
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RepeatVistorType] [varchar](20) NULL,
	[LastVisited] [datetime] NULL,
	[SessionIdOld] [varchar](50) NULL,
	[SessionIdNew] [varchar](50) NULL,
	[DomainName] [varchar](100) NULL,
	[UserAgent] [varchar](200) NULL,
	[ThemeId] [int] NULL,
	[TimeLogged] [datetime] NULL
) ON [PRIMARY]
