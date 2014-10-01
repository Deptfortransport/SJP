// *********************************************** 
// NAME             : SJPCheckConstraintHelper.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 24 May 2011
// DESCRIPTION  	: SJPCheckConstraintHelper class providing helper methods
// ************************************************
// 

using SJP.Common;
using ICJP = TransportDirect.JourneyPlanning.CJPInterface;

namespace SJP.UserPortal.JourneyControl
{
    public class SJPCheckConstraintHelper
    {
        /// <summary>
        /// Parses a check process strring into a CJP CheckProcess type
        /// </summary>
        public static ICJP.CheckProcess GetCJPCheckProcess(string sjpCheckProcess)
        {
            if (!string.IsNullOrEmpty(sjpCheckProcess))
            {
                switch (sjpCheckProcess.ToLower().Trim())
                {
                    #region SJP check process
                    case "securitycheck":
                        return ICJP.CheckProcess.securityCheck;
                    case "egress":
                        return ICJP.CheckProcess.egress;
                    #endregion
                    default:
                        throw new SJPException(
                            string.Format("Error parsing SJP Check Process string into an CJP CheckProcess, unrecognised value[{0}]", sjpCheckProcess),
                            false, SJPExceptionIdentifier.JCErrorParsingSJPCheckConstraint);
                }
            }
            
            return ICJP.CheckProcess.unknown;
        }

        /// <summary>
        /// Parses a congestion reason strring into a CJP CongestionReason type
        /// </summary>
        public static ICJP.CongestionReason GetCJPCongestionReason(string sjpCongestionReason)
        {
            if (!string.IsNullOrEmpty(sjpCongestionReason))
            {
                switch (sjpCongestionReason.ToLower().Trim())
                {
                    #region SJP congestion reason
                    case "queue":
                        return ICJP.CongestionReason.queue;
                    case "crowding":
                        return ICJP.CongestionReason.crowding;
                    #endregion
                    default:
                        throw new SJPException(
                            string.Format("Error parsing SJP Congestion Reason string into an CJP CongestionReason, unrecognised value[{0}]", sjpCongestionReason),
                            false, SJPExceptionIdentifier.JCErrorParsingSJPCheckConstraint);
                }
            }

            return ICJP.CongestionReason.unknown;
        }
    }
}
