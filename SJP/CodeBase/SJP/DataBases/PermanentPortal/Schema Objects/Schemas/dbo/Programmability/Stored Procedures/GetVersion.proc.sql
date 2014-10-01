

/* Create stored procedures */

-- GetVersion
CREATE PROCEDURE GetVersion
AS
 SELECT PVALUE FROM PROPERTIES
 WHERE PNAME='propertyservice.version'