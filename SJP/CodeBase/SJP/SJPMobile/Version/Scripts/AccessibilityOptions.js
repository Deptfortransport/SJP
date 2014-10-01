// Initialise javascript stuff
$(document).ready(function () {

    setupPlanJourney();
    setupStopsList();
    scrollToMainContent();

    // Following lines are to rebind the jquery plugins or js events after ajax update of the update panel
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function () {
        setupPlanJourney();
        scrollToMainContent();
    });

});

// Sets the plan journey button enabled disabled state
function setupPlanJourney() {
    if (($('select[id*=stopList] option:selected').length > 0) &&
        ($('select[id*=stopList] option:selected').val() != "")) {
        
        $('a[id*=planJourneyBtn]').removeClass('buttonMagDisabled');
        $('a[id*=planJourneyBtn]').removeAttr('disabled');

        hideMessages();
    }
    else {

        if (!$('a[id*=planJourneyBtn]').hasClass('buttonMagDisabled')) {
            $('a[id*=planJourneyBtn]').addClass('buttonMagDisabled');
            $('a[id*=planJourneyBtn]').attr('disabled', 'disabled');
        }
    }
}

// Sets up the stop lists
function setupStopsList() {

    $(document).on("change", "select[id*=countryList]", function () {
        setupPlanJourney();
    });

    $(document).on("change", "select[id*=countyList]", function () {
        setupPlanJourney();
    });

    $(document).on("change", "select[id*=districtList]", function () {
        setupPlanJourney();
    });

    $(document).on("change", "select[id*=stopList]", function () {
        setupPlanJourney();
    });
}

// Scrolls the screen to the main content area
function scrollToMainContent() {

    var scrollToElement = $('#country');

    var messageShown = (($('div.contentMessages').length > 0) && ($('div.contentMessages').css('display') == 'block'));
    
    // If messages being shown, scroll to it
    if (messageShown) {
        $('html, body').animate({
            scrollTop: (scrollToElement.offset().top) + 'px'
        }, 'fast');
    }
}

// Hides the messages div
function hideMessages() {
    $('div.contentMessages').css('display', 'none');
}