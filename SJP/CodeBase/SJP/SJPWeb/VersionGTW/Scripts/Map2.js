
var map;
var directionsDisplay;
var directionsService;
var startLocation;
var endLocation;

$(document).ready(function () {
    setupPage();
    loadScript();
});

//
function setupPage() {
    $('#lcg-page').css({ width: '100%', height: '100%' });
    $('.mapgoogle').css({ width: '100%', height: $(document).height() + 'px' });
    $('.mapgoogle .mapContainer .directions').css({ height: $(document).height() - 10 + 'px' });
}

// Loads the map api script
function loadScript() {
    var script = document.createElement("script");
    script.type = "text/javascript";
    script.src = "http://maps.googleapis.com/maps/api/js?sensor=false&region=GB&callback=initialise";
    document.body.appendChild(script);
}

// Initialises the map after the map api script has loaded
function initialise() {
    initialiseLocations();
    initialiseMap();
    initialiseRoute();
}

// Initialises the locations to display the map for
function initialiseLocations() {

    // Read the hidden fields for the locations
    try {
        var locCoordStart = $("input[id*=mapStartLocationCoordinate]").val();
        var latLongStart = locCoordStart.split(',');

        var locCoordEnd = $("input[id*=mapEndLocationCoordinate]").val();
        var latLongEnd = locCoordEnd.split(',');

        startLocation = new google.maps.LatLng(latLongStart[0], latLongStart[1]);
        endLocation = new google.maps.LatLng(latLongEnd[0], latLongEnd[1]);
    }
    catch (err) { }
}

// Initialises the map
function initialiseMap() {

    directionsService = new google.maps.DirectionsService();
    directionsDisplay = new google.maps.DirectionsRenderer();

    var myOptions = {
        zoom: 16,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        center: startLocation
    }

    // Default to UK map if start location not found
    if (startLocation == null) {
        myOptions = {
            zoom: 5,
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            center: new google.maps.LatLng(54.6167, -3.6167)
        };
    }

    map = new google.maps.Map(document.getElementById("mapCanvas"), myOptions);

    directionsDisplay.setMap(map);
    directionsDisplay.setPanel(document.getElementById("directionsPanel"));
}

// Calculates the route
function initialiseRoute() {

    if (startLocation != null && endLocation != null) {
        var mode = "WALKING";

        var overrideTravelMode = $("input[id*=mapTravelMode]").val();
        if (overrideTravelMode != null) {
            mode = overrideTravelMode;
        }

        var request = {
            origin: startLocation,
            destination: endLocation,
            // Note that Javascript allows us to access the constant
            // using square brackets and a string value as its
            // "property."
            travelMode: google.maps.TravelMode[mode]
        };
        directionsService.route(request, function (response, status) {
            if (status == google.maps.DirectionsStatus.OK) {
                directionsDisplay.setDirections(response);
            }
        });
    }
}