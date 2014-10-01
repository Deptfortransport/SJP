CREATE  PROCEDURE [dbo].[GetContent] 
(
    @Group      varchar(100),
    @Language char(2)
)
AS
BEGIN

    --First look up the culture code and GroupId 
    --for the data passed. It is better to do two look 
    --ups than perform lots of joins later on...
    DECLARE @GroupId int
   
    --Check the language:
    IF @Language NOT IN ('en', 'fr')
        BEGIN
            RAISERROR ('%s is an invalid language', 16, 1, @Language)
        END
    --END IF

    --Look up the group:
    SELECT @GroupId = GroupId 
      FROM ContentGroup
     WHERE [Name] = @Group

    --Check the group:
    IF @GroupId IS NULL
    	BEGIN
    	RAISERROR ('%s is an invalid group', 16, 1, @Group)
    	END

   
    --Now select the correct content.
    --The first part of the query gets all content for the  group and culture
	--It also take into account any overriddent content
    SELECT Content.ControlName,
           Content.PropertyName,
           ISNULL(ContentOverride.ContentValue, Content.ContentValue) ContentValue
      FROM Content
      LEFT JOIN ContentOverride
		ON Content.GroupId = ContentOverride.GroupId
		AND Content.ControlName = ContentOverride.ControlName
		AND Content.PropertyName = ContentOverride.PropertyName
		AND CAST(FLOOR( CAST( GETDATE() AS FLOAT ) ) AS DATETIME) >= CAST(FLOOR( CAST(ContentOverride.StartDate AS FLOAT ) ) AS DATETIME)
		AND CAST(FLOOR( CAST( GETDATE() AS FLOAT ) ) AS DATETIME) <= CAST(FLOOR( CAST(ContentOverride.EndDate AS FLOAT ) ) AS DATETIME)
     WHERE Content.GroupId = @GroupId
		AND Content.CultureCode = @Language
    --The second part of the query adds items that exist for en-GB culture
    --but not specified for the other cultures, to ensure there is a full
    --set of data.
    UNION ALL
    (
        SELECT C1.ControlName,
               C1.PropertyName,
               C1.ContentValue
          FROM Content C1
         WHERE CultureCode = 'en'
           AND GroupId = @GroupId
           AND NOT EXISTS (SELECT 1
                             FROM Content C2
                            WHERE C2.[ControlName] = C1.[ControlName]
                              AND C2.[PropertyName] = C1.[PropertyName]
                              AND CultureCode = @Language
                              AND GroupId = @GroupId
                           )
    )


END

