
CREATE    PROCEDURE [dbo].[spGetVersion] AS
	SELECT [Field],[Value] FROM ImportDetails ORDER BY [ID]
Return