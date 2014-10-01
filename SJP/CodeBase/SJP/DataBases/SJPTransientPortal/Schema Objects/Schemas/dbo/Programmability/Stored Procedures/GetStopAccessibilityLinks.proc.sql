CREATE PROCEDURE [dbo].[GetStopAccessibilityLinks]
	
AS
BEGIN

	SELECT  [StopNaPTAN],
			[StopOperator],
			[LinkUrl],
			[WEFDate],
			[WEUDate]
	  FROM  [dbo].[StopAccessibilityLinks]

END