// Map variables
var map;
var boundingBox;
var credentialsKey = "Ak69Q6umkGu6qZ3RWz266OPnEhtgVl-scLR6MtlUyWTthStvwCxoFLpGrHfQi7vt";
var iconFlag_Pink = "../VersionGTW/Images/maps/Pink-flag.png";
var iconFlag_Blue = "../VersionGTW/Images/maps/Blue-flag.png";
var iconInfoboxPointer = "../VersionGTW/Images/maps/pointer.png";
var locationPins = null;
var routePins = null;
var theInfobox;
var startInfobox;
var endInfobox;

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

    var printerFriendly = getAspElement("printerFriendlyFlag", "input", "");

    if (printerFriendly.val()) {
        getMap();
    }
    else {
        Microsoft.Maps.loadModule('Microsoft.Maps.Overlays.Style', { callback: getMap });
    }
}

// Determines the map mode and displays an appropriate map
function getMap() {
    showJourneyMap();
}

// Initialises a map
function loadMap() {

    var printerFriendly = getAspElement("printerFriendlyFlag", "input", "");

    if (printerFriendly.val()) {
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
            showDashboard: false,
            disableUserInput: true
        });
    }
    else {
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
            disableBirdseye: false,
            showDashboard: true,
            customizeOverlays: true
        });
    }

    locationPins = new Microsoft.Maps.EntityCollection();
    routePins = new Microsoft.Maps.EntityCollection();
    infoBoxes = new Microsoft.Maps.EntityCollection();

    map.entities.push(locationPins);

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

    var printerFriendly = getAspElement("printerFriendlyFlag", "input", "");

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

                if (!printerFriendly.val()) {
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

                if (!printerFriendly.val()) {
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
                if (!printerFriendly.val()) {
                    Microsoft.Maps.Events.addHandler(pushpin, 'click', showInfobox);
                }
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
    var routeShape = new Microsoft.Maps.Polyline(routePoints, { strokeColor: new Microsoft.Maps.Color(200, 0, 0, 200) });
    map.entities.push(routeShape);

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
