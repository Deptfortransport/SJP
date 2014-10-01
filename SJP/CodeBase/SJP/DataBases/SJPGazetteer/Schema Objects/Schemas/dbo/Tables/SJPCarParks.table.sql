CREATE TABLE [dbo].[SJPCarParks] (
    [CarParkID]           NVARCHAR (50)  NOT NULL,
    [CarParkName]         NVARCHAR (150) NOT NULL,
    [VenueServed]         NVARCHAR (20)  NULL,
    [MapOfSiteURL]        NVARCHAR (150) NULL,
    [InterchangeDuration] INT            NOT NULL,
    [CoachSpaces]         INT            NULL,
    [CarSpaces]           INT            NULL,
    [DisabledSpaces]      INT            NULL,
    [BlueBadgeSpaces]     INT            NULL
);







