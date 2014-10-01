CREATE PROCEDURE [dbo].[Generate_NO_RESULTS_EVENT_Monthly_Extract]
	@current_month int,
	@current_year int
AS
	SET NOCOUNT ON
	SET DATEFIRST 1
	SET XACT_ABORT ON

	SET @current_month=ISNULL(@current_month,datepart(Month,GETDATE()-1))
	SET @current_year=ISNULL(@current_year,datepart(YEAR,GETDATE()-1))
   
	SELECT   CONVERT(varchar,Submitted,103) as "Date",COUNT(*) as "No Results Count"
	  FROM [SJPReportStaging].[dbo].[NoResultsEvent]
	  where DATEPART(MONTH,Submitted) = @current_month
	  and DATEPART(YEAR,Submitted) = @current_year
	  group by  CONVERT(varchar,Submitted,103)
	  order by  CONVERT(varchar,Submitted,103)
     
RETURN 0