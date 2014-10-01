// *********************************************** 
// NAME             : EventParser.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 June 2011
// DESCRIPTION  	: Event parser class to convert an external event into an internal SJP LogEvent
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SJP.Common.EventLogging;
using SJP.Reporting.Events;
using TDPEL = TransportDirect.Common.Logging;
using TDPCJPE = TransportDirect.ReportDataProvider.CJPCustomEvents;

namespace SJP.Reporting.EventReceiver
{
    /// <summary>
    /// Event parser class to convert an external event into an internal SJP LogEvent
    /// </summary>
    public static class EventParser
    {
        /// <summary>
        /// Parses a TDP CJPCustomEvent (TDP LogEvent) into an SJP CJPCustomEvent (SJP LogEvent)
        /// </summary>
        /// <param name="tdpLogEvent"></param>
        /// <returns></returns>
        public static LogEvent ParseTDPCJPLogEvent(TDPEL.LogEvent tdpLogEvent)
        {
            if (tdpLogEvent != null)
            {
                if (tdpLogEvent is TDPCJPE.JourneyWebRequestEvent)
                {
                    TDPCJPE.JourneyWebRequestEvent jwre = (TDPCJPE.JourneyWebRequestEvent)tdpLogEvent;

                    JourneyWebRequestType jwrt = (JourneyWebRequestType)Enum.Parse(typeof(JourneyWebRequestType), jwre.RequestType.ToString());

                    JourneyWebRequestEvent sjpLogEvent = new JourneyWebRequestEvent(
                        jwre.SessionId, jwre.JourneyWebRequestId, jwre.Submitted,
                        jwrt, jwre.RegionCode, jwre.Success, jwre.RefTransaction);

                    sjpLogEvent.Time = jwre.Time;

                    return sjpLogEvent;
                }
                else if (tdpLogEvent is TDPCJPE.LocationRequestEvent)
                {
                    TDPCJPE.LocationRequestEvent lre = (TDPCJPE.LocationRequestEvent)tdpLogEvent;

                    JourneyPrepositionCategory jpc = (JourneyPrepositionCategory)Enum.Parse(typeof(JourneyPrepositionCategory), lre.PrepositionCategory.ToString());

                    LocationRequestEvent sjpLogEvent = new LocationRequestEvent(
                        lre.JourneyPlanRequestId, jpc, lre.AdminAreaCode, lre.RegionCode);

                    sjpLogEvent.Time = lre.Time;

                    return sjpLogEvent;
                }
                else if (tdpLogEvent is TDPCJPE.InternalRequestEvent)
                {
                    TDPCJPE.InternalRequestEvent ire = (TDPCJPE.InternalRequestEvent)tdpLogEvent;

                    InternalRequestType irt = (InternalRequestType)Enum.Parse(typeof(InternalRequestType), ire.RequestType.ToString());

                    InternalRequestEvent sjpLogEvent = new InternalRequestEvent(
                        ire.SessionId, ire.InternalRequestId, ire.Submitted, irt, ire.FunctionType,
                        ire.Success, ire.RefTransaction
                        );

                    sjpLogEvent.Time = ire.Time;

                    return sjpLogEvent;
                }
                else if (tdpLogEvent is TDPEL.OperationalEvent)
                {
                    try
                    {
                        TDPEL.OperationalEvent tdpOE = (TDPEL.OperationalEvent)tdpLogEvent;

                        // Parse the external types (assuming tdp/sjp values are the same)
                        SJPTraceLevel traceLevel = (SJPTraceLevel)Enum.Parse(typeof(SJPTraceLevel), tdpOE.Level.ToString());
                        SJPEventCategory eventCategory = (SJPEventCategory)Enum.Parse(typeof(SJPEventCategory), tdpOE.Category.ToString());

                        OperationalEvent sjpLogEvent = new OperationalEvent(eventCategory, tdpOE.SessionId, traceLevel, tdpOE.Message);
                        sjpLogEvent.Time = tdpOE.Time;
                        sjpLogEvent.UpdateContextProperties(tdpOE.MethodName, tdpOE.TypeName, tdpOE.AssemblyName, tdpOE.MethodName);

                        return sjpLogEvent;
                    }
                    catch (Exception ex)
                    {
                        Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, String.Format("{0} Exception: {1} StackTrace: {2}",
                                Messages.Service_UnknownEventReceived,
                                ex.Message,
                                ex.StackTrace)));
                        // Return null
                    }
                }
            }

            return null;
        }
    }
}
