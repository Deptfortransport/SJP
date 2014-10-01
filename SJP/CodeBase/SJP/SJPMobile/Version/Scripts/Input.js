var currentVenueControl = null;
var currentLocationInputText = null, currentLocationInputValue = null, currentLocationInputType = null;

// Initialise javascript stuff
$(document).ready(function () {
    setJSHdnSettingFieldValue('jsEnabled', $('div.locationControl'), 'true');

    setInputAccess();
    setupCurrentLocation();
    setupTimePicker();
    setupDatePicker();
    setupCalendar();
    setupNow();
    setupVenueSelector();
    setEmptyLocationText();
    setupAccessibleOptions();
    setupJourneyType();
    setupPlanJourney();
    setupCycleParkMap();
    scrollToMainContent();

    // Following lines are to rebind the jquery plugins or js events after ajax update of the update panel
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function () {
        scrollToMainContent();
        setJSHdnSettingFieldValue('jsEnabled', $('div.locationControl'), 'true');
        setInputAccess();
        setupCurrentLocation();
        setupCalendar();
        setupNow();
        setupAccessibleOptions();
        setEmptyLocationText();
        setupJourneyType();
    });

});

// Sets up the collapsible options dropdown
function setupAccessibleOptions() {

    var accessibilityOptions = $('.accessibilityOptions .collapse')

    // Only collapse if journeyType options class indicates it should be
    if (accessibilityOptions.length > 0) {
        $('.accessibilityOptions .ui-collapsible-content').slideUp(0);
    }

    // Show journey type options when the heading is clicked
    $('.accessibilityOptions .ui-collapsible-heading').click(function () {
        $('.accessibilityOptions .ui-collapsible-content').slideToggle(300);
    });

    // Hide the opitons when clicked
    $('.accessibilityOptions input[name*=mobilityOptions]').click(function () {
        var optionText = $(".accessibilityOptions input[name*=mobilityOptions]:checked").parent().attr('title');
        $('.accessibilityOptions h2').text(optionText);
        $('.accessibilityOptions .ui-collapsible-content').slideUp(400);
        // Update hidden value to allow server to detect user has manually updated
        $('input[id*=assistanceOptionSelected]').val(optionText);
    });
}

// Sets up the collapsible journey type dropdown
function setupJourneyType() {

    var journeyTypeOptions = $('.journeyTypeRow .collapse')

    // Only collapse if journeyType options class indicates it should be
    if (journeyTypeOptions.length > 0) {
        $('.journeyTypeRow .ui-collapsible-content').slideUp(0);
    }

    // Show journey type options when the heading is clicked
    $('.journeyTypeRow .ui-collapsible-heading').click(function () {
        $('.journeyTypeRow .ui-collapsible-content').slideToggle(300);
    });

    // Update the selected journey type text
    $('.journeyTypeRow input[name*=journeyTypeRdo]').click(function () {
        var journeyTypeText = $(".journeyTypeRow input[name*=journeyTypeRdo]:checked").next().text();
        $('.journeyTypeRow h2').text(journeyTypeText);
        $('.journeyTypeRow .ui-collapsible-content').slideUp(300);
        // Update hidden value to allow server to detect user has manually updated
        $('input[id*=journeyTypeSelected]').val(journeyTypeText);
    });
}

// Displays updating locations in the inputs,
// used to provide immediate feed back. Function attached to be server side code
function toggleLocation() {

    // Can't disable the toggle location button as the server partialpostback does not work
    //$("input[id*=travelFromToVenueToggle]").attr('disabled', 'disabled');

    // Disable the current location button
    $("input[id*=currentLocationButton]").attr('disabled', 'disabled');

    // Update the location input box
    var fromLocationControl = $('#fromLocation');
    var toLocationControl = $('#toLocation');

    // Keep the current input hidden values
    var currentFromLocationInputValue = $(fromLocationControl).find("input[id*=locationInput_hdnValue]").val();
    var currentFromLocationInputType = $(fromLocationControl).find("input[id*=locationInput_hdnType]").val();
    var currentToLocationInputValue = $(toLocationControl).find("input[id*=locationInput_hdnValue]").val();
    var currentToLocationInputType = $(toLocationControl).find("input[id*=locationInput_hdnType]").val();

    // Update the location input box, and reinsert the hidden values
    $(fromLocationControl).find("input[id*=locationInput]").attr('disabled', 'disabled');
    $(fromLocationControl).find("input[id*=locationInput]").val("Updating...");
    $(fromLocationControl).find("input[id*=locationInput_hdnValue]").val(currentFromLocationInputValue);
    $(fromLocationControl).find("input[id*=locationInput_hdnType]").val(currentFromLocationInputType);

    $(toLocationControl).find("input[id*=locationInput]").attr('disabled', 'disabled');
    $(toLocationControl).find("input[id*=locationInput]").val("Updating...");
    $(toLocationControl).find("input[id*=locationInput_hdnValue]").val(currentToLocationInputValue);
    $(toLocationControl).find("input[id*=locationInput_hdnType]").val(currentToLocationInputType);

    // Hide elements as required
    hideMessages();

    return true;
}

// Displays the please wait image when plan journey selected, 
// used to provide immediate feed back where the mobile device has slow network
function setupPlanJourney() {

    if ($('div.waitContainer').hasClass('hide')) {
        $('div.waitContainer').css('display', 'none');
    }

    $(document).on("click", "a[id*=planJourneyBtn]", function () {
        hideMessages();
        $('div.journeySummary').css('display', 'none');
        $('div.submittabreturn').css('display', 'none');
        $('div.waitContainer').css('display', 'block');
    });
}

// Scrolls the screen to the main content area
function scrollToMainContent() {

    var scrollToElement = $('.journeyInput');

    var messageShown = (($('div.contentMessages').length > 0) && ($('div.contentMessages').css('display') == 'block'));
    var waitShown = (($('div.waitContainer').length > 0) && !($('div.waitContainer').hasClass('hide')));
    var summaryShown = (($('div.journeySummary').length > 0));

    // If messages being shown, scroll to it
    if (messageShown) {
        $('html, body').animate({
            scrollTop: (scrollToElement.offset().top) + 'px'
        }, 'fast');
    }
    else {
        var mobilesummary = $('.mobilesummary');
        
        // If on mobile summary page and wait is displayed, scroll to it
        if ((mobilesummary.length > 0) && (waitShown)) {
            $('html, body').animate({
                scrollTop: (scrollToElement.offset().top) + 'px'
            }, 'fast');
        }
        // If on mobile summary page and summary is displayed, scroll to it
        else if ((mobilesummary.length > 0) && (summaryShown)) {
            $('html, body').animate({
                scrollTop: (scrollToElement.offset().top) + 'px'
            }, 'fast');
        }

    }
}

// Hides the messages div
function hideMessages() {
    $('div.contentMessages').css('display', 'none');
}

// Hide the input fields to prevent focus highlight issues on some devices
function hideInputs() {
    $('input[name*=locationInput]').css('display', 'none');
    $('input[name*=outwardDate]').css('display', 'none');
    $('input[name*=outwardTime]').css('display', 'none');
    $('a[id *= parkselected]').css('display', 'none');
}
// Show the input fields
function showInputs() {
    $('input[name*=locationInput]').css('display', 'inline-block');
    $('input[name*=outwardDate]').css('display', 'inline-block');
    $('input[name*=outwardTime]').css('display', 'inline-block');
    $('a[id *= parkselected]').css('display', 'inline-block');
}

// Performs a postback to update the location parks dropdown
function updateLocationPark() {

    $('a[id*=parkselected]').text("Retrieving cycle parks...");

    // Get UpdatePanel containing the location dropdown
    var updateContainer = $('div.journeyInput div.locationParkRow');
    var updatePanel = getAspElement('updateContentPark', 'div', updateContainer);

    if (updatePanel.length > 0) {
        try {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            __doPostBack($(updatePanel).attr('id'), '');

        } catch (err) {
        }
    }

}
/// ----------------------------------------------------------

/// --------------- CURRENT LOCATION ------------------------
// Initialises the current location button
function setupCurrentLocation() {

    // Display the current location button (hidden by default)
    $("input[id*=currentLocationButton]").css({ display: "block" });

    $("input[id*=currentLocationButton]").die();
    $("input[id*=currentLocationButton]").bind("click", function (event) {
        currentVenueControl = this.parentNode.parentNode.parentNode;
        // Disable the current location button
        $(this).attr('disabled', 'disabled');

        // Disable the toggle locaiton button
        $("input[id*=travelFromToVenueToggle]").attr('disabled', 'disabled');

        // Keep the current input text, in case geolocation errors
        var location = $(currentVenueControl).find("input[id*=locationInput]");
        currentLocationInputText = $(location[2]).val(); // Get the third in list as the "hdn" inputs are first
        currentLocationInputValue = $(currentVenueControl).find("input[id*=locationInput_hdnValue]").val();
        currentLocationInputType = $(currentVenueControl).find("input[id*=locationInput_hdnType]").val();

        // Update the location input box
        $(currentVenueControl).find("input[id*=locationInput]").attr('disabled', 'disabled');
        $(currentVenueControl).find("input[id*=locationInput]").val("Retrieving location...");
        $(currentVenueControl).find("input[id*=locationInput]").removeClass("locationError");

        // Hide elements as required
        hideMessages();

        getGeolocation();

        // Return false to prevent button postback to server
        return false;
    });
}

// Calls the current location functionality
function getGeolocation() {
    navigator.geolocation.getCurrentPosition(geolocationResult, geolocationError, { timeout: 10000 });
}

// Current location error handler
function geolocationError(error) {

    // Update the location input box with previous values
    if (currentLocationInputText != null) {
        $(currentVenueControl).find("input[id*=locationInput]").val(currentLocationInputText);
        $(currentVenueControl).find("input[id*=locationInput_hdnValue]").val(currentLocationInputValue);
        $(currentVenueControl).find("input[id*=locationInput_hdnType]").val(currentLocationInputType);
    }

    switch (error.code) {
        case error.PERMISSION_DENIED: alert("Unable to retrieve your location, please enter a postcode, location, station or select a venue");
            break;

        case error.POSITION_UNAVAILABLE: alert("Could not detect your location, please enter a postcode, location, station or select a venue");
            break;

        case error.TIMEOUT: alert("Retrieving your location timed out, please try again or enter a postcode, location, station or select a venue");
            break;

        default: alert("Error retrieving your location, please enter a postcode, location, station or select a venue");
            break;
    }

    // Re-enable the current location button
    $(currentVenueControl).find(".locationCurrent").removeAttr('disabled');
    $(currentVenueControl).find("input[id*=locationInput]").removeAttr('disabled');
    $("input[id*=travelFromToVenueToggle]").removeAttr('disabled');
}

// Current location handler
function geolocationResult(position) {
    
    var coord = position.coords.latitude + "," + position.coords.longitude;

    // Update the location input box
    $(currentVenueControl).find("input[id*=locationInput]").val("My Location");

    // Update the location control hidden field with the position coordinates
    $(currentVenueControl).find("input[id*=locationInput_hdnValue]").val(coord);
    $(currentVenueControl).find("input[id*=locationInput_hdnType]").val("CoordinateLL"); // Must be the SJPLocationType enum value

    // Remove watermark
    $(currentVenueControl).find("input[id*=locationInput]").removeClass("watermarkText");
    $(currentVenueControl).find("input[id*=locationInput]").removeClass("locationError");
    
    // Re-enable the current location button
    $(currentVenueControl).find(".locationCurrent").removeAttr('disabled');
    $(currentVenueControl).find("input[id*=locationInput]").removeAttr('disabled');
    $("input[id*=travelFromToVenueToggle]").removeAttr('disabled');
}
/// ----------------------------------------------------------

/// -------------------------- DATE --------------------------
// Sets up the date calendar picker
function setupDatePicker() {

    $(document).on("click", "input[name*=outwardDate]", function () {

        // Ensure no other date has selected style
        $('.collapseDate .daySelected').removeClass('daySelected');
        // Ensure selected date has style
        $("input[name*=data]:checked").parent().addClass('daySelected');
        // Ensure selected month is shown
        $('.collapseDate .collapseDateDays').css('display', 'none');
        $("input[name*=data]:checked").parent().parent().slideDown(0);

        hideMessages();

        $('#datepage').css('display', 'block');

        hideInputs();

        // Focus onto the back link, to allow selection via keyboard
        var datepage = $('#datepage');
        if (datepage) {
            datepage.find('input:checked').focus();
        }
    });

    $(document).on("click", "input[name*=data]", function () {
        // Set the selected date
        $('input[name*=outwardDate]').val($(this).val());
        $(this).parent().addClass('daySelected');

        showInputs();

        $('#datepage').css('display', 'none');

        $('html, body').animate({
            scrollTop: '0px'
        }, 'slow');

        resetArriveByFlag();

    });

    $(document).on("click", "a[id*=closedate]", function () {
        showInputs();
        $('#datepage').css('display', 'none');
    });
}

// Adds click event to calendar display
function setupCalendar() {
    // Hide all months
    $('.collapseDate .collapseDateDays').css('display', 'none');

    // Show month options when the heading is clicked
    $('.collapseDate h3').click(function () {
        // Hide all month options
        $('.collapseDate .collapseDateDays').css('display', 'none');
        // Show this month options
        $(this).next().slideDown(300);
    });
}

// Updates the styling if the now button is not shown
function setupNow() {

    var dateWidth = '127px';
    var timeWidth = '130px';

    var deviceAgent = navigator.userAgent.toLowerCase();
    if (deviceAgent.match(/e10i/i)) { // sonyericsson x10 mini/e10i (android 240screen)
        dateWidth = '97px';
        timeWidth = '110px';
    }

    if ($('.eventDateTime .nowSelect').length == 0)
        // && ($('.eventDateTime .nowSelect').css('display') == 'none'))
    {
        // Now not shown, resize the date time inputs
        $('.dateSelect .setdatebox input.dateEntry').css('width', dateWidth);
        $('.timePicker .settimebox input.timeInput').css('width', timeWidth);
    }
}

/// ----------------------------------------------------------

/// ------------------------- TIME ---------------------------
// Sets up the time picker
function setupTimePicker() {
    $(document).on("click", "input[name*=outwardTime]", function () {

        hideMessages();

        $('#timepage').css('display', 'block');

        hideInputs();

        // Scroll to the selected time
        var timeOption = $('div.timePicker input[name*=timeRadioList]:checked');

        $('html, body').animate({
            scrollTop: ($(timeOption).offset().top - 80) + 'px'
        }, 'slow');

        // To allow scrolling via keyboard
        var header = getAspElement("timeSelectorLabel", "h2", $('div.timePicker'));
        if (header) {
            header.focus();
        }
    });

    $(document).on("click", "input[name*=timeRadioList]", function () {
        // Set the selected time
        $('input[name*=outwardTime]').val($(this).val());

        showInputs();

        $('#timepage').css('display', 'none');

        $('html, body').animate({
            scrollTop: '0px'
        }, 'slow');

        resetArriveByFlag();

    });

    $(document).on("click", "a[id*=closetime]", function () {

        showInputs();

        $('#timepage').css('display', 'none');
    });
}

// Resets the arrive by date time flag
function resetArriveByFlag() {

    // Reset the now flag
    $('input[name*=isNowFlag]').val('false');

    // If location input is in to venue mode, then must reset
    // the arrive by flag to be "arrive by"
    if ($('input[name*=isArriveByFlag]').val() == "false"
            && $('input[name*=isToVenueFlag]').val() == "true") {

        $('input[name*=isArriveByFlag]').val('true');

        // Hide the leave at text and display the arrive by
        $('.eventDateTitleDiv span.hide').css('display', 'block');
        $('.eventDateTitleDiv span.show').css('display', 'none');
    }
}
/// ----------------------------------------------------------

/// ------------------------- VENUE ---------------------------
// Sets up the venue selector
function setupVenueSelector() {

    // Show venue page on venue location input click
    $(document).on("click", "input[name*=locationInput]:not(.locationBox)", function () {

        hideMessages();

        // this sets the location control that has 'focus' (for venue to venue cases)
        currentVenueControl = this.parentNode.parentNode;
        $('div#venuespage').css('display', 'block');

        document.activeElement.blur(); // Remove focus from the input field?

        hideInputs();
        
        // Focus onto the back link, to allow selection via keyboard
        var venuePage = $('div#venuespage');
        if (venuePage) {
            venuePage.find('.topNavLeft').focus();
        }
    });

    // Show venue page on venue link click
    $(document).on("click", "a[id*=venueInputPageLnk]", function () {

        hideMessages();

        currentVenueControl = this.parentNode.parentNode;
        $('div#venuespage').css('display', 'block');

        hideInputs();

        // Focus onto the back link, to allow selection via keyboard
        var venuePage = $('div#venuespage');
        if (venuePage) {
            venuePage.find('.topNavLeft').focus();
        }
        return false;
    });

    // Clears the grey watermark text when the input recieves focus
    $(document).on("click", "input[name*=locationInput].locationBox", function () {
        $(this).val('');
        $(this).removeClass("watermarkText");
        $(this).removeClass("locationError");
    });
    $(document).on("focus", "input[name*=locationInput].locationBox.watermarkText", function () {
        $(this).val('');
        $(this).removeClass("watermarkText");
        $(this).removeClass("locationError");
    });

    // Set selected venue when radio button selection changes
    $(document).on("click", "input[name*=locationSelector]", function () {
        var venueid = $(this).attr('id').substring(2);
        $(currentVenueControl).find('input[id*=locationInput]').val($(this).val());
        $(currentVenueControl).find('input[id*=locationInput_hdnValue]').val(venueid);
        $(currentVenueControl).find('input[id*=locationInput_hdnType]').val("Venue");

        // Remove watermark
        $(currentVenueControl).find("input[id*=locationInput]").removeClass("watermarkText");
        $(currentVenueControl).find("input[id*=locationInput]").removeClass("locationError");

        showInputs();

        $('div#venuespage').css('display', 'none');

        $('html, body').animate({
            scrollTop: '0px'
        }, 'slow');

        updateLocationPark();
    });
    
    // Sets visible focus style when tabbing through venue options
    $(document).on("focus", "input[name=locationSelector]", function () {
        $(this).next().css({ color: '#e6007e'});
        $(this).parent().css('border-color','#e6007e');
    });
    $(document).on("blur", "input[name=locationSelector]", function () {
        $(this).next().css('color', 'inherit');
        $(this).parent().css('border-color', '#cccccc');
    });

    // Close venue page
    $(document).on("click", "a[id*=closevenues]", function () {
        showInputs();
        $('div#venuespage').css('display', 'none');

        $('html, body').animate({
            scrollTop: '0px'
        }, 'slow');

        return false;
    });
}

// Add watermark to input fields
function setEmptyLocationText() {
    $("input[name*=locationInput]").each(function () {
        if ($(this).val() == '') {
            $(this).val($(this).data('sjpdefaultvalue'));
            if ($(this).hasClass("locationBox")) {
                $(this).addClass("watermarkText");
            }
        }
    });
}

// Sets readonly status on controls which do not allow user text entry
function setInputAccess() {
    // makes the location input box readonly if the control is venue only
    if ($('div[id*=fromLocation]').find('input[id*=currentLocationButton]').length == 0) {
        $('div[id*=fromLocation]').find('input[id*=locationInput]').attr('readonly', true);
        $('div[id*=fromLocation]').find('input[id*=locationInput]').removeClass('locationBox');
    }

    if ($('div[id*=toLocation]').find('input[id*=currentLocationButton]').length == 0) {
        $('div[id*=toLocation]').find('input[id*=locationInput]').attr('readonly', true);
        $('div[id*=toLocation]').find('input[id*=locationInput]').removeClass('locationBox');
    }

    // Make field readonly always to prevent mobile devices from display text entry 
    $("input[name*=outwardDate]").attr('readonly', true);

    // Make field readonly always to prevent mobile devices from display text entry 
    $("input[name*=outwardTime]").attr('readonly', true);

    // Make field readonly always to prevent mobile devices from display text entry 
    $("input[name*=parkselected]").attr('readonly', true);
}
/// ----------------------------------------------------------

/// ---------------------- CYCLE MAP -------------------------
// Sets up the cycle map selection
function setupCycleParkMap() {
    $(document).on("click", "a[id*=parkselected]", function () {

        // Check if link has disabled styling
        if ($('div[id*=locationParkDiv]').hasClass('locationParkRowDisabled')) {
            // disabled, do not show map container
        }
        else {
            var documentHeight = $(document).height();

            $("div[id*=cycleParkMapContainer]").css({ display: 'block', height: documentHeight });

            hideInputs();

            $('html, body').animate({
                scrollTop: '0px'
            }, 'fast');
        }
        return false;
    });

    $(document).on("click", "a[id*=closeparkmap]", function () {
        showInputs();
        $('div[id*=cycleParkMapContainer]').css('display', 'none');
        return false;
    });

    // Handles the radio button list that appears if there is no map for the venue
    $(document).on("change", "input[name*=cycleParksNoMap]", function () {
        $('a[id*=parkselected]').text($("label[for*=" + $(this).attr('id') + "] span.ui-btn-text").text());
        $('input[id*=selectedCycleParkID]').val($(this).val());
        showInputs();
        $('div[id*=cycleParkMapContainer]').css('display', 'none');
        
        $('html, body').animate({
            scrollTop: '0px'
        }, 'slow');
    });

    $(document).on("click", "input[name*=parks]", function () {
        setCyclePark($(this).val(), $("label[for*=" + $(this).attr('id') + "]").text());
    });
}

// Sets the selected cycle park from the map
function setCyclePark(parkId, parkName) {
    $('a[id*=parkselected]').text(parkName);
    $('input[id*=selectedCycleParkID]').val(parkId);
    showInputs();
    $("div[id*=cycleParkMapContainer]").css('display', 'none');

    $('html, body').animate({
        scrollTop: '0px'
    }, 'slow');
}
/// ----------------------------------------------------------