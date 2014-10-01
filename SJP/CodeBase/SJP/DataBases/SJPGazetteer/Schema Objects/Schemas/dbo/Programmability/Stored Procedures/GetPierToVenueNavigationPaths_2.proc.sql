
CREATE PROCEDURE [dbo].[GetPierToVenueNavigationPaths]
AS
	BEGIN

    SELECT pvnp.NavigationID,
           DefaultDuration, 
           Distance, 
           ToNaPTAN, 
           FromNaPTAN, 
           VenueNaPTAN,
           CultureCode as TransferLanguage,
           TransferDescription as TransferText
      FROM [dbo].[SJPPierVenueNavigationPath] pvnp
 LEFT JOIN [dbo].[SJPPierVenueNavigationPathTransfers] pvnpt
        ON pvnp.NavigationID = pvnpt.NavigationID

	END