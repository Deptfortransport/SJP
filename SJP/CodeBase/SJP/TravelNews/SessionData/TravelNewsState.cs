// *********************************************** 
// NAME             : TravelNewsState.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: TravelNewsState is used to hold data for the displayed travel news data.
// ************************************************
// 
                
using System;
using System.Collections;
using SJP.UserPortal.TravelNews.TravelNewsData;
using System.Collections.Generic;

namespace SJP.UserPortal.TravelNews.SessionData
{
    /// <summary>
    /// TravelNewsState is used to hold session data for the displayed data on
    /// TravelNews.aspx
    /// </summary>
    [Serializable()]
    public class TravelNewsState
    {
        #region Private members

        //private properties
        private TransportType ttSelectedTransport = TransportType.All;
        private string sSelectedRegion = TravelNewsRegion.All.ToString();
        private DelayType detSelectedDelays = DelayType.Major;
        private DisplayType ditSelectedDetails = DisplayType.Full;
        private string dataDateTime = string.Empty;
        private bool helpDisplayed = false;
        private DateTime selectedDateTime = DateTime.MinValue;
        private TravelNewsViewType selectedViewType;
        private SeverityFilter selectedSeverityFilter = SeverityFilter.Default;
        private string searchPhrase = string.Empty;
        private ArrayList searchTokens;
        private IncidentType selectedIncidentType = IncidentType.All;
        private SeverityLevel selectedSeverity = SeverityLevel.Critical;
        private bool selectedVenuesFlag = true;
        private bool selectedAllVenuesFlag = true;
        private List<string> searchNaptans = new List<string>();
        private string selectedIncidentActive = "All";
        
        #region Previous filter selection properties

        private TransportType ttLastSelectedTransport = TransportType.All;
        private string sLastSelectedRegion = "All";
        private DelayType detLastSelectedDelays = DelayType.Major;
        private DateTime lastSelectedDateTime = new DateTime();
        private string lastSearchPhrase = string.Empty;
        private string lastSelectedIncident = string.Empty;
        private IncidentType lastSelectedIncidentType = IncidentType.All;
        private TravelNewsViewType lastSelectedViewType;

        #endregion

        // to track when page language is changed to prevent view from being reset
        private string pageLanguage = string.Empty;
        private string selectedIncident = string.Empty;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TravelNewsState()
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Set default state
        /// </summary>
        public void SetDefaultState()
        {
            ttSelectedTransport = TransportType.All;
            sSelectedRegion = TravelNewsRegion.All.ToString();
            detSelectedDelays = DelayType.All;
            ditSelectedDetails = DisplayType.Full;
            dataDateTime = string.Empty;
            helpDisplayed = false;
            selectedDateTime = DateTime.Now;
            selectedViewType = TravelNewsViewType.Details;
            selectedSeverityFilter = SeverityFilter.Default;
            selectedSeverity = SeverityLevel.VerySlight;
            selectedVenuesFlag = true;
            selectedAllVenuesFlag = true;
            searchNaptans = new List<string>();
            selectedIncidentActive = "All";

            // Default values for variables storing last selected values
            ttLastSelectedTransport = TransportType.All;
            sLastSelectedRegion = "All";
            detLastSelectedDelays = DelayType.All;
            lastSelectedDateTime = DateTime.MinValue;
            lastSelectedViewType = TravelNewsViewType.Details;

            // Default values for variables storing last and current 
            // selected incident type
            selectedIncidentType = IncidentType.All;
            lastSelectedIncidentType = IncidentType.All;

           
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read/Write property SelectedTransport.Holds value 
        /// selected from transport drop down on ShowNewsControl.
        /// </summary>
        public TransportType SelectedTransport
        {
            get { return ttSelectedTransport; }
            set { ttSelectedTransport = value; }
        }

        /// <summary>
        /// Read/Write property SelectedTransport.Holds value 
        /// selected from details drop down on ShowNewsControl.
        /// </summary>
        public DisplayType SelectedDetails
        {
            get { return ditSelectedDetails; }
            set { ditSelectedDetails = value; }
        }

        /// <summary>
        /// Read/Write property SelectedTransport.Holds value 
        /// selected from delays drop down on ShowNewsControl.
        /// </summary>
        public DelayType SelectedDelays
        {
            get { return detSelectedDelays; }
            set { detSelectedDelays = value; }
        }

        /// <summary>
        /// Read/Write property SelectedTransport.Holds value 
        /// selected from region drop down on ShowNewsControl.
        /// </summary>
        public string SelectedRegion
        {
            get { return sSelectedRegion; }
            set { sSelectedRegion = value; }
        }

        /// <summary>
        /// Read Write property HelpDisplayed. Used when showing the Help control
        /// to get round the ForceRedirect that occurs, losing viewstate.
        /// </summary>
        public bool HelpDisplayed
        {
            get { return helpDisplayed; }
            set { helpDisplayed = value; }
        }

        /// <summary>
        /// Read Write property SelectedDate. Holds the DateTime value of the date
        /// that the user has selected to view travel news data for
        /// </summary>
        public DateTime SelectedDate
        {
            get { return selectedDateTime; }
            set { selectedDateTime = value; }
        }

        /// <summary>
        /// Read Write property SelectedView. Holds the ViewType value that specifies whether
        /// to display travel news incidents in a details table or a map view
        /// </summary>
        public TravelNewsViewType SelectedView
        {
            get { return selectedViewType; }
            set { selectedViewType = value; }
        }

        /// <summary>
        /// Read Write property SelectedSeverityFilter. Holds incident severity level 
        /// that the user has selected to view travel news data for
        /// </summary>
        public SeverityFilter SelectedSeverityFilter
        {
            get { return selectedSeverityFilter; }
            set { selectedSeverityFilter = value; }
        }
        
        /// <summary>
        /// Read/Write property. Holds selected severity level of the travel news story
        /// </summary>
        public SeverityLevel SelectedSeverity 
        {
            get { return selectedSeverity; }
            set { selectedSeverity = value; }
        }

        /// <summary>
        /// Read Write property TravelNewsPageLanguage. Holds the page language
        /// used to prevent default state being intialised when switching language
        /// </summary>
        public string PageLanguage
        {
            get { return pageLanguage; }
            set { pageLanguage = value; }
        }

        /// <summary>
        /// Read Write property SelectedIncident. Holds the incident selected.
        /// Used to allow zoom in to the incident if it was displayed when switching language
        /// </summary>
        public string SelectedIncident
        {
            get { return selectedIncident; }
            set { selectedIncident = value; }
        }

        /// <summary>
        /// Read Write Search phrase
        /// </summary>
        public string SearchPhrase
        {
            get { return searchPhrase; }
            set { searchPhrase = value; }
        }

        /// <summary>
        /// Read Write Search tokens
        /// </summary>
        public ArrayList SearchTokens
        {
            get { return searchTokens; }
            set { searchTokens = value; }
        }

        /// <summary>
        /// Read Write Search Naptans
        /// </summary>
        public List<string> SearchNaptans
        {
            get { return searchNaptans; }
            set { searchNaptans = value; }
        }

        /// <summary>
        /// Read/Write. Indicates if venues is selected
        /// </summary>
        public bool SelectedVenuesFlag
        {
            get { return selectedVenuesFlag; }
            set { selectedVenuesFlag = value; }
        }

        /// <summary>
        /// Read/Write. Indicates if all venues is selected
        /// </summary>
        public bool SelectedAllVenuesFlag
        {
            get { return selectedAllVenuesFlag; }
            set { selectedAllVenuesFlag = value; }
        }

        /// <summary>
        /// Read/Write. Incident active status, All, Active or Inactive
        /// </summary>
        public string SelectedIncidentActive
        {
            get { return selectedIncidentActive; }
            set { selectedIncidentActive = value; }
        }

        #region Previous values for the TravelNewsState filter

        /// <summary>
        /// Read/Write property LastSelectedTransport.Holds last value 
        /// selected from transport drop down on ShowNewsControl.
        /// </summary>
        public TransportType LastSelectedTransport
        {
            get { return ttLastSelectedTransport; }
            set { ttLastSelectedTransport = value; }
        }

        /// <summary>
        /// Read Write property LastSelectedDate. Holds the DateTime value of the date
        /// that the user has selected previously to view travel news data for
        /// </summary>
        public DateTime LastSelectedDate
        {
            get { return lastSelectedDateTime; }
            set { lastSelectedDateTime = value; }
        }

        /// <summary>
        /// Read/Write property LastSelectedTransport.Holds previous value 
        /// selected from region drop down on ShowNewsControl.
        /// </summary>
        public string LastSelectedRegion
        {
            get { return sLastSelectedRegion; }
            set { sLastSelectedRegion = value; }
        }

        /// <summary>
        /// Read/Write property LastSelectedTransport.Holds previous value 
        /// selected from delays drop down on ShowNewsControl.
        /// </summary>
        public DelayType LastSelectedDelays
        {
            get { return detLastSelectedDelays; }
            set { detLastSelectedDelays = value; }
        }

        /// <summary>
        /// Read/Write property LastSarchPhrase.Holds previous value 
        /// selected from search phrase on ShowNewsControl.
        /// </summary>
        public string LastSearchPhrase
        {
            get { return lastSearchPhrase; }
            set { lastSearchPhrase = value; }
        }

        /// <summary>
        /// Read Write property LastSelectedIncident. Holds the incident selected.
        /// previously Used to allow zoom in to the incident if it was displayed when switching language
        /// </summary>
        public string LastSelectedIncident
        {
            get { return lastSelectedIncident; }
            set { lastSelectedIncident = value; }
        }

        /// <summary>
        /// Read Write property SelectedIncidentType. Holds the IncidentType value
        /// selected from Incident Type dropdown on ShowNewsControl
        /// </summary>
        public IncidentType SelectedIncidentType
        {
            get { return selectedIncidentType; }
            set { selectedIncidentType = value; }
        }

        /// <summary>
        /// Read Write property LastSelectedIncidentType. Holds the previous IncidentType value
        /// selected from Incident Type dropdown on ShowNewsControl
        /// </summary>
        public IncidentType LastSelectedIncidentType
        {
            get { return lastSelectedIncidentType; }
            set { lastSelectedIncidentType = value; }
        }

        /// <summary>
        /// Read Write property LastSelectedView. Holds the previous ViewType value that specifies whether
        /// to display travel news incidents in a details table or a map view
        /// </summary>
        public TravelNewsViewType LastSelectedView
        {
            get { return lastSelectedViewType; }
            set { lastSelectedViewType = value; }
        }

        #endregion

        #endregion

       
    }
}
