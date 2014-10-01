// *********************************************** 
// NAME             : Codes.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 27 Mar 2011
// DESCRIPTION  	: Codes class containing success and error codes from CJP
// ************************************************
// 
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.PropertyManager;
using SJP.Common.Extenders;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// Codes class containing success and error codes from CJP
    /// </summary>
    public static class Codes
    {
        // Success/Error code numbers returned by the CJP, 
        // values will be set from properties
        public static int CjpOK = 0;
        public static int CjpNoPublicJourney = 18;
        public static int CjpJourneysRejected = 19;
        public static int CjpAwkwardOvernightRemoved = 30;
        public static int CjpRoadEngineOK = 100;
        public static int CjpRoadEngineMin = 100;
        public static int CjpRoadEngineMax = 199;

        public static int JourneyWebMajorNoResults = 1;
        public static int JourneyWebMinorPast = 1;
        public static int JourneyWebMinorFuture = 2;
        public static int JourneyWebMajorGeneral = 9;
        public static int JourneyWebMinorDisplayable = 2;

        public static int TTBOMajorOK = 0;
        public static int TTBOMinorOK = 1;
        public static int TTBOMinorNoResults = 1;
        public static int TTBONoTimetableServiceFound = 302;

        // Sucess/Error code numbers returned by the CyclePlanner service,
        // values will be set from properties
        public static int CTPOK = 100;
        public static int CTPUndeterminedError = 1;
        public static int CTPSystemException = 2;
        public static int CTPOperationNotSupported = 3;
        public static int CTPInvalidRequest = 4;
        public static int CTPErrorConnectingToDatabase = 5;
        public static int CTPNoJourneyCouldBeFound = 6;

        // Success/Error code numbers created by the CJPManager itself
        public static int CjpCallError = 999;

        // Sucess/Error code numbers created by the CyclePlannerManager itself
        public const int CTPCallError = 9999;

        // Success/Error code numbers used internally by SJP
        public const int SJPNoEarlierServicesOutward = 5001;
        public const int SJPNoLaterServicesOutward = 5002;
        public const int SJPNoEarlierServicesReturn = 5003;
        public const int SJPNoLaterServicesReturn = 5004;

        #region Constructor

        /// <summary>
        /// Constructor Static
        /// </summary>
        static Codes()
        {
            IPropertyProvider properties = Properties.Current;

            // Set code values
            CjpOK = properties[Keys.CodeCjpOK].Parse<int>();
            CjpNoPublicJourney = properties[Keys.CodeCjpNoPublicJourney].Parse<int>();
            CjpJourneysRejected = properties[Keys.CodeCjpJourneysRejected].Parse<int>();
            CjpAwkwardOvernightRemoved = properties[Keys.CodeCjpAwkwardOvernightRejected].Parse<int>();
            CjpRoadEngineOK = properties[Keys.CodeCjpRoadEngineOK].Parse<int>();
            CjpRoadEngineMin = properties[Keys.CodeCjpRoadEngineMin].Parse<int>();
            CjpRoadEngineMax = properties[Keys.CodeCjpRoadEngineMax].Parse<int>();

            JourneyWebMajorNoResults = properties[Keys.CodeJourneyWebMajorNoResults].Parse<int>();
            JourneyWebMinorPast = properties[Keys.CodeJourneyWebMinorPast].Parse<int>();
            JourneyWebMinorFuture = properties[Keys.CodeJourneyWebMinorFuture].Parse<int>();
            JourneyWebMajorGeneral = properties[Keys.CodeJourneyWebMajorGeneral].Parse<int>();
            JourneyWebMinorDisplayable = properties[Keys.CodeJourneyWebMinorDisplayable].Parse<int>();

            TTBOMajorOK = properties[Keys.CodeTTBOMajorOK].Parse<int>();
            TTBOMinorOK = properties[Keys.CodeTTBOMinorOK].Parse<int>();
            TTBOMinorNoResults = properties[Keys.CodeTTBOMinorNoResults].Parse<int>();
            TTBONoTimetableServiceFound = properties[Keys.CodeTTBONoTimetableServiceFound].Parse<int>();

            CTPOK = properties[Keys.CodeCTPOK].Parse<int>();
            CTPUndeterminedError = properties[Keys.CodeCTPUndeterminedError].Parse<int>();
            CTPSystemException = properties[Keys.CodeCTPSystemException].Parse<int>();
            CTPOperationNotSupported = properties[Keys.CodeCTPOperationNotSupported].Parse<int>();
            CTPInvalidRequest = properties[Keys.CodeCTPInvalidRequest].Parse<int>();
            CTPErrorConnectingToDatabase = properties[Keys.CodeCTPErrorConnectingToDatabase].Parse<int>();
            CTPNoJourneyCouldBeFound = properties[Keys.CodeCTPNoJourneyCouldBeFound].Parse<int>();

        }

        #endregion
    }
}
