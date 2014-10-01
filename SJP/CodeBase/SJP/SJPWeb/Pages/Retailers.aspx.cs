// *********************************************** 
// NAME             : Retailers.aspx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 06 Apr 2011
// DESCRIPTION  	: Retailers page containing retailers to handoff journey to for ticketing
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SJP.Common;
using SJP.Common.EventLogging;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.Retail;
using SJP.UserPortal.ScreenFlow;
using SJP.UserPortal.SJPWeb.Adapters;
using Logger = System.Diagnostics.Trace;

namespace SJP.UserPortal.SJPWeb.Pages
{
    /// <summary>
    /// Retailers page
    /// </summary>
    public partial class Retailers : SJPPage
    {
        #region Private Fields

        // Journeys to be retailed
        string journeyRequestHash = string.Empty;
        Journey journeyOutward = null;
        Journey journeyReturn = null;

        // Retail handoff page which will perform the handoff build and post to retailer
        private string URL_RetailerHandoff = string.Empty;

        // Helpers
        private JourneyHelper journeyHelper = new JourneyHelper();
        private RetailerHelper retailerHelper = new RetailerHelper();
        
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Retailers()
            : base(Global.SJPResourceManager)
        {
            pageId = PageId.Retailers;
        }

        #endregion

        #region Page_Init, Page_Load, Page_PreRender

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            AddJavascript("Common.js");
            AddJavascript("Retailers.js");

            rptRetailers.ItemDataBound += new RepeaterItemEventHandler(rptRetailers_ItemDataBound);

            SetupRetailerHandoffURL();

            DisplayRetailers();
        }
                        
        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetupResources();

            // If javascript is enabled, then the JourneyOptions page may not have had the chance to 
            // sync the selected journey to session (i.e. no postbacks) - so query string (containing journey 
            // id's here) andsession should be synched
            SynchSelectedJourneys();
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Event handler for the Retailer repeater item data bound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptRetailers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Retailer retailer = (Retailer)e.Item.DataItem;

                if (retailer != null)
                {
                    // Populate retailer row details
                    SJPModeType retailerMode = GetRetailerMode(retailer.Modes);

                    if (retailerMode != SJPModeType.Unknown)
                    {
                        // Get the resource key used for this retailer
                        string resourceKey = retailer.ResourceKey;

                        // Get the handoff url, this will load the aspx to build the handoff xml and post to retailer
                        string handoffUrl = retailerHelper.BuildRetailerHandoffURL(URL_RetailerHandoff, retailer, 
                            journeyRequestHash, journeyOutward, journeyReturn);

                        #region Image

                        Image imgRetailerIcon = (Image)e.Item.FindControl("imgRetailerIcon");
                        
                        if (imgRetailerIcon != null)
                        {
                            // Set image based on retailer's resource key
                            imgRetailerIcon.ImageUrl = ImagePath + GetResource(string.Format("{0}.Image.Path", resourceKey));
                            imgRetailerIcon.AlternateText = GetResource(string.Format("{0}.Image.AlternateText", resourceKey));
                            imgRetailerIcon.ToolTip = imgRetailerIcon.AlternateText;
                        }

                        #endregion

                        #region Button

                        Button btnRetailerHandoff = (Button)e.Item.FindControl("btnRetailerHandoff");

                        if (btnRetailerHandoff != null)
                        {
                            // Set the retailer button text
                            btnRetailerHandoff.Text = Server.HtmlDecode(GetResource(string.Format("{0}.HandoffButton.Text", resourceKey)));
                            btnRetailerHandoff.ToolTip = Server.HtmlDecode(GetResource(string.Format("{0}.HandoffButton.ToolTip", resourceKey)));
                            
                            btnRetailerHandoff.CommandArgument = retailer.Id;
                            btnRetailerHandoff.Click += new EventHandler(btnRetailerHandoff_Click);
                            
                            // Add handoff url
                            if (!string.IsNullOrEmpty(handoffUrl))
                            {
                                btnRetailerHandoff.OnClientClick = string.Format("openWindow('{0}')", handoffUrl);
                            }
                        }

                        #endregion

                        #region Hyperlink (for non-javascript)

                        HyperLink lnkRetailerHandoff = (HyperLink)e.Item.FindControl("lnkRetailerHandoff");
                        
                        if (lnkRetailerHandoff != null)
                        {
                            // Set the retailer link text
                            lnkRetailerHandoff.Text = Server.HtmlDecode(GetResource(string.Format("{0}.HandoffButton.Text", resourceKey)));
                            lnkRetailerHandoff.ToolTip = Server.HtmlDecode(GetResource(string.Format("{0}.HandoffButton.ToolTip", resourceKey)));

                            // Add handoff url
                            if (!string.IsNullOrEmpty(handoffUrl))
                            {
                                lnkRetailerHandoff.NavigateUrl = handoffUrl;
                            }
                        }

                        #endregion
                    }
                }
            }
        }

        /// <summary>
        /// Event handler for Retailer Handoff button click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRetailerHandoff_Click(object sender, EventArgs e)
        {
            DisplayMessage(new SJPMessage("Retailers.Message.JourneyHandedOff.Text", SJPMessageType.Info));
        }

        /// <summary>
        /// Event handler for Back button click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            SetPageTransfer(PageId.JourneyOptions);
            AddQueryStringForPage(PageId.JourneyOptions);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Loads resource strings for labels/controls
        /// </summary>
        private void SetupResources()
        {
            lblBookingSummary.Text = GetResource("Retailers.BookingSummary.Text");
            lblDisclaimer1.Text = GetResource("Retailers.Disclaimer1.Text");
            lblDisclaimer2.Text = GetResource("Retailers.Disclaimer2.Text");
            btnBack.Text = Server.HtmlDecode(GetResource("Retailers.Back.Text"));
            btnBack.ToolTip = Server.HtmlDecode(GetResource("Retailers.Back.ToolTip"));
        }

        /// <summary>
        /// Sets the retailer handoff url
        /// </summary>
        private void SetupRetailerHandoffURL()
        {
            PageTransferDetail ptd = GetPageTransferDetail(PageId.RetailerHandoff);

            if (ptd != null)
            {
                URL_RetailerHandoff = ResolveClientUrl(ptd.PageUrl);
            }
            else
            {
                // This shouldnt happen, but may do if page hasn't been setup correctly
                Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error,
                    "RetailerHandoff page url was not found, sitemap may be missing the page definition. Unable to perform retail handoff."));
            }
        }

        /// <summary>
        /// Loads the retailers to display in the data repeater
        /// </summary>
        private void DisplayRetailers()
        {
            // Get the journeys
            journeyHelper.GetJourneys(out journeyRequestHash, out journeyOutward, out journeyReturn);

            // Get the retailers for these journeys
            List<Retailer> retailers = retailerHelper.GetRetailersForJourneys(journeyOutward, journeyReturn);
            
            // If retailers exist for the journeys, then display them
            if (retailers.Count > 0)
            {
                // Sort the retailers
                retailers.Sort(new Retailer());

                pnlRetailers.Visible = true;

                rptRetailers.DataSource = retailers;

                rptRetailers.DataBind();
            }
            else
            {
                // Display error
                DisplayMessage(new SJPMessage("Retailers.Error.NoRetailers.Text", SJPMessageType.Error));

                // And hide any non relavent labels 
                lblBookingSummary.Visible = false;
            }
        }

        /// <summary>
        /// Displays a message
        /// </summary>
        private void DisplayMessage(SJPMessage sjpMessage)
        {
            ((SJPWeb)this.Master).DisplayMessage(sjpMessage);
        }
                
        /// <summary>
        /// Returns a single SJPModeType from a list of SJPModeTypes
        /// </summary>
        /// <param name="modes"></param>
        private SJPModeType GetRetailerMode(List<SJPModeType> modes)
        {
            if (modes.Count == 1)
            {
                return modes[0];
            }
            else
            {
                // Use a hierachy to determine which to return!
                if (modes.Contains(SJPModeType.Rail))
                {
                    return SJPModeType.Rail;
                }
                else if (modes.Contains(SJPModeType.Coach))
                {
                    return SJPModeType.Coach;
                }
                else if (modes.Contains(SJPModeType.Ferry))
                {
                    return SJPModeType.Ferry;
                }
                else
                {
                    return SJPModeType.Unknown;
                }
            }
        }

        /// <summary>
        /// Updates the session with the selected journey ids
        /// </summary>
        private void SynchSelectedJourneys()
        {
            if (journeyOutward != null)
            {
                journeyHelper.SetJourneySelected(true, journeyOutward.JourneyId);
            }

            if (journeyReturn != null)
            {
                journeyHelper.SetJourneySelected(false, journeyReturn.JourneyId);
            }
        }

        #endregion
    }
}