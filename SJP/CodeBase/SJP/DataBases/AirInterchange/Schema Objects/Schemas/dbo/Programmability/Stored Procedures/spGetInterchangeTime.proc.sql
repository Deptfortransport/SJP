
CREATE    PROCEDURE [dbo].[spGetInterchangeTime]
(
@FromNodeID varchar(24),
@FromNodeTypeID varchar(12),
@FromOperatorCode varchar(12),
@ToNodeID varchar(24),
@ToNodeTypeID varchar(12),
@ToOperatorCode varchar(12),
@NullCombo int
)
AS
/****** 
Many combinations of the paramaters can be null.  The client will 
have validated the parameter null combination using a look up table.
The same index for that look up table will be passed to this 
procedure and then flow of control statements used.
Magic numbers will be used for this case statement.
Only way to not use magic numbers would be to define a table 
which the client application could update with the values.  
This would introduce unnecessary complexity to the design.
Hence any updates the the client parameter null combination
must be mirrored here.

NOTE that using a strategy of replacing null values with '%' is viewed
as likely to introduce performance limitations, in that there are a number
of combinations making it difficult to ensure that there is no combination
that will take an inordinate time to return on a query.
If performance limitations are found then the recomended startegy is to
define a seperate SP for each of the cases below.

Below are current values as defined in 
		//  No parameter is null			: 0
		//  FromOperator is Null			: 4
		//  ToOperator is Null			: 32
		//  From and To Operatore Null		: 36
		//  All 3 Froms are Null			: 7
		//  3 Froms + ToOperator are null		: 39  (7 + 32) --- {ToId, ToIDType} are only non null params
		//  All 3 Tos are Null			: 56
		//  3 Tos + FromOperator are null		: 60  (56 + 4) --- {FromId, FromIDType} are only non null params

******/

IF (@NullCombo = 0)
	Select [ID],NULL,NULL,TotalTime From Interchange
	Where  FromNodeID = @FromNodeID
	AND FromNodeTypeID = @FromNodeTypeID
	AND FromOperatorCode = @FromOperatorCode
	AND ToNodeID = @ToNodeID
	AND ToNodeTypeID = @ToNodeTypeID
	AND  ToOperatorCode = @ToOperatorCode

ELSE IF (@NullCombo = 4)
	Select [ID],NULL,NULL,TotalTime From Interchange
	Where  FromNodeID = @FromNodeID
	AND FromNodeTypeID = @FromNodeTypeID
	AND ToNodeID = @ToNodeID
	AND ToNodeTypeID = @ToNodeTypeID
	AND  ToOperatorCode = @ToOperatorCode

ELSE IF (@NullCombo = 7)
	Select [ID],FromNodeID,FromNodeTypeID,TotalTime From Interchange
	Where ToNodeID = @ToNodeID
	AND ToNodeTypeID = @ToNodeTypeID
	AND ToOperatorCode = @ToOperatorCode

ELSE IF (@NullCombo = 32)
	Select [ID],NULL,NULL,TotalTime From Interchange
	Where  FromNodeID = @FromNodeID
	AND FromNodeTypeID = @FromNodeTypeID
	AND FromOperatorCode = @FromOperatorCode
	AND ToNodeID = @ToNodeID
	AND ToNodeTypeID = @ToNodeTypeID

ELSE IF (@NullCombo = 36)
	Select [ID],NULL,NULL,TotalTime From Interchange
	Where  FromNodeID = @FromNodeID
	AND FromNodeTypeID = @FromNodeTypeID
	AND ToNodeID = @ToNodeID
	AND ToNodeTypeID = @ToNodeTypeID

ELSE IF (@NullCombo = 39)
	Select [ID],FromNodeID,FromNodeTypeID,TotalTime From Interchange
	Where  ToNodeID = @ToNodeID
	AND ToNodeTypeID = @ToNodeTypeID

ELSE IF (@NullCombo = 56)
	Select [ID],ToNodeID,ToNodeTypeID,TotalTime From Interchange
	Where FromNodeID = @FromNodeID
	AND FromNodeTypeID = @FromNodeTypeID
	AND FromOperatorCode = @FromOperatorCode

ELSE IF (@NullCombo = 60)
	Select [ID],ToNodeID,ToNodeTypeID,TotalTime From Interchange
	Where  FromNodeID = @FromNodeID
	AND FromNodeTypeID = @FromNodeTypeID