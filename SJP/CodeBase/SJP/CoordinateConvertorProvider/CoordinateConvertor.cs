// *********************************************** 
// NAME             : CoordinateConvertor.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 31 Mar 2011
// DESCRIPTION  	: CoordinateConvertor class used to call CoordinateConvertor web service
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.ServiceModel;
using SJP.Common;
using SJP.Common.EventLogging;
using SJP.Common.Extenders;
using SJP.Common.LocationService;
using SJP.Common.PropertyManager;
using CCWS = SJP.UserPortal.CoordinateConvertorProvider.CoordinateConvertorWebService;
using Logger = System.Diagnostics.Trace;

namespace SJP.UserPortal.CoordinateConvertorProvider
{
    /// <summary>
    /// CoordinateConvertor class used to call CoordinateConvertor web service
    /// </summary>
    public class CoordinateConvertor : ICoordinateConvertor, IDisposable
    {
        #region Private members

        private CCWS.CoordinateConvertorSoapClient webServiceCoordinateConvertor = null;

        #endregion

        #region constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public CoordinateConvertor()
        {
            // Read configuration properites
            string serviceAddress = Properties.Current["CoordinateConvertor.WebService.URL"];
            int serviceTimeout = Properties.Current["CoordinateConvertor.WebService.Timeout.Seconds"].Parse<int>(60); // Default to 60 seconds

            if (string.IsNullOrEmpty(serviceAddress))
            {
                throw new SJPException("CoordinateConverter - Property[CoordinateConvertor.WebService.URL] is missing or invalid, unable to initialise Coordinate Convertor Web Service",
                    false, SJPExceptionIdentifier.CCCoordinateConvertorWebServiceUrlNotValid);
            }

            //initialise the service
            BasicHttpBinding serviceBinding = new BasicHttpBinding();
            serviceBinding.ReceiveTimeout = new TimeSpan(0, 0, serviceTimeout);

            EndpointAddress serviceEndpoint = new EndpointAddress(serviceAddress);

            webServiceCoordinateConvertor = new CCWS.CoordinateConvertorSoapClient(serviceBinding, serviceEndpoint); 
        }

        #endregion

        #region ICoordinateConvertor methods

        /// <summary>
        /// Convert a LatitudeLongitude into an OSGR by calling the CoordinateConverter web service
        /// </summary>
        /// <param name="latlong"></param>
        /// <returns></returns>
        public OSGridReference GetOSGridReference(LatitudeLongitude latlong)
        {
            LatitudeLongitude[] latlongs = new LatitudeLongitude[1];
            latlongs[0] = latlong;

            OSGridReference[] osgrs = GetOSGridReference(latlongs);

            // Assume if all went well, there will be only one osgr to return. 
            // Any exceptions will have been logged and get thrown to caller.
            return osgrs[0];
        }

        /// <summary>
        /// Converts an OSGR into a LatitudeLongitude by calling the CoordinateConvertor web service 
        /// </summary>
        /// <param name="osgr"></param>
        /// <returns></returns>
        public LatitudeLongitude GetLatitudeLongitude(OSGridReference osgr)
        {
            // Place OSGR into an array
            OSGridReference[] osgrs = new OSGridReference[1];
            osgrs[0] = osgr;

            // Call web service
            LatitudeLongitude[] latlongs = GetLatitudeLongitude(osgrs);

            // Assume if all went well, there will be only one latitudelongitude to return. 
            // Any exceptions will have been logged and get thrown to caller.
            return latlongs[0];
        }

        /// <summary>
        /// Converts the LatitudeLongitudes into OSGRs by calling the CoordinateConverter web service
        /// </summary>
        /// <param name="latlongs"></param>
        /// <returns></returns>
        public OSGridReference[] GetOSGridReference(LatitudeLongitude[] latlongs)
        {
            if (SJPTraceSwitch.TraceVerbose)
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose,
                    "CoordinateConverter - Web service call to ConvertLatitudeLongitudetoOSGR started."));
            }
            
            List<OSGridReference> result = new List<OSGridReference>();

            DateTime callStartTime = DateTime.UtcNow;
            bool successful = false;
            try
            {
                List<CCWS.LatitudeLongitude> ccwsLatlongs = new List<CCWS.LatitudeLongitude>();

                // Convert the LocationService LatitudeLongitude into the CCWS type
                foreach (LatitudeLongitude latlong in latlongs)
                {
                    CCWS.LatitudeLongitude ccwsLatlong = new CCWS.LatitudeLongitude();
                    
                    ccwsLatlong.Latitude = latlong.Latitude;
                    ccwsLatlong.Longitude = latlong.Longitude;
                    
                    ccwsLatlongs.Add(ccwsLatlong);
                }

                CCWS.OSGridReference[] ccwsResult  = webServiceCoordinateConvertor.ConvertLatitudeLongitudetoOSGR(ccwsLatlongs.ToArray());

                // Convert the CCSW osgr into the LocationService type
                foreach (CCWS.OSGridReference ccwsOsgr in ccwsResult)
                {
                    OSGridReference osgr = new OSGridReference();

                    osgr.Easting = ccwsOsgr.Easting;
                    osgr.Northing = ccwsOsgr.Northing;

                    result.Add(osgr);
                }

                successful = true;
            }
            catch (Exception ex)
            {
                string message = string.Format(
                    "CoordinateConvertor - An exception has occured when trying to call the CyclePlannerWebService method[{0}]. Error: {1}",
                    "ConvertLatitudeLongitudetoOSGR",
                    ex.Message);

                LogException(message, ex);

                //log an operational event and create new TDException using the web exception 
                throw new SJPException(message, true, SJPExceptionIdentifier.CCCoordinateConvertorWebServiceCallError, ex);
            }
            finally
            {
                LogCallEvent("ConvertLatitudeLongitudetoOSGR", callStartTime, successful);
            }
            if (SJPTraceSwitch.TraceVerbose)
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose,
                    "CoordinateConvertor - Web service call to ConvertLatitudeLongitudetoOSGR completed."));
            }
            return result.ToArray();
        }

        /// <summary>
        /// Converts the OSGRs into LatitudeLongitudes by calling the CoordinateConvertor web service
        /// </summary>
        /// <param name="osgr"></param>
        /// <returns></returns>
        public LatitudeLongitude[] GetLatitudeLongitude(OSGridReference[] osgrs)
        {
            if (SJPTraceSwitch.TraceVerbose)
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose,
                    "CoordinateConvertor - Web service call to ConvertOSGRtoLatitudeLongitude started."));
            }

            //declare the output object
            List<LatitudeLongitude> result = new List<LatitudeLongitude>();

            DateTime callStartTime = DateTime.UtcNow;
            bool successful = false;

            try
            {
                List<CCWS.OSGridReference> ccwsOsgrs = new List<CCWS.OSGridReference>();

                // Convert the LocationService osgr into the CCSW type
                foreach (OSGridReference osgr in osgrs)
                {
                    CCWS.OSGridReference ccwsOsgr = new CCWS.OSGridReference();

                    ccwsOsgr.Easting = Convert.ToInt32(osgr.Easting);
                    ccwsOsgr.Northing = Convert.ToInt32(osgr.Northing);

                    ccwsOsgrs.Add(ccwsOsgr);
                }

                // Call webservice
                CCWS.LatitudeLongitude[] ccwsResult = webServiceCoordinateConvertor.ConvertOSGRtoLatitudeLongitude(ccwsOsgrs.ToArray());

                // Convert the CCSW latlong into the LocationService type
                foreach (CCWS.LatitudeLongitude ccwsLatlong in ccwsResult)
                {
                    LatitudeLongitude latlong = new LatitudeLongitude();

                    latlong.Latitude = ccwsLatlong.Latitude;
                    latlong.Longitude = ccwsLatlong.Longitude;

                    result.Add(latlong);
                }

                // Set succesful here if we want to note down that the call produced successful results
                successful = true;
            }
            catch (Exception ex)
            {
                string message = string.Format(
                    "CoordinateConvertor - An exception has occured when trying to call the CyclePlannerWebService method[{0}]. Error: {1}",
                    "ConvertOSGRtoLatitudeLongitude",
                    ex.Message);

                LogException(message, ex);

                //log an operational event and create new TDException using the web exception 
                throw new SJPException(message, true, SJPExceptionIdentifier.CCCoordinateConvertorWebServiceCallError, ex);
            }
            finally
            {
                //log the callevent
                LogCallEvent("ConvertOSGRtoLatitudeLongitude", callStartTime, successful);
            }

            if (SJPTraceSwitch.TraceVerbose)
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose,
                    "CoordinateConvertor - Web service call to ConvertOSGRtoLatitudeLongitude completed."));
            }

            return result.ToArray();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method that logs an exception if one is detected when calling the web service
        /// </summary>
        private void LogException(string message, Exception ex)
        {
            OperationalEvent oe = new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, message);
            Logger.Write(oe);
        }

        /// <summary>
        /// Method that logs the web service call time, and whether it was successfule
        /// </summary>
        /// <param name="callStartTime"></param>
        /// <param name="successful"></param>
        private void LogCallEvent(string webServiceMethod, DateTime callStartTime, bool successful)
        {
            string message = string.Format(
                "CoordinateConvertor - Web service call made to Method[{0}] Start[{1}] Successful[{2}]",
                webServiceMethod,
                callStartTime,
                successful);

            OperationalEvent oe = new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Info, message);
            Logger.Write(oe);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't 
        // own unmanaged resources itself, but leave the other methods
        // exactly as they are. 
        ~CoordinateConvertor()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (webServiceCoordinateConvertor != null)
                {
                    webServiceCoordinateConvertor.Close();
                    webServiceCoordinateConvertor = null;
                }

            }
        }

        #endregion
    }
}
