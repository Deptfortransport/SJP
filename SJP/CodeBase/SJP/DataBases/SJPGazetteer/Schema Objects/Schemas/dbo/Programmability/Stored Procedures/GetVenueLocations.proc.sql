CREATE PROCEDURE [dbo].[GetVenueLocations]
AS
BEGIN

	-- Gets all Venue locations from the SJPNonPostcodeLocations table
	-- and joins the SJPVenueAdditionalData to return a complete Venue data row
	SELECT 
		 REPLACE(ParentId, 'napt:', '') as [ParentID]
		,[Name]
		,[DisplayName]
		,[Type]
		,[Naptan]
		,[LocalityID]
		,[Easting]
		,[Northing]
		,[NearestTOID]
		,[NearestPointE]
		,[NearestPointN]
		,[AdminAreaID]
		,[DistrictID]
		,ISNULL(ISNULL(VenueData.UseNaPTANforJourneyPlanning,ParentVenueData.UseNaPTANforJourneyPlanning),0) UseNaPTANforJourneyPlanning
		,ISNULL(ISNULL(VenueData.CycleToVenueDistance, ParentVenueData.CycleToVenueDistance),0) CycleToVenueDistance
		,ISNULL(VenueData.VenueMapURL, ParentVenueData.VenueMapURL) VenueMapURL
		,ISNULL(VenueData.VenueTravelNewsRegion, ParentVenueData.VenueTravelNewsRegion) VenueTravelNewsRegion
		,ISNULL(VenueData.VenueWalkingRoutesMapURL, ParentVenueData.VenueWalkingRoutesMapURL) VenueWalkingRoutesMapURL
		,ISNULL(ISNULL(VenueData.AccesibleJourneyToVenue, ParentVenueData.AccesibleJourneyToVenue),0) AccesibleJourneyToVenue
		,ISNULL(VenueData.VenueRiverServiceAvailable, ParentVenueData.VenueRiverServiceAvailable) VenueRiverServiceAvailable
		,VenueData.VenueGroupID
		,VenueData.VenueGroupName
	FROM dbo.SJPNonPostcodeLocations VenueLocation
	LEFT JOIN dbo.SJPVenueAdditionalData VenueData
		On VenueLocation.Naptan = VenueData.VenueNaPTAN
	LEFt JOIN dbo.SJPVenueAdditionalData ParentVenueData
		On REPLACE(VenueLocation.ParentId, 'napt:', '') = ParentVenueData.VenueNaPTAN
	WHERE [Type] = 'VENUEPOI'
END
