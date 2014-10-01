// *********************************************** 
// NAME             : PublicJourneyContinuousDetail.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 27 Mar 2011
// DESCRIPTION  	: PublicJourneyContinuousDetail class representing the detail of a
// continuous journey leg, e.g. walking
// ************************************************
// 
                
using System;
using TransportDirect.JourneyPlanning.CJPInterface;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// PublicJourneyContinuousDetail class representing the detail of a
    /// continuous journey leg
    /// </summary>
    [Serializable()]
    public class PublicJourneyContinuousDetail : PublicJourneyDetail
    {
        #region Constructor

        /// <summary>
        /// Default constructor - defined to allow XML serialisation.
        /// </summary>
        public PublicJourneyContinuousDetail()
        {
        }

        /// <summary>
        /// Takes a CJP leg and creates a new intance of this class populated with
        /// the leg information
        /// </summary>
        /// <param name="leg">A journey leg passed back from the CJP</param>
        public PublicJourneyContinuousDetail(Leg leg)
            : base(leg)
        {
            ContinuousLeg continuousLeg = leg as ContinuousLeg;
            
            // Typical duration returned by CJP is in minutes
            durationSecs = continuousLeg.typicalDuration * 60;
        }

        #endregion
    }
}
