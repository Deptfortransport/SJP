// *********************************************** 
// NAME             : SJPModeTypeHelper.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 27 Mar 2011
// DESCRIPTION  	: SJPModeTypeHelper class providing helper methods 
// ************************************************
// 

using SJP.Common;
using ICJP = TransportDirect.JourneyPlanning.CJPInterface;
using SJP.Common.LocationService;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// SJPModeTypeHelper class providing helper methods 
    /// </summary>
    public class SJPModeTypeHelper
    {
        /// <summary>
        /// Parses a CJP ModeType into an SJPModeType
        /// </summary>
        /// <param name="cjpModeType"></param>
        public static SJPModeType GetSJPModeType(ICJP.ModeType cjpModeType)
        {
            switch (cjpModeType)
            {
                #region CJP interface mode type

                case ICJP.ModeType.Air:
                    return SJPModeType.Air;
                case ICJP.ModeType.Bus:
                    return SJPModeType.Bus;
                case ICJP.ModeType.Car:
                    return SJPModeType.Car;
                case ICJP.ModeType.CheckIn:
                    return SJPModeType.CheckIn;
                case ICJP.ModeType.CheckOut:
                    return SJPModeType.CheckOut;
                case ICJP.ModeType.Coach:
                    return SJPModeType.Coach;
                case ICJP.ModeType.Cycle:
                    return SJPModeType.Cycle;
                case ICJP.ModeType.Drt:
                    return SJPModeType.Drt;
                case ICJP.ModeType.Ferry:
                    return SJPModeType.Ferry;
                case ICJP.ModeType.Metro:
                    return SJPModeType.Metro;
                case ICJP.ModeType.Rail:
                    return SJPModeType.Rail;
                case ICJP.ModeType.RailReplacementBus:
                    return SJPModeType.RailReplacementBus;
                case ICJP.ModeType.Taxi:
                    return SJPModeType.Taxi;
                case ICJP.ModeType.Tram:
                    return SJPModeType.Tram;
                case ICJP.ModeType.Transfer:
                    return SJPModeType.Transfer;
                case ICJP.ModeType.Underground:
                    return SJPModeType.Underground;
                case ICJP.ModeType.Walk:
                    return SJPModeType.Walk;
                #endregion
                default:
                    throw new SJPException(
                        string.Format("Error parsing CJP ModeType into an SJPModeType, unrecognised value[{0}]", cjpModeType.ToString()), 
                        false, SJPExceptionIdentifier.JCErrorParsingCJPModeType);
            }
        }

        /// <summary>
        /// Parses a SJPModeType into an CJP ModeType
        /// </summary>
        /// <param name="cjpModeType"></param>
        public static ICJP.ModeType GetCJPModeType(SJPModeType sjpModeType)
        {
            switch (sjpModeType)
            {
                #region SJP mode type
                case SJPModeType.Air:
                    return ICJP.ModeType.Air;
                case SJPModeType.Bus:
                    return ICJP.ModeType.Bus;
                case SJPModeType.Car:
                    return ICJP.ModeType.Car;
                case SJPModeType.CheckIn:
                    return ICJP.ModeType.CheckIn;
                case SJPModeType.CheckOut:
                    return ICJP.ModeType.CheckOut;
                case SJPModeType.Coach:
                    return ICJP.ModeType.Coach;
                case SJPModeType.Cycle:
                    return ICJP.ModeType.Cycle;
                case SJPModeType.Drt:
                    return ICJP.ModeType.Drt;
                case SJPModeType.Ferry:
                    return ICJP.ModeType.Ferry;
                case SJPModeType.Metro:
                    return ICJP.ModeType.Metro;
                case SJPModeType.Rail:
                    return ICJP.ModeType.Rail;
                case SJPModeType.RailReplacementBus:
                    return ICJP.ModeType.RailReplacementBus;
                case SJPModeType.Taxi:
                    return ICJP.ModeType.Taxi;
                case SJPModeType.Tram:
                    return ICJP.ModeType.Tram;
                case SJPModeType.Transfer:
                    return ICJP.ModeType.Transfer;
                case SJPModeType.Underground:
                    return ICJP.ModeType.Underground;
                case SJPModeType.Walk:
                    return ICJP.ModeType.Walk;
                #endregion
                default:
                    throw new SJPException(
                        string.Format("Error parsing SJPModeType into an CJP ModeType, unrecognised value[{0}]", sjpModeType.ToString()),
                        false, SJPExceptionIdentifier.JCErrorParsingSJPModeType);
            }
        }

        /// <summary>
        /// Parses an SJP ParkingInterchangeMode into an SJPModeType
        /// </summary>
        /// <param name="cjpModeType"></param>
        public static SJPModeType GetSJPModeType(ParkingInterchangeMode pim)
        {
            switch (pim)
            {
                #region SJP ParkingInterchangeMode mode type
                case ParkingInterchangeMode.Rail:
                    return SJPModeType.TransitRail;
                case ParkingInterchangeMode.Shuttlebus:
                    return SJPModeType.TransitShuttleBus;
                case ParkingInterchangeMode.Cycle:
                    return SJPModeType.Cycle;
                case ParkingInterchangeMode.Metro:
                    return SJPModeType.Metro;
                case ParkingInterchangeMode.Walk:
                    return SJPModeType.Walk;
                #endregion
                default:
                    throw new SJPException(
                        string.Format("Error parsing SJP ParkingInterchangeMode into an SJPModeType, unrecognised value[{0}]", pim.ToString()),
                        false, SJPExceptionIdentifier.JCErrorParsingParkingInterchangeMode);
            }
        }
    }
}
