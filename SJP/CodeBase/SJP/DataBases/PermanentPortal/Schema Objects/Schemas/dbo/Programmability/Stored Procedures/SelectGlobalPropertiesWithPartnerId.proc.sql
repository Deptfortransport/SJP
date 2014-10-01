
---------------------------------------------------------------------
-- Update to SelectGlobalPropertiesWithPartnerId Proc
---------------------------------------------------------------------
CREATE PROCEDURE SelectGlobalPropertiesWithPartnerId

AS
BEGIN

	SELECT (CAST(PartnerId AS varchar(10)) + '.' + PNAME) AS PNAME, PVALUE, ThemeId
	FROM PROPERTIES P
	WHERE P.AID = '<DEFAULT>'
	AND P.GID = '<DEFAULT>'

END