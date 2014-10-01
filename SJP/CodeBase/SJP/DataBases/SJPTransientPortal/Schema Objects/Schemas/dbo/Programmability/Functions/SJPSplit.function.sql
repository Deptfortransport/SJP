CREATE FUNCTION [dbo].[SJPSplit]
(
	@List nvarchar(2000),
	@SplitOn nvarchar(5)
)
RETURNS @returntable TABLE 
(
	Id int identity(1,1),
	Value nvarchar(100)
)
AS
BEGIN
		While (Charindex(@SplitOn,@List)>0)
		Begin

			Insert Into @returntable (value)
			Select 
				Value = ltrim(rtrim(Substring(@List,1,Charindex(@SplitOn,@List)-1)))

			Set @List = Substring(@List,Charindex(@SplitOn,@List)+len(@SplitOn),len(@List))
		End

		Insert Into @returntable (Value)
		Select Value = ltrim(rtrim(@List))
	
		Return
END