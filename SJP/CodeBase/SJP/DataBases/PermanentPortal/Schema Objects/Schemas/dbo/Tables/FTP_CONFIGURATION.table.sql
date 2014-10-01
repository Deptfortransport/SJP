CREATE TABLE [dbo].[FTP_CONFIGURATION] (
    [FTP_CLIENT]             INT           NULL,
    [DATA_FEED]              VARCHAR (16)  NOT NULL,
    [IP_ADDRESS]             VARCHAR (16)  NOT NULL,
    [USERNAME]               VARCHAR (32)  NOT NULL,
    [PASSWORD]               VARCHAR (32)  NOT NULL,
    [LOCAL_DIR]              VARCHAR (128) NOT NULL,
    [REMOTE_DIR]             VARCHAR (128) NOT NULL,
    [FILENAME_FILTER]        VARCHAR (32)  NOT NULL,
    [MISSING_FEED_COUNTER]   INT           NOT NULL,
    [MISSING_FEED_THRESHOLD] INT           NOT NULL,
    [DATA_FEED_DATETIME]     DATETIME      NULL,
    [DATA_FEED_FILENAME]     VARCHAR (128) NOT NULL,
    [REMOVE_FILES]           BIT           NULL
);

