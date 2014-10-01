CREATE PROCEDURE [dbo].[AddRepeatVisitorEventWebLogReader]
(
@RepeatVistorType varchar(20), 
@LastVisited datetime, 
@SessionIdOld varchar(50), 
@SessionIdNew varchar(50),
@DomainName varchar(100),
@UserAgent varchar(200),
@ThemeId int,
@TimeLogged datetime
)
As
BEGIN
	-- THIS IS A TEMPORARY STORED PROCEDURE TO INSERT DATA FOR SPECIFIC FOR DATA OBTAINED BY WEB LOG READER

	-- Perform check for session id to not insert duplicates
	Insert into RepeatVisitorEvent (RepeatVistorType, LastVisited, SessionIdOld, SessionIdNew, DomainName, UserAgent, ThemeId, TimeLogged)
	Values (@RepeatVistorType, @LastVisited, @SessionIdOld, @SessionIdNew, @DomainName, @UserAgent, @ThemeId, @TimeLogged)

END
    