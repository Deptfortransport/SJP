-- =============================================
-- SCRIPT TO UPDATE DEV MACHINE SPECIFIC SETTINGS - OVERWRITES PRODUCTION SETTINGS IN DB PROJECT IF BUILD CONFIGURATION = DEBUG
-- =============================================

-- Analytics content, all added to the group 'Analytics'

USE SJPContent
Go

DECLARE @Group varchar(100) = 'Analytics'
DECLARE @CultEn varchar(2) = 'en'
DECLARE @CultFr varchar(2) = 'fr'

-- Live analytics tag
EXEC AddContent @Group, @CultEn, 'Default', 'Analytics.Tag.Host', ''

-- Live adverts tag
EXEC AddContent @Group, @CultEn, 'Default', 'Adverts.Tag.Service', ''
EXEC AddContent @Group, @CultEn, 'Default', 'Adverts.Tag.Placeholders', ''

GO