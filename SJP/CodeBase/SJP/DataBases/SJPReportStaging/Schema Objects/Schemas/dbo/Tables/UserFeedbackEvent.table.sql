CREATE TABLE [dbo].[UserFeedbackEvent] (
    [Id]                 INT           IDENTITY (1, 1) NOT NULL,
    [SessionId]          NVARCHAR (88) NULL,
    [SubmittedTime]      DATETIME      NULL,
    [FeedbackType]       VARCHAR (50)  NULL,
    [AcknowledgedTime]   DATETIME      NULL,
    [AcknowledgmentSent] BIT           NULL,
    [UserLoggedOn]       BIT           NULL,
    [TimeLogged]         DATETIME      NULL
);

