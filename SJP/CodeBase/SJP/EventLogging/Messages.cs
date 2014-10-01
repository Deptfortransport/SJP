// *********************************************** 
// NAME             : Messages.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Contains event logging messages 
// ************************************************


namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Summary description for Messages.
    /// </summary>
    public class Messages
    {
        // **** Property validation messages ***
        public const string CustomPublisherNameMismatch = "Custom Publisher Class [{0}] has an invalid class name or invalid identifier. According to the given class identifier the class name should be specified in property key [{1}].";
        public const string CustomPublisherInvalidId = "Custom Publisher Class [{0}] does not have a valid identifier assigned to it. Current identifier value is [{1}].";
        public const string UndefinedPublisher = "A Publisher specified for an event [{0}] has an id [{1}] that does not match any of the publishers available.";
        public const string FilePublisherDirectoryBad = "Directory [{0}] of file publisher specified in property [{1}] does not exist.";
        public const string FilePublisherRotationBad = "Rotation value [{0}] of file publisher specified in property [{1}] must be an integer and greater than zero.";
        public const string QueuePublisherMSMQBad = "MSMQ [{0}] of queue publisher specified in property [{1}] cannot be written to. MSMQ may not exist or path may be invalid.";
        public const string QueuePublisherDeliveryBad = "Delivery method [{0}] of queue publisher specified in property [{1}] is invalid. Must take value Express|Recoverable.";
        public const string EmailPublisherAddressBad = "Email address [{0}] of email publisher specified in property [{1}] does not follow RFC 822 standard.";
        public const string LoggingPropertyValidatorKeyBad = "LoggingPropertyValidators unable to validate key [{0}]";
        public const string EventLogPublisherEventLogBad = "Event Log [{0}] on machine [{1}] belonging to event log publisher does not exist. Values were specified in properties [{2}] and [{3}].";
        public const string EventLogPublisherEventLogNotFound = "Event Log belonging to event log publisher cannot be found. Reason: [{0}]. Values were specified in properties [{1}] and [{2}].";
        public const string CustomPublisherArrayBad = "Custom Publisher array cannot be null. Number of elements must be zero or more.";
        public const string EventHasZeroPublishers = "One or more publishers have not been defined for the event specified in key [{0}]. It is mandatory to specify one or more publishers for this event.";
        public const string IncorrectClassType = "Class [{0}] specified in property key [{1}] does not derive from class [{2}].";
        public const string UnknownCustomEventClassName = "The name of the Custom Event class [{0}] is unknown. Ensure: (1) the correct Custom Event class 'name' is passed to the CustomEventLevelSwitch. (2) The SJPTraceListener has been added to the trace listener collection. (3) The Custom Event class has been defined in the logging service configuration properties.";
        public const string CustomEventSwitchNotInitialised = "The CustomEventSwitch class instance has not been initialised. Ensure that the SJPTraceListener has been created first.";

        public const string OperationalEventLevelInvalid = "An Operational Event has been created using an invalid SJPTraceLevel.";

        // *** Standard Publisher messages ***
        public const string ConsolePublisherWriteEvent =
            "Exception occured when Console Publisher attempted to write log event to the console.";
        public const string EmailPublisherWriteEvent =
            "Exception occurred while attempting to send the log event using Email Publisher.  The value(s) that may have caused the exception are:{0}";
        public const string EventLogPublisherConstructor =
            "Exception [{0}] occured when constructing an Event Log Publisher:  Event Log Name:[{1}] Event Log Source:[{2}] Event Log Machine:[{3}]";
        public const string EventLogPublisherWriteEvent =
            "Exception occured when publishing the event using Event Log Publisher:- Event Log Name:[{0}] Event Log Source:[{1}] Event Log Machine:[{2}]";
        public const string FilePublisherConstructor =
            "Exception occured while attempting to set-up the File Publisher.  The value(s) that may have caused the exception are:{0}";
        public const string FilePublisherWriteEvent =
            "Exception occured while attempting to write the log event [{0}] to the file [{1}].";
        public const string QueuePublisherConstructor =
            "Exception occured while attempting to set-up the Message Queue.  The value(s) that may have caused the exception are:{0}";
        public const string QueuePublisherWriteEvent =
            "Exception occured while attempting to send the logEvent to the Message Queue. The value(s) that may have caused the exception are:{0}";

        // Custom Email Publisher messages
        public const string CustomEmailPublisherSaveFailed = "CustomEmailPublisher failed to save attachment as a file to the filepath [{0}]";
        public const string CustomEmailPublisherFileDeleteFailed = "CustomEmailPublisher failed to delete file attachment from filepath [{0}]. Note that this error may have been triggered by the CustomEmailPublisher failing to publish the event - ie the directory being deleted may be locked.";
        public const string CustomEmailPublisherConstructor = "One or more errors occurred during the construction of CustomEmailPublisher.";
        public const string CustomEmailPublisherPublisherFromAddressBad = "Sendee email address [{0}] does not follow RFC 822 standard.";
        public const string CustomEmailPublisherPublisherSmtpServerBad = "Email SMTP Server [{0}] is invalid.";
        public const string CustomEmailPublisherWorkingDirMissing = "Working directory [{0}] is missing or inaccessible.";
        public const string CustomEmailPublisherUnsupportedEventType = "Unsupported LogEvent type was passed for publishing to CustomEmailPublisher";
        public const string CustomEmailPublisherWriteEventFailed =
            "Exception occurred while attempting to send the log event using CustomEmailPublisher (Identifier:[{0}]).  The CustomEmailEvent being published had the following values:To:[{1}], From:[{2}] Subject:[{3}] Priority:[{4}] SmtpServer:[{5}] MessageBody:[{6}]";


        // SJPTraceListener messages
        public const string SJPTraceListenerUnsupportedPrototype = "Unsupported SJPTraceListener write prototype called with parameter(s): [{0}]. Change this call to use Write(object) prototype!";
        public const string SJPTraceSwitchNotInitialised = "The SJPTraceSwitch class instance has not been initialised. Ensure that the SJPTraceListener has been created first.";
        public const string SJPTraceListenerWriteUnimplemented = "A prototype of SJPTraceListener.Write has been used but is not supported - use void Write(object) instead.";
        public const string SJPTraceListenerConstructor = "One or more errors occurred during the construction of SJPTraceListener for component with property Application Id [{0}] and Group Id [{1}].";
        public const string SJPTraceListenerDefaultPublisherFailure = "Failed to write an event using the default publisher. The default publisher was used because another publisher failed to write the event - check inner exception for details.";
        public const string ConfiguredPublisherFailed = "Failed to write a custom event of type [{0}] using it's configured publisher [{1}]. Reason: [{2}]";
        public const string DefaultFormatterOutput = "SJP-EVENT\t{0}\tEventClassName:[{1}]\tWARNING:This event data was formatted using DefaultFormatter and may have resulted in data loss.";
        public const string SJPTraceListenerSubscriberNotificationFailed = "SJPTraceListener failed to notify subscriber/s with property values.";
        public const string SJPTraceListenerPropertyChange = "SJPTraceListener received notification from property provider that properties have changed. The properties that changed may or may not be relevent to the Event Logging Service.";
        public const string SJPTraceListenerDefaultPublisherUsed = "A custom event of type [{0}] was published using the default publisher.";
        public const string SJPTraceListenerUnknownClass = "An attempt to log an object that is not of type LogEvent occurred.";

        // Messages for custom events
        public const string InternalRequestEventInvalidFunctionType = "The function type [{0}] is invalid. Function types must be exactly two characters long.";

        // formatter class messages for custom events
        public const string JourneyWebRequestEventFileFormat = "JourneyWebRequestEvent\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}";
        public const string InternalRequestEventFileFormat = "InternalRequestEvent\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}";
        public const string LocationRequestEventFileFormat = "LocationRequestEvent\t{0}\t{1}\t{2}\t{3}\t{4}";
        public const string StopEventRequestEventFileFormat = "StopEventRequestEvent\t{0}\t{1}\t{2}\t{3}\t{4}";
        public const string NoResultsEventFileFormat = "NoResultsEvent\t{0}\t{1}\t{2}";
        public const string ExposedServicesEventFileFormat = "ExposedServicesEvent\t{0}\t{1}\t{2}\t{3}\t{4}";

        public const string EnhancedExposedServicesStartEventFileFormat = "EnhancedExposedServicesStartEvent\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}";

        public const string EnhancedExposedServicesFinishEventFileFormat = "EnhancedExposedServicesFinishEvent\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}";

        static Messages()
        { }
    }
}
