// *********************************************** 
// NAME             : Map1.aspx.cs      
// AUTHOR           : David Lane
// DATE CREATED     : 29 May 2012
// DESCRIPTION  	: Map page for Bing maps
// ************************************************
// 

using SJP.Common;
using SJP.Common.ResourceManager;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.ScreenFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SJP.Common.LocationService;

namespace SJP.UserPortal.SJPWeb.Pages
{
    /// <summary>
    /// Page for Bing maps
    /// </summary>
    public partial class Map : SJPPage
    {
        // Resource strings
        private string TXT_JourneyDirectionOutward = string.Empty;
        private string TXT_JourneyDirectionReturn = string.Empty;
        private string TXT_JourneyHeaderOutward = string.Empty;
        private string TXT_JourneyHeaderReturn = string.Empty;
        private string TXT_JourneyArriveBy = string.Empty;
        private string TXT_JourneyLeavingAt = string.Empty;
        private string TXT_Transport = string.Empty;
        private string TXT_Changes = string.Empty;
        private string TXT_Leave = string.Empty;
        private string TXT_Arrive = string.Empty;
        private string TXT_JourneyTime = string.Empty;
        private string TXT_Select = string.Empty;
        private static string RG = SJPResourceManager.GROUP_JOURNEYOUTPUT;
        private static string RC = SJPResourceManager.COLLECTION_JOURNEY;

        /// <summary>
        /// Constructor
        /// </summary>
        public Map()
            : base(Global.SJPResourceManager)
        {
            pageId = PageId.MapBing;
        }

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetupPage();
            AddJavascript("Map1.js");
            AddJavascript("Common.js");
        }

        /// <summary>
        /// Setup the page content
        /// </summary>
        private void SetupPage()
        {
            // Don't display the sidebars
            ((SJPWeb)this.Master).DisplaySideBarLeft = false;
            ((SJPWeb)this.Master).DisplaySideBarRight = false;

            SJPPage page = (SJPPage)Page;

            // Check we have journey request hash and journey id
            if (page.QueryStringContains(QueryStringKey.JourneyRequestHash))
            {
                // Are we displaying outward or return?
                if (page.QueryStringContains(QueryStringKey.JourneyIdOutward))
                {
                    // Add the route information to the hidden field
                    SetupRoute(true);
                }
                else if (page.QueryStringContains(QueryStringKey.JourneyIdReturn))
                {
                    // Add the route information to the hidden field
                    SetupRoute(false);
                }
                else
                {
                    DisplayErrorAndHideMap();
                }
            }
            else
            {
                DisplayErrorAndHideMap();
            }

            btnBack.Text = Server.HtmlDecode(GetResource("AccessibilityOptions.Back.Text"));
            btnBack.ToolTip = Server.HtmlDecode(GetResource("AccessibilityOptions.Back.ToolTip"));
            cycleHeader.InnerText = GetResource("CycleMap.Header");
            lblCycleSoftContent.Text = GetResource("CycleMap.SoftContent");

            // Get printer friendly page URL and add jrh and jio/jir parameters
            string printerFriendlyUrl = PrinterFriendlyPageURL();
            URLHelper urlHelper = new URLHelper();
            if (page.QueryStringContains(QueryStringKey.JourneyRequestHash))
            {
                printerFriendlyUrl = urlHelper.AddQueryStringPart(printerFriendlyUrl, QueryStringKey.JourneyRequestHash, 
                    HttpContext.Current.Request.QueryString[QueryStringKey.JourneyRequestHash]);
            }
            if (page.QueryStringContains(QueryStringKey.JourneyIdOutward))
            {
                printerFriendlyUrl = urlHelper.AddQueryStringPart(printerFriendlyUrl, QueryStringKey.JourneyIdOutward, "1");
            }
            else
            {
                printerFriendlyUrl = urlHelper.AddQueryStringPart(printerFriendlyUrl, QueryStringKey.JourneyIdReturn, "1");
            }

            btnPrinterFriendly.Text = GetResource("JourneyOptions.PrinterFriendly.Text");
            btnPrinterFriendly.ToolTip = GetResource("JourneyOptions.PrinterFriendly.ToolTip");
            btnPrinterFriendly.OnClientClick = string.Format("openWindow('{0}')", printerFriendlyUrl);
            btnPrinterFriendly.Visible = true;
            lnkPrinterFriendly.Text = GetResource("JourneyOptions.PrinterFriendly.Text");
            lnkPrinterFriendly.ToolTip = GetResource("JourneyOptions.PrinterFriendly.ToolTip");
            lnkPrinterFriendly.NavigateUrl = printerFriendlyUrl;
            string RG = SJPResourceManager.GROUP_JOURNEYOUTPUT;
            string RC = SJPResourceManager.COLLECTION_JOURNEY;
            instructionHeading.Text = GetResource(RG, RC, "DetailsCycleControl.InstructionHeading.Text");
            arriveHeading.Text = GetResource(RG, RC, "DetailsCycleControl.ArriveHeading.Text");
        }

        /// <summary>
        /// Get the details of the route
        /// </summary>
        /// <param name="outward"></param>
        private void SetupRoute(bool outward)
        {
            JourneyResultHelper resultHelper = new JourneyResultHelper();
            JourneyHelper journeyHelper = new JourneyHelper();

            // Journey request/result
            ISJPJourneyRequest journeyRequest = resultHelper.JourneyRequest;
            ISJPJourneyResult journeyResult = resultHelper.CheckJourneyResultAvailability();

            // Journey to be shown
            Journey journey = null;

            SJPPage page = (SJPPage)Page;
            TXT_JourneyDirectionOutward = page.GetResource(RG, RC, "JourneyOutput.JourneyDirection.Outward.Text");
            TXT_JourneyDirectionReturn = page.GetResource(RG, RC, "JourneyOutput.JourneyDirection.Return.Text");
            TXT_JourneyHeaderOutward = page.GetResource(RG, RC, "JourneyOutput.JourneyHeader.Outward.Text");
            TXT_JourneyHeaderReturn = page.GetResource(RG, RC, "JourneyOutput.JourneyHeader.Return.Text");
            TXT_JourneyArriveBy = page.GetResource(RG, RC, "JourneyOutput.JourneyHeader.ArriveBy.Text");
            TXT_JourneyLeavingAt = page.GetResource(RG, RC, "JourneyOutput.JourneyHeader.LeavingAt.Text");
            TXT_Transport = page.GetResource(RG, RC, "JourneyOutput.HeaderRow.Transport.Text");
            TXT_Changes = page.GetResource(RG, RC, "JourneyOutput.HeaderRow.Changes.Text");
            TXT_Leave = page.GetResource(RG, RC, "JourneyOutput.HeaderRow.Leave.Text");
            TXT_Arrive = page.GetResource(RG, RC, "JourneyOutput.HeaderRow.Arrive.Text");
            TXT_JourneyTime = page.GetResource(RG, RC, "JourneyOutput.HeaderRow.JourneyTime.Text");
            TXT_Select = page.GetResource(RG, RC, "JourneyOutput.HeaderRow.Select.Text");

            string temp = page.GetResource(RG, RC, "Map.Error.NoJourney");

            if (journeyResult != null)
            {
                if (outward)
                {
                    if (journeyResult.OutwardJourneys.Count == 1)
                    {
                        journey = journeyResult.OutwardJourneys[0];
                    }
                }
                else
                {
                    if (journeyResult.ReturnJourneys.Count == 1)
                    {
                        journey = journeyResult.ReturnJourneys[0];
                    }
                }
            }

            if (journey == null)
            {
                // Nothing to display
                DisplayErrorAndHideMap();
            }
            else
            {
                // Build and set journey coordinates
                MapHelper mapHelper = new MapHelper(Global.SJPResourceManager);

                // Get map points and add to the hidden journey map points field
                journeyPoints.Value = mapHelper.GetJourneyMapPoints(journey, ((SJPPage)Page).ImagePath, false);

                // Initialise the cycle leg details
                JourneyLeg theCycleLeg = null;
                foreach (JourneyLeg leg in journey.JourneyLegs)
                {
                    if (leg.Mode == SJPModeType.Cycle)
                    {
                        theCycleLeg = leg;
                        cycleLeg.Initialise(journeyRequest, leg);
                        break;
                    }
                }

                string origin = outward ? journeyRequest.Origin.DisplayName : journeyRequest.ReturnOrigin.DisplayName;
                string destination = outward ? journeyRequest.Destination.DisplayName : journeyRequest.ReturnDestination.DisplayName;
                DateTime journeyTime = outward ? journeyRequest.OutwardDateTime : journeyRequest.ReturnDateTime;
                string resource = outward ? TXT_JourneyHeaderOutward : TXT_JourneyHeaderReturn;
                string resourceDateTime = outward ? TXT_JourneyArriveBy : TXT_JourneyLeavingAt;

                journeyHeader.Text = string.Format(
                        resource,
                        origin, destination);
                journeyDateTime.Text = string.Format(resourceDateTime, journeyTime.ToString("dd/MM/yyyy HH:mm"));
                journeyDirection.Text = outward ? TXT_JourneyDirectionOutward : TXT_JourneyDirectionReturn;

                startLocation.Text = origin;
                endLocation.Text = destination;

                if ((outward && (journeyRequest.Origin is SJPVenueLocation) && !(journeyRequest.Destination is SJPVenueLocation)) || 
                    (!outward && (journeyRequest.ReturnOrigin is SJPVenueLocation)))
                {
                    startLocation.Text = startLocation.Text + " - " + theCycleLeg.LegStart.Location.DisplayName;
                }
                else
                {
                    endLocation.Text = endLocation.Text + " - " + theCycleLeg.LegEnd.Location.DisplayName;
                }
            }
        }

        /// <summary>
        /// Back button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            // Transfer to journey options
            SJPPage page = (SJPPage)Page;
            page.SetPageTransfer(PageId.JourneyOptions);

            // Set the query string values for the JourneyOptions page,
            // this allows the result for the correct request to be loaded
            AddQueryStringForPage(PageId.JourneyOptions);
        }

        /// <summary>
        /// Returns the printer friendly page url, or empty string if not found
        /// </summary>
        private string PrinterFriendlyPageURL()
        {
            PageTransferDetail ptd = GetPageTransferDetail(PageId.PrintableMapBing);

            if (ptd != null)
            {
                return ResolveClientUrl(ptd.PageUrl);
            }

            return string.Empty;
        }

        /// <summary>
        /// Displays a message
        /// </summary>
        private void DisplayMessage(SJPMessage sjpMessage)
        {
            ((SJPWeb)this.Master).DisplayMessage(sjpMessage);
        }

        /// <summary>
        /// Bomb out
        /// </summary>
        private void DisplayErrorAndHideMap()
        {
            // Display error
            DisplayMessage(new SJPMessage("Map.Error.NoJourney", SJPMessageType.Error));

            // Hide the map area (back button still visible)
            cycleJourney.Visible = false;
        }
    }
}
