CREATE PROCEDURE [dbo].[GetAlternativeSuggestionList]
	@searchstring varchar(100),
	@maxRecords int
AS
BEGIN
	DECLARE @LikesFound int
	SELECT @LikesFound = COUNT(*) FROM dbo.SJPNonPostcodeLocations
	WHERE DisplayName LIKE '%' + @searchstring + '%'
	
	If (@LikesFound < @maxRecords)
	BEGIN
		SELECT TOP(@maxRecords) DisplayName, [Type], LocalityID, Naptan, DATASETID
		FROM dbo.SJPNonPostcodeLocations
		WHERE DisplayName LIKE '%' + @searchstring + '%' 
		UNION	
		SELECT TOP(@maxRecords - @LikesFound) DisplayName, [Type], LocalityID, Naptan, DATASETID
		FROM dbo.SJPNonPostcodeLocations
		WHERE SOUNDEX (@searchstring) = SOUNDEX (DisplayName) 
	END
	ELSE
	BEGIN
		SELECT TOP(@maxRecords) DisplayName, [Type], LocalityID, Naptan, DATASETID
		FROM dbo.SJPNonPostcodeLocations
		WHERE DisplayName LIKE '%' + @searchstring + '%' 
	END
END