
---------------------------------------------------------------------
-- Update to SelectGroupPropertiesWithPartnerId Proc
---------------------------------------------------------------------
CREATE PROCEDURE SelectGroupPropertiesWithPartnerId
(
	@GID char(50)
)
AS
BEGIN
	SELECT (CAST(PartnerId AS varchar(10)) + '.' + PNAME) AS PNAME, PVALUE, ThemeId
	FROM PROPERTIES P
	WHERE P.GID = @GID

END