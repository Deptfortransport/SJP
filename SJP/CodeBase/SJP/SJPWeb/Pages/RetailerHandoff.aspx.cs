// *********************************************** 
// NAME             : RetailerHandoff.aspx      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 07 Apr 2011
// DESCRIPTION  	: RetailerHandoff page which posts journey data to a retailer site
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Xml;
using SJP.Common;
using SJP.Common.EventLogging;
using SJP.Common.LocationService;
using SJP.Common.ServiceDiscovery;
using SJP.Common.Web;
using SJP.Reporting.Events;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.Retail;
using SJP.UserPortal.SessionManager;
using SJP.UserPortal.SJPWeb.Adapters;
using Logger = System.Diagnostics.Trace;

namespace SJP.UserPortal.SJPWeb.Pages
{
    /// <summary>
    /// RetailerHandoff page
    /// </summary>
    public partial class RetailerHandoff : SJPPage
    {
        #region Controls and private members

        /// <summary>
        /// The XML to be passed to the retailer
        /// </summary>
        private string handOffXml = string.Empty;

        /// <summary>
        /// The retailer that was selected from the ticket retailers page
        /// </summary>
        private Retailer retailer = null;
                        
        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public RetailerHandoff()
            : base(Global.SJPResourceManager)
        {
            pageId = PageId.RetailerHandoff;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The XML to be passed to the retailer suitable for embedding in an HTML document
        /// (i.e. encoded as HTML text)
        /// </summary>
        public string EncodedRetailerXml
        {
            get { return HttpUtility.HtmlEncode(handOffXml); }
        }

        /// <summary>
        /// The handoff URL of the retailer
        /// </summary>
        public string RetailerUrl
        {
            get { return retailer.HandoffUrl; }
        }

        #endregion Properties

        #region Page_Init, Page_Load, Page_PreRender

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            GetRetailerDetails();

            GenerateHandoffXml();
            
            // Only perform post if retailer is found and allows handoff, 
            // otherwise display the handoff xml on this page
            if ((retailer != null) && (retailer.HandoffSupported) && (!string.IsNullOrEmpty(handOffXml)))
            {
                LogEvent();

                RemotePost myremotepost = new RemotePost();

                myremotepost.Url = RetailerUrl;
                myremotepost.JSDisabledText = GetResource("RetailerHandOff.JavascriptDisabled.Text");
                myremotepost.Add("SJPBookingHandoffXML", EncodedRetailerXml);
                myremotepost.Post();

                Response.End();
            }
            else
            {
                #region Errors 

                // Error scenarios
                if (retailer == null)
                {
                    // No retailer
                    DisplayMessage(new SJPMessage("RetailerHandOff.Error.NoRetailers.Text", SJPMessageType.Error));
                }
                else if (string.IsNullOrEmpty(handOffXml))
                {
                    // No handoff xml (error trying to create it)
                    DisplayMessage(new SJPMessage("RetailerHandOff.Error.HandoffXml.Text", SJPMessageType.Error));
                }

                #endregion
            }
        }

        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetupDebug();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets details of retailer selected on ticket retailers page
        /// </summary>
        private void GetRetailerDetails()
        {
            // Read the retailer id from the query string
            string retailerId = Request.QueryString[QueryStringKey.Retailer];

            if (!string.IsNullOrEmpty(retailerId))
            {
                // And obtain the retailer details
                IRetailerCatalogue retailerCatalogue = SJPServiceDiscovery.Current.Get<IRetailerCatalogue>(ServiceDiscoveryKey.RetailerCatalogue);

                retailer = retailerCatalogue.FindRetailer(retailerId);

                // Check if the handoff xml should be shown, using the test retailer
                if (retailer == null)
                {
                    RetailerHelper retailerHelper = new RetailerHelper();

                    Retailer testRetailer = retailerHelper.GetTestXMLRetailer();

                    if (testRetailer.Id.Equals(retailerId))
                    {
                        retailer = testRetailer;
                    }
                }
            }
        }

        /// <summary>
        /// Generates the retailer handoff XML document based on the journey. This document is
        /// available through the EncodedRetailerXml property.
        /// </summary>
        private void GenerateHandoffXml()
        {
           // Journeys to handoff
            string journeyRequestHash = null;
            Journey journeyOutward = null;
            Journey journeyReturn = null;

            JourneyHelper journeyHelper = new JourneyHelper();
            SessionHelper sessionHelper = new SessionHelper();
                        
            // Get the journeys
            journeyHelper.GetJourneys(out journeyRequestHash, out journeyOutward, out journeyReturn);

            // Get the journey request (to identify the venue naptan, which might be needed for the handoff xml)
            ISJPJourneyRequest journeyRequest = sessionHelper.GetSJPJourneyRequest(journeyRequestHash);
            string outwardOriginVenueNaptan = string.Empty;
            string outwardDestinationVenueNaptan = string.Empty;
            string returnOriginVenueNaptan = string.Empty;
            string returnDestinationVenueNaptan = string.Empty;

            if (journeyRequest != null)
            {
                if (journeyRequest.Origin != null && journeyRequest.Origin is SJPVenueLocation)
                {
                    outwardOriginVenueNaptan = journeyRequest.Origin.Naptan[0];
                }

                if (journeyRequest.Destination != null && journeyRequest.Destination is SJPVenueLocation)
                {
                    outwardDestinationVenueNaptan = journeyRequest.Destination.Naptan[0];
                }
                
                if (journeyRequest.ReturnOrigin != null && journeyRequest.ReturnOrigin is SJPVenueLocation)
                {
                    returnOriginVenueNaptan = journeyRequest.ReturnOrigin.Naptan[0];
                }

                if (journeyRequest.ReturnDestination != null && journeyRequest.ReturnDestination is SJPVenueLocation)
                {
                    returnDestinationVenueNaptan = journeyRequest.ReturnDestination.Naptan[0];
                }
            }

            try
            {
                // Generate the handoff XML
                XmlDocument handoffXmlDoc = (XmlDocument)RetailHandoffConvertor.GenerateHandoffXml(
                        SJPSessionManager.Current.Session.SessionID, 
                        journeyOutward, journeyReturn,
                        outwardOriginVenueNaptan, outwardDestinationVenueNaptan, 
                        returnOriginVenueNaptan, returnDestinationVenueNaptan);

                handOffXml = handoffXmlDoc.OuterXml.ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error,
                    string.Format("Error parsing journey into a retail handoff xml document, JourneyRequestHash[{0}], Outward JourneyId[{1}], ReturnJourneyId[{2}]. Error message: {3}.",
                        journeyRequestHash,
                        (journeyOutward != null) ? journeyOutward.JourneyId.ToString() : string.Empty,
                        (journeyReturn != null) ? journeyReturn.JourneyId.ToString() : string.Empty,
                        ex.Message),
                     ex));

            }
        }
        
        /// <summary>
        /// Logs the event that the user is performing handoff to retailer site
        /// </summary>
        private void LogEvent()
        {
            RetailerHandoffEvent handoffEvent = new RetailerHandoffEvent(
                retailer.Id,
                SJPSessionManager.Current.Session.SessionID,
                false);
            Logger.Write(handoffEvent);
        }

        /// <summary>
        /// Displays a message
        /// </summary>
        private void DisplayMessage(SJPMessage sjpMessage)
        {
            ((SJPWeb)this.Master).DisplayMessage(sjpMessage);
        }

        /// <summary>
        /// Loads the text box with the handoff xml, if no xml then textbox is hidden
        /// </summary>
        private void SetupDebug()
        {
            if (DebugHelper.ShowDebug)
            {
                debugDiv.Visible = true;

                btnRetailerHandoff.Text = "Handoff";
                lblHandoffURL.Text = "Or enter handoff URL: ";

                // Show journey handoff XML
                if ((!string.IsNullOrEmpty(handOffXml)) && (!Page.IsPostBack))
                {
                    txtbxHandoffXML.Text = handOffXml;
                }

                // Show handoff retailers
                if (!Page.IsPostBack)
                {
                    #region Retailer options

                    RetailerHelper retailerHelper = new RetailerHelper();
                    JourneyHelper journeyHelper = new JourneyHelper();

                    string journeyRequestHash = null;
                    Journey journeyOutward = null;
                    Journey journeyReturn = null;

                    // Get the journeys
                    journeyHelper.GetJourneys(out journeyRequestHash, out journeyOutward, out journeyReturn);

                    // Get the retailers for these journeys
                    List<Retailer> retailers = retailerHelper.GetRetailersForJourneys(journeyOutward, journeyReturn);

                    // If retailers exist for the journeys, then display them
                    if (retailers.Count > 0)
                    {
                        // Remove any retailers without a handoff url
                        List<Retailer> retailersFiltered = new List<Retailer>();

                        foreach (Retailer retailer in retailers)
                        {
                            if (!string.IsNullOrEmpty(retailer.HandoffUrl))
                            {
                                retailersFiltered.Add(retailer);
                            }
                        }

                        // Sort the retailers
                        retailersFiltered.Sort(new Retailer());

                        // Update the journeys dropdown list
                        retailersRadioList.DataSource = retailersFiltered;
                        retailersRadioList.DataTextField = "HandoffUrl";
                        retailersRadioList.DataValueField = "HandoffUrl";

                        retailersRadioList.DataBind();

                        retailersRadioList.SelectedIndex = 0;
                    }

                    #endregion
                }
            }
        }
        
        #endregion

        #region Event handlers

        /// <summary>
        /// Event handler for Retailer Handoff button click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRetailerHandoff_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtbxHandoffXML.Text))
            {
                string url = string.Empty;

                // Use textbox url in preference
                if (!string.IsNullOrEmpty(txtbxHandoffURL.Text))
                {
                    url = txtbxHandoffURL.Text;
                }
                else
                {
                    url = retailersRadioList.SelectedValue;
                }

                // Handoff
                if (!string.IsNullOrEmpty(url))
                {

                    RemotePost myremotepost = new RemotePost();

                    myremotepost.Url = url;
                    myremotepost.Add("SJPBookingHandoffXML", HttpUtility.HtmlEncode(txtbxHandoffXML.Text));
                    myremotepost.Post();

                    Response.End();
                }
            }
        }

        #endregion
    }
}