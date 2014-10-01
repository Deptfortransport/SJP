// *********************************************** 
// NAME             : PublicJourneyFrequencyDetail.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 27 Mar 2011
// DESCRIPTION  	: PublicJourneyFrequencyDetail class representing the detail of a
// frequency based journey leg, e.g. tube, bus
// ************************************************
//              
                
using System;
using TransportDirect.JourneyPlanning.CJPInterface;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// PublicJourneyFrequencyDetail class representing the detail of a
    /// frequency based journey leg
    /// </summary>
    [Serializable()]
    public class PublicJourneyFrequencyDetail : PublicJourneyDetail
    {
        #region Private members

        /// <summary>
        /// Transport frequency per minute returned by CJP
        /// </summary>
        private int frequency;
        /// <summary>
        /// Minimum transport frequency per minute returned by CJP
        /// </summary>
        private int minFrequency;
        /// <summary>
        /// Minimum transport frequency per minute returned by CJP
        /// </summary>
        private int maxFrequency;
        /// <summary>
        /// Average duration of leg in minutes returned by CJP
        /// </summary>
        private int typicalDuration;
        /// <summary>
        /// Max duration of leg in minutes returned by CJP
        /// </summary>
        private int maxDuration;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public PublicJourneyFrequencyDetail()
        {
        }

        /// <summary>
        /// Takes a CJP leg and creates a new intance of this class populated with
        /// the leg information
        /// </summary>
        /// <param name="leg">A journey leg passed back from the CJP</param>
        public PublicJourneyFrequencyDetail(Leg leg)
            : base(leg)
        {
            FrequencyLeg frequencyLeg = leg as FrequencyLeg;
            frequency = frequencyLeg.frequency;
            minFrequency = frequencyLeg.minFrequency;
            maxFrequency = frequencyLeg.maxFrequency;
            typicalDuration = frequencyLeg.typicalDuration;
            maxDuration = frequencyLeg.maxDuration;

            // Duration is same as typical duration, values returned by CJP are in minutes
            durationSecs = typicalDuration * 60;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Frequency in minutes.
        /// </summary>
        public int Frequency
        {
            get { return frequency; }
            set { frequency = value; }
        }

        /// <summary>
        /// Minimum frequency in minutes.
        /// </summary>
        public int MinFrequency
        {
            get { return minFrequency; }
            set { minFrequency = value; }
        }

        /// <summary>
        /// Maximum frequency in minutes.
        /// </summary>
        public int MaxFrequency
        {
            get { return maxFrequency; }
            set { maxFrequency = value; }
        }

        /// <summary>
        /// The typical journey duration in minutes.
        /// </summary>
        public int TypicalDuration
        {
            get { return typicalDuration; }
            set { typicalDuration = value; }
        }
        
        /// <summary>
        /// The maximum journey duration in minutes.
        /// </summary>
        public int MaxDuration
        {
            get { return maxDuration; }
            set { maxDuration = value; }
        }

        #endregion

    }
}
