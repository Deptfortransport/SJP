CREATE TABLE [dbo].[SJPTravelcards]
(
	[TravelCardID]		NVARCHAR (20)  NOT NULL,
	[TravelCardName]	NVARCHAR (50)  NULL,
	[ValidFrom]			DATETIME       NOT NULL,
    [ValidTo]			DATETIME       NOT NULL
)