// *********************************************** 
// NAME             : FileNamePlaceholder      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: DiscriptionPlaceholder
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SJP.Common.ResourceManager;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.SJPWeb.Adapters;
using SJP.Common;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DetailsCarControl : System.Web.UI.UserControl
    {
        #region Constants

        private const string ARRIVETIMEFORMATE = "HH:mm";

        #endregion

        #region Private Fields

        // Resource manager
        private static string RG = SJPResourceManager.GROUP_JOURNEYOUTPUT;
        private static string RC = SJPResourceManager.COLLECTION_JOURNEY;
        private SJPResourceManager RM = Global.SJPResourceManager;

        private JourneyLeg currentJourneyLeg;

        private List<FormattedJourneyDetail> journeyDetails;

        #endregion

        #region Page_Init, Page_Load, Page_PreRender

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (currentJourneyLeg != null)
            {
                Language language = CurrentLanguage.Value;

                lblDistanceTotalHead.Text = RM.GetString(language, RG, RC, "DetailsCarControl.TotalDistanceHeading.Text") + " ";
                lblDistanceTotal.Text = ConvertMetersToKm(currentJourneyLeg.Distance);
                lblDurationTotalHead.Text = RM.GetString(language, RG, RC, "DetailsCarControl.TotalDurationHeading.Text") + " ";
                lblDurationTotal.Text = GetDuration(currentJourneyLeg.Duration);
                
                // Only display roads label if roads exist
                lblRoads.Text = GetRoads(currentJourneyLeg);
                if (lblRoads.Text.Length > 0)
                {
                    lblRoadsHead.Text = RM.GetString(language, RG, RC, "DetailsCarControl.MajorRoadsHeading.Text") + " ";
                }
            }
        }

        #endregion

        #region Control Event Handlers

        /// <summary>
        /// legDetail_DataBound event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void legDetail_DataBound(object sender, RepeaterItemEventArgs e)
        {
            SJPPage page = (SJPPage)Page;

            Language language = CurrentLanguage.Value;

            if (e.Item.ItemType == ListItemType.AlternatingItem
                || e.Item.ItemType == ListItemType.Item
                || e.Item.ItemType == ListItemType.SelectedItem)
            {
                Label distance = e.Item.FindControlRecursive<Label>("distance");
                Label instruction = e.Item.FindControlRecursive<Label>("instruction");
                Label arrive = e.Item.FindControlRecursive<Label>("arrive");
                Image highTrafficSymbol = e.Item.FindControlRecursive<Image>("highTrafficSymbol");

                FormattedJourneyDetail carDetail = e.Item.DataItem as FormattedJourneyDetail;
                if (carDetail.TotalDistance.HasValue)
                {
                    distance.Text = carDetail.ConvertMetresToKm(carDetail.TotalDistance.Value);
                }
                else
                    distance.Text = "-";

                instruction.Text = carDetail.Instruction;

                if (carDetail.HighTrafficLevel)
                {
                    highTrafficSymbol.Visible = true;
                    highTrafficSymbol.ImageUrl = page.ImagePath + RM.GetString(language, RG, RC, "DetailsCarControl.HighTrafficSymbol.Url");
                    highTrafficSymbol.AlternateText = RM.GetString(language, RG, RC, "DetailsCarControl.HighTrafficSymbol.AlternateText");
                    highTrafficSymbol.ToolTip = RM.GetString(language, RG, RC, "DetailsCarControl.HighTrafficSymbol.ToolTip");
                }

                if (carDetail.ArriveTime.HasValue)
                {
                    DateTime arriveDateTime = carDetail.ArriveTime.Value;

                    // If its the first direction (after the "Starting from.."), or the last direction,
                    // then round to ensure the time is the same as the container journey leg leave/arrive
                    if ((e.Item.ItemIndex <= 1)
                        || (e.Item.ItemIndex == (journeyDetails.Count - 1)))
                    {
                        if (arriveDateTime.Second >= 30)
                            arriveDateTime = arriveDateTime.AddMinutes(1);
                    }
                    
                    arrive.Text = arriveDateTime.ToString(ARRIVETIMEFORMATE);
                }
            }

            if (e.Item.ItemType == ListItemType.Header)
            {
                Label distanceHeading = e.Item.FindControlRecursive<Label>("distanceHeading");
                Label instructionHeading = e.Item.FindControlRecursive<Label>("instructionHeading");
                Label arriveHeading = e.Item.FindControlRecursive<Label>("arriveHeading");

                distanceHeading.Text = RM.GetString(language, RG, RC, "DetailsCarControl.DistanceHeading.Text");
                instructionHeading.Text = RM.GetString(language, RG, RC, "DetailsCarControl.InstructionHeading.Text");
                arriveHeading.Text = RM.GetString(language, RG, RC, "DetailsCarControl.ArriveHeading.Text");
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialise
        /// </summary>
        /// <param name="journeyLegDetail"></param>
        public void Initialise(JourneyLeg currentJourneyLeg)
        {
            SJPPage page = (SJPPage)Page;

            this.currentJourneyLeg = currentJourneyLeg;
            
            DefaultCarJourneyDetailFormatter carDetailFormatter = new DefaultCarJourneyDetailFormatter(currentJourneyLeg,
                CurrentLanguage.Value, Global.SJPResourceManager);

            journeyDetails = carDetailFormatter.GetJourneyDetails();

            legDetail.DataSource = journeyDetails;

            legDetail.DataBind();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Return a formatted string from converting the given metres
        /// to a km (only 1 decimal place will be returned in the string).
        /// </summary>
        /// <param name="metres"></param>
        /// <returns></returns>
        private string ConvertMetersToKm(int metres)
        {
            Language language = CurrentLanguage.Value;

            double result = (double)metres / 1000;

            string formattedString = result.ToString("F1") + RM.GetString(language, RG, RC, "JourneyOutput.Text.Km");

            // Return the result
            return formattedString;
        }

        /// <summary>
        /// Returns a formatted string for a duration timespan
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        private string GetDuration(TimeSpan duration)
        {
            Language language = CurrentLanguage.Value;

            // In seconds
            double durationInSeconds = duration.TotalSeconds;

            // Get the minutes
            double durationInMinutes = durationInSeconds / 60;

            // Check to see if seconds is less than 30 seconds.
            if (durationInSeconds < 31)
            {
                return "< 30 " + RM.GetString(language, RG, RC, "JourneyOutput.Text.Seconds");
            }
            else
            {
                // Round to the nearest minute
                durationInMinutes = Round(durationInMinutes);

                // Calculate the number of hours in the minute
                int hours = (int)durationInMinutes / 60;

                // Get the minutes (afer the hours has been subracted so always < 60)
                int minutes = (int)durationInMinutes % 60;

                // If greater than 1 hour - retrieve "hours", if 1 or less, retrieve "hour"
                string hourString = hours > 1 ?
                    RM.GetString(language, RG, RC, "JourneyOutput.Text.Hours") :
                    RM.GetString(language, RG, RC, "JourneyOutput.Text.Hour");

                // If greater than 1 minute - retrive "minutes", if 1 or less, retrieve "minute"
                string minuteString = minutes > 1 ?
                    RM.GetString(language, RG, RC, "JourneyOutput.Text.Minutes") :
                    RM.GetString(language, RG, RC, "JourneyOutput.Text.Minute");

                StringBuilder formattedString = new StringBuilder();

                if (hours > 0)
                {
                    formattedString.Append(hours);
                    formattedString.Append(hourString);
                    formattedString.Append(" ");
                }

                formattedString.Append(minutes);
                formattedString.Append(minuteString);

                return formattedString.ToString();
            }
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
        private int Round(double valueToRound)
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
        /// Returns a formatted string of the major roads used in the car journey
        /// </summary>
        private string GetRoads(JourneyLeg currentJourneyLeg)
        {
            StringBuilder roads = new StringBuilder();

            if (currentJourneyLeg != null)
            {
                // Used to temporarily hold all the road names so that
                // duplicates can be filtered out.
                List<string> roadsList = new List<string>();

                string road = string.Empty;

                // Get all the roads for the journey
                foreach (RoadJourneyDetail roadJourneyDetail in currentJourneyLeg.JourneyDetails)
                {
                    road = roadJourneyDetail.RoadNumber;

                    // Add the road name only if is not empty and it hasn't
                    // already been added
                    if (!string.IsNullOrEmpty(road))
                    {
                        if (!roadsList.Contains(road))
                        {
                            roadsList.Add(road);
                        }
                    }
                }

                // Construct the roads text
                foreach (string r in roadsList)
                {
                    roads.Append(r);
                    roads.Append("; ");
                }
            }

            return roads.ToString();
        }
        
        #endregion
    }
}