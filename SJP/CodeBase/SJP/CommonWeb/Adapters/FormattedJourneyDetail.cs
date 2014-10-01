using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SJP.Common.PropertyManager;
using SJP.Common.Extenders;

namespace SJP.Common.Web
{
    public class FormattedJourneyDetail
    {
        #region Private Fields
        private int step;
        private int? distance;
        private int distanceActual;
        private int? totalDistance;
        private string instruction;
        private DateTime? arrive;
        private int durationSecs;
        private string toids;
        private string osgrs;

        // True if the high traffic level symbol needs to be displayed.
        protected bool highTrafficLevel;

        // int to determine how many decimal places should be used for the distance display
        protected int distanceDecimalPlaces = 1;
        #endregion

        #region Constructor
        public FormattedJourneyDetail()
        {
           
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets/Sets journey detail index
        /// </summary>
        public int Step
        {
            get { return step; }
            set { step = value; }
        }

        /// <summary>
        /// Gets or sets journey detail instrucion 
        /// </summary>
        public string Instruction
        {
            get { return instruction; }
            set { instruction = value; }
        }

        /// <summary>
        /// Gets or sets total cumulative journey detail distance in metres.
        /// </summary>
        public int? TotalDistance
        {
            get { return totalDistance; }
            set { totalDistance = value; }
        }

        /// <summary>
        /// Gets or sets journey detail distance in metres
        /// </summary>
        public int? Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        /// <summary>
        /// Gets or sets journey detail distance in metres, cannot be null
        /// </summary>
        public int DistanceActual
        {
            get { return distanceActual; }
            set { distanceActual = value; }
        }

        /// <summary>
        /// Gets or sets the arrival time of the journey detail
        /// </summary>
        public DateTime? ArriveTime
        {
            get { return arrive; }
            set { arrive = value; }
        }

        /// <summary>
        /// Gets or sets the duration of the journey detail
        /// </summary>
        public int DurationActual
        {
            get { return durationSecs; }
            set { durationSecs = value; }
        }

        /// <summary>
        /// True if the high traffic level symbol needs to be displayed.
        /// </summary>
        public bool HighTrafficLevel
        {
            get { return highTrafficLevel; }
            set { highTrafficLevel = value; }
        }

        /// <summary>
        /// Gets or sets the toids string
        /// </summary>
        public string TOIDs
        {
            get { return toids; }
            set { toids = value; }
        }

        /// <summary>
        /// Gets or sets the osgrs string
        /// </summary>
        public string OSGRs
        {
            get { return osgrs; }
            set { osgrs = value; }
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Return a formatted string by converting the given metres
        /// to a mileage
        /// </summary>
        /// <param name="metres">Metres to convert</param>
        /// <returns>Formatted string</returns>
        public virtual string ConvertMetresToMileage(int metres)
        {
            // Retrieve the conversion factor from the Properties Service.
            double conversionFactor = Properties.Current["Web.Controls.MileageConverter"].Parse(0);

            double result = (double)metres / conversionFactor;

            string distanceFormat = GetDistanceFormat();

            // Return the result
            return result.ToString(distanceFormat);
        }

        /// <summary>
        /// Return a formatted string by converting the given metres
        /// to km.
        /// </summary>
        /// <param name="metres">Metres to convert</param>
        /// <returns>Formatted string</returns>
        public virtual string ConvertMetresToKm(int metres)
        {
            double result = (double)metres / 1000;

            string distanceFormat = GetDistanceFormat();

            // Return the result
            return result.ToString(distanceFormat);
        }

        /// <summary>
        /// Method to return the text format of a distance to be converted to string.
        /// The distanceDecimalPlaces value is used to determine number decimal places
        /// </summary>
        /// <returns></returns>
        public string GetDistanceFormat()
        {
            string numberFormat = "F";

            // determine the format based on number of decimal places, 3 or less (this can be changed in future as its only set to avoid large numbers)
            if ((distanceDecimalPlaces >= 0) && (distanceDecimalPlaces <= 3))
            {
                numberFormat += distanceDecimalPlaces.ToString();
            }
            else
            {
                numberFormat += "1";
            }

            return numberFormat;
        }
        #endregion
    }
}