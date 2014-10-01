CREATE PROCEDURE [dbo].[ImportSJPAdditionalVenueData]
(
	@XML text
)
AS
/*
SP Name: ImportSJPAdditionalVenueData
Input : XML string as text
Output: None
Description:  It takes xml data as string and inserts the data into SJP venue additional data.
*/

SET NOCOUNT ON

DECLARE @DocID int, @XMLPathData varchar(200)

-- Loading xml document 
EXEC sp_xml_preparedocument @DocID OUTPUT, @XML
-- setting the node 
SET @XMLPathData =  '/SJPAdditionalVenueData/Venue'

-- starting trasaction 
BEGIN TRANSACTION

	-- Delete from SJPPierVenueNavigationPathTransfers table
	DELETE FROM [dbo].[SJPPierVenueNavigationPathTransfers]

	-- Delete from SJPPierVenueNavigationPath table
	DELETE FROM [dbo].[SJPPierVenueNavigationPath]

	-- Delete from SJPRiverServices table 
	DELETE FROM [dbo].[SJPRiverServices]

	-- Delete from SJPVenueAdditionalData table
	DELETE FROM  [dbo].[SJPVenueAdditionalData]
	
	
	-- Insert additional venue data
	INSERT INTO [dbo].[SJPVenueAdditionalData]
	(
			[VenueNaPTAN]
           ,[UseNaPTANforJourneyPlanning]
           ,[CycleToVenueDistance]
           ,[VenueMapURL]
           ,[VenueWalkingRoutesMapURL]
           ,[VenueTravelNewsRegion]
           ,[AccesibleJourneyToVenue]
		   ,[VenueRiverServiceAvailable]
		   ,[VenueGroupID]
		   ,[VenueGroupName]
	) 
	SELECT
		 X.[VenueNaPTAN]
        ,X.[UseNaPTANforJourneyPlanning]
        ,X.[CycleToVenueDistance]
        ,X.[VenueMapURL]
        ,X.[VenueWalkingRoutesMapURL]
        ,X.[VenueTravelNewsRegion]
        ,X.[AccesibleJourneyToVenue]
		,X.[VenueRiverServiceAvailable]
        ,REPLACE(X.[VenueGroupID],'oda:','')
        ,X.[VenueGroupName]
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(
		[VenueNaPTAN] [nvarchar](20),
		[UseNaPTANforJourneyPlanning] [bit] 'UseNaPTANForJourneyPlanning',
		[CycleToVenueDistance] [int] ,
		[VenueMapURL] [nvarchar](300),
		[VenueWalkingRoutesMapURL] [nvarchar](300),
		[VenueTravelNewsRegion] [nvarchar](50),
		[AccesibleJourneyToVenue] [bit],
		[VenueRiverServiceAvailable] [nvarchar] (10),
		[VenueGroupID] [nvarchar] (30) 'InVenueGroup/VenueGroupID',
		[VenueGroupName] [nvarchar] (100) 'InVenueGroup/VenueGroupName'
	) X 
	
SET @XMLPathData =  '/SJPAdditionalVenueData/Venue/VenueRiverServices/VenueRiverService/VenuePier/RemotePier'

	-- Insert river services data
	INSERT INTO [dbo].[SJPRiverServices]
	(
		 [VenueNaPTAN]
        ,[VenuePierNaPTAN]
        ,[RemotePierNaPTAN]
        ,[VenuePierName]
        ,[RemotePierName]
	) 
	SELECT DISTINCT 
		 X.[VenueNaPTAN] 
        ,X.[VenuePierNaPTAN]
        ,X.[RemotePierNaPTAN]
        ,X.[VenuePierName]
        ,X.[RemotePierName]
	FROM
	OPENXML (@DocID, @XMLPathData , 2)
	WITH
	(
		[VenueNaPTAN] [nvarchar](20) '../../../../VenueNaPTAN',
		[VenuePierNaPTAN] [nvarchar](20) '../VenuePierNaPTAN',
		[RemotePierNaPTAN] [nvarchar](20),
		[VenuePierName] [nvarchar](150) '../VenuePierName',
		[RemotePierName] [nvarchar](150)
	) X

SET @XMLPathData =  '/SJPAdditionalVenueData/Venue/VenueRiverServices/VenueRiverService/VenuePier/PierVenueNavigationPath'

	-- Insert pier navigation path data
	INSERT INTO [dbo].[SJPPierVenueNavigationPath]
	(
		 [NavigationID]
        ,[DefaultDuration]
        ,[Distance]
        ,[ToNaPTAN]
        ,[FromNaPTAN]
        ,[VenueNaPTAN]
		,[ToVenue]
	) 
	SELECT DISTINCT 
		 X.[NavigationID]
        ,X.[DefaultDuration]
        ,X.[Distance]
        ,X.[ToNaPTAN]
        ,X.[FromNaPTAN]
        ,X.[VenueNaPTAN]
		,X.[PathtoVenue]
	FROM
	OPENXML (@DocID, @XMLPathData , 2)
	WITH
	(
		[NavigationID] [nvarchar](50),
		[DefaultDuration] [time],
		[Distance] [int],
		[ToNaPTAN] [nvarchar](20) 'To/PlaceRef',
		[FromNaPTAN] [nvarchar](20) 'From/PlaceRef',
		[VenueNaPTAN] [nvarchar](20) '../../../../VenueNaPTAN',
		[PathtoVenue] [bit]
	) X


SET @XMLPathData =  '/SJPAdditionalVenueData/Venue/VenueRiverServices/VenueRiverService/VenuePier/PierVenueNavigationPath/NavigationPathName'

	-- Insert pier navigation path transfer data
	INSERT INTO [dbo].[SJPPierVenueNavigationPathTransfers]
	(
		 [NavigationID]
		,[CultureCode]
		,[TransferDescription]
	)
	SELECT
		 X.[NavigationID]
		,X.[NavigationPathLanguage]
		,X.[NavigationPathText]
	FROM
	OPENXML (@DocID, @XMLPathData , 2)
	WITH
	(
		[NavigationID]	[nvarchar] (50) '../NavigationID',
		[NavigationPathLanguage]	[char] (2),
		[NavigationPathText]		[nvarchar] (max)
	) X



COMMIT TRANSACTION 

-- Removing xml doc from memorry
EXEC sp_xml_removedocument @DocID

