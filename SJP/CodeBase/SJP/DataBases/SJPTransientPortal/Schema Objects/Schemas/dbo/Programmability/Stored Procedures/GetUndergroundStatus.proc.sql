CREATE PROCEDURE [dbo].[GetUndergroundStatus]
AS
BEGIN

	 SELECT [LineStatusId],
			[LineStatusDetails],
			[LineId],
			[LineName],
			[StatusId],
			[StatusDescription],
			[StatusIsActive],
			[StatusCssClass] 
	   FROM	[dbo].[UndergroundStatus]
   ORDER BY [LineName]

END