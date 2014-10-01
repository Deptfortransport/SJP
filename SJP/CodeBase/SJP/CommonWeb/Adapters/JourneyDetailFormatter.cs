// *********************************************** 
// NAME             : JourneyDetailFormatter.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Abstract Class to provide common functionality to formate journey details for Road and Car Journeys
// ************************************************


using System;
using System.Collections.Generic;
using System.Globalization;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using SJP.Common.ResourceManager;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.SessionManager;

namespace SJP.Common.Web
{
    /// <summary>
    /// Abstract Class to provide common functionality to formate journey details for Road and Car Journeys
    /// </summary>
    public abstract class JourneyDetailFormatter
    {
        #region Instance Variables

        protected SJPResourceManager resourceManager;

        // Used to control rendering of the step number in the table.
        protected int stepNumber;

        // Keeps the accumulated distance of the journey currently being rendered.
        protected int accumulatedDistance;

        // Keeps the accumulated arrival time (updated after each leg)
        protected DateTime currentArrivalTime;

        // Constant representing space used in string formatting
        protected readonly static string space = " ";

        // Constant representing space used in string formatting
        protected readonly static string comma = ",";

        // Constant representing full stop used in string formatting
        protected readonly static string fullstop = ".";

        // Constant representing dash/hyphen used in string formatting
        protected readonly static string dash = "-";

        // Constant representing seperator used in string formatting
        protected readonly static string seperator = ", ";

        // Road journey to render.
        protected JourneyLeg journey;

        // Language for returned text
        protected Language currentLanguage;

        // True if journey details are for outward journey, false if return
        //protected bool outward;

        // bool to display/hide the toll charge cost 
        //(the url and text will continue to be displayed but cost part removed)
        protected bool displayTollCharge = true;

        // bool to display/hide the congestion symbol (traffic warning icon)
        protected bool displayCongestionSymbol = true;

        // bool to display/hide the congestion charge (e.g. in London congestion zone)
        protected bool displayCongestionCharge = true;

        // int to determine how many decimal places should be used for the distance display
        protected int distanceDecimalPlaces = 1;

        protected bool isCJPUser = false;

        protected bool print = false;

        protected JourneyViewState journeyViewState;

        #endregion

         #region Public Properties
        /// <summary>
        /// The accummulated distance of a journey in miles. The value of the
        /// accummulated distance is calculated during the formatting of
        /// journey instructions and is incremented by GetDistance.
        /// </summary>
        protected virtual int? TotalDistance
        {
            get
            {
                return accumulatedDistance;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="journey"></param>
        /// <param name="outward"></param>
        /// <param name="currentLanguage"></param>
        /// <param name="print"></param>
        public JourneyDetailFormatter(JourneyLeg journey,
            Language currentLanguage,
            bool print,
            SJPResourceManager resourceManager)
        {
            this.journey = journey;
            this.currentLanguage = currentLanguage;
            this.print = print;
            this.resourceManager = resourceManager;

            journeyViewState = SJPSessionManager.Current.JourneyState;
        }
        #endregion

        #region Abstract Methods
        /// <summary>
        /// A hook method called by processRoadJourney to process each road journey
        /// instruction. The returned string array should contain details for each 
        /// instruction (e.g, road name, distance, directions)
        /// </summary>
        /// <param name="journeyDetailIndex">Array index into roadJourney.Details</param>
        /// <returns>details for each instruction (e.g, road name, distance, directions)</returns>
        protected abstract FormattedJourneyDetail ProcessJourneyDetail(int journeyDetailIndex, bool showCongestionCharge);

        /// <summary>
        /// A hook method called by processRoadJourney to add the first element to the 
        /// ordered list of journey instructions. The returned string array
        /// should contain details for the first instruction.
        /// </summary>
        /// <returns>details for first instruction</returns>
        protected abstract FormattedJourneyDetail AddFirstDetailLine();

        /// <summary>
        /// A hook method called by processRoadJourney to add the last element to the 
        /// ordered list of journey instructions. The returned string array
        /// should contain details for the last instruction.
        /// </summary>
        /// <returns>details for last instruction</returns>
        protected abstract FormattedJourneyDetail AddLastDetailLine();
        #endregion

        #region Public methods
        /// <summary>
        /// Returns an ordered list of journey details. Each object in the
        /// list contains details for a single journey instruction. Each object
        /// is a string array of details (e.g, road name, distance, directions)
        /// The contents of the list and each string array element is
        /// determined by methods in subclasses that produce specific results
        /// dependant on purpose (e.g. output for web page, email, and so on).
        /// </summary>
        /// <returns>Returns an ordered list of journey instructions where each 
        /// element is a string array of details (e.g, road name, distance, directions)</returns>
        public virtual List<FormattedJourneyDetail> GetJourneyDetails()
        {
            return ProcessJourney(-1, -1);
        }

        /// <summary>
        /// Returns an ordered list of journey details. Each object in the
        /// list contains details for a single journey instruction. Each object
        /// is a string array of details (e.g, road name, distance, directions)
        /// The contents of the list and each string array element is
        /// determined by methods in subclasses that produce specific results
        /// dependant on purpose (e.g. output for web page, email, and so on).
        /// </summary>
        /// <param name="startIndex">The journey instruction index to start the directions at</param>
        /// <param name="endIndex">The journey intstrunction index to end the directions at</param>
        /// <returns>Returns an ordered list of journey instructions where each 
        /// element is a string array of details (e.g, road name, distance, directions)</returns>
        public virtual List<FormattedJourneyDetail> GetJourneyDetails(int startIndex, int endIndex)
        {
            return ProcessJourney(startIndex, endIndex);
        }

        

        #endregion Public methods

        #region Protected Methods
        /// <summary>
        /// The is a template method that controls the format process. It calls
        /// into hook methods to generate specific output defined by 
        /// subclasses.
        /// </summary>
        /// <returns>Returns an ordered list of journey instructions where each 
        /// element is a string array of details (e.g, road name, distance, directions)</returns>
        protected virtual List<FormattedJourneyDetail> ProcessJourney(int startIndex, int endIndex)
        {
            List<FormattedJourneyDetail> details = new List<FormattedJourneyDetail>();

            #region Congestion Charge
            bool showCongestionCharge = true;

            //Check if congestion charge cost should be shown for this journey
            if (journeyViewState.CongestionChargeAdded)
            {
                showCongestionCharge = false;
            }

            #endregion

            if ((journey == null) || (journey.JourneyDetails.Count == 0))
            {
                return details;
            }
            else
            {
                initFormatting();

                details.Add(AddFirstDetailLine());

                for (int journeyDetailIndex = 0;
                    journeyDetailIndex < journey.JourneyDetails.Count;
                    journeyDetailIndex++)
                {
                    // Do the pre-processing. This sets up all image flags, attribute text strings needed by the 
                    // various cycle text methods
                    PreProcessJourneyDetail(journeyDetailIndex);

                    details.Add(ProcessJourneyDetail(journeyDetailIndex, showCongestionCharge));

                    // Do the post-processing. This resets any temporary flags (e.g. image flags) ready for the next detail
                    PostProcessJourneyDetail();
                }

                details.Add(AddLastDetailLine());

                return details;
            }
        }

        /// <summary>
        /// Returns the current instruction step number. Getting the instruction step 
        /// number also increments it
        /// </summary>
        protected virtual int GetCurrentStepNumber()
        {
            int currentStepNumber = stepNumber;
            stepNumber++;
            return currentStepNumber;
        }
    
        /// <summary>
        /// Called by the template method ProcessRoadJourney prior to 
        /// formatting. Initialises instance variables used during the
        /// formatting process.
        /// </summary>
        protected virtual void initFormatting()
        {
            accumulatedDistance = 0;
            stepNumber = 1;
            currentArrivalTime = journey.StartTime;
        }

        /// <summary>
        /// Returns the distance for the current driving instruction. 
        /// </summary>
        /// <param name="journeyDetailIndex">Array index into roadJourney.JourneyDetails for journey instruction</param>
        /// <returns>distance in metres</returns>
        protected virtual int? GetDistance(int journeyDetailIndex)
        {
            JourneyDetail detail = journey.JourneyDetails[journeyDetailIndex];

            if (detail.Distance >= 100)
                return detail.Distance;

            return null;
        }

        /// <summary>
        /// Returns the distance for the current driving instruction, does not check if distance > 100.
        /// </summary>
        /// <param name="journeyDetailIndex">Array index into roadJourney.JourneyDetails for journey instruction</param>
        /// <returns>distance in metres</returns>
        protected virtual int GetDistanceActual(int journeyDetailIndex)
        {
            JourneyDetail detail = journey.JourneyDetails[journeyDetailIndex];

            return detail.Distance;
        }


        /// <summary>
        /// Returns a formatted string (defined by FormatDuration method)
        /// of the duration of the current driving instruction.
        /// </summary>
        /// <param name="detail">Array index into roadJourney.Details for journey instruction</param>
        /// <returns>Formatted string of the duration.</returns>
        protected virtual string GetDuration(int journeyDetailIndex)
        {
            JourneyDetail detail = journey.JourneyDetails[journeyDetailIndex];
            return FormatDuration(detail.Duration);
        }

        /// <summary>
        /// Returns a formatted string (defined by FormatDuration method)
        /// of the duration of the current driving instruction.
        /// </summary>
        /// <param name="detail">Array index into roadJourney.Details for journey instruction</param>
        /// <returns>Formatted string of the duration.</returns>
        protected virtual int GetDurationActual(int journeyDetailIndex)
        {
            JourneyDetail detail = journey.JourneyDetails[journeyDetailIndex];
            return detail.Duration;
        }

        /// <summary>
        /// Formats a duration (expressed in seconds) into a 
        /// string in the form "hh:mm". If the duration is
        /// less than 30 seconds then a "less 30 secs" string is returned.
        /// The returned time is rounded to the nearest minute.
        /// </summary>
        /// <param name="durationInSeconds">Duration in seconds.</param>
        /// <returns>Formatted string of the duration.</returns>
        protected virtual string FormatDuration(long durationInSeconds)
        {

            // Get the minutes
            double durationInMinutes = (double)durationInSeconds / 60.0;

            // Check to see if less than 30 seconds
            if (durationInMinutes / 60.0 < 1.00 &&
                durationInMinutes % 60.0 < 0.5)
            {
                string secondsString = GetResourceString("CarJourneyDetailsTableControl.labelDurationSeconds");

                return "< 30 " + secondsString;
            }
            else
            {
                // Duration is greater than 30 seconds

                // Round to the nearest minute
                durationInMinutes = Round(durationInMinutes);

                // Calculate the number of hours in the minute
                int hours = (int)durationInMinutes / 60;

                // Get the minutes (afer the hours has been subracted so always < 60)
                int minutes = (int)durationInMinutes % 60;

                return String.Format("{0:D2}:{1:D2}", hours, minutes);

            }
        }


        /// <summary>
        /// Returns a formatted string of the arrival time for the current
        /// driving instruction in the format defined by the method FormatTime.
        /// The time is calculated by adding the current journey
        /// instruction duration to an accumulated journey time.
        /// </summary>
        /// <param name="detail">Array index into roadJourney.Details for journey instruction</param>
        /// <returns>Formatted arrival time</returns>
        protected virtual DateTime GetArrivalTime(int journeyDetailIndex)
        {
            return currentArrivalTime;
        }

        /// <summary>
        /// Returns a formatted string of the toids of the current driving instruction.
        /// </summary>
        /// <param name="detail">Array index for journey instruction</param>
        protected virtual string GetTOIDs(int journeyDetailIndex)
        {
            return string.Empty;
        }

        /// <summary>
        /// Returns a formatted string of the osgrs of the current driving instruction.
        /// </summary>
        /// <param name="detail">Array index for journey instruction</param>
        protected virtual string GetOSGRs(int journeyDetailIndex)
        {
            return string.Empty;
        }

        /// <summary>
        /// Return a formatted string by converting the given metres
        /// to a mileage
        /// </summary>
        /// <param name="metres">Metres to convert</param>
        /// <returns>Formatted string</returns>
        protected virtual string ConvertMetresToMileage(int metres)
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
        protected virtual string ConvertMetresToKm(int metres)
        {
            double result = (double)metres / 1000;

            string distanceFormat = GetDistanceFormat();

            // Return the result
            return result.ToString(distanceFormat);
        }

        /// <summary>
        /// Formats the given time and date for display.
        /// Seconds are rounded to the nearest minute
        /// </summary>
        /// <param name="time">Time to format</param>
        /// <returns>Formatted string of the time in the form hh:mm (dd/mm/yy)</returns>
        protected virtual string FormatDateTime(DateTime time)
        {
            if (time.Second >= 30)
            {
                return time.AddMinutes(1).ToString("HH:mm (dd/MM)");
            }
            else
            {
                return time.ToString("HH:mm (dd/MM)");
            }
        }

        /// <summary>
        /// Gets the resource content for the specified key
        /// </summary>
        /// <param name="key">Resource key</param>
        /// <returns>string identified by resource key in content database</returns>
        protected string GetResourceString(string key)
        {
            return resourceManager.GetString
                (currentLanguage,
                    SJPResourceManager.GROUP_JOURNEYOUTPUT,
                    SJPResourceManager.COLLECTION_JOURNEY,
                    key);
        }

        /// <summary>
        /// Method to return the text format of a distance to be converted to string.
        /// The distanceDecimalPlaces value is used to determine number decimal places
        /// </summary>
        /// <returns></returns>
        protected string GetDistanceFormat()
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

        /// <summary>
        /// Gets corresponding capitalised string.
        /// </summary>
        /// <param name="upper"></param>
        /// <param name="originalString"></param>
        /// <returns></returns>
        protected string ChangeFirstCharacterCapitalisation(string originalString, bool upper)
        {
            string firstChar = originalString[0].ToString();
            firstChar = upper ? firstChar.ToUpper(CultureInfo.CurrentCulture)
                : firstChar.ToLower(CultureInfo.CurrentCulture);

            string newString = firstChar + originalString.Substring(1);

            return newString;
        }

        /// <summary>
        /// Rounds the given double to the nearest int.
        /// If double is 0.5, then rounds up.
        /// Using this instead of Math.Round because Math.Round
        /// ALWAYS returns the even number when rounding a .5 -
        /// this is not behaviour we want.
        /// </summary>
        /// <param name="valueToRound">Value to round.</param>
        /// <returns>Nearest integer</returns>
        protected static int Round(double valueToRound)
        {
            // Get the decimal point
            double valueFloored = Math.Floor(valueToRound);
            double remain = valueToRound - valueFloored;

            if (remain >= 0.5)
                return (int)Math.Ceiling(valueToRound);
            else
                return (int)Math.Floor(valueToRound);
        }

        /// <summary>
        /// Performs pre-processing of the Journey Detail.
        /// </summary>
        protected virtual void PostProcessJourneyDetail()
        {
            // Default implementation - does nothing
        }

        /// <summary>
        /// Performs post-processing of the Journey Detail.
        /// </summary>
        /// <param name="journeyDetailIndex"></param>
        protected virtual void PreProcessJourneyDetail(int journeyDetailIndex)
        {
            // Default implementation - does nothing
        }

        #endregion
    }
}