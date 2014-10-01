// *********************************************** 
// NAME             : SJPVenueLocation.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 19 Apr 2011
// DESCRIPTION  	: Represents the SJP Olympic venue location
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Text;

namespace SJP.Common.LocationService
{
    /// <summary>
    ///  Represents the SJP Olympic venue location
    /// </summary>
    [Serializable]
    public class SJPVenueLocation : SJPLocation
    {
        #region Private Fields

        private int cycleToVenueDistance = 0;
        private string venueMapUrl = string.Empty;
        private string venueWalkingRoutesUrl = string.Empty;
        private string venueTravelNewsRegion = string.Empty;
        private RiverServiceAvailableType venueRiverServiceAvailable = RiverServiceAvailableType.No;
        
        // Used for grouping venues in select dropdown
        private string venueGroupID = string.Empty;
        private string venueGroupName = string.Empty;

        #region User changeable values

        // Venue naptans to use for an accessible journey (can be null)
        private List<string> accessibleNaptans = new List<string>();

        // Journey planning related properties - only set when building journey request and plan journey
        private string selectedParkID = string.Empty;
        private SJPVenueJourneyMode venueJourneyMode = SJPVenueJourneyMode.PublicTransport;

        // Journey planning related properties - Date time to use in preference to those in the journey request
        private DateTime selectedOutwardDateTime = DateTime.MinValue;
        private DateTime selectedReturnDateTime = DateTime.MinValue;

        // Journey planning related properties - River Services Pier Naptan to use
        private string selectedPierNaptan = string.Empty;
        private string selectedName = string.Empty;
        private OSGridReference selectedGridReference = new OSGridReference();

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for SJPVenueLocation that populates all member variables with default data.
        /// </summary>
        /// <returns>SJPVenueLocation containing default data</returns>
        public SJPVenueLocation()
            :base()
        {
        }

        /// <summary>
        /// SJPVenueLocation Constructor
        /// </summary>
        /// <param name="locationName"></param>
        /// <param name="locationDisplayName"></param>
        /// <param name="locationLocality"></param>
        /// <param name="toids"></param>
        /// <param name="naptans"></param>
        /// <param name="locationParent"></param>
        /// <param name="locationType"></param>
        /// <param name="gridReference"></param>
        /// <param name="cycleGridReference"></param>
        /// <param name="isGNATStation"></param>
        /// <param name="useNaptan"></param>
        /// <param name="sourceID"></param>
         public SJPVenueLocation(string locationName, string locationDisplayName, string locationLocality,
                            List<string> toids, List<string> naptans, string locationParent, SJPLocationType locationType,
                            OSGridReference gridReference, OSGridReference cycleGridReference, 
                            bool isGNATStation, bool useNaptan, 
                            int adminAreaCode, int districtCode,
                            string sourceID)
             :base(locationName,locationDisplayName,locationLocality,toids,naptans,locationParent,locationType,locationType,
                gridReference, cycleGridReference,isGNATStation, useNaptan, adminAreaCode, districtCode, sourceID)
        {
        }

        #endregion

        #region Public Methods

         /// <summary>
         /// Returns a hash code of this SJPVenueLocation. 
         /// </summary>
         /// <returns></returns>
         public override int GetSJPHashCode()
         {
             // Does not use native GetHashCode as this can return different hash codes 
             // for instances of the "same" object.

             int hashCode = base.GetSJPHashCode();
                          
             // Venue locations specific values
             hashCode = hashCode ^ selectedParkID.GetHashCode() 
                 ^ selectedOutwardDateTime.GetHashCode() ^ selectedReturnDateTime.GetHashCode()
                 ^ selectedPierNaptan.GetHashCode() ^ selectedName.GetHashCode();

             // Venue accessible naptan values
             if (accessibleNaptans != null)
             {
                 // list object returns different instance hashcodes, so manually add
                 foreach (string naptan in accessibleNaptans)
                 {
                     hashCode = hashCode ^ naptan.GetHashCode();
                 }
             }

             return hashCode;
         }

         #endregion

        #region Public Properties
       
       /// <summary>
       /// The maximum distance in km for cycle journeys to this venue
       /// </summary>
        public int CycleToVenueDistance
        {
            get { return cycleToVenueDistance; }
            set { cycleToVenueDistance = value; }
        }

        /// <summary>
        /// A URL to the location of a map of the venue
        /// </summary>
        public string VenueMapUrl
        {
            get { return venueMapUrl; }
            set { venueMapUrl = value; }
        }

        /// <summary>
        /// A URL to the location of a map of walking routes to the venue
        /// </summary>
        public string VenueWalkingRoutesUrl
        {
            get { return venueWalkingRoutesUrl; }
            set { venueWalkingRoutesUrl = value; }
        }
        
        /// <summary>
        /// Represents the venue travel news region
        /// </summary>
        public string VenueTravelNewsRegion
        {
            get { return venueTravelNewsRegion;}
            set { venueTravelNewsRegion = value; }
        }

        /// <summary>
        /// Represents the type of river service availability for this venue
        /// </summary>
        public RiverServiceAvailableType VenueRiverServiceAvailable
        {
            get { return venueRiverServiceAvailable; }
            set { venueRiverServiceAvailable = value; }
        }

        /// <summary>
        /// The venue group id
        /// </summary>
        public string VenueGroupID
        {
            get { return venueGroupID; }
            set { venueGroupID = value; }
        }

        /// <summary>
        /// The venue group name
        /// </summary>
        public string VenueGroupName
        {
            get { return venueGroupName; }
            set { venueGroupName = value; }
        }

        #region User changeable properties

        /// <summary>
        /// Naptans to use for an accessible journey. If this value is Null or contains no items, then 
        /// the location Naptans should be used
        /// </summary>
        public List<string> AccessibleNaptans
        {
            get { return accessibleNaptans; }
            set { accessibleNaptans = value; }
        }

        /// <summary>
        /// Represents the selected car or cycle park ID from/to which journey planning needed
        /// </summary>
        public string SelectedSJPParkID
        {
            get { return selectedParkID; }
            set { selectedParkID = value; }
        }

        /// <summary>
        /// Determines type of journey planned with the Venue i.e from/to cyclepark/carpark
        /// The value determines which coordinates/toids to be used to plan a journey
        /// </summary>
        public SJPVenueJourneyMode VenueJourneyMode
        {
            get { return venueJourneyMode; }
            set { venueJourneyMode = value; }
        }

        /// <summary>
        /// Indicates the Date Time to use when this Venue location is used in the journey planning, overriding 
        /// the journey request Outward Date Time
        /// e.g. The Arrive At car park date time location
        /// </summary>
        public DateTime SelectedOutwardDateTime
        {
            get { return selectedOutwardDateTime; }
            set { selectedOutwardDateTime = value; }
        }

        /// <summary>
        /// Indicates the Date Time to use when this Venue location is used in the journey planning, overriding 
        /// the journey request Return Date Time
        /// e.g. The Leave By car park date time location
        /// </summary>
        public DateTime SelectedReturnDateTime
        {
            get { return selectedReturnDateTime; }
            set { selectedReturnDateTime = value; }
        }

        /// <summary>
        /// Represents the selected venue pier NaPTAN for journey planning with river services
        /// </summary>
        public string SelectedPierNaptan 
        {
            get { return selectedPierNaptan; }
            set { selectedPierNaptan = value; } 
        }

        /// <summary>
        /// The name to use in for journey planning (in place of the SJPLocation name)
        /// </summary>
        public string SelectedName
        {
            get { return selectedName; }
            set { selectedName = value; }
        }

        /// <summary>
        /// The grid reference to use in for journey planning (in place of the SJPLocation grid reference)
        /// </summary>
        public OSGridReference SelectedGridReference
        {
            get { return selectedGridReference; }
            set { selectedGridReference = value; }
        }

        #endregion

        #endregion

        #region Protected methods override

        /// <summary>
        /// Deep copies this SJPVenueLocation location object's values into a new SJPVenueLocation
        /// </summary>
        /// <param name="target">SJPLocation target is ignored</param>
        /// <returns>new SJPVenueLocation object copied into</returns>
        protected override SJPLocation CopyTo(SJPLocation target)
        {
            SJPVenueLocation sjpVenueLocation = (SJPVenueLocation)base.CopyTo(new SJPVenueLocation());

            // Only copy the non-changeable venue location values
            sjpVenueLocation.cycleToVenueDistance = this.cycleToVenueDistance;
            sjpVenueLocation.venueMapUrl = this.venueMapUrl;
            sjpVenueLocation.venueWalkingRoutesUrl = this.venueWalkingRoutesUrl;
            sjpVenueLocation.venueTravelNewsRegion = this.venueTravelNewsRegion;
            sjpVenueLocation.venueRiverServiceAvailable = this.venueRiverServiceAvailable;
            sjpVenueLocation.venueGroupID = this.venueGroupID;
            sjpVenueLocation.venueGroupName = this.venueGroupName;

            return sjpVenueLocation;
        }

        /// <summary>
        /// Returns a formatted string representing the contents of this SJPVenueLocation
        /// </summary>
        /// <returns></returns>
        public override string ToString(bool htmlLineBreaks)
        {
            string linebreak = (htmlLineBreaks) ? "<br />" : "\r\n";

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(base.ToString(htmlLineBreaks));

            sb.AppendLine(string.Format("cycleToVenueDistance: {0} {1}", cycleToVenueDistance.ToString(), linebreak));
            sb.AppendLine(string.Format("venueMapUrl: {0} {1}", venueMapUrl.ToString(), linebreak));
            sb.AppendLine(string.Format("venueWalkingRoutesUrl: {0} {1}", venueWalkingRoutesUrl.ToString(), linebreak));
            sb.AppendLine(string.Format("venueTravelNewsRegion: {0} {1}", venueTravelNewsRegion.ToString(), linebreak));
            sb.AppendLine(string.Format("venueRiverServiceAvailable: {0} {1}", venueRiverServiceAvailable.ToString(), linebreak));
            sb.AppendLine(string.Format("venueGroupID: {0} {1}", venueGroupID.ToString(), linebreak));
            sb.AppendLine(string.Format("venueGroupName: {0} {1}", venueGroupName.ToString(), linebreak));
            sb.Append("accessibleNaptans: ");
            foreach (string accessibleNaptan in accessibleNaptans)
            {
                sb.Append(accessibleNaptan);
                sb.Append(" ");
            }
            sb.AppendLine(linebreak);
            sb.AppendLine(string.Format("selectedParkID: {0} {1}", selectedParkID.ToString(), linebreak));
            sb.AppendLine(string.Format("venueJourneyMode: {0} {1}", venueJourneyMode.ToString(), linebreak));
            sb.AppendLine(string.Format("selectedOutwardDateTime: {0} {1}", selectedOutwardDateTime.ToString(), linebreak));
            sb.AppendLine(string.Format("selectedReturnDateTime: {0} {1}", selectedReturnDateTime.ToString(), linebreak));
            sb.AppendLine(string.Format("selectedPierNaptan: {0} {1}", selectedPierNaptan.ToString(), linebreak));
            sb.AppendLine(string.Format("selectedName: {0} {1}", selectedName.ToString(), linebreak));
            sb.AppendLine(string.Format("selectedGridReference: {0},{1} {2}", selectedGridReference.Easting, selectedGridReference.Northing, linebreak));
            
            return sb.ToString();
        }

        #endregion
    }
}
