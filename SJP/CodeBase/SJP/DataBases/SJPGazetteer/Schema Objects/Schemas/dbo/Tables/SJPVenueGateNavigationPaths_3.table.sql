CREATE TABLE [dbo].[SJPVenueGateNavigationPaths] (
    [NavigationPathId]   NVARCHAR (50)  NOT NULL,
    [NavigationPathName] NVARCHAR (150) NOT NULL,
    [FromNaptan]         NVARCHAR (20)  NOT NULL,
    [ToNaptan]           NVARCHAR (20)  NOT NULL,
    [TransferDuration]   TIME (0)       NOT NULL,
    [TransferDistance]   INT            NOT NULL,
    [GateNaptan]         NVARCHAR (20)  NOT NULL
);







