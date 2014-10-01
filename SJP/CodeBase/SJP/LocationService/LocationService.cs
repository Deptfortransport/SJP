// *********************************************** 
// NAME             : LocationService.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 Apr 2011
// DESCRIPTION  	: LocationService class to cache and return locations
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;

namespace SJP.Common.LocationService
{
    /// <summary>
    /// LocationService class to cache and return locations
    /// </summary>
    public class LocationService
    {
        #region Private members

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public LocationService()
        {
            LoadData();
        }

        #endregion

        #region Public methods

        #region SJPLocationCache

        /// <summary>
        /// Searches for an SJPLocation by matching the supplied searchString.
        /// If location type is Venue, it restricts the search to Venues.
        /// </summary>
        /// <returns>SJPLocation or null if no match found</returns>
        public SJPLocation GetSJPLocation(string searchString, SJPLocationType locType)
        {
            SJPLocation location = null;
            switch (locType)
            {
                case SJPLocationType.Venue:
                    location = SJPVenueLocationCache.GetVenueLocation(searchString);
                    break;
                case SJPLocationType.Station:
                    location = SJPLocationCache.GetNaptanLocation(searchString);
                    break;
                case SJPLocationType.StationGroup:
                    location = SJPLocationCache.GetGroupLocation(searchString);
                    break;
                case SJPLocationType.Locality:
                    location = SJPLocationCache.GetLocalityLocation(searchString);
                    break;
                case SJPLocationType.Postcode:
                    location = SJPLocationCache.GetPostcodeLocation(searchString);
                    break;
                case SJPLocationType.Unknown:
                    location = SJPLocationCache.GetUnknownLocation(searchString);
                    break;
            }
            return location;
        }

        /// <summary>
        /// Returns the complete list of Olympic Venues as a List of SJP Locations containing Displayable Name, Type and NaPTAN.
        /// </summary>
        /// <returns>Dictionary<Name,NaPTAN></returns>
        public List<SJPLocation> GetAlternateLocations(string searchString)
        {
            List<SJPLocation> alternateLocations = SJPLocationCache.GetAlternativeSJPLocations(searchString);
            return alternateLocations;
        }

        /// <summary>
        /// Returns an SJPLocation for the supplied name and grid reference.
        /// The location is built by finding the closest Locality SJPLocation to the coordinate, and using that
        /// as the basis for the Coordinate SJPLocation
        /// </summary>
        /// <param name="gridRef"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public SJPLocation GetSJPLocationForCoordinate(string name, OSGridReference gridRef)
        {
            SJPLocation location = null;

            if (gridRef != null && gridRef.IsValid)
            {
                // Find the locality location (coordinate location must have a locality for passing into the journey planner)
                SJPLocation localityLocation = SJPLocationCache.GetLocalityLocationForCoordinate(gridRef);

                if (localityLocation != null)
                {
                    bool appendLocalityName = Properties.Current[Keys.CoordinateLocation_AppendLocalityName].Parse(false);

                    // Set name/displayname 
                    // (if missing use the locality location, otherwise append locality to name provided)
                    string displayName = string.Empty;

                    if (string.IsNullOrEmpty(name))
                    {
                        // No name supplied, used the locality name
                        name = localityLocation.DisplayName;
                        displayName = localityLocation.DisplayName;
                    }
                    else if (appendLocalityName &&  (!name.Contains(localityLocation.Name)))
                    {
                        // Append the locality name (if flag set)
                        displayName = string.Format("{0} ({1})", name, localityLocation.Name);
                    }
                    else
                    {
                        // Name supplied (and/or locality name already appended to it), use what was supplied
                        displayName = name;
                        name = displayName.SubstringFirst('(').Trim();
                    }

                    // Create location using the locality properties where needed
                    location = new SJPLocation(
                        name,
                        displayName,
                        localityLocation.Locality,
                        localityLocation.Toid,
                        localityLocation.Naptan,
                        localityLocation.Parent,
                        SJPLocationType.CoordinateEN,
                        SJPLocationType.CoordinateEN,
                        gridRef,
                        gridRef,
                        false,
                        false,
                        localityLocation.AdminAreaCode,
                        localityLocation.DistrictCode,
                        string.Empty);
                }
            }

            return location;
        }

        #endregion

        #region SJPVenueLocationCache

        /// <summary>
        /// Returns the complete list of Olympic Venues as a List of SJP Locations containing Displayable Name, Type and NaPTAN.
        /// </summary>
        /// <returns>Dictionary<Name,NaPTAN></returns>
        public List<SJPLocation> GetSJPVenueLocations()
        {
            List<SJPLocation> venueLocations = SJPVenueLocationCache.GetVenuesLocations();
            return venueLocations;
        }

        /// <summary>
        /// Returns the SJP Cycle Park for the cycle park Id
        /// </summary>
        /// <param name="cycleParkId">Cycle park Id</param>
        /// <returns></returns>
        public SJPVenueCyclePark GetSJPVenueCyclePark(string cycleParkId)
        {
            return SJPVenueLocationCache.GetSJPVenueCyclePark(cycleParkId);
        }

        /// <summary>
        /// Returns all the SJP Cycle Parks associated with the specified SJP venue
        /// </summary>
        /// <param name="venueNaPTAN">SJP venue NaPTAN</param>
        /// <returns></returns>
        public List<SJPVenueCyclePark> GetSJPVenueCycleParks(List<string> venueNaPTANs)
        {
            return SJPVenueLocationCache.GetVenueCycleParks(venueNaPTANs);
        }

        /// <summary>
        /// Overloaded. Returns all the SJP Cycle Parks associated with the specified SJP venue
        /// </summary>
        /// <param name="venueNaPTANs"></param>
        /// <param name="outwardDate"></param>
        /// <param name="returnDate"></param>
        /// <returns></returns>
        public List<SJPVenueCyclePark> GetSJPVenueCycleParks(List<string> venueNaPTANs, DateTime outwardDate, DateTime returnDate)
        {
            List<SJPVenueCyclePark> cycleParks = GetSJPVenueCycleParks(venueNaPTANs);
            
            return FilterCycleParks(cycleParks, outwardDate, returnDate);
        }

        /// <summary>
        /// Returns the SJP Car Park for the car park Id
        /// </summary>
        /// <param name="carParkId">Car park Id</param>
        /// <returns></returns>
        public SJPVenueCarPark GetSJPVenueCarPark(string carParkId)
        {
            return SJPVenueLocationCache.GetSJPVenueCarPark(carParkId);
        }

        /// <summary>
        /// Returns all the SJP Car Parks associated with the specified SJP venue
        /// </summary>
        /// <param name="venueNaPTAN">SJP venue NaPTAN(s)</param>
        /// <returns></returns>
        public List<SJPVenueCarPark> GetSJPVenueCarParks(List<string> venueNaPTANs)
        {
            List<SJPVenueCarPark> carParks = SJPVenueLocationCache.GetVenueCarParks(venueNaPTANs);

            if (carParks != null)
            {
                // Keep only car parks which have spaces
                carParks = carParks.Where(acp => acp.CarSpaces > 0).ToList();
            }

            return carParks;
        }
        
        /// <summary>
        /// Returns all the SJP Car Parks associated with the specified SJP venue valid for the dates provided
        /// </summary>
        /// <param name="venueNaPTAN">SJP venue NaPTAN</param>
        /// <returns></returns>
        public List<SJPVenueCarPark> GetSJPVenueCarParks(List<string> venueNaPTANs, DateTime outwardDate, DateTime returnDate)
        {
            // Get all car parks for naptans
            List<SJPVenueCarPark> carParks = GetSJPVenueCarParks(venueNaPTANs);

            // Remove any car parks not valid on dates provided and which don't have any spaces
            return FilterCarParks(carParks, outwardDate, returnDate);
        }

        /// <summary>
        /// Returns all the SJP Blue Badge Car Parks associated with the specified SJP venue
        /// </summary>
        /// <param name="venueNaPTAN">SJP venue NaPTAN</param>
        /// <returns></returns>
        public List<SJPVenueCarPark> GetSJPVenueBlueBadgeCarParks(List<string> venueNaPTANs)
        {
            List<SJPVenueCarPark> carParks = SJPVenueLocationCache.GetVenueCarParks(venueNaPTANs);

            if (carParks != null)
            {
                // Keep only car parks which have BlueBadge or Disabled spaces
                carParks = carParks.Where(acp => (acp.BlueBadgeSpaces + acp.DisabledSpaces) > 0).ToList();
            }

            return carParks;
        }

        /// <summary>
        /// Returns all the SJP Blue Badge Car Parks associated with the specified SJP venue valid for the dates provided
        /// </summary>
        /// <param name="venueNaPTAN">SJP venue NaPTAN</param>
        /// <returns></returns>
        public List<SJPVenueCarPark> GetSJPVenueBlueBadgeCarParks(List<string> venueNaPTANs, DateTime outwardDate, DateTime returnDate)
        {
            // Get all car parks for naptans
            List<SJPVenueCarPark> carParks = GetSJPVenueBlueBadgeCarParks(venueNaPTANs);

            if (carParks != null)
            {
                // Remove any car parks not valid on dates provided
                carParks = FilterCarParks(carParks, outwardDate, returnDate);
            }

            return carParks;
        }

        /// <summary>
        /// Returns all the river services associated with the specified SJP venue
        /// </summary>
        /// <param name="venueNaPTANs"></param>
        /// <returns></returns>
        public List<SJPVenueRiverService> GetSJPVenueRiverServices(List<string> venueNaPTANs)
        {
            // Get all river services for naptans
            List<SJPVenueRiverService> riverServices = SJPVenueLocationCache.GetVenueRiverServices(venueNaPTANs.FirstOrDefault());
            
            return riverServices;
        }

        /// <summary>
        /// Returns all the pier navigation paths associated with the specified SJP venue
        /// </summary>
        /// <param name="venueNaPTANs"></param>
        /// <returns></returns>
        public List<SJPPierVenueNavigationPath> GetSJPVenuePierNavigationPaths(List<string> venueNaPTANs)
        {
            // Get all pier navigation paths for naptans
            List<SJPPierVenueNavigationPath> pierNavigationPaths = SJPVenueLocationCache.GetVenuePierNavigationPaths(venueNaPTANs.FirstOrDefault());

            return pierNavigationPaths;
        }

        /// <summary>
        /// Returns a pier navigation path associated with the specified SJP venue and venue pier
        /// </summary>
        /// <param name="venueNaPTANs"></param>
        /// <returns></returns>
        public SJPPierVenueNavigationPath GetSJPVenuePierNavigationPaths(List<string> venueNaPTANs, string venuePierNaPTAN, bool isToVenue)
        {
            SJPPierVenueNavigationPath navigationPath = null;

            if ((venueNaPTANs != null) && (!string.IsNullOrEmpty(venuePierNaPTAN)))
            {
                // Get the navigation paths for venue
                List<SJPPierVenueNavigationPath> navigationPaths = GetSJPVenuePierNavigationPaths(venueNaPTANs);

                if (navigationPaths != null)
                {
                    // Find the required navigation path,
                    // check by looking at the path From or To value
                    foreach (SJPPierVenueNavigationPath np in navigationPaths)
                    {
                        // Going to the venue, find the path from the venue's pier
                        if (isToVenue
                            && np.FromNaPTAN.Equals(venuePierNaPTAN))
                        {
                            navigationPath = np;
                            break;
                        }
                        // Coming from the venue, find the path to the venue's pier
                        else if (!isToVenue
                            && np.ToNaPTAN.Equals(venuePierNaPTAN))
                        {
                            navigationPath = np;
                            break;
                        }
                    }
                }
            }

            return navigationPath;
        }

        /// <summary>
        /// Returns the SJP Venue Gate for the supplied venue gate NaPTAN ID
        /// </summary>
        /// <param name="venueGateNaPTAN">Venue Gate NaPTAN</param>
        /// <returns></returns>
        public SJPVenueGate GetSJPVenueGate(string venueGateNaPTAN)
        {
            return SJPVenueLocationCache.GetSJPVenueGate(venueGateNaPTAN);
        }

        /// <summary>
        /// Returns all the Check Constraints associated with the specified SJP venue gate
        /// </summary>
        /// <param name="venueGateNaPTANs">SJP venue gate NaPTANs</param>
        /// <returns></returns>
        public List<SJPVenueGateCheckConstraint> GetSJPVenueGateCheckConstraints(List<string> venueGateNaPTANs)
        {
            List<SJPVenueGateCheckConstraint> checkConstraints = SJPVenueLocationCache.GetVenueGateCheckConstraints(venueGateNaPTANs.FirstOrDefault());
            return checkConstraints;
        }

        /// <summary>
        /// Returns a Check Constraint associated with the specified SJP venue gate
        /// </summary>
        /// <param name="venueGate">SJP venue gate</param>
        /// <param name="isVenueEntry">Is check constraint for entry or exit of venue</param>
        /// <returns></returns>
        public SJPVenueGateCheckConstraint GetSJPVenueGateCheckConstraints(SJPVenueGate venueGate, bool isVenueEntry)
        {
            SJPVenueGateCheckConstraint venueGateCheckConstraint = null;

            if (venueGate != null)
            {
                List<string> gateNaptans = new List<string>();
                gateNaptans.Add(venueGate.GateNaPTAN);

                List<SJPVenueGateCheckConstraint> vgccs = GetSJPVenueGateCheckConstraints(gateNaptans);

                // Return the correct gate check constraint
                if (vgccs != null)
                {
                    foreach (SJPVenueGateCheckConstraint vgcc in vgccs)
                    {
                        // Assumes there is only one "IsEntry" check constraint for a gate
                        if (vgcc.IsEntry == isVenueEntry)
                        {
                            venueGateCheckConstraint = vgcc;
                            break;
                        }
                    }
                }
            }

            return venueGateCheckConstraint;
        }
                
        /// <summary>
        /// Returns all the Navigation Paths associated with the specified SJP venue gate
        /// </summary>
        /// <param name="venueGateNaPTANs">SJP venue gate NaPTANs</param>
        /// <returns></returns>
        public List<SJPVenueGateNavigationPath> GetSJPVenueGateNavigationPaths(List<string> venueGateNaPTANs)
        {
            List<SJPVenueGateNavigationPath> navigationPaths = SJPVenueLocationCache.GetVenueGateNavigationPaths(venueGateNaPTANs.FirstOrDefault());
            return navigationPaths;
        }

        /// <summary>
        /// Returns a Navigation Path associated with the specified SJP venue and venue gate
        /// </summary>
        /// <returns></returns>
        public SJPVenueGateNavigationPath GetSJPVenueGateNavigationPaths(SJPLocation venue, SJPVenueGate venueGate, bool isToVenue)
        {
            SJPVenueGateNavigationPath venueGateNavigationPath = null;

            if ((venue is SJPVenueLocation) && (venueGate != null))
            {
                SJPVenueLocation venueLocation = (SJPVenueLocation)venue;

                List<string> gateNaptans = new List<string>();
                gateNaptans.Add(venueGate.GateNaPTAN);

                List<SJPVenueGateNavigationPath> vgnps = GetSJPVenueGateNavigationPaths(gateNaptans);

                // Return the correct gate navigation path
                if (vgnps != null)
                {
                    string fromNaptan = isToVenue ? venueGate.GateNaPTAN : venueLocation.Naptan[0];
                    string toNaptan = isToVenue ? venueLocation.Naptan[0] : venueGate.GateNaPTAN;

                    foreach (SJPVenueGateNavigationPath vgnp in vgnps)
                    {
                        // Assumes there is only one navigation path for the from/to venue gate naptan
                        if ((vgnp.FromNaPTAN.Equals(fromNaptan)) &&
                            (vgnp.ToNaPTAN.Equals(toNaptan)))
                        {
                            venueGateNavigationPath = vgnp;
                            break;
                        }
                    }
                }
            }

            return venueGateNavigationPath;
        }

        /// <summary>
        /// Returns the list of SJPVenueAccess containing stations to use for the specified venue naptan and datetime
        /// </summary>
        public List<SJPVenueAccess> GetSJPVenueAccessData(List<string> venueNaPTANs, DateTime datetime)
        {
            List<SJPVenueAccess> venueAccessData = SJPVenueLocationCache.GetVenueAccessData(venueNaPTANs.FirstOrDefault());

            return FilterVenueAccessData(venueAccessData, datetime);
        }

        #endregion

        #region SJPGNATLocationCache

        /// <summary>
        /// Returns the complete list of GNAT stations as a List of SJPGNATLocations containing Displayable Name, Type, NaPTAN and accesibilitty attributes.
        /// </summary>
        /// <returns>Dictionary<Name,NaPTAN></returns>
        public List<SJPGNATLocation> GetGNATLocations()
        {
            List<SJPGNATLocation> venueLocations = SJPGNATLocationCache.GetGNATList();
            return venueLocations;
        }
        
        /// <summary>
        /// Checks whether a Naptan is usable with the accesibility options supplied
        /// </summary>
        /// <returns>bool<Name,NaPTAN></returns>
        public bool IsGNAT(string naptan, bool stepFree, bool assistanceRequired)
        {
            bool isValid = SJPGNATLocationCache.IsGNAT(naptan,stepFree,assistanceRequired);
            return isValid;
        }

        /// <summary>
        /// Checks whether the admin area and district code are found in the GNAT admin areas list 
        /// and has the required GNAT attributes
        /// </summary>
        /// <param name="adminAreaCode">Admin Area code</param>
        /// <param name="districtCode">District code</param>
        /// <param name="stepFreeAccess">Check step free access available in admin area/district</param>
        /// <param name="assistanceAvailable">Check assistance available in admin area/district</param>
        public bool IsGNATAdminArea(int adminAreaCode, int districtCode, bool stepFreeAccess, bool assistanceAvailable)
        {
            return SJPGNATLocationCache.IsGNATAdminArea(adminAreaCode, districtCode, stepFreeAccess, assistanceAvailable);
        }

        #endregion

        #endregion

        #region Private methods

        /// <summary>
        /// Method which loads the location data
        /// </summary>
        private void LoadData()
        {
            // Use static classes to load data
            SJPVenueLocationCache.LoadVenues();
            SJPGNATLocationCache.LoadGNATStations();
            SJPLocationCache.LoadLocations();
            SJPLocationCache.LoadPostcodes();
        }

        #region Car/Cycle park filtering

        /// <summary>
        /// Filters the SJPVenueCarParks to only retain those which are valid for the dates specified 
        /// and those which have spaces
        /// </summary>
        /// <param name="carParks"></param>
        /// <param name="outwardDate"></param>
        /// <param name="returnDate"></param>
        /// <returns></returns>
        private List<SJPVenueCarPark> FilterCarParks(List<SJPVenueCarPark> carParks, DateTime outwardDate, DateTime returnDate)
        {
            if (carParks != null)
            {
                List<SJPVenueCarPark> filteredCarParks = new List<SJPVenueCarPark>();

                bool checkOutwardDate = (outwardDate != DateTime.MinValue);
                bool checkReturnDate = (returnDate != DateTime.MinValue);

                // Temp flag for car park validity
                bool valid = false;

                foreach (SJPVenueCarPark carPark in carParks)
                {
                    #region Validate dates

                    if (carPark.Availability != null)
                    {
                        foreach (SJPParkAvailability availability in carPark.Availability)
                        {
                            // If there is at least one Availability where the requested date is ok, then car park is valid
                            List<DayOfWeek> daysValid = availability.GetDaysOfWeek();

                            valid = IsDateValid(daysValid, availability.FromDate, availability.ToDate, outwardDate, returnDate, checkOutwardDate, checkReturnDate);
                            
                            if (valid)
                                break;
                        }
                    }
                    else
                    {
                        // No avilability details, assume its valid
                        valid = true;
                    }

                    #endregion

                    #region Validate spaces

                    // Check for spaces
                    if ((carPark.CarSpaces 
                        + carPark.BlueBadgeSpaces 
                        + carPark.DisabledSpaces 
                        + carPark.CoachSpaces) <= 0)
                    {
                        valid = false;
                    }

                    #endregion

                    if (valid)
                    {
                        filteredCarParks.Add(carPark);
                    }

                    // Reset flag
                    valid = false;
                }

                if (filteredCarParks.Count > 0)
                {
                    return filteredCarParks;
                }
            }

            return null;
        }

        /// <summary>
        /// Filters the SJPVenueCyclePark to only retain those which are valid for the dates specified
        /// and those which have spaces
        /// </summary>
        /// <param name="cycleParks"></param>
        /// <param name="outwardDate"></param>
        /// <param name="returnDate"></param>
        /// <returns></returns>
        private List<SJPVenueCyclePark> FilterCycleParks(List<SJPVenueCyclePark> cycleParks, DateTime outwardDate, DateTime returnDate)
        {
            if (cycleParks != null)
            {
                List<SJPVenueCyclePark> filteredCycleParks = new List<SJPVenueCyclePark>();

                bool checkOutwardDate = (outwardDate != DateTime.MinValue);
                bool checkReturnDate = (returnDate != DateTime.MinValue);

                // Temp flag for park validity
                bool valid = false;

                foreach (SJPVenueCyclePark cyclePark in cycleParks)
                {
                    #region Validate dates

                    if (cyclePark.Availability != null)
                    {
                        foreach (SJPParkAvailability availability in cyclePark.Availability)
                        {
                            // If there is at least one Availability where the requested date is ok, then park is valid
                            List<DayOfWeek> daysValid = availability.GetDaysOfWeek();
                            
                            TimeSpan overnight = new TimeSpan(3, 0, 0);
                            bool isOvernight = false;

                            if (availability.DailyClosingTime < overnight)
                            {
                                // Journey uses a cycle park that is open into the next morning so subtract a day for 
                                // validation purposes if journey is in the early hours
                                isOvernight = true;
                                if (outwardDate != DateTime.MinValue)
                                {
                                    if (outwardDate.TimeOfDay < overnight)
                                    {
                                        outwardDate.Subtract(new TimeSpan(24, 0, 0));
                                    }
                                }
                                if (returnDate != DateTime.MinValue)
                                {
                                    if (returnDate.TimeOfDay < overnight)
                                    {
                                        returnDate.Subtract(new TimeSpan(24, 0, 0));
                                    }
                                }
                            }
                            
                            valid = IsDateValid(daysValid, availability.FromDate, availability.ToDate, outwardDate, returnDate, checkOutwardDate, checkReturnDate);
                            if (valid)
                            {
                                valid = IsTimeValid(availability.DailyOpeningTime, availability.DailyClosingTime, outwardDate, returnDate, checkOutwardDate, checkReturnDate, isOvernight);
                            }
                                                        
                            if (valid)
                                break;
                        }
                    }
                    else
                    {
                        // No avilability details, assume its valid
                        valid = true;
                    }

                    #endregion

                    #region Validate spaces

                    // Check for spaces
                    if (cyclePark.NumberOfSpaces <= 0)
                    {
                        valid = false;
                    }

                    #endregion

                    if (valid)
                    {
                        filteredCycleParks.Add(cyclePark);
                    }

                    // Reset flag
                    valid = false;
                }

                if (filteredCycleParks.Count > 0)
                {
                    return filteredCycleParks;
                }
            }

            return null;
        }

        /// <summary>
        /// Checks if the date(s) supplied are valid for From and To date range and DaysOfWeek list
        /// </summary>
        private bool IsDateValid(List<DayOfWeek> daysValid, DateTime fromDate, DateTime toDate,
            DateTime outwardDate, DateTime returnDate, bool checkOutwardDate, bool checkReturnDate)
        {
            bool valid = false;

            // Outward and return journey check
            if (checkOutwardDate && checkReturnDate &&
                fromDate.Date <= outwardDate.Date &&
                toDate.Date >= returnDate.Date)
            {
                if (daysValid.Contains(outwardDate.DayOfWeek) &&
                    daysValid.Contains(returnDate.DayOfWeek))
                {
                    valid = true;
                }
            }
            // Outward only journey check
            else if (checkOutwardDate && !checkReturnDate && fromDate.Date <= outwardDate.Date && toDate.Date >= outwardDate.Date)
            {
                if (daysValid.Contains(outwardDate.DayOfWeek))
                {
                    valid = true;
                }
            }
            // Return only journey check (shouldn't ever reach here, as you can't do return only journey!)
            else if (!checkOutwardDate && checkReturnDate && toDate.Date >= returnDate.Date && fromDate.Date <= returnDate.Date)
            {
                if (daysValid.Contains(returnDate.DayOfWeek))
                {
                    valid = true;
                }
            }
            else if (!checkOutwardDate && !checkReturnDate)
            {
                // Dates supplied are invalid, so assume park is valid
                valid = true;
            }

            return valid;
        }

        /// <summary>
        /// Checks if the time(s) supplied are valid against the opening and closing times
        /// </summary>
        private bool IsTimeValid(TimeSpan openingTime, TimeSpan closingTime, DateTime outwardDate, 
            DateTime returnDate, bool checkOutwardDate, bool checkReturnDate, bool overnight)
        {
            bool outwardOK = false;
            bool returnOK = false;
            TimeSpan overnightRollover = new TimeSpan(3, 0, 0);

            if (checkOutwardDate)
            {
                if ((openingTime <= outwardDate.TimeOfDay && closingTime >= outwardDate.TimeOfDay)
                    || (overnight && outwardDate.TimeOfDay <= overnightRollover)
                    || (overnight && outwardDate.TimeOfDay >= openingTime))
                {
                    outwardOK = true;
                }
            }
            else outwardOK = true;

            if (checkReturnDate)
            {
                if (openingTime <= returnDate.TimeOfDay && closingTime >= returnDate.TimeOfDay
                    || (overnight && returnDate.TimeOfDay <= overnightRollover)
                    || (overnight && returnDate.TimeOfDay >= openingTime))
                {
                    returnOK = true;
                }
            }
            else returnOK = true;

            if (outwardOK && returnOK) return true;
            else return false;
        }

        #endregion

        #region Venue Access filtering

        /// <summary>
        /// Filters the SJPVenueAccess to only retain those which are valid for the datetime specified 
        /// </summary>
        /// <returns></returns>
        private List<SJPVenueAccess> FilterVenueAccessData(List<SJPVenueAccess> venueAccessData, DateTime datetime)
        {
            if (venueAccessData != null)
            {
                List<SJPVenueAccess> filteredVenueAccess = new List<SJPVenueAccess>();

                bool checkOutwardDate = (datetime != DateTime.MinValue);

                // Temp flag for validity
                bool valid = false;

                foreach (SJPVenueAccess va in venueAccessData)
                {
                    #region Validate dates

                    if (va.AccessFrom <= datetime && va.AccessTo >= datetime)
                    {
                        valid = true;
                    }
                    
                    #endregion
                    
                    if (valid)
                    {
                        filteredVenueAccess.Add(va);
                    }

                    // Reset flag
                    valid = false;
                }

                if (filteredVenueAccess.Count > 0)
                {
                    return filteredVenueAccess;
                }
            }

            return null;
        }


        #endregion

        #endregion
    }
}
