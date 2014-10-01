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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SJP.Common;
using SJP.Common.DataServices;
using SJP.Common.ResourceManager;
using SJP.Common.ServiceDiscovery;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.SJPWeb.Adapters;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DetailsCycleControl : System.Web.UI.UserControl
    {
        #region Constants

        private const string ARRIVETIMEFORMATE = "HH:mm";

        #endregion

        #region Private Fields

        // Resource manager
        private static string RG = SJPResourceManager.GROUP_JOURNEYOUTPUT;
        private static string RC = SJPResourceManager.COLLECTION_JOURNEY;
        private SJPResourceManager RM = Global.SJPResourceManager;

        private ISJPJourneyRequest journeyRequest = null;

        private JourneyLeg currentJourneyLeg;

        private List<FormattedJourneyDetail> journeyDetails;

        private bool showDebugInfo = false;

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

                lblDistanceTotalHead.Text = RM.GetString(language, RG, RC, "DetailsCycleControl.TotalDistanceHeading.Text") + " ";
                lblDistanceTotal.Text = ConvertMetersToKm(currentJourneyLeg.Distance);
                lblDurationTotalHead.Text = RM.GetString(language, RG, RC, "DetailsCycleControl.TotalDurationHeading.Text") + " ";
                lblDurationTotal.Text = GetDuration(currentJourneyLeg.Duration);
                lblOptions.Text = GetOptions(journeyRequest);
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

            if (e.Item.ItemType == ListItemType.AlternatingItem
                || e.Item.ItemType == ListItemType.Item
                || e.Item.ItemType == ListItemType.SelectedItem)
            {
                Label distance = e.Item.FindControlRecursive<Label>("distance");
                Label instruction = e.Item.FindControlRecursive<Label>("instruction");
                Label arrive = e.Item.FindControlRecursive<Label>("arrive");
                HtmlGenericControl cyclePathName = e.Item.FindControlRecursive<HtmlGenericControl>("cyclePathName");
                HtmlGenericControl cycleInstruction = e.Item.FindControlRecursive<HtmlGenericControl>("cycleInstruction");
                HtmlGenericControl cycleAttributeText = e.Item.FindControlRecursive<HtmlGenericControl>("cycleAttributeText");
                Image cyclePathImage = e.Item.FindControlRecursive<Image>("cyclePathImage");
                Image cycleManoeuvreImage = e.Item.FindControlRecursive<Image>("cycleManoeuvreImage");
                HtmlControl cyclePathImageSeparator = e.Item.FindControlRecursive<HtmlControl>("cyclePathImageSeparator");

                FormattedCycleJourneyDetail cycleDetail = e.Item.DataItem as FormattedCycleJourneyDetail;
                if (cycleDetail.TotalDistance.HasValue)
                {
                    distance.Text = cycleDetail.ConvertMetresToKm(cycleDetail.TotalDistance.Value);
                }
                else
                    distance.Text = "-";

                instruction.Text = cycleDetail.Instruction;

                if (cycleDetail.ArriveTime.HasValue)
                {
                    DateTime arriveDateTime = cycleDetail.ArriveTime.Value;

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

                cyclePathName.InnerHtml = cycleDetail.PathName;
                cyclePathName.Visible = !string.IsNullOrEmpty(cycleDetail.PathName);

                cycleInstruction.InnerHtml = cycleDetail.CycleInstruction;
                cycleInstruction.Visible = !string.IsNullOrEmpty(cycleDetail.CycleInstruction);

                cycleAttributeText.InnerHtml = cycleDetail.CycleAttributes;
                cycleAttributeText.Visible = !string.IsNullOrEmpty(cycleDetail.CycleAttributes);

                if (!string.IsNullOrEmpty(cycleDetail.PathImageUrl))
                {
                    cyclePathImage.ImageUrl = ResolveClientUrl(page.ImagePath + cycleDetail.PathImageUrl);
                    cyclePathImage.AlternateText = cycleDetail.PathImageText;
                    cyclePathImage.ToolTip = cycleDetail.PathImageText;
                    cyclePathImage.Visible = true;
                }

                if (!string.IsNullOrEmpty(cycleDetail.ManoeuvreImage))
                {
                    cyclePathImageSeparator.Visible = true;
                    
                    cycleManoeuvreImage.ImageUrl = ResolveClientUrl(page.ImagePath + cycleDetail.ManoeuvreImage);
                    cycleManoeuvreImage.AlternateText = cycleDetail.ManoeuvreImageText;
                    cycleManoeuvreImage.ToolTip = cycleDetail.ManoeuvreImageText;
                    cycleManoeuvreImage.Visible = true;
                }

                // Debug information
                HtmlGenericControl debugInfoDiv = e.Item.FindControlRecursive<HtmlGenericControl>("debugInfoDiv");
                debugInfoDiv.Visible = showDebugInfo;
                if (showDebugInfo)
                {
                    Label lblDebugInfo = e.Item.FindControlRecursive<Label>("lblDebugInfo");

                    StringBuilder debugInfo = new StringBuilder();
                    debugInfo.Append(string.Format("dist[{0}m] ", cycleDetail.DistanceActual));
                    debugInfo.Append(string.Format("duration[{0}s] ", cycleDetail.DurationActual));
                    debugInfo.AppendLine("<br />");
                    debugInfo.Append(string.Format("toids[{0}] ", cycleDetail.TOIDs));
                    debugInfo.AppendLine("<br />");
                    debugInfo.Append(string.Format("osgrs[{0}] ", cycleDetail.OSGRs));
                    debugInfo.AppendLine("<br />");

                    lblDebugInfo.Text = debugInfo.ToString();
                }
            }

            if (e.Item.ItemType == ListItemType.Header)
            {
                Language language = CurrentLanguage.Value;

                Label distanceHeading = e.Item.FindControlRecursive<Label>("distanceHeading");
                Label instructionHeading = e.Item.FindControlRecursive<Label>("instructionHeading");
                Label arriveHeading = e.Item.FindControlRecursive<Label>("arriveHeading");

                distanceHeading.Text = RM.GetString(language, RG, RC, "DetailsCycleControl.DistanceHeading.Text");
                instructionHeading.Text = RM.GetString(language, RG, RC, "DetailsCycleControl.InstructionHeading.Text");
                arriveHeading.Text = RM.GetString(language, RG, RC, "DetailsCycleControl.ArriveHeading.Text");
            }
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Initialise
        /// </summary>
        /// <param name="journeyLegDetail"></param>
        public void Initialise(ISJPJourneyRequest journeyRequest, JourneyLeg currentJourneyLeg)
        {
            SJPPage page = (SJPPage)Page;

            this.journeyRequest = journeyRequest;
            this.currentJourneyLeg = currentJourneyLeg;
            this.showDebugInfo = DebugHelper.ShowDebug;
           
            DefaultCycleJourneyDetailFormatter cycleDetailFormatter = new DefaultCycleJourneyDetailFormatter(currentJourneyLeg, 
                CurrentLanguage.Value, Global.SJPResourceManager);

            journeyDetails = cycleDetailFormatter.GetJourneyDetails();

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
            double result = (double)metres / 1000;

            string formattedString = result.ToString("F1") + RM.GetString(CurrentLanguage.Value, RG, RC, "JourneyOutput.Text.Km");

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
        /// Returns a formatted string for the cycle journey option parameters to display
        /// </summary>
        /// <param name="journeyRequest"></param>
        /// <returns></returns>
        private string GetOptions(ISJPJourneyRequest journeyRequest)
        {
            StringBuilder journeyOptions = new StringBuilder();

            if (journeyRequest != null)
            {
                #region Type of journey

                IDataServices dataServices = SJPServiceDiscovery.Current.Get<IDataServices>(ServiceDiscoveryKey.DataServices);
                                
                journeyOptions.Append(dataServices.GetText(DataServiceType.CycleRouteType, journeyRequest.CycleAlgorithm, Global.SJPResourceManager, CurrentLanguage.Value));
                journeyOptions.Append(" ");
                journeyOptions.Append(RM.GetString(CurrentLanguage.Value, RG, RC, "DetailsCycleControl.Journey.Text"));
                journeyOptions.Append(". ");

                #endregion

                #region MaxSpeed

                // Speed is the cycle UserPreference at index 5
                decimal speedInKm = Convert.ToDecimal(journeyRequest.UserPreferences[5].PreferenceValue);

                string speedInKmText = Decimal.Round(speedInKm, 0).ToString();

                journeyOptions.Append(string.Format(
                    RM.GetString(CurrentLanguage.Value, RG, RC, "DetailsCycleControl.AverageCyclingSpeed.Text"),
                    speedInKmText));
                journeyOptions.Append(". ");

                #endregion
            }

            return journeyOptions.ToString();
        }

        #endregion
    }
}