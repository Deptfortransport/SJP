// *********************************************** 
// NAME             : CoordinateConvertor.asmx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 31 Mar 2011
// DESCRIPTION  	: CoordinateConvertor web service
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using GIQ60Lib;
using SJP.Common.EventLogging;
using SJP.Common.PropertyManager;
using Logger = System.Diagnostics.Trace;

namespace SJP.WebService.CoordinateConvertorService
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://sjp.london2012.com/CoordinateConvertorService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CoordinateConvertor : System.Web.Services.WebService
    {
        #region Private members

        // Static objects
        private static OSTransformation osTransform = null;
        private static bool osTransformInitialised = false;  // Flag to indicate if Transformation object needs to be initialised
        private static bool osTransformInitialisedOK = true; // Flag used to prevent multiple attempts at initialising the Transformation object

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public CoordinateConvertor()
        {
            // Only call the initialise if it the object hasn't already been created
            if ((!osTransformInitialised) && (osTransformInitialisedOK))
            {
                osTransformInitialised = InitialiseOSTransformation();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Initialises the Transformation object
        /// </summary>
        /// <returns></returns>
        private bool InitialiseOSTransformation()
        {
            // Flag to track initialisation
            bool initialisedOK = true;
            OperationalEvent operationalEvent;

            try
            {
                operationalEvent = new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose,
                    Messages.Service_InitialisingTransformObject);
                Logger.Write(operationalEvent);

                // Used to capture any errors from the OSTransformationClass
                eErrorCode errorCode = eErrorCode.eFailure;

                string coordinateConvertorDataPath = Properties.Current["Coordinate.Convertor.DataPath"]; //@"D:\SJP\ThirdParty\Quest\";
                string initialiseString = Properties.Current["Coordinate.Convertor.InitialiseString"]; //"GIQ.6.0"; 

                // Set up the convertor    
                osTransform = new OSTransformationClass();

                // Set the data path for the convertor
                errorCode = osTransform.SetDataFilesPath(coordinateConvertorDataPath);

                if (errorCode == eErrorCode.eSuccess)
                {
                    // Initialise the convertor
                    errorCode = osTransform.SetArea(eArea.eAreaGreatBritain);
                    errorCode = osTransform.Initialise(initialiseString);

                }

                // Set the local initialise flag
                initialisedOK = (errorCode == eErrorCode.eSuccess);

                if (initialisedOK)
                {
                    operationalEvent = new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Info,
                        Messages.Service_InitialisedTransformObject);
                    Logger.Write(operationalEvent);
                }
                else
                {
                    throw new Exception(string.Format(Messages.Service_ErrorInitialisingTransformObjectCode, errorCode.ToString()));
                }

            }
            catch (Exception ex)
            {
                initialisedOK = false;

                operationalEvent = new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error,
                    Messages.Service_ErrorInitialisingTransformObject, ex);
                Logger.Write(operationalEvent);
            }

            // Set the static variable indicating the Transformation object was initialised ok
            osTransformInitialisedOK = initialisedOK;

            // Return 
            return initialisedOK;
        }

        /// <summary>
        /// Converts a LatitudeLongitude into an OSGridReference
        /// </summary>
        /// <param name="latlong"></param>
        /// <returns></returns>
        private OSGridReference ConvertToOSGR(LatitudeLongitude latlong)
        {
            double easting = 0;
            double northing = 0;

            if (osTransformInitialised)
            {
                if (osTransform != null)
                {
                    // Temp values
                    eVertDatum verticalHeightDataType; // Only used because Transform object asks for it
                    double height; // Only used because Transform object asks for it

                    // Insert the OSGR in to the object
                    eErrorCode errorCode = osTransform.SetETRS89Geodetic(latlong.Latitude, latlong.Longitude, 0);

                    if (errorCode == eErrorCode.eSuccess)
                    {
                        // Convert the OSGR to Lattitude/longitude
                        osTransform.GetOSGB36(out easting, out northing, out height, out verticalHeightDataType);

                    }
                    else
                    {
                        string message = string.Format(Messages.Service_ErrorPopulatingLatitudeLongitude,
                        latlong.Latitude,
                        latlong.Longitude,
                        errorCode.ToString());

                        throw new Exception(message);
                    }
                }
                else
                {
                    throw new Exception(Messages.Service_TransformObjectNull);
                }
            }
            else
            {
                throw new Exception(Messages.Service_TransformObjectNotInitialised);
            }

            // Will only reach here if all the above has worked
            OSGridReference osgr = new OSGridReference();
            osgr.Easting = (int)easting;
            osgr.Northing = (int)northing;

            return osgr;
        }

        /// <summary>
        /// Performs the conversion of an OSGridReference to an LatitudeLongitude
        /// </summary>
        /// <param name="osgr"></param>
        /// <returns></returns>
        private LatitudeLongitude ConvertToLatitudeLongitude(OSGridReference osgr)
        {
            double latitude = 0;
            double longitude = 0;

            if (osTransformInitialised)
            {
                if (osTransform != null)
                {
                    // Temp values
                    eVertDatum verticalHeightDataType; // Only used because Transform object asks for it
                    double height; // Only used because Transform object asks for it

                    // Insert the OSGR in to the object
                    eErrorCode errorCode = osTransform.SetOSGB36(osgr.Easting, osgr.Northing, 0, out verticalHeightDataType);

                    if (errorCode == eErrorCode.eSuccess)
                    {
                        // Convert the OSGR to Lattitude/longitude
                        osTransform.GetETRS89Geodetic(out latitude, out longitude, out height);
                    }
                    else
                    {
                        string message = string.Format(Messages.Service_ErrorPopulatingOSGR,
                            osgr.Easting.ToString(),
                            osgr.Northing.ToString(),
                            errorCode.ToString());

                        throw new Exception(message);
                    }
                }
                else
                {
                    throw new Exception(Messages.Service_TransformObjectNull);
                }
            }
            else
            {
                throw new Exception(Messages.Service_TransformObjectNotInitialised);
            }

            // Will only reach here if all the above has worked
            LatitudeLongitude latlong = new LatitudeLongitude();

            latlong.Latitude = latitude;
            latlong.Longitude = longitude;

            return latlong;
        }

        #endregion

        #region Web methods

        /// <summary>
        /// Performs the conversion from a LatitudeLongitude to an OSGR
        /// </summary>
        /// <param name="latlongs"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = false)]
        public OSGridReference[] ConvertLatitudeLongitudetoOSGR(LatitudeLongitude[] latlongs)
        {
            List<OSGridReference> arrayosgr = new List<OSGridReference>();

            OperationalEvent operationalEvent;

            try
            {
                if ((latlongs != null) && (latlongs.Length > 0))
                {
                    foreach (LatitudeLongitude latlong in latlongs)
                    {
                        #region Log

                        if (SJPTraceSwitch.TraceVerbose)
                        {
                            operationalEvent = new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose,
                                string.Format(Messages.Service_ConvertLatitudeLongitudetoOSGR, latlong.Latitude, latlong.Longitude));
                            Logger.Write(operationalEvent);
                        }

                        #endregion

                        OSGridReference osgr = ConvertToOSGR(latlong);
                        arrayosgr.Add(osgr);

                        #region Log

                        if (SJPTraceSwitch.TraceVerbose)
                        {
                            operationalEvent = new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose,
                                string.Format(Messages.Service_LatitudeLongitudeConverterdToOSGR, latlong.Latitude, latlong.Longitude, osgr.Easting, osgr.Northing));
                            Logger.Write(operationalEvent);
                        }

                        #endregion
                    }
                }
                else
                {
                    #region Invalid Latitude Longitude array, throw error

                    string message;

                    if (latlongs == null)
                    {
                        message = string.Format(Messages.Service_LatitudeLongitudeInvalid, Messages.Service_LatitudeLongitudeIsNull);
                    }
                    else
                    {
                        message = string.Format(Messages.Service_LatitudeLongitudeInvalid, string.Format(Messages.Service_LatitudeLongitudeArrayLength, latlongs.Length));
                    }
                    throw new Exception(message);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                // Log exception
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, ex.Message));

                // Throw the exception to the caller
                SoapException se = new SoapException(Messages.Service_InternalError, SoapException.ServerFaultCode, ex);

                throw se;
            }
            // Return array to caller
            return arrayosgr.ToArray();
        }

        /// <summary>
        /// Returns the Latitude/Longitude of the supplied OSGR
        /// </summary>
        /// <param name="osgr"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = false)]
        public LatitudeLongitude[] ConvertOSGRtoLatitudeLongitude(OSGridReference[] osgrs)
        {
            // The return object
            List<LatitudeLongitude> arrayLatLong = new List<LatitudeLongitude>();

            OperationalEvent operationalEvent;

            try
            {
                if ((osgrs != null) && (osgrs.Length > 0))
                {
                    foreach (OSGridReference osgr in osgrs)
                    {
                        #region Log

                        if (SJPTraceSwitch.TraceVerbose)
                        {
                            operationalEvent = new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose,
                                string.Format(Messages.Service_ConvertOSGRtoLatitudeLongitude, osgr.Easting, osgr.Northing));
                            Logger.Write(operationalEvent);
                        }

                        #endregion

                        // Get the LatitudeLongitude
                        LatitudeLongitude latlong = ConvertToLatitudeLongitude(osgr);
                        arrayLatLong.Add(latlong);

                        #region Log

                        if (SJPTraceSwitch.TraceVerbose)
                        {
                            operationalEvent = new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose,
                                string.Format(Messages.Service_OSGRConverterdToLatitudeLongitude, osgr.Easting, osgr.Northing, latlong.Latitude, latlong.Longitude));
                            Logger.Write(operationalEvent);
                        }

                        #endregion
                    }
                }
                else
                {
                    #region Invalid OSGR array, throw error
                    string message;

                    if (osgrs == null)
                    {
                        message = string.Format(Messages.Service_OSGRInvalid, Messages.Service_OSGRIsNull);
                    }
                    else
                    {
                        message = string.Format(Messages.Service_OSGRInvalid, string.Format(Messages.Service_OSGRArrayLength, osgrs.Length));
                    }

                    throw new Exception(message);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                // Log exception
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, ex.Message));

                // Throw the exception to the caller
                SoapException se = new SoapException(Messages.Service_InternalError, SoapException.ServerFaultCode, ex);

                throw se;
            }

            // Return array to caller
            return arrayLatLong.ToArray();
        }

        #region Test methods

        /// <summary>
        /// Used to test if the web service is running
        /// </summary>
        /// <returns>True if the web service is running</returns>
        [WebMethod]
        public bool TestActive()
        {
            return true;
        }

        /// <summary>
        /// Used to test an OSGRR can be converted to a LatitudeLongitude by passing in a test value 
        /// to the ConvertOSGRtoLatitudeLongitude web method
        /// </summary>
        /// <returns>True if coordinate converted without error</returns>
        [WebMethod]
        public LatitudeLongitude TestConvertOSGRToLatitudeLongitude(int easting, int northing)
        {
            try
            {
                string message = string.Format("TEST - Calling ConvertOSGRToLatitudeLongitude for OSGR [{0},{1}]", easting, northing);
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, message));

                OSGridReference[] osgrs = new OSGridReference[1];
                osgrs[0] = new OSGridReference();
                osgrs[0].Easting = easting;
                osgrs[0].Northing = northing;

                LatitudeLongitude[] latlongs = ConvertOSGRtoLatitudeLongitude(osgrs);

                return latlongs[0];
            }
            catch (Exception ex)
            {
                // Log exception
                string message = "TEST - Call to ConvertOSGRToLatitudeLongitude web method threw an exception: " + ex.Message;
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, message));

                // Throw the exception to the caller
                SoapException se = new SoapException(Messages.Service_InternalError, SoapException.ServerFaultCode, ex);

                throw se;
            }
        }

        /// <summary>
        /// Used to test an LatitudeLongitude can be converted to an OSGR by passing in a test value 
        /// to the ConvertLatitudeLongitudeToOGR web method
        /// </summary>
        /// <returns>True if coordinate converted without error</returns>
        [WebMethod]
        public OSGridReference TestConvertLatitudeLongitudeToOSGR(double latitude, double longitude)
        {
            try
            {
                string message = string.Format("TEST - Calling ConvertLatitudeLongitudeToOGR for LatitudeLongitude [{0},{1}]", latitude, longitude);
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, message));

                LatitudeLongitude[] latlongs = new LatitudeLongitude[1];
                latlongs[0] = new LatitudeLongitude();
                latlongs[0].Latitude = latitude;
                latlongs[0].Longitude = longitude;

                OSGridReference[] osgrs = ConvertLatitudeLongitudetoOSGR(latlongs);

                return osgrs[0];
            }
            catch (Exception ex)
            {
                // Log exception
                string message = "TEST - Call to ConvertLatitudeLongitudeToOGR web method threw an exception: " + ex.Message;
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, message));

                // Throw the exception to the caller
                SoapException se = new SoapException(Messages.Service_InternalError, SoapException.ServerFaultCode, ex);

                throw se;
            }
        }

        #endregion

        #endregion
    }
}