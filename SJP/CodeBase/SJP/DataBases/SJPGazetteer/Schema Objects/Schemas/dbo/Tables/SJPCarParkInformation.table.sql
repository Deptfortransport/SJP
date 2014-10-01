CREATE TABLE [dbo].[SJPCarParkInformation]
(
	[CarParkID]       NVARCHAR (50)  NOT NULL, 
	[CultureCode]	  CHAR (2)		 NOT NULL DEFAULT('en'),
	[InformationText] NVARCHAR (MAX) NOT NULL
)
