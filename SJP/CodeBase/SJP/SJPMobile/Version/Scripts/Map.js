// Map variables
var map;
var boundingBox;
var credentialsKey = "Ak69Q6umkGu6qZ3RWz266OPnEhtgVl-scLR6MtlUyWTthStvwCxoFLpGrHfQi7vt";
var iconFlag_Pink = "./Version/Images/maps/Pink-flag.png";
var iconFlag_Blue = "./Version/Images/maps/Blue-flag.png";
var iconMyLocation = "./Version/Images/maps/My-Location-30.png";
var iconInfoboxPointer = "./Version/Images/maps/pointer.png";
var pin_currentLocation;
var currentLocationPins = null;
var locationPins = null;
var routePins = null;
var theInfobox;

var startInfobox;
var endInfobox;

// Enum values for MapMode
var MapMode_Location = "Location";
var MapMode_LocationCurrent = "LocationCurrent";
var MapMode_Journey = "Journey";
var MapMode_WalkLeg = "WalkLeg";

// Geolocation variables
var geoUserResponded = false;
var geoUserRespondedTimeout;

// Initialise javascript stuff
$(document).ready(function () {
    displayLoading(true);
    loadScript();
});


// Loads the map api script
function loadScript() {
    var script = document.createElement("script");
    script.type = "text/javascript";
    script.src = "http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0&onScriptLoad=initialise";
    document.body.appendChild(script);
}

// Initialises the map after the map api script has loaded
function initialise() {
    getMap();
    setupVenueMap();
    setupCurrentLocationButton();
}

// Determines the map mode and displays an appropriate map
function getMap() {
    // Check for mode to use
    var modeField = getAspElement("modeType", "input", "");

    if (modeField.val() == MapMode_LocationCurrent) {
        getGeolocation();
    }
    else if (modeField.val() == MapMode_Location) {
        showLocation(false, null);
    }
    else if (modeField.val() == MapMode_Journey) {
        showJourneyMap();
    }
    else if (modeField.val() == MapMode_WalkLeg) {
        // hideMyLocation();
        showJourneyMap();
    }
}

// Initialises a map
function loadMap() {

    map = new Microsoft.Maps.Map(document.getElementById("mapDiv"),
    {
        credentials: credentialsKey,
        mapTypeId: Microsoft.Maps.MapTypeId.road,
        zoom: 14,
        showScalebar: false,
        showCopyright: false,
        enableClickableLogo: false,
        enableSearchLogo: false,
        showScalebar: false,
        disableBirdseye: true,
        showDashboard: false
    });

    // Center around the uk
    map.setView({ zoom: 5, center: new Microsoft.Maps.Location(54.6167, -3.6167) })

    locationPins = new Microsoft.Maps.EntityCollection();
    currentLocationPins = new Microsoft.Maps.EntityCollection();
    routePins = new Microsoft.Maps.EntityCollection();
    infoBoxes = new Microsoft.Maps.EntityCollection();

    map.entities.push(locationPins);
    map.entities.push(currentLocationPins);
    
    // Create the location infobox, setting html etc happens on the click event
    theInfobox = new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(0, 0),
                {
                    visible: false,
                    offset: new Microsoft.Maps.Point(-33, 2),
                    zIndex: 0
                });
    map.entities.push(theInfobox);

    Microsoft.Maps.Events.addHandler(map, 'viewchangeend', displayLoading(false));
}

// Displays the map loading message over the map
function displayLoading(show) {
    if (show) {
        // Display the loading message
        $('.mapLoading').css({ display: "inherit" });
    }
    else {
        // Hide the loading message
        $('.mapLoading').css({ display: "none" });
    }
}

function hideMyLocation() {
    $('.myLocationMap').css({ display: "none" });
}

// Show the specificed location
function showLocation(currentlocation, text_offset) {

    // Read the hidden fields for the location to display
    var locName = getAspElement("mapLocationName", "input", "");
    var displayName = locName.val();

    var locCoord = getAspElement("mapLocationCoordinate", "input", "");
    var latLong = locCoord.val().split(',');

    // Create the map if required
    if (map == null) {
        loadMap();
    }

    // Center on the location
    map.setView({ zoom: 14, center: new Microsoft.Maps.Location(latLong[0], latLong[1]) })

    if (!text_offset) {
        text_offset = new Microsoft.Maps.Point(2, 2);
    }

    // For current location logic (to allow existing locations and route to remain)
    if (currentlocation) {

        // Remove the existing currentLocation pin (in case the user has moved)
        if (pin_currentLocation != null) {
            currentLocationPins.clear();
        }

        // Add a pin for the currentlocation
        pin_currentLocation = new Microsoft.Maps.Pushpin(
        new Microsoft.Maps.Location(latLong[0], latLong[1]),
        {
            title: displayName,
            textOffset: text_offset,
            typeName: 'mapPushPin',
            icon: iconMyLocation,
            width: 200,
            height: 28,
            anchor: new Microsoft.Maps.Point(8, 28)
        })
        pin_currentLocation.title = displayName;

        currentLocationPins.push(pin_currentLocation);
        Microsoft.Maps.Events.addHandler(pin_currentLocation, 'click', showTheInfobox);
    }
    // For location logic
    else {

        // Add a pin for the location
        var pin_location = new Microsoft.Maps.Pushpin(
        new Microsoft.Maps.Location(latLong[0], latLong[1]),
        {
            title: displayName,
            textOffset: text_offset,
            typeName: 'mapPushPin',
            icon: iconFlag_Pink,
            width: 200,
            height: 28,
            anchor: new Microsoft.Maps.Point(8, 28)
        })
        pin_location.title = displayName;

        locationPins.push(pin_location);
        Microsoft.Maps.Events.addHandler(pin_location, 'click', showTheInfobox);
    }
}

// Shows a route map
function showJourneyMap() {
    // Get the map points to display
    var journeyPoints = getAspElement("journeyPoints", "input", "");
    var journeyDetails = journeyPoints.val();
    journeyDetails = journeyDetails.split('|');

    // Track the start and end locations to allow map to be zoomed out to show the journey
    var startLat, startLong, endLat, endLong;

    // Create the map if required
    if (map == null) {
        loadMap();
    }

    // Build the journey route points, and pins
    var routePoints = new Array();

    var routePointNumber = 0;

    // Each point will contain: lat,long,text
    for (counter = 0; counter < journeyDetails.length; counter = counter + 1) {
        var waypoint = journeyDetails[counter];
        waypoint = waypoint.split('@');

        // Must be 5 parts: lat,long,description,show pushpin,show route
        if (waypoint.length == 5) {

            // Start point
            if (counter == 0) {

                startLat = waypoint[0];
                startLong = waypoint[1];
                var pushpin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(startLat, startLong),
                {
                    typeName: 'mapPushPin',
                    icon: iconFlag_Blue,
                    width: 24,
                    height: 29,
                    anchor: new Microsoft.Maps.Point(8, 28)
                });
                pushpin.Description = waypoint[2];
                routePins.push(pushpin);
                Microsoft.Maps.Events.addHandler(pushpin, 'click', showStartInfobox);

                var startInfoboxHTML = '<div class="infobox"><a class="infobox_close" href="javascript:hideStartInfobox()"></a>';
                startInfoboxHTML = startInfoboxHTML + '<div class="infobox_content">' + waypoint[2] + '</div></div>';
                startInfoboxHTML = startInfoboxHTML + '<div classe="infobox_pointer"><img src="' + iconInfoboxPointer + '"></div>';
                   
                startInfobox = new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(startLat, startLong),
                {
                    visible: false,
                    offset: new Microsoft.Maps.Point(-30, -2),
                    htmlContent: startInfoboxHTML,
                    zIndex: 0
                });
                map.entities.push(startInfobox);
            }
            // End point
            else if (counter == (journeyDetails.length - 1)) {

                endLat = waypoint[0];
                endLong = waypoint[1];
                var pushpin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(endLat, endLong),
                {
                    typeName: 'mapPushPin',
                    icon: iconFlag_Pink,
                    width: 24,
                    height: 29,
                    anchor: new Microsoft.Maps.Point(8, 28)
                });
                pushpin.Description = waypoint[2];
                routePins.push(pushpin);
                Microsoft.Maps.Events.addHandler(pushpin, 'click', showEndInfobox);

                var endInfoboxHTML = '<div class="infobox"><a class="infobox_close" href="javascript:hideEndInfobox()"></a>';
                endInfoboxHTML = endInfoboxHTML + '<div class="infobox_content">' + waypoint[2] + '</div></div>';
                endInfoboxHTML = endInfoboxHTML + '<div classe="infobox_pointer"><img src="' + iconInfoboxPointer + '"></div>';

                endInfobox = new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(endLat, endLong),
                {
                    visible: false,
                    offset: new Microsoft.Maps.Point(-30, -2),
                    htmlContent: endInfoboxHTML,
                    zIndex: 0
                });
                map.entities.push(endInfobox);
            }
            else if (waypoint[3] == 'true') {
                var pushpin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(waypoint[0], waypoint[1]),
                {
                    typeName: 'mapPushPin',
                    icon: iconFlag_Pink,
                    width: 24,
                    height: 29,
                    anchor: new Microsoft.Maps.Point(8, 28)
                });
                pushpin.Description = waypoint[2];
                routePins.push(pushpin);
                Microsoft.Maps.Events.addHandler(pushpin, 'click', showInfobox);
            }

            if (waypoint[4] == 'true') {
                routePoints[routePointNumber] = new Microsoft.Maps.Location(waypoint[0], waypoint[1]);
                routePointNumber = routePointNumber + 1;
            }
        }
    }

    // Build the box to display map centered around the journey
    var boundingBox = Microsoft.Maps.LocationRect.fromLocations(
        new Microsoft.Maps.Location(startLat, startLong),
        new Microsoft.Maps.Location(endLat, endLong)
    );

    // Need a bigger box so get the margins of the existing one
    var north = boundingBox.getNorth();
    var south = boundingBox.getSouth();
    var west = boundingBox.getWest();
    var east = boundingBox.getEast();

    // Expand the margins by 10%
    var expand = 0.1;
    north = north + (expand * (north - south));
    south = south - (expand * (north - south));
    east = east + (expand * (east - west));
    west = west - (expand * (east - west));

    boundingBox = Microsoft.Maps.LocationRect.fromEdges(north, west, south, east, '', '');

    // Center around the journey
    map.setView({ bounds: boundingBox })

    // Add the journey route and location pins
    map.entities.push(routePins);

    // No route unless there are at least 2 points
    if (routePointNumber > 1) {
        var routeShape = new Microsoft.Maps.Polyline(routePoints, { strokeColor: new Microsoft.Maps.Color(200, 0, 0, 200) });
        map.entities.push(routeShape);
    }

    // Display the view journey button (to zoom back to journey)
    $('.viewJourney').css({ display: "block" });

    // Zoom to journey click handler
    $('.viewJourneyDiv input').click(function (event) {
        // Center around the journey
        if (map && boundingBox) {
            map.setView({ bounds: boundingBox })
        }
        event.preventDefault();
    });
}
/// ----------------------------------------------------------

/// --------------- INFO BOX EVENT HANDLERS ------------------
function showTheInfobox(e) {
    if (e.targetType == 'pushpin') {

        var pin = e.target;
        var location = e.target.getLocation();
        var infoBoxEvent = this;
        
        var theInfoboxHTML = '<div class="infobox"><a class="infobox_close" href="javascript:hideTheInfobox()"></a>';
        theInfoboxHTML = theInfoboxHTML + '<div class="infobox_content">' + pin.title + '</div></div>';
        theInfoboxHTML = theInfoboxHTML + '<div classe="infobox_pointer"><img src="' + iconInfoboxPointer + '"></div>';
        
        theInfobox.setLocation(location);
        theInfobox.setOptions({ 
            visible: true, 
            htmlContent: theInfoboxHTML
        });
    }
}
function hideTheInfobox(e) {
    theInfobox.setOptions({ visible: false });
}
function showStartInfobox(e) {
    endInfobox.setOptions({ visible: false });
    startInfobox.setOptions({ visible: true, showPointer: true, offset: new Microsoft.Maps.Point(-30, -2) });
}
function showEndInfobox(e) {
    startInfobox.setOptions({ visible: false });
    endInfobox.setOptions({ visible: true, showPointer: true, offset: new Microsoft.Maps.Point(-30, -2) });
}
function hideStartInfobox(e) {
    startInfobox.setOptions({ visible: false });
}
function hideEndInfobox(e) {
    endInfobox.setOptions({ visible: false });
}
/// ----------------------------------------------------------

/// --------------- CURRENT LOCATION ------------------------
// Initialises the current location button
function setupCurrentLocationButton() {

    // Display the current location button (hidden by default)
    $('.locationCurrent').css({ display: "inherit" });

    $('.locationCurrent').die();
    $('.locationCurrent').bind('click', function (event) {

        getGeolocation();

        return false;
    });
}

// Calls the current location functionality
function getGeolocation() {

    // Disable the current location button
    geolocationButton(false);

    // Create the map if required
    if (map == null) {
        loadMap();
    }
    
    // If user doesnt respond and no error is returned
    geoUserResponded = false;

    navigator.geolocation.getCurrentPosition(geolocationResult, geolocationError, { timeout: 9000 });

    displayLoading(true);

    // Set timeout for user not responding
    geoUserRespondedTimeout = setTimeout(geolocationNoResponse, 15000);
}

// Enables or disables the current location button
function geolocationButton(enable) {

    if (enable) {
        // Enable the current location button
        $('.locationCurrent').removeAttr('disabled');
    }
    else {
        // Disable the current location button
        $('.locationCurrent').attr('disabled', 'disabled');
    }
}

// Handles current location no user response
function geolocationNoResponse() {

    if (!geoUserResponded) {
        displayLoading(false);

        // Re-enable the current location button
        geolocationButton(true);
    }

    if (geoUserRespondedTimeout) {
        clearTimeout();
    }
}

// Current location error handler
function geolocationError(error) {

    geoUserResponded = true;
    
    switch (error.code) {
        case error.PERMISSION_DENIED: alert("Unable to retrieve your location");
            break;

        case error.POSITION_UNAVAILABLE: alert("Could not detect your location");
            break;

        case error.TIMEOUT: alert("Retrieving your location timed out, please try again");
            break;

        default: alert("Error retrieving your location");
            break;
    }

    // Re-enable the current location button
    geolocationButton(true);

    // Hide loading and clear no response timeout
    displayLoading(false);
    geolocationNoResponse();
}

// Current location handler
function geolocationResult(position) {

    geoUserResponded = true;

    var coord = position.coords.latitude + "," + position.coords.longitude;

    // Update the location name in the page
    var locName = getAspElement("mapLocationName", "input", "");
    $(locName).val("My Location");

    // Update the location coordinate in the page
    var locCoord = getAspElement("mapLocationCoordinate", "input", "");
    $(locCoord).val(coord);

    // Update the map mode field
    var modeField = getAspElement("modeType", "input", "");
    modeField.val(MapMode_LocationCurrent);

    // show lcation on the map
    showLocation(true, new Microsoft.Maps.Point(-35, 2));

    // Re-enable the current location button
    geolocationButton(true);

    // Display the use location button (to return back to input page)
    $('.useLocationDiv').css({ display: "block" });

    // Hide loading and clear no response timeout
    displayLoading(false);
    geolocationNoResponse();

    return false;
}

/// ----------------------------------------------------------

/// --------------- VENUE MAP IMAGE  ------------------------
function showOriginVenueMap() {

    var documentHeight = $(document).height();

    $('div[id*=originVenueMapControl]').css({ display: 'block', height: documentHeight });
    $(document).scrollTop(0);
}
function showDestinationVenueMap() {

    var documentHeight = $(document).height();

    $('div[id*=destinationVenueMapControl]').css({ display: 'block', height: documentHeight });
    $(document).scrollTop(0);
}

// Setup click event handling for the venue map
function setupVenueMap() {
    
    // attaches the the click event of the div containing the venue image, to allow display full size when clicked
    $(document).on("click", ".venueMapDiv", function () {

        var img = new Image();
        img.onload = function () {

            var pic_real_height = this.height;
            var pic_real_width = this.width;

            $('.venueMapImgDiv').css('width', pic_real_width + 'px');
            $('.venueMapImgDiv').css('height', pic_real_height + 'px');
        }
        img.src = $('.venueMapDiv img').attr('src');

        return false;
    });

    // closes any open map pages
    $(document).on("click", "a[id*=closevenuemap]", function () {
        $('div[id*=VenueMapControl]').css('display', 'none');
        $('.venueMapImgDiv').css('width', '100%');
        $('.venueMapImgDiv').css('height', 'auto');
        return false;
    });
}
/// ----------------------------------------------------------
