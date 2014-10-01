

---------------------------------------------------------------------
-- Update to SelectApplicationPropertiesWithPartnerId Proc
---------------------------------------------------------------------
CREATE PROCEDURE SelectApplicationPropertiesWithPartnerId
(
	@AID char(50)
	
)	 
AS
BEGIN

	SELECT (CAST(PartnerId AS varchar(10)) + '.' + PNAME) AS PNAME, PVALUE, ThemeId
	FROM PROPERTIES P
	WHERE P.AID = @AID
END