// *********************************************** 
// NAME             : Messages.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 31 Mar 2011
// DESCRIPTION  	: Messages used in the CoordinateConvertor web service
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SJP.WebService.CoordinateConvertorService
{
    /// <summary>
    /// Messages used in the CoordinateConvertor web service
    /// </summary>
    public class Messages
    {
        // Initialisation Messages
        public const string Init_InitialisationStarted = "Initialisation of Coordinate Convertor Service started.";
        public const string Init_TraceListenerFailed = "Failed to initialise the Trace Listener class: {0}";
        public const string Init_DotNETTraceListenerFailed = "Failed to initialise a default .NET trace listener. Error:[{0}].";
        public const string Init_ServiceAddFailed = "Failed to add a service to the cache: [{0}].";
        public const string Init_Completed = "Initialisation of Transaction Service completed successfully.";

        // Service Messages
        public const string Service_InternalError = "Coordinate Convertor encountered a problem while processing the request.";
        public const string Service_InitialisingTransformObject = "Initialising the OSTransformationClass.";
        public const string Service_InitialisedTransformObject = "The OSTransformationClass was initialised OK.";
        public const string Service_ErrorInitialisingTransformObject = "Error occurred initialising the OSTransformationClass object. No coordinates will be transformed.";
        public const string Service_ErrorInitialisingTransformObjectCode = "Error occurred attempting to initialise the OSTransformationClass object. Error code returned: {0}";
        public const string Service_ErrorPopulatingOSGR = "Error attempting to pass OSGR [{0},{1}] into the transformation object. Error code returned: {2}";
        public const string Service_ErrorPopulatingLatitudeLongitude = "Error attempting to pass LatitudeLongitude [{0},{1}] into the transformation object. Error code returned: {2}";
        public const string Service_TransformObjectNull = "Unexpected error, OSTransform object is null. Unable to convert coordinate.";
        public const string Service_TransformObjectNotInitialised = "Unexpected error, OSTransform object was not initialised. Unable to convert coordinate.";

        public const string Service_ConvertOSGRtoLatitudeLongitude = "Converting OSGR[{0},{1}] to Latitude Longitude";
        public const string Service_ConvertLatitudeLongitudetoOSGR = "Converting LatitudeLongitude[{0},{1}] to OSGR";

        public const string Service_OSGRConverterdToLatitudeLongitude = "OSGR[{0},{1}] converted to Latitude Longitude[{2},{3}]";
        public const string Service_LatitudeLongitudeConverterdToOSGR = "LatitudeLongitude[{0},{1}] converted to OSGR[{2},{3}]";

        public const string Service_OSGRInvalid = "The OSGR(s) provided in the web request was invalid. {0}";
        public const string Service_OSGRIsNull = "The OSGR array is null.";
        public const string Service_OSGRArrayLength = "The OSGR array length is [{0}].";

        public const string Service_LatitudeLongitudeInvalid = "The LatitudeLongitude(s) provided in the web request was invalid. {0}";
        public const string Service_LatitudeLongitudeIsNull = "The LatitudeLongitude array is null.";
        public const string Service_LatitudeLongitudeArrayLength = "The LatitudeLongitude array length is [{0}].";
    }
}