// *********************************************** 
// NAME             : SummaryInstructionAdapter.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 06 Feb 2012
// DESCRIPTION  	: Helper class to construct journey summary details
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SJP.Common.ResourceManager;
using System.Globalization;

namespace SJP.Common.Web
{
    /// <summary>
    /// Helper class to construct journey summary details
    /// </summary>
    public class SummaryInstructionAdapter
    {
        #region Private Fields

        #region Constants

        private string SHORT_TIME_FORMAT = "HH:mm";
        private string SHORT_DATE_FORMAT = "dd/MM";

        #endregion

        #region Resources

        // Resource manager
        private static string RG = SJPResourceManager.GROUP_JOURNEYOUTPUT;
        private static string RC = SJPResourceManager.COLLECTION_JOURNEY;
        private SJPResourceManager resourceManager = null;

        // Resource strings
        private string TXT_Hours = string.Empty;
        private string TXT_Hour = string.Empty;
        private string TXT_Minutes = string.Empty;
        private string TXT_Minute = string.Empty;
        
        #endregion

        private bool isMobile = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public SummaryInstructionAdapter(SJPResourceManager resourceManager, bool isMobile)
        {
            this.resourceManager = resourceManager;
            this.isMobile = isMobile;

            InitialiseText();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the time
        /// </summary>
        public string GetTime(DateTime dateTime, DateTime requestDateTime)
        {
            if (dateTime.Second >= 30)
                dateTime = dateTime.AddMinutes(1);

            // Check to see if the date is different from the request date.
            // For example, if the user has searched for a journey commencing on
            // a Sunday, but the first available train is on a Monday
            if (dateTime.Day != requestDateTime.Day)
            {
                // Days are different, return the time with the dates appended
                if (isMobile)
                {
                    return string.Format("{0} ({1})",
                        dateTime.ToString(SHORT_TIME_FORMAT),
                        dateTime.ToString(SHORT_DATE_FORMAT));
                }
                else
                {
                    return string.Format("{0}<br />({1})",
                        dateTime.ToString(SHORT_TIME_FORMAT),
                        dateTime.ToString(SHORT_DATE_FORMAT));
                }
            }
            else
            {
                // Dates are the same, simply return the time.
                return dateTime.ToString(SHORT_TIME_FORMAT);
            }
        }

        /// <summary>
        /// Returns the journey time (duration)
        /// </summary>
        /// <returns></returns>
        public string GetJourneyTime(DateTime startTime, DateTime endTime)
        {
            // The result journey time to return
            StringBuilder journeyTime = new StringBuilder();

            #region Calculate duration, rounding for consistency with start and end times display

            if (startTime.Second >= 30)
                startTime = startTime.AddSeconds(60 - startTime.Second);
            else
                startTime = startTime.Subtract(new TimeSpan(0, 0, startTime.Second));

            if (endTime.Second >= 30)
                endTime = endTime.AddSeconds(60 - endTime.Second);
            else
                endTime = endTime.Subtract(new TimeSpan(0, 0, endTime.Second));

            TimeSpan duration = endTime.Subtract(startTime);

            #endregion

            #region Determine hours/minutes

            //Get duration hours and minutes
            int durationHours = 0;
            int durationMinutes = 0;

            //Greater than 24 hours case
            if (duration.Days > 0)
            {
                // For each day, there are 24 hours
                durationHours = 24 * duration.Days + duration.Hours;
            }
            else if (duration.Hours != 0)
            {
                durationHours = duration.Hours;
            }

            durationMinutes = duration.Minutes;

            // Round up if necessary for consistency with start/end times.
            //if (duration.Seconds >= 30)
            //    durationMinutes += 1;

            // If the rounding up of minutes takes it to 60,
            // then increment hours by 1 and set mins to zero
            if (durationMinutes == 60)
            {
                durationHours++;
                durationMinutes = 0;
            }

            #endregion

            #region Build journey time string

            if (durationHours > 0)
                journeyTime.Append(durationHours.ToString(CultureInfo.CurrentCulture.NumberFormat));

            if (durationHours == 1)
                journeyTime.Append(TXT_Hour);
            else if (durationHours > 1)
                journeyTime.Append(TXT_Hours);

            if (durationMinutes != 0)
            {
                // if hour was not equal to 0 then add a space
                if (durationHours != 0)
                    journeyTime.Append(" ");

                // Check to see if minutes requires a 0 padding.
                // Pad with 0 only if an hour was present and minute is a single digit.
                if (durationMinutes < 10 & durationHours != 0)
                    journeyTime.Append("0");

                journeyTime.Append(durationMinutes.ToString(CultureInfo.CurrentCulture.NumberFormat));

                if (durationMinutes > 1)
                    journeyTime.Append(TXT_Minutes);
                else
                    journeyTime.Append(TXT_Minute);

            }
            else if (durationHours != 0 && durationMinutes == 0)
            {
                // Display "00" mins
                journeyTime.Append(" ");
                journeyTime.Append("00");
                journeyTime.Append(TXT_Minutes);
            }
            else if (durationHours == 0 && durationMinutes == 0)
            {
                // This leg has 0 hours 0 minutes, e.g. a journey to itself.
                // Should never really happen, but still required otherwise
                // no duration will be displayed.
                journeyTime.Append("0");
                journeyTime.Append(TXT_Minute);
            }

            #endregion

            return journeyTime.ToString();
        }

        /// <summary>
        /// Gets the mode of transports text
        /// </summary>
        /// <param name="modeTypes">Array of mode types used in the journey</param>
        /// <returns></returns>
        public string GetTransport(SJPModeType[] modeTypes)
        {
            int i;
            string resourceManagerKey;
            string mode;
            SortedList sortedModes;
            string modes = String.Empty;

            Language language = CurrentLanguage.Value;

            //Read the strings from Resourcing Manager given the enumerations.
            //Alphabetically sort the laguage-converted modes.
            sortedModes = new SortedList(modeTypes.Length);
            for (i = 0; i < modeTypes.Length; i++)
            {
                if (modeTypes[i] != SJPModeType.Transfer)
                {
                    resourceManagerKey = "TransportMode." + modeTypes[i].ToString();
                    mode = resourceManager.GetString(language, resourceManagerKey);
                    sortedModes.Add(mode, mode);
                }
            }

            //Create the comma-separated mode list.
            if (sortedModes.Count > 0)
            {
                for (i = 0; i < sortedModes.Count - 1; i++)
                {
                    modes += sortedModes.GetByIndex(i) + ", ";
                }
                //add final mode
                modes += sortedModes.GetByIndex(i);
            }

            return modes;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method which initialises the text string used by this control
        /// </summary>
        private void InitialiseText()
        {
            Language language = CurrentLanguage.Value;

            TXT_Hours = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.Hours");
            TXT_Hour = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.Hour");
            TXT_Minutes = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.Minutes");
            TXT_Minute = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.Minute");
        }

        #endregion
    }
}
