// *********************************************** 
// NAME             : StopEventRequestPopulator.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 26 Apr 2011
// DESCRIPTION  	: Class holding methods used to build and convert StopEvent requests
// ************************************************
// 

using System;
using System.Collections.Generic;
using SJP.Common;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using ICJP = TransportDirect.JourneyPlanning.CJPInterface;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// Class holding methods used to build and convert StopEvent requests
    /// </summary>
    public class StopEventRequestPopulator
    {
        #region Private members

        private ISJPJourneyRequest sjpJourneyRequest;
        private IPropertyProvider properties;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public StopEventRequestPopulator(ISJPJourneyRequest sjpJourneyRequest)
        {
            this.sjpJourneyRequest = sjpJourneyRequest;
            this.properties = Properties.Current;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates the CJP Stop EventRequest objects needed to call the CJP for the current 
        /// ISJPJourneyRequest, and returns them encapsulated in an array of CJPStopEventCall objects
        /// </summary>
        /// <returns></returns>
        public CJPStopEventCall[] PopulateRequestsCJPStopEvent(int referenceNumber, int seqNo, string sessionId, bool referenceTransaction, int userType, string language)
        {
            List<CJPStopEventCall> cjpStopEventCalls = new List<CJPStopEventCall>();

            ICJP.EventRequest request = null;

            // Outward journey required (or its a replan outward journey required)
            if ((sjpJourneyRequest.IsOutwardRequired && !sjpJourneyRequest.IsReplan)
                || (sjpJourneyRequest.ReplanIsOutwardRequired && sjpJourneyRequest.IsReplan))
            {
                request = PopulateSingleStopEventRequest(sjpJourneyRequest, false,
                                                    referenceNumber, seqNo++,
                                                    sessionId, referenceTransaction,
                                                    userType, language);

                cjpStopEventCalls.Add(new CJPStopEventCall(request, false));
            }

            // Return journey required (or its a reaplan return journey required)
            if ((sjpJourneyRequest.IsReturnRequired && !sjpJourneyRequest.IsReplan)
                || (sjpJourneyRequest.ReplanIsReturnRequired && sjpJourneyRequest.IsReplan))
            {
                request = PopulateSingleStopEventRequest(sjpJourneyRequest, true,
                                                referenceNumber, seqNo++,
                                                sessionId, referenceTransaction,
                                                userType, language);

                cjpStopEventCalls.Add(new CJPStopEventCall(request, true));
            }

            return cjpStopEventCalls.ToArray();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Populates a single CJP StopEventRequest from the SJPJourneyRequest
        /// </summary>
        private ICJP.EventRequest PopulateSingleStopEventRequest(ISJPJourneyRequest sjpJourneyRequest,
            bool isReturn,
            int referenceNumber,
            int seqNo,
            string sessionId,
            bool referenceTransaction,
            int userType,
            string language)
        {
            #region Initialise the request

            ICJP.StopEventRequest seRequest = InitialiseNewRequest(referenceNumber, seqNo,
                                                                sessionId, referenceTransaction,
                                                                userType, language);

            #endregion

            #region Stop location and date/times
            
            if (isReturn)
            {
                // Request is for services which:
                
                // depart from the return origin venue pier stop
                seRequest.NaPTANIDs = sjpJourneyRequest.ReturnOrigin.Naptan.ToArray();
                seRequest.locality = sjpJourneyRequest.ReturnOrigin.Locality;
                seRequest.intermediateStops = ICJP.IntermediateStopsType.All;
                                
                seRequest.arriveDepart = ICJP.EventArriveDepartType.Depart;
                seRequest.startTime = sjpJourneyRequest.IsReplan ?
                    sjpJourneyRequest.ReplanReturnDateTime :
                    sjpJourneyRequest.ReturnDateTime;
            }
            else
            {
                // Request is for services which:
                
                // arrive at the outward destination venue pier stop
                seRequest.NaPTANIDs = sjpJourneyRequest.Destination.Naptan.ToArray();
                seRequest.locality = sjpJourneyRequest.Destination.Locality;
                seRequest.intermediateStops = ICJP.IntermediateStopsType.All;
                                
                seRequest.arriveDepart = ICJP.EventArriveDepartType.Arrive;
                seRequest.startTime = sjpJourneyRequest.IsReplan ?
                    sjpJourneyRequest.ReplanOutwardDateTime :
                    sjpJourneyRequest.OutwardDateTime; 
            }

            seRequest.realTime = properties[Keys.StopEventRequest_RealTimeRequired].Parse(false);

            #endregion

            #region Location filter

            bool includeLocationFilter = properties[Keys.StopEventRequest_IncludeLocationFilter].Parse(true);

            if (includeLocationFilter)
            {
                ICJP.RequestStopFilter rsFilter = new ICJP.RequestStopFilter();

                if (isReturn)
                {
                    // Request is for services which:

                    // go to the remote pier stop
                    rsFilter.actual = false; // see interface for explanation
                    rsFilter.NaPTANIDs = sjpJourneyRequest.ReturnDestination.Naptan.ToArray();
                    seRequest.destinationFilter = rsFilter;
                }
                else
                {
                    // Request is for services which:

                    // come from the remote pier stop
                    rsFilter.actual = false; // see interface for explanation
                    rsFilter.NaPTANIDs = sjpJourneyRequest.Origin.Naptan.ToArray();
                    seRequest.originFilter = rsFilter;
                }
            }

            #endregion

            #region Service filter

            // No service filter
            seRequest.serviceFilter = null;

            #endregion

            #region Modes

            List<SJPModeType> modes = sjpJourneyRequest.Modes;

            seRequest.modeFilter = new ICJP.Modes();
            seRequest.modeFilter.include = true;
            seRequest.modeFilter.modes = GetModeArray(modes);

            #endregion

            #region Range/Sequence

            int sequence = sjpJourneyRequest.Sequence;
            if (sequence <= 0)
            {
                sequence = sjpJourneyRequest.IsReplan ?
                    properties[Keys.StopEventRequest_RangeSequence_Replan].Parse(3) :
                    properties[Keys.StopEventRequest_RangeSequence].Parse(3);
            }

            // Number of services to return
            seRequest.rangeType = ICJP.RangeType.Sequence;
            seRequest.sequence = sequence;
            seRequest.interval = DateTime.MinValue;

            // Request all instances of a frequent bus/coach journey (if we ever use this for bus/coach journeys)
            seRequest.firstServiceEventOnly = false;

            #endregion
                        
            return seRequest;
        }

        /// <summary>
        /// Instantiates a CJP EventRequest and populates some common attributes. 
        /// </summary>
        private ICJP.StopEventRequest InitialiseNewRequest(int referenceNumber, int seqNo, string sessionId,
                                                       bool referenceTransaction, int userType,
                                                       string language)
        {
            ICJP.StopEventRequest request = new ICJP.StopEventRequest();

            request.referenceTransaction = referenceTransaction;
            request.requestID = SqlHelper.FormatRef(referenceNumber) + FormatSeqNo(seqNo);
            request.language = language;
            request.sessionID = sessionId;
            
            return request;
        }

        /// <summary>
        /// Create an array of ICJP.Mode from the given array of SJPModeType
        /// Modes are used within the CJP EventRequest
        /// </summary>
        private ICJP.Mode[] GetModeArray(List<SJPModeType> modes)
        {
            if (modes != null)
            {
                List<ICJP.Mode> modeResult = new List<ICJP.Mode>();

                foreach (SJPModeType sjpMode in modes)
                {
                    ICJP.Mode mode = new ICJP.Mode();

                    mode.mode = SJPModeTypeHelper.GetCJPModeType(sjpMode);

                    modeResult.Add(mode);
                }

                return modeResult.ToArray();
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Formats a sequence number for use as a request-id
        /// </summary>
        /// <param name="seqNo"></param>
        /// <returns>Formatted string</returns>
        protected string FormatSeqNo(int seqNo)
        {
            return seqNo.ToString("-0000");
        }

        #endregion
    }
}
