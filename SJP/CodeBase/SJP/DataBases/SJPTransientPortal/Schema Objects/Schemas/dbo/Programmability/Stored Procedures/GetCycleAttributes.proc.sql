CREATE  PROCEDURE [dbo].[GetCycleAttributes]
AS
BEGIN

    SELECT      [CycleAttributeId],
                [Type], -- the type (Link or Node)
                [Group],  -- the group representing the attibute integer position in the attributes list
                [Category], -- the category of the attribute
                [ResourceName], 
            	[Mask],
				[CycleInfrastructure],
				[CycleRecommendedRoute]
    FROM        [CycleAttribute]
    WHERE       [ShowAttribute] = 1

END
