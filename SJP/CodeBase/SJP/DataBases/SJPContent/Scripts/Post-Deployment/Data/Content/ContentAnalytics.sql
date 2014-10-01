-- =============================================
-- Content script to add Google Analytics tag resource data
-- =============================================

------------------------------------------------
-- Analytics and Adverts content, all added to the group 'Analytics'
------------------------------------------------

DECLARE @Group varchar(100) = 'Analytics'
DECLARE @CultEn varchar(2) = 'en'
DECLARE @CultFr varchar(2) = 'fr'

-- Tidy up first, helps keep content table clean,
-- and ensures this script contains complete control of content for this Group
EXEC DeleteAllGroupContent @Group

-- Example format for Analytics tag content:
-- group, language, collection(PageId), resourceKey, value

-- DON'T FORGET TO REPLACE ALL ' WITH '' IN THE TAG


-- Live analytics tag
EXEC AddContent @Group, @CultEn, 'Default', 'Analytics.Tag.Host', 
'
<script type="text/javascript">

  var _gaq = _gaq || [];
  _gaq.push([''_setAccount'', ''UA-23241082-1'']);
  _gaq.push([''_trackPageview'']);

  (function() {
    var ga = document.createElement(''script''); ga.type = ''text/javascript''; ga.async = true;
    ga.src = (''https:'' == document.location.protocol ? ''https://ssl'' : ''http://www'') + ''.google-analytics.com/ga.js'';
    var s = document.getElementsByTagName(''script'')[0]; s.parentNode.insertBefore(ga, s);
  })();

</script>
'
EXEC AddContent @Group, @CultEn, 'Default', 'Analytics.Tag.Tracker', 
''


-- Live adverts tag
EXEC AddContent @Group, @CultEn, 'Default', 'Adverts.Tag.Service', 
'
<script type="text/javascript">

	var googletag = googletag || {};
	googletag.cmd = googletag.cmd || [];

	(function() {
		var gads = document.createElement(''script'');
		gads.async = true;
		gads.type = ''text/javascript'';
		var useSSL = ''https:'' == document.location.protocol;
		gads.src = (useSSL ? ''https:'' : ''http:'') +''//www.googletagservices.com/tag/js/gpt.js'';
		var node = document.getElementsByTagName(''script'')[0];
		node.parentNode.insertBefore(gads, node);
	})();

</script>
'
EXEC AddContent @Group, @CultEn, 'Default', 'Adverts.Tag.Placeholders', 
'
<script type="text/javascript">

	googletag.cmd.push(function() {

	  //Adslot 1 declaration
      var slot1= googletag.defineSlot(''/7064/og/spectators/travel'', [[300,100]], ''top-ad'').addService(googletag.pubads());

      //Adslot 2 declaration
      <!-- var slot2= googletag.defineSlot(''/7064/og/spectators/travel'', [[300,250]], ''page-advert-3rdrail'').addService(googletag.pubads()); -->

      googletag.pubads().setTargeting(''lang'',[''en'']);
      googletag.pubads().enableAsyncRendering();
      googletag.enableServices();

	});

</script>
'

GO