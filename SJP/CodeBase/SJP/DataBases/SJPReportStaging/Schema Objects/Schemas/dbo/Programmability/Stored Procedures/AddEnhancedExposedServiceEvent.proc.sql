
-- Create stored procedure to log events in the ReportStaging DB

CREATE PROCEDURE [dbo].[AddEnhancedExposedServiceEvent]
	
	(
	@PartnerId SmallInt, 
	@InternalTransactionId Varchar(40), 
    @ExternalTransactionId Varchar(100)= 'default_value',
	@ServiceType varchar(200),
	@OperationType varchar(100),	
	@EventTime datetime,
	@IsStartEvent bit,
	@CallSuccessful bit
	) 
AS

BEGIN
	SET NOCOUNT OFF

	DECLARE @localized_string_UnableToInsert AS VARCHAR(256)
   	SET @localized_string_UnableToInsert = 'Unable to Insert a new record into EnhancedExposedServiceEvent Table'
	-- SET @ExternalTransactionId = ISNULL(@ExternalTransactionId,'NA')
	INSERT INTO EnhancedExposedServiceEvent (EESEPartnerId, EESEInternalTransactionId, EESEExternalTransactionId, EESEServiceType, EESEOperationType, EESEEventTime, EESEIsStartEvent, EESECallSuccessful)
	VALUES (@PartnerId, @InternalTransactionId, @ExternalTransactionId, @ServiceType, @OperationType, @EventTime, @IsStartEvent, @CallSuccessful)
		
	IF @@error <> 0
	    BEGIN
	        RAISERROR (@localized_string_UnableToInsert, 1,1)
		RETURN -1
	    END
	ELSE
	    RETURN @@rowcount

END