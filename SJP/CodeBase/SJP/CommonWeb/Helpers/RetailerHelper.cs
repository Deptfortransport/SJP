// *********************************************** 
// NAME             : RetailerHelper.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 07 Apr 2011
// DESCRIPTION  	: RetailerHelper class containg helper methods
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using SJP.Common;
using SJP.Common.EventLogging;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.Retail;
using Logger = System.Diagnostics.Trace;

namespace SJP.Common.Web
{
    /// <summary>
    /// RetailerHelper class containg helper methods
    /// </summary>
    public class RetailerHelper
    {
        #region Private members

        private IRetailerCatalogue retailerCatalogue = null;
        private ITravelcardCatalogue travelcardCatalogue = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public RetailerHelper()
        {
            retailerCatalogue = SJPServiceDiscovery.Current.Get<IRetailerCatalogue>(ServiceDiscoveryKey.RetailerCatalogue);
            travelcardCatalogue = SJPServiceDiscovery.Current.Get<ITravelcardCatalogue>(ServiceDiscoveryKey.TravelcardCatalogue);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Method which builds a list of retailers which can be used to ticket the supplied journeys
        /// </summary>
        /// <param name="journeyOutward">Outward journey to parse (if null it is ignored)</param>
        /// <param name="journeyReturn">Return journey to parse (if null it is ignored)</param>
        /// <returns>List of Retailers, will be empty if no retailers found</returns>
        public List<Retailer> GetRetailersForJourneys(Journey journeyOutward, Journey journeyReturn)
        {
            // Return result
            List<Retailer> retailers = new List<Retailer>();

            // Build up the retailers list for the journey
            if (journeyOutward != null)
            {
                GetRetailers(journeyOutward, ref retailers);
            }

            if (journeyReturn != null)
            {
                GetRetailers(journeyReturn, ref retailers);
            }


            if (DebugHelper.ShowDebug)
            {
                // Inject a Show XML retailer
                if (retailers.Count > 0)
                {
                    retailers.Add(GetTestXMLRetailer());
                }
                else
                {
                    // Could be a mode which has no retailer defined,
                    // but still want to display the Show XML retailer
                    List<SJPModeType> modes = new List<SJPModeType>();

                    if (journeyOutward != null)
                    {
                        modes.AddRange(journeyOutward.GetUsedModes());
                    }
                    if (journeyReturn != null)
                    {
                        modes.AddRange(journeyReturn.GetUsedModes());
                    }

                    if (modes.Contains(SJPModeType.Coach)
                        || modes.Contains(SJPModeType.Bus)
                        || modes.Contains(SJPModeType.Ferry)
                        || modes.Contains(SJPModeType.Rail))
                    {
                        retailers.Add(GetTestXMLRetailer());
                    }
                }
            }

            return retailers;
        }

        /// <summary>
        /// Method which builds a list of retailers which can be used to ticket the journey leg specified
        /// </summary>
        /// <returns>List of Retailers, will be empty if no retailers found</returns>
        public List<Retailer> GetRetailersForJourneyLeg(List<JourneyLeg> journeyLegs, int index)
        {
            // Return result
            List<Retailer> retailers = new List<Retailer>();

            if (journeyLegs != null)
            {
                JourneyLeg leg = journeyLegs[index];
                
                GetRetailers(leg, ref retailers);

                if (retailers.Count > 0)
                {
                    // If retailers were found for leg, check if the leg is affected
                    // by scenarios:
                    List<Retailer> tempRetailers = new List<Retailer>();
                    List<int> affectedJourneyLegIndexes = new List<int>();

                    GetCombinedCoachRailRetailers(journeyLegs, ref tempRetailers, ref affectedJourneyLegIndexes);

                    if (affectedJourneyLegIndexes.Contains(index))
                    {
                        // Leg is affected, return the retailers found for the scenario instead
                        retailers = tempRetailers;
                    }
                }
            }
            return retailers;
        }

        /// <summary>
        /// Returns the Test retailer used to allow the handoff XML to be shown on a page
        /// </summary>
        /// <returns></returns>
        public Retailer GetTestXMLRetailer()
        {
            Retailer retailer = new Retailer("TEST", new List<SJPModeType>(1) { SJPModeType.Rail }, "Test", "", "", "", "", "", "Retailers.Retailer.Test.Rail");

            return retailer;
        }

        /// <summary>
        /// Method checks if the specified leg has a combined coach rail retailer
        /// </summary>
        /// <param name="journeyLegs"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsJourneyLegForCombinedCoachRailRetailer(List<JourneyLeg> journeyLegs, int index)
        {
            List<Retailer> retailers = new List<Retailer>();
            List<int> affectedJourneyLegIndexes = new List<int>();

            if (journeyLegs != null)
            {
                // Specific scenarios
                GetCombinedCoachRailRetailers(journeyLegs, ref retailers, ref affectedJourneyLegIndexes);

                // Leg found for the combined coach rail retailer check?
                return affectedJourneyLegIndexes.Contains(index);
            }
            
            return false;
        }

        /// <summary>
        /// Method checks if the specified leg is covered by the Games Travelcard
        /// </summary>
        /// <param name="journeyLegs"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsJourneyLegCoveredByTravelcard(List<JourneyLeg> journeyLegs, int index)
        {
            bool showTravelcardSwitch = Properties.Current["Retail.Travelcard.ProcessJourneyLeg.Switch"].Parse(false);
            bool hasTravelcard = false;

            if (showTravelcardSwitch)
            {
                DateTime started = DateTime.Now; // For logging

                if ((journeyLegs != null) && (journeyLegs.Count > index))
                {
                    JourneyLeg leg = journeyLegs[index];

                    // Check if journey leg is covered by travelcard
                    if (IsJourneyLegFrequencyDetail(leg))
                    {
                        // Frequency leg has no start and end date times,
                        // calculate these and pass into the travelcard check
                        DateTime startTime = DateTime.MinValue;
                        DateTime endTime = DateTime.MinValue;

                        GetJourneyLegDateTime(journeyLegs, index, ref startTime, ref endTime);

                        hasTravelcard = travelcardCatalogue.HasTravelcard(leg, startTime, endTime);
                    }
                    else
                    {
                        hasTravelcard = travelcardCatalogue.HasTravelcard(leg);
                    }
                }

                if (SJPTraceSwitch.TraceVerbose)
                {
                    TimeSpan duration = DateTime.Now.Subtract(started);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Verbose,
                        string.Format("Travelcard - Journey leg processing: started[{0}] duration[{1}ms] hasTravelcard[{2}]",
                        started, duration.TotalMilliseconds, hasTravelcard )));
                }
            }

            return hasTravelcard;
        }
        
        /// <summary>
        /// Builds the RetailHandoff SJP page url, adding the retailer and journeys to be used
        /// </summary>
        /// <returns></returns>
        public string BuildRetailerHandoffURL(string url, Retailer retailer, string journeyRequestHash, Journey journeyOutward, Journey journeyReturn)
        {
            string retailHandoffUrl = string.Empty;

            if (!string.IsNullOrEmpty(url))
            {
                URLHelper urlHelper = new URLHelper();
                
                NameValueCollection nvc = new NameValueCollection();

                if (retailer != null)
                {
                    nvc.Add(QueryStringKey.Retailer, retailer.Id);
                }

                if (!string.IsNullOrEmpty(journeyRequestHash))
                {
                    nvc.Add(QueryStringKey.JourneyRequestHash, journeyRequestHash);
                }

                if (journeyOutward != null)
                {
                    nvc.Add(QueryStringKey.JourneyIdOutward, journeyOutward.JourneyId.ToString());
                }

                if (journeyReturn != null)
                {
                    nvc.Add(QueryStringKey.JourneyIdReturn, journeyReturn.JourneyId.ToString());
                }

                retailHandoffUrl = urlHelper.AddQueryStringParts(url,nvc);
            }

            return retailHandoffUrl;
        }

        /// <summary>
        /// Overloaded. Builds the RetailHandoff SJP page url, adding the retailer and journeys to be used
        /// </summary>
        /// <returns></returns>
        public string BuildRetailerHandoffURL(string url, Retailer retailer, string journeyRequestHash, int journeyId, bool isReturn)
        {
            string retailHandoffUrl = string.Empty;

            if (!string.IsNullOrEmpty(url))
            {
                URLHelper urlHelper = new URLHelper();

                NameValueCollection nvc = new NameValueCollection();

                if (retailer != null)
                {
                    nvc.Add(QueryStringKey.Retailer, retailer.Id);
                }

                if (!string.IsNullOrEmpty(journeyRequestHash))
                {
                    nvc.Add(QueryStringKey.JourneyRequestHash, journeyRequestHash);
                }

                if (isReturn)
                {
                    nvc.Add(QueryStringKey.JourneyIdReturn, journeyId.ToString());
                }
                else
                {
                    nvc.Add(QueryStringKey.JourneyIdOutward, journeyId.ToString());
                }

               

                retailHandoffUrl = urlHelper.AddQueryStringParts(url, nvc);
            }

            return retailHandoffUrl;
        }

        /// <summary>
        /// Builds SJP retailer page url with supplied journey request hash and ourward/return journey id
        /// </summary>
        /// <param name="retailerHandoffUrl"></param>
        /// <param name="journeyRequestHash"></param>
        /// <param name="journeyId"></param>
        /// <param name="isReturn"></param>
        /// <returns></returns>
        public string BuildRetailersURL(string retailersUrl, string journeyRequestHash, int journeyId, bool isReturn)
        {
            URLHelper urlHelper = new URLHelper();
            NameValueCollection queryValues = new NameValueCollection();

            if (!string.IsNullOrEmpty(retailersUrl))
            {
                queryValues.Add(QueryStringKey.JourneyRequestHash, journeyRequestHash);
                if (isReturn)
                {
                    queryValues.Add(QueryStringKey.JourneyIdReturn, journeyId.ToString());
                }
                else
                {
                    queryValues.Add(QueryStringKey.JourneyIdOutward, journeyId.ToString());
                }

                return urlHelper.AddQueryStringParts(retailersUrl, queryValues);
            }

            return string.Empty;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Checks the journey for any retailers which could ticket it, by looping through all the legs 
        /// (and their details) for any operator/mode combinations which have a retailer. 
        /// Updates the referenced retailers list
        /// </summary>
        private void GetRetailers(Journey journey, ref List<Retailer> retailers)
        {
            List<int> affectedJourneyLegIndexes = new List<int>();

            // Specific scenarios:
            // This will populate the retailers and identify the legs where the "combined coach and rail" retailer applies
            GetCombinedCoachRailRetailers(journey.JourneyLegs, ref retailers, ref affectedJourneyLegIndexes);
                        
            // Check each leg of the journey
            for (int i = 0; i < journey.JourneyLegs.Count; i++)
            {
                // Allowed to check this leg?
                if (!affectedJourneyLegIndexes.Contains(i))
                {
                    JourneyLeg leg = journey.JourneyLegs[i];

                    GetRetailers(leg, ref retailers);
                }
            }
        }

        /// <summary>
        /// Checks the journey leg for any retailers which could ticket it, by looping through all the legs 
        /// details for any operator/mode combinations which have a retailer. 
        /// Updates the referenced retailers list
        /// </summary>
        private void GetRetailers(JourneyLeg journeyLeg, ref List<Retailer> retailers)
        {
            IList<Retailer> retailersFound = null;
            PublicJourneyDetail pjd = null;

            foreach (JourneyDetail detail in journeyLeg.JourneyDetails)
            {
                // Only interested in ticketing public journeys
                if (detail is PublicJourneyDetail)
                {
                    pjd = (PublicJourneyDetail)detail;

                    foreach (ServiceDetail service in pjd.Services)
                    {
                        // Get the retailers if they exist for Operator and Mode
                        retailersFound = retailerCatalogue.FindRetailers(service.OperatorCode, detail.Mode);

                        if (retailersFound != null)
                        {
                            UpdateRetailers(retailersFound, ref retailers);
                        }
                        else
                        {
                            // Get the retailers for Mode only
                            retailersFound = retailerCatalogue.FindRetailers(string.Empty, detail.Mode);

                            UpdateRetailers(retailersFound, ref retailers);
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Method detects the Combined Coach and Rail scenario, 
        /// returning the retailers to use and the list of journey leg indexes which are affected
        /// </summary>
        /// <param name="journeyLegs"></param>
        /// <param name="retailers"></param>
        /// <param name="journeyLegIndexes"></param>
        private void GetCombinedCoachRailRetailers(List<JourneyLeg> journeyLegs, ref List<Retailer> retailers, ref List<int> journeyLegIndexes)
        {
            // Tests for a Coach->(Walk)->Rail leg sequence (and for return scenario) where:
            //  - Coach Service is Games Transport Coach Service (Operator Code "5364"), and
            //  - Coach leg Starts/Ends at Ebbsfleet Coach station (Naptan "240010073"), and
            //  - Rail leg Ends/Starts at Ebbsfleet Rail station (Naptan "9100EBSFDOM"), and
            //  - Rail Service is South Eastern (Operator Code "RailSE"),
            // then this can be considered as a "Combined DMT Javelin" journey, and therefore these legs
            // are to be retailed by the Coach Service Retailer (for Operator Code "5364")

            if ((journeyLegs != null) && (journeyLegs.Count > 0))
            {
                string coachServiceOperatorCode = Properties.Current["Retail.CombinedCoachRail.CoachServiceOperatorCode"].Parse("5364");
                string railServiceOperatorCode = Properties.Current["Retail.CombinedCoachRail.RailServiceOperatorCode"].Parse("RailSE");
                string coachStationNaptan = Properties.Current["Retail.CombinedCoachRail.CoachStationNaptan"].Parse("2400100073");
                string railStationNaptan = Properties.Current["Retail.CombinedCoachRail.RailStationNaptan"].Parse("9100EBSFDOM");

                IList<Retailer> retailersFound = null;

                // Check the legs for the Coach->Rail scenario
                for (int i = 0; i < journeyLegs.Count; i++)
                {
                    JourneyLeg leg = journeyLegs[i];

                    if (leg.Mode == SJPModeType.Coach)
                    {
                        #region Find Combined Coach Rail scenarios

                        // Is the coach leg one we're interested in?
                        if (DoesJourneyLegContainServiceOperator(leg, coachServiceOperatorCode))
                        {
                            #region Coach->Rail

                            // Does the Coach End at appropriate station?
                            if (DoesJourneyLegStartEndAtNaptan(leg, coachStationNaptan, false))
                            {
                                // Find the next Rail leg if it exists, ignoring Walk leg
                                for (int j = i + 1; j < journeyLegs.Count; j++)
                                {
                                    JourneyLeg nextLeg = journeyLegs[j];

                                    if (nextLeg.Mode == SJPModeType.Rail)
                                    {
                                        // Coach->Rail scenario found

                                        // Is the rail leg one we're interested in?
                                        if (DoesJourneyLegContainServiceOperator(nextLeg, railServiceOperatorCode))
                                        {
                                            // Both coach and rail legs are for the identified services

                                            // Does the Rail Start at the appropriate station?
                                            if (DoesJourneyLegStartEndAtNaptan(nextLeg, railStationNaptan, true))
                                            {
                                                // Combined Coach/Rail scenario found! 
                                                // Return the retailers for the Coach, and flag the 
                                                // affected legs

                                                // Get the retailers for the coach service operator code.
                                                // Assume one exists, otherwise pointless doing this scenario check!
                                                retailersFound = retailerCatalogue.FindRetailers(coachServiceOperatorCode, SJPModeType.Coach);

                                                if (retailersFound != null)
                                                {
                                                    UpdateRetailers(retailersFound, ref retailers);
                                                }
                                                
                                                // Add leg indexes, to inform caller to not check these legs
                                                if (!journeyLegIndexes.Contains(i))
                                                    journeyLegIndexes.Add(i);
                                                if (!journeyLegIndexes.Contains(j))
                                                    journeyLegIndexes.Add(j);
                                            }
                                        }

                                    }
                                    else if (nextLeg.Mode == SJPModeType.Walk)
                                    {
                                        // Skip walk leg
                                        continue;
                                    }
                                    else
                                    {
                                        // Another mode detected, this is not a Coach->Rail scenario
                                        break;
                                    }
                                }
                            }

                            #endregion

                            #region Rail->Coach

                            // Does the Coach Start at appropriate station?
                            else if (DoesJourneyLegStartEndAtNaptan(leg, coachStationNaptan, true))
                            {
                                // Find the previous Rail leg if it exists, ignoring Walk leg
                                for (int j = i - 1; j >= 0; j--)
                                {
                                    JourneyLeg previousLeg = journeyLegs[j];

                                    if (previousLeg.Mode == SJPModeType.Rail)
                                    {
                                        // Rail->Coach scenario found

                                        // Is the rail leg one we're interested in?
                                        if (DoesJourneyLegContainServiceOperator(previousLeg, railServiceOperatorCode))
                                        {
                                            // Both coach and rail legs are for the identified services

                                            // Does the Rail End at the appropriate station?
                                            if (DoesJourneyLegStartEndAtNaptan(previousLeg, railStationNaptan, false))
                                            {
                                                // Combined Coach/Rail scenario found! 
                                                // Return the retailers for the Coach, and flag the 
                                                // affected legs

                                                // Get the retailers for the coach service operator code.
                                                // Assume one exists, otherwise pointless doing this scenario check!
                                                retailersFound = retailerCatalogue.FindRetailers(coachServiceOperatorCode, SJPModeType.Coach);

                                                if (retailersFound != null)
                                                {
                                                    UpdateRetailers(retailersFound, ref retailers);
                                                }

                                                // Add leg indexes, to inform caller to not check these legs
                                                if (!journeyLegIndexes.Contains(i))
                                                    journeyLegIndexes.Add(i);
                                                if (!journeyLegIndexes.Contains(j))
                                                    journeyLegIndexes.Add(j);
                                            }
                                        }

                                    }
                                    else if (previousLeg.Mode == SJPModeType.Walk)
                                    {
                                        // Skip walk leg
                                        continue;
                                    }
                                    else
                                    {
                                        // Another mode detected, this is not a Rail->Coach scenario
                                        break;
                                    }
                                }
                            }

                            #endregion
                        }

                        #endregion
                    } 
                    
                    // Check all legs in case there are multiple scenarios
                    continue;
                }
            }
        }

        /// <summary>
        /// Returns true if a Service exists in the leg for the provided operator code
        /// </summary>
        /// <param name="leg"></param>
        /// <param name="operatorCode"></param>
        /// <returns></returns>
        private bool DoesJourneyLegContainServiceOperator(JourneyLeg leg, string operatorCode)
        {
            if ((leg != null) && (!string.IsNullOrEmpty(operatorCode)))
            {
                PublicJourneyDetail pjd = null;

                foreach (JourneyDetail detail in leg.JourneyDetails)
                {
                    // Only interested in checking public journeys
                    if (detail is PublicJourneyDetail)
                    {
                        pjd = (PublicJourneyDetail)detail;

                        foreach (ServiceDetail service in pjd.Services)
                        {
                            if (service.OperatorCode.Equals(operatorCode))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
        
        /// <summary>
        /// Returns true if the Leg starts at or ends at the provided naptan 
        /// </summary>
        /// <param name="leg"></param>
        /// <param name="naptan"></param>
        /// <param name="atLegStart"></param>
        /// <returns></returns>
        private bool DoesJourneyLegStartEndAtNaptan(JourneyLeg leg, string naptan, bool atLegStart)
        {
            if (leg != null)
            {
                if (atLegStart && (leg.LegStart.Location.Naptan.Contains(naptan)))
                {
                    return true;
                }
                else if (!atLegStart && (leg.LegEnd.Location.Naptan.Contains(naptan)))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if the leg contains a PublicJourneyFrequencyDetail
        /// </summary>
        /// <param name="leg"></param>
        /// <returns></returns>
        private bool IsJourneyLegFrequencyDetail(JourneyLeg leg)
        {
            if (leg != null)
            {
                if (leg.JourneyDetails != null)
                {
                    // Public Jouney Detail will only have one "JourneyDetail" per leg
                    if (leg.JourneyDetails.Count > 0)
                    {
                        JourneyDetail detail = leg.JourneyDetails[0];

                        if (detail is PublicJourneyFrequencyDetail)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the start and end datetimes for a journey leg, 
        /// uses the previous or next legs to calculate when it's start/end datetime is 0
        /// </summary>
        /// <param name="journeyLegs"></param>
        /// <param name="index"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        private void GetJourneyLegDateTime(List<JourneyLeg> journeyLegs, int index, ref DateTime startTime, ref DateTime endTime)
        {
            JourneyLeg leg = journeyLegs[index];
            JourneyLeg legPrevious = null;
            JourneyLeg legNext = null;

            DateTime tempStartTime = leg.StartTime;
            DateTime tempEndTime = leg.EndTime;
            TimeSpan duration = new TimeSpan(0, 0, 0);

            // Place in try catch for safety.
            // Following journey leg looping shouldnt error as its been debug tested on various loop scenarios
            try
            {
                #region Start time

                if (leg.StartTime == DateTime.MinValue)
                {
                    // Loop backwards through the legs
                    for (int i = (index - 1); i >= 0; i--)
                    {
                        // Get previous leg
                        legPrevious = journeyLegs[i];

                        if (legPrevious.EndTime != DateTime.MinValue)
                        {
                            // Leg has end datetime, add the running duration total as we may 
                            // have looped multiple times 
                            tempStartTime = legPrevious.EndTime.Add(duration);
                            break;
                        }
                        else if (legPrevious.StartTime != DateTime.MinValue)
                        {
                            duration = duration.Add(legPrevious.Duration);

                            // Leg has start datetime, add the running duration total
                            tempStartTime = legPrevious.StartTime.Add(duration);
                            break;
                        }
                        else
                        {
                            // Leg has neither a start or end datetime, update runnding duration total
                            duration = duration.Add(legPrevious.Duration);
                        }
                    }
                }

                // Reset duration
                duration = new TimeSpan(0, 0, 0);

                #endregion

                #region End time

                if (leg.EndTime == DateTime.MinValue)
                {
                    // Loop forwards through the legs
                    for (int i = (index + 1); i < journeyLegs.Count; i++)
                    {
                        // Get next leg
                        legNext = journeyLegs[i];

                        if (legNext.StartTime != DateTime.MinValue)
                        {
                            // Leg has start datetime, subtract the running duration total as we may 
                            // have looped multiple times 
                            tempEndTime = legNext.StartTime.Subtract(duration);
                            break;
                        }
                        else if (legNext.EndTime != DateTime.MinValue)
                        {
                            duration = duration.Add(legNext.Duration);

                            // Leg has start datetime, add the running duration total
                            tempEndTime = legNext.EndTime.Subtract(duration);
                            break;
                        }
                        else
                        {
                            // Leg has neither a start or end datetime, update runnding duration total
                            duration = duration.Add(legNext.Duration);
                        }
                    }
                }

                #endregion

                // Return values
                startTime = tempStartTime;
                endTime = tempEndTime;
            }
            catch
            {
                // Ignore errors, will use the default datetime values
            }
        }

        /// <summary>
        /// Updates the referenced retailer list with retailers found (if the list doesnt 
        /// already contain retailer)
        /// </summary>
        /// <param name="retailersFound"></param>
        /// <param name="retailers"></param>
        private void UpdateRetailers(IList<Retailer> retailersFound, ref List<Retailer> retailers)
        {
            if (retailersFound != null)
            {
                foreach (Retailer retailer in retailersFound)
                {
                    // And add the retailers to the return object
                    if (!retailers.Contains(retailer))
                    {
                        retailers.Add(retailer);
                    }
                }
            }
        }

        #endregion

        
    }
}