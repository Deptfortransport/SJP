﻿CREATE PROCEDURE [dbo].[GetVersion]
AS

SELECT PVALUE FROM PROPERTIES
	WHERE PNAME='propertyservice.version'

RETURN 0