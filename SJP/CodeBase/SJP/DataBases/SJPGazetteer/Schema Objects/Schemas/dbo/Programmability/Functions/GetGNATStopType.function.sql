CREATE FUNCTION [dbo].[GetGNATStopType]
(
	@StopNaPTAN VARCHAR(20), 
	@StopOperator VARCHAR(4)
)
RETURNS VARCHAR(20)
AS
BEGIN
	DECLARE @StopType VARCHAR(20)

	-- Default to Bus
	SELECT @StopType = 'Bus'

	IF (LEFT(@StopNaPTAN,4) = '9100')
		SELECT @StopType = 'Rail'

	IF (LEFT(@StopNaPTAN,4) = '9200')
		SELECT @StopType = 'Air'

	IF (LEFT(@StopNaPTAN,4) = '9000')
		SELECT @StopType = 'Coach'

	IF (LEFT(@StopNaPTAN,4) = '9400')
	BEGIN
		SELECT @StopType = 'Tram'

		IF(@StopOperator = 'LU')
			SELECT @StopType = 'Underground'

		IF(@StopOperator = 'DL')
			SELECT @StopType = 'DLR'

	END

	IF (LEFT(@StopNaPTAN,4) = '9300')
		SELECT @StopType = 'Ferry'

	RETURN @StopType

END