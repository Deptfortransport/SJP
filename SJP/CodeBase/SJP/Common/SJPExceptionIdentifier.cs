// *********************************************** 
// NAME             : SJPExceptionIdentifier.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Defines an enumeration to uniquely identify exceptions
// ************************************************
// 
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Common
{
    /// <summary>
    ///  Defines unique exception identifier values
    /// </summary>
    public enum SJPExceptionIdentifier
    {
        Undefined = -1,

        // Event Logging Service - 100 - 199
        ELSEventLogConstructor = 100,
        ELSEventLogPublisherWritingEvent = 101,
        ELSFilePublisherWritingEvent = 102,
        ELSConsolePublisherWritingEvent = 109,
        ELSEmailPublisherWritingEvent = 105,
        ELSCustomEmailPublisherSavingStream = 128,
        ELSCustomEmailPublisherWritingEvent = 127,
        ELSCustomEmailPublisherUnsupportedEvent = 126,
        ELSCustomEmailPublisherConstructor = 130,
        ELSQueuePublisherConstructor = 106,
        ELSQueuePublisherWritingEvent = 107,
        ELSGlobalTraceLevelUndefined = 111,
        ELSSwitchOnUnknownEvent = 113,
        ELSLoggingPropertyValidatorUnknownKey = 114,
        ELSDefaultPublisherFailed = 104,
        ELSPropertyValidatorStringToEnum = 110,
        ELSCustomEventSwitchUndefined = 112,
        ELSCustomOperationalConstructor = 115,
        ELSSJPTraceListenerConstructor = 118,
        ELSTraceLevelUninitialised = 121,
        ELSSJPTraceListenerUnknownObject = 103,
        ELSInvalidFunctionType = 131,

        // Location Service 200 -219
        LSFailureLoadingVenueCache = 200,
        LSFailureReadingVenueCache = 201,
        LSFailureLoadingGNATCache = 202,
        LSFailureReadingGNATCache = 203,
        LSErrorGettingUnknownLocation = 204,
        LSErrorGettingPostcodeLocation = 205,
        LSErrorGettingNaPTANLocation = 206,
        LSErrorGettingLocalityLocation = 207,
        LSErrorGettingGroupLocation = 208,
        LSErrorGettingAlternativeLocations = 209,
        LSErrorParsingLocationType = 210,

        // Resource Manaer 220 - 229
        RMLanguageNotHandled = 220,

        // Property Service 600 - 699
        PSInvalidPropertyFile = 600,
        PSInvalidVersion = 601,
        PSDatabaseFailure = 602,
        PSInvalidAssembly = 604,
        PSInvalidAssemblyType = 605,
        PSBadEnum = 606,
        PSLockedPropertyNotEncrypted = 607,
        PSUnencryptedPropertyExists = 608,
        PSMissingProperty = 609,

        // LocationJSGenerator 700-709
        LJSGenInitialisationFailed = 701,
        LJSGenLocatonDataLoadFailed = 702,
        LJSGenAliasDataLoadFailed = 703,
        LJSGenAliasLocatonAddFailed = 704,
        LJSGenGetScriptLocationFailed = 705,
        LJSGenCreateScriptFailed = 706,
        
        // State Server 800-899
        SSFailedLocking = 800,
        SSErrorLocking = 801,
        SSErrorSaving = 802,
        SSErrorReading = 803,
        SSErrorDeleting = 804,

        // Command & Control 900-999
        CCAgentInitialisationFailed = 900,
        CCAgentMonitoringItemsLoadFailed = 901,
        CCAgentMonitoringItemsRecheckFailed = 902,
        CCAgentChecksumRecheckFailed = 903,
        CCAgentFileRecheckFailed = 904,
        CCAgentDBRecheckFailed = 905,
        CCAgentWMIRecheckFailed = 906,
        
        // Screen Flow 1000-1099
        SFMConstructorFailed = 1000,
        SFMScreenFlowPageIdError = 1001,

        // Database Infrastructure 1100-1199
        DSUnknownType = 1100,
        DSStoredProcedureNotPresent = 1101,
        DSPollingIntervalNotFound = 1102,
        DSGroupsNotFound = 1103,
        DSDatabaseNamePropertyNotValid = 1104,
        DSDatabaseNamePropertyNotPresent = 1105,
        DSNULLParameters = 1106,
        DSSQLHelperDatabaseNotFound = 1107,
        DSDataAccessError = 1108,
        DSQueryExecuteError = 1109,
        DSMethodIllegalType = 1110,

        // Service Discovery 1200-1201
        SDInvalidKey = 1200,

        // Retail 1300-1399
        RERetailerCatalogueSQLCommandFailed = 1300,
        REErrorReadingRetailXmlSchema = 1301,
        REErrorValidatingRetailXmlSchema = 1302,
        REErrorValidatingHandoffXml = 1303,
        RETravelcardCataglogueSQLCommandFailed = 1304,

        // Journey Control 1400-1499
        JCErrorParsingCJPModeType = 1400,
        JCErrorParsingSJPModeType = 1401,
        JCErrorParsingCJPAssistanceServiceType = 1402,
        JCErrorParsingCJPAccessSummary = 1403,
        JCUnsupportedJourneyRequestPopulator = 1404,
        JCErrorParsingParkingInterchangeMode = 1405,
        JCErrorParsingSJPCheckConstraint = 1406,

        // Cycle Planner Service 1500-1599
        CYCyclePlannerWebServiceUrlNotValid = 1500,
        CYCyclePlannerWebServiceCallError = 1501,

        // Journey Planner Runner 300 - 399
        JPRInvalidSJPJourneyRequest = 300,

        // Coordinate Convertor Service 1600-1603
        CCServiceTraceInitFailed = 1600,
        CCServiceServiceAddFailed = 1601,
        CCCoordinateConvertorWebServiceUrlNotValid = 1602,
        CCCoordinateConvertorWebServiceCallError = 1603,

        // SJPWeb 1700-1799
        SWUndefinedPlannerMode = 1700,
        SWErrorBuildingJourneyRequestFromString = 1701,
        
        // Reporting 1800-1899
        RDPSQLHelperInsertFailed = 1801,
        RDPSQLHelperStoredProcedureFailure = 1802,
        RDPSJPCustomEventPublisherConstruction = 1803,
        RDPSJPOperationalEventPublisherConstruction = 1804,
        RDPUnsupportedSJPCustomEventPublisherEvent = 1805,
        RDPUnsupportedSJPOperationalEventPublisherEvent = 1806,
        RDPCJPCustomEventPublisherConstruction = 1807,
        RDPUnsupportedCJPCustomEventPublisherEvent = 1808,

        // Event Publisher 1900-1999
        RDPFailedPublishingOperationalEvent = 1900,
        RDPFailedPublishingPageEntryEvent = 1901,
        RDPFailedPublishingJourneyPlanRequestEvent = 1902,
        RDPFailedPublishingJourneyPlanResultsEvent = 1903,
        RDPFailedPublishingCyclePlannerRequestEvent = 1904,
        RDPFailedPublishingCyclePlannerResultEvent = 1905,
        RDPFailedPublishingRepeatVisitorEvent = 1906,
        RDPFailedPublishingRetailerHandoffEvent = 1907,
        RDPFailedPublishingLandingPageEvent = 1908,
        RDPFailedPublishingWorkloadEvent = 1909,
        RDPFailedPublishingDataGatewayEvent = 1910,
        RDPFailedPublishingStopEventRequestEvent = 1911,
        RDPFailedPublishingReferenceTransactionEvent = 1912,
        RDPFailedPublishingCJPJourneyWebRequestEvent = 1913,
        RDPFailedPublishingCJPLocationRequestEvent = 1914,
        RDPFailedPublishingCJPInternalRequestEvent = 1915,
        RDPFailedPublishingNoResultsEvent = 1916,

        // Event Receiver 2000-2099
        RDPEventReceiverQueueReceiveInitFailed = 2000,
        RDPEventReceiverStartFailure = 2001,
        RDPEventReceiverInitFailed = 2002,
        RDPEventReceiverUnknownPropertyKey = 2003,

        // Web Log Reader 2100 - 2199
        RDPWebLogReaderDefaultLoggerFailed = 2100,
        RDPWebLogReaderTDServiceAddFailed = 2101,
        RDPWebLogReaderTDTraceInitFailed = 2102,
        RDPWebLogReaderUnknownPropertyKey = 2103,
        RDPWebLogReaderInvalidProperties = 2104,
        RDPWebLogReaderArchiveFailed = 2105,
        RDPWebLogReaderMissingFields = 2106,
        RDPWebLogReaderNoFieldTokens = 2107,
        RDPWebLogReaderInvalidArg = 2108,
        RDPWebLogReaderFailedReadingWebLog = 2109,
        RDPWebLogReaderFailedStoringWebLogData = 2110,

        // TravelNews 2200 - 2299
        TNImportUnexpectedError = 2200,
        TNImportFileNotFound = 2201,
        TNImportInvalidXML = 2202,
        TNImportDBConnectionError = 2203,
        TNImportStoredProcedureError = 2204,
        TNSQLHelperStoredProcedureFailure = 2205,
        TNSQLHelperError = 2206,

        // Cycle Journey - GPX transformation
        CYXmlGPXFileTransformationFailed = 2300,

        // GNAT Importer
        GNImportFileNotFound = 2401,

        // DataLoader 2500-2599
        DLDataLoaderDefaultLoggerFailed = 2500,
        DLDataLoaderServiceAddFailed = 2501,
        DLDataLoaderTraceInitFailed = 2502,
        DLDataLoaderInvalidArgument = 2503,
        DLDataLoaderInvalidConfiguration = 2504,
        DLDataLoaderUnexpectedException = 2505,
        DLDataLoaderInvokingTransferTaskFailed = 2506,
        DLDataLoaderInvokingLoadTaskFailed = 2507,
        DLDataLoaderFileNotFound = 2508,
        DLDataLoaderFileTransferLocationsNotFound = 2509,
        DLDataLoaderXmlFileTransferError = 2510,
        DLDataLoaderXmlFileSaveError = 2511,
        DLDataLoaderXmlFileReadError = 2512,
        DLDataLoaderXmlSchemaFileUnspecified = 2513,
        DLDataLoaderXmlNamespaceUnspecified = 2514,
        DLDataLoaderXmlSchemaValidationFailed = 2515,
        DLDataLoaderDatabaseUnspecified = 2516,
        DLDataLoaderDatabaseConnectionError = 2517,
        DLDataLoaderStoredProcedureUnspecified = 2518,
        DLDataLoaderStoredProcedureError = 2519,
    }
}
