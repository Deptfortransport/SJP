// *********************************************** 
// NAME             : SJPAccessibilityTypeHelper.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Apr 2011
// DESCRIPTION  	: SJPAccessibilityTypeHelper class providing helper methods
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICJP = TransportDirect.JourneyPlanning.CJPInterface;
using SJP.Common;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// SJPAccessibilityTypeHelper class providing helper methods
    /// </summary>
    public class SJPAccessibilityTypeHelper
    {
        /// <summary>
        /// Parses a CJP AssistanceServiceType into an SJPAccessibilityType
        /// </summary>
        public static SJPAccessibilityType GetSJPAccessibilityType(ICJP.AssistanceServiceType cjpAssistanceServiceType)
        {
            switch (cjpAssistanceServiceType)
            {
                case ICJP.AssistanceServiceType.boarding:
                    return SJPAccessibilityType.ServiceAssistanceBoarding;
                case ICJP.AssistanceServiceType.porterage:
                    return SJPAccessibilityType.ServiceAssistancePorterage;
                case ICJP.AssistanceServiceType.wheelchair:
                    return SJPAccessibilityType.ServiceAssistanceWheelchair;
                case ICJP.AssistanceServiceType.wheelchairBooked:
                    return SJPAccessibilityType.ServiceAssistanceWheelchairBooked;
                case ICJP.AssistanceServiceType.other:
                    return SJPAccessibilityType.ServiceAssistanceOther;
                default:
                    throw new SJPException(
                        string.Format("Error parsing CJP AssistanceServiceType into an SJPAccessibilityType, unrecognised value[{0}]", cjpAssistanceServiceType.ToString()),
                        false, SJPExceptionIdentifier.JCErrorParsingCJPAssistanceServiceType);
            }
        }

        /// <summary>
        /// Parses a CJP AccessSummary into an SJPAccessibilityType
        /// </summary>
        public static SJPAccessibilityType GetSJPAccessibilityType(ICJP.AccessSummary cjpAccessSummary)
        {
            ICJP.AccessFeature feature = cjpAccessSummary.accessFeature;
            ICJP.Transition transition = cjpAccessSummary.transition;

            switch (feature)
            {
                case ICJP.AccessFeature.barrier:
                    return SJPAccessibilityType.AccessBarrier;

                case ICJP.AccessFeature.confinedSpace:
                    return SJPAccessibilityType.AccessConfinedSpace;

                case ICJP.AccessFeature.escalator:
                    if (transition == ICJP.Transition.down)
                        return SJPAccessibilityType.AccessEscalatorDown;
                    else if (transition == ICJP.Transition.up)
                        return SJPAccessibilityType.AccessEscalatorUp;
                    break;

                case ICJP.AccessFeature.footpath:
                    return SJPAccessibilityType.AccessFootpath;

                case ICJP.AccessFeature.lift:
                    if (transition == ICJP.Transition.down)
                        return SJPAccessibilityType.AccessLiftDown;
                    else if (transition == ICJP.Transition.up)
                        return SJPAccessibilityType.AccessLiftUp;
                    break;

                case ICJP.AccessFeature.narrowEntrance:
                    return SJPAccessibilityType.AccessNarrowEntrance;

                case ICJP.AccessFeature.none:
                    return SJPAccessibilityType.AccessNone;

                case ICJP.AccessFeature.openSpace:
                    return SJPAccessibilityType.AccessOpenSpace;

                case ICJP.AccessFeature.other:
                    return SJPAccessibilityType.AccessOther;

                case ICJP.AccessFeature.pavement:
                    return SJPAccessibilityType.AccessPavement;

                case ICJP.AccessFeature.queueManagement:
                    return SJPAccessibilityType.AccessQueueManagement;

                case ICJP.AccessFeature.ramp:
                    if (transition == ICJP.Transition.down)
                        return SJPAccessibilityType.AccessRampDown;
                    else if (transition == ICJP.Transition.up)
                        return SJPAccessibilityType.AccessRampUp;
                    break;

                case ICJP.AccessFeature.seriesOfStairs:
                    return SJPAccessibilityType.AccessSeriesOfStairs;

                case ICJP.AccessFeature.shuttle:
                    return SJPAccessibilityType.AccessShuttle;

                case ICJP.AccessFeature.stairs:
                    if (transition == ICJP.Transition.down)
                        return SJPAccessibilityType.AccessStairsDown;
                    else if (transition == ICJP.Transition.up)
                        return SJPAccessibilityType.AccessStairsUp;
                    break;

                case ICJP.AccessFeature.street:
                    return SJPAccessibilityType.AccessStreet;

                case ICJP.AccessFeature.travelator:
                    return SJPAccessibilityType.AccessTravelator;

                case ICJP.AccessFeature.passage:
                    return SJPAccessibilityType.AccessPassage;

                case ICJP.AccessFeature.unknown:
                    return SJPAccessibilityType.AccessUnknown;

                default:
                    throw new SJPException(
                        string.Format("Error parsing CJP AccessSummary into an SJPAccessibilityType, unrecognised value[{0}]", cjpAccessSummary.accessFeature.ToString()),
                        false, SJPExceptionIdentifier.JCErrorParsingCJPAccessSummary);
            }

            // Default to returning an unknown type
            return SJPAccessibilityType.Unknown;
        }
    }
}
