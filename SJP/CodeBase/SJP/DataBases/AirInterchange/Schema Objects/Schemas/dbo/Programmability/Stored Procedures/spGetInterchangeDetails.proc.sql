
CREATE   PROCEDURE [dbo].[spGetInterchangeDetails]
(
@ID int
)
AS

SELECT * FROM Interchange WHERE ID = @ID;
SELECT * FROM Link WHERE InterchangeID = @ID ORDER BY [Order];
SELECT * FROM LegNode WHERE InterchangeID = @ID ORDER BY LegNode.[Order];
--Only the sum of NodeAction.duration is required
SELECT LegNodeID,Sum(Duration) as SumDuration FROM NodeAction 
Group BY LegNodeID
ORDER BY LegNodeID;
SELECT * FROM Mapping WHERE InterchangeID = @ID ORDER BY [Order];