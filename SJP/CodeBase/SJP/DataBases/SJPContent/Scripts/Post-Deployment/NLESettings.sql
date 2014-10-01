-- =============================================
-- SCRIPT TO UPDATE NLE SPECIFIC SETTINGS 
-- Not run during deployment in any configuration but to be copied with release files and 
-- manually run during deployment to NLE.
-- =============================================

USE SJPContent
Go

------------------------------------------------
-- Analytics content, all added to the group 'Analytics' - differs dev/live/nle
------------------------------------------------

--COMMENTED OUT AS IT HAS BEEN DECIDED NLE WILL USE PROD SETTINGS TO SIMPLIFY DEPLOYMENTS
--NB This script MUST be the last one included as it updates changes made by previous scripts.

--DECLARE @Group varchar(100) = 'Analytics'
--DECLARE @CultEn varchar(2) = 'en'
--DECLARE @CultFr varchar(2) = 'fr'

-- Default analytics tag, as suplied by Google.
--EXEC AddContent @Group, @CultEn, 'Default', 'Analytics.Tag.Host', 
--'
--<script type="text/javascript">

--  var _gaq = _gaq || [];
--  _gaq.push([''_setAccount'', ''UA-23241277-1'']);
--  _gaq.push([''_trackPageview'']);

--  (function() {
--    var ga = document.createElement(''script''); ga.type = ''text/javascript''; ga.async = true;
--    ga.src = (''https:'' == document.location.protocol ? ''https://ssl'' : ''http://www'') + ''.google-analytics.com/ga.js'';
--    var s = document.getElementsByTagName(''script'')[0]; s.parentNode.insertBefore(ga, s);
--  })();

--</script>
--'

--GO