// *********************************************** 
// NAME             : CycleJourneyGPXDownload.aspx.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 11 May 2011
// DESCRIPTION  	: Cycle Journey GPX download page.
//                    The page will not show if GPX creationg gets succeeded and just return the GPX file.
//                    In the event of error the page will display error.
// ************************************************


using System;
using System.Linq;
using SJP.Common;
using SJP.Common.EventLogging;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.SJPWeb.Adapters;
using Logger = System.Diagnostics.Trace;

namespace SJP.UserPortal.SJPWeb.Pages
{
    /// <summary>
    /// Cycle Journey GPX download page.
    /// </summary>
    public partial class CycleJourneyGPXDownload : SJPPage
    {
        #region Variables

        private string journeyRequestHash = null;
        private Journey journeyOutward = null;
        private Journey journeyReturn = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public CycleJourneyGPXDownload()
            : base(Global.SJPResourceManager)
        {
            pageId = PageId.CycleJourneyGPXDownload;
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
            // CycleGPXDownload links contains the query string representing the outward or return 
            // journeys for which GPX generation needed.
            SynchSelectedJourneys();

            if (!GenerateGPXFile())
            {
                // If we reach this point, then the GPX file was not successfully created, so we should 
                // display an error message to the user
                DisplayErrorMessage();
            }
        }

        
        #endregion

        #region Private Methods
        /// <summary>
        /// Generates the GPX file and sends the respond back to the client
        /// </summary>
        /// <returns>True if GPX file generation succeeded</returns>
        private bool GenerateGPXFile()
        {
            JourneyResultHelper resultHelper = new JourneyResultHelper();

            if (resultHelper.IsJourneyResultAvailable)
            {
                JourneyHelper journeyHelper = new JourneyHelper();

                // Retrieve selected journeys (using query string first, then session)
                journeyHelper.GetJourneys(out journeyRequestHash, out journeyOutward, out journeyReturn);

                byte[] gpx = null;

                JourneyLeg cycleLeg = null;

                try
                {
                    
                    if (journeyOutward != null)
                    {
                        if (journeyOutward.IsCycleJourney())
                        {
                            cycleLeg = journeyOutward.JourneyLegs.SingleOrDefault(jl => jl.Mode == SJPModeType.Cycle);
                        }
                    }
                    else
                    {
                        if (journeyReturn.IsCycleJourney())
                        {
                            cycleLeg = journeyReturn.JourneyLegs.SingleOrDefault(jl => jl.Mode == SJPModeType.Cycle);
                        }
                    }

                    if (cycleLeg != null)
                    {
                        GPXCycleJourneyDetailFormatter gpxformatter = new GPXCycleJourneyDetailFormatter(cycleLeg, CurrentLanguage.Value, resourceManager);

                        gpx = gpxformatter.GenerateGPXFile();

                        gpxformatter = null;

                        string gpxFileName = string.Format(GetResource("CycleJourneyGPXDownload.FileName"), cycleLeg.LegStart.Location.DisplayName, cycleLeg.LegEnd.Location.DisplayName);

                        // Convert all spaces, browser seems to truncate name at first space
                        gpxFileName = gpxFileName.Replace(" ", "_");

                        if (gpx != null && gpx.Length > 0)
                        {
                            Response.Clear();
                            Response.AddHeader("Content-Disposition", 
                                string.Format("attachment;filename={0}",Server.HtmlEncode(string.IsNullOrEmpty(gpxFileName.Trim())?"cycle.gpx": gpxFileName)));
                            Response.AddHeader("Content-Length", gpx.Length.ToString());
                            Response.ContentType = "application/octet-stream";
                            Response.BinaryWrite(gpx);
                                                                        
                            return true;
                        }
                    }
                }
                catch (SJPException sjpEx)
                {
                    OperationalEvent operationalEvent = new OperationalEvent
                        (SJPEventCategory.Infrastructure, SJPTraceLevel.Error, sjpEx.Message);
                    
                    Logger.Write(operationalEvent);
                    return false;
                }
                catch (Exception ex)
                {
                    OperationalEvent operationalEvent = new OperationalEvent
                        (SJPEventCategory.Infrastructure, SJPTraceLevel.Error, ex.Message, ex);
                    Logger.Write(operationalEvent);
                    return false;
                }

            }
            return false;
        }


        /// <summary>
        /// Sets up error message to be displayed in the case of error generating GPX file
        /// </summary>
        private void DisplayErrorMessage()
        {
            SJPMessage errorMessage = new SJPMessage("CycleJourneyGPXDownload.Error.Text", SJPMessageType.Error);

            ((SJPWeb)this.Master).DisplayMessage(errorMessage);

            // Display the message seperator div (just a line seperator image)
            messageSeprator.Visible = true;
        }

        /// <summary>
        /// Updates the session with the selected journey ids
        /// </summary>
        private void SynchSelectedJourneys()
        {
            // Query string may have no journey ids if user has collapsed all journeys, therefore update
            // session with none selected

            JourneyHelper journeyHelper = new JourneyHelper();

            int journeyIdOutward = journeyHelper.GetJourneySelectedQueryString(true);
            int journeyIdReturn = journeyHelper.GetJourneySelectedQueryString(false);

            journeyHelper.SetJourneySelected(true, journeyIdOutward);

            journeyHelper.SetJourneySelected(false, journeyIdReturn);
        }


        #endregion
    }
}