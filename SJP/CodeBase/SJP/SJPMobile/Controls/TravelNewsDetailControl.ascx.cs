// *********************************************** 
// NAME             : TravelNewsDetailControl.ascx.cs      
// AUTHOR           : Mark Danforth
// DATE CREATED     : 12 Apr 2012
// DESCRIPTION  	: A template for holding the details of a news story
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Text;
using SJP.Common;
using SJP.Common.LocationService;
using SJP.Common.ResourceManager;
using SJP.Common.ServiceDiscovery;
using SJP.Common.Web;
using SJP.UserPortal.TravelNews.TravelNewsData;

namespace SJP.UserPortal.SJPMobile.Controls
{
    /// <summary>
    /// TravelNewsDetailControl
    /// </summary>
    public partial class TravelNewsDetailControl : System.Web.UI.UserControl
    {
        #region Variables

        private SJPResourceManager RM = Global.SJPResourceManager;

        private LocationHelper locationHelper = null;

        private TravelNewsItem tni = null;
        
        #endregion

        #region Page Load

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            locationHelper = new LocationHelper();
        }

        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetupControls();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Initialise
        /// </summary>
        /// <param name="tni"></param>
        public void Initialise(TravelNewsItem tni)
        {
            this.tni = tni;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Setsup the controls
        /// </summary>
        public void SetupControls()
        {
            if (tni != null)
            {
                Language language = CurrentLanguage.Value;

                detailNewsTitle.InnerText = ((SJPPageMobile)Page).GetResourceMobile("TravelNewsDetail.Heading.Text");
                closeNewsItem.Text = ((SJPPageMobile)Page).GetResourceMobile("JourneyInput.Back.Text");

                // Headline
                detailNewsHeadlineLbl.InnerText = Server.HtmlEncode(tni.HeadlineText);

                // Dates
                startDateContent.Text = tni.StartDateTime.ToString(RM.GetString(language, "TravelNews.StartDate.Format"));
                startDateLbl.Text = string.Format(RM.GetString(language, "TravelNews.StartDate.Text"), "");

                statusDateContent.Text = tni.LastModifiedDateTime.ToString(RM.GetString(language, "TravelNews.StatusDate.Format"));
                statusDateLbl.Text = string.Format(RM.GetString(language, "TravelNews.StatusDate.Updated.Text"), "");

                if (tni.ClearedDateTime != DateTime.MinValue)
                {
                    if (DateTime.Now > tni.ClearedDateTime)
                    {
                        statusDateContent.Text = tni.ClearedDateTime.ToString(RM.GetString(language, "TravelNews.StatusDate.Format"));
                        statusDateLbl.Text = string.Format(RM.GetString(language, "TravelNews.StatusDate.Cleared.Text"), "");
                    }
                }

                // Summary
                operatorContent.Text = !string.IsNullOrEmpty(tni.PublicTransportOperator) ? tni.PublicTransportOperator : tni.Operator;
                operatorLbl.Text = RM.GetString(language, "TravelNews.Operator.Text");

                severityContent.Text = tni.OlympicIncident ? tni.OlympicSeverityDescription : tni.SeverityDescription;
                severityLbl.Text = RM.GetString(language, "TravelNews.Severity.Text");

                incidentTypeContent.Text = tni.IncidentType;
                incidentTypeLbl.Text = RM.GetString(language, "TravelNews.IncidentType.Text");

                venuesAffectedContent.Text = locationHelper.GetLocationNames(tni.OlympicVenuesAffected);
                venuesAffectedContent.Visible = tni.OlympicVenuesAffected.Count > 0;
                venuesAffectedLbl.Text = RM.GetString(language, "TravelNews.VenuesAffected.Text");
                venuesAffectedLbl.Visible = tni.OlympicVenuesAffected.Count > 0;

                locationTextLbl.Text = string.Format(RM.GetString(language, "TravelNews.locationText.Text"), "");
                locationTextContent.Text = tni.Location;


                // Detail
                detailTextContent.Text = tni.DetailText; // Do not HtmlEncode because any found "search token" matched add a <span class=highlight> tag
                detailTextLbl.Text = RM.GetString(language, "TravelNews.Detail.Text");

                // Specator advice
                travelAdviceContent.Text = tni.OlympicTravelAdvice;
                travelAdviceContent.Visible = !string.IsNullOrEmpty(tni.OlympicTravelAdvice);
                travelAdviceLbl.Text = RM.GetString(language, "TravelNews.SpectatorAdvice.Text");
                travelAdviceLbl.Visible = !string.IsNullOrEmpty(tni.OlympicTravelAdvice);
            }
        }

        #endregion
    }
}