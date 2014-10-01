var currentNewsItem = null;
var currentNewsItemParent = null;

// Initialise javascript stuff
$(document).ready(function () {
    setupTravelNewsFilter();
    setupResults();
    setupVenueSelector();
    setupVenueLabel();

    // Following lines are to rebind the dropdowns after ajax update of the update panel
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function () {
        setupResults();
    });

});

// Sets up the collapsible options dropdown
function setupTravelNewsFilter() {

    var filterOptions = $('.tnFilter .collapse')

    // Only collapse if filter class indicates it should be
    if (filterOptions.length > 0) {
        $('.tnFilter .ui-collapsible-content').slideUp(0);
    }

    // Show filter options when the heading is clicked
    $('.tnFilter .ui-collapsible-heading').click(function () {
        $('.tnFilter .ui-collapsible-content').slideToggle(300);
    });
}

// Sets result client side sliding open/close effects
function setupResults() {

    // Hide waiting
    if ($('div.waitContainer').hasClass('hide')) {
        $('div.waitContainer').css('display', 'none');
    }

    // Travel news
    $('div.travelNews a[id*=showDetailsBtn]').die();
    // bind client side events and stop it to propagate so updatepanel doesn't fire
    $('div.travelNews a[id*=showDetailsBtn]').live('click', function (event) {
        var newsDiv = $('div.travelNews');
        currentNewsItem = $('input[id*=' + $(this).find('input[name*=newsId]').val() + ']').parent();

        var documentHeight = $(document).height();

        currentNewsItem.css({ display: 'block', height: documentHeight });

        $('html, body').animate({
            scrollTop: '0px'
        }, 'fast');

        event.preventDefault();
    });

    $(document).on("click", "a[id*=closeNewsItem]", function () {
        if (currentNewsItem) {
            currentNewsItem.css("display", "none");
        }
        currentNewsItem = null;
        return false;
    });


    // Underground news
    $('div.undergroundStatus a[id*=showDetailsBtn]').die();
    // bind client side events and stop it to propagate so updatepanel doesn't fire
    $('div.undergroundStatus a[id*=showDetailsBtn]').live('click', function (event) {
        var newsDiv = $('div.undergroundStatus');
        var newsId = $(this).find('input[name*=statusLineId]').val();
        currentNewsItem = $('input[id*=undergroundStatusControl_' + newsId + ']').parent();

        var documentHeight = $(document).height();

        currentNewsItem.css({ display: 'block', height: documentHeight });

        $('html, body').animate({
            scrollTop: '0px'
        }, 'fast');

        event.preventDefault();
    });

    $(document).on("click", "a[id*=closeNewsItem]", function () {
        if (currentNewsItem) {
            currentNewsItem.css("display", "none");
        }
        currentNewsItem = null;
        return false;
    });
}

// Sets up the venue label for intial page only
function setupVenueLabel() {

    if ($('span[id*=selectedVenue]').text() != "") {
        $('span[id*=selectedVenue]').css('display', 'block');
    }
}

// Sets up the venue selector
function setupVenueSelector() {

    // News filter modes click handler
    $(document).on("click", "input[name*=newsModes]", function () {
        newsfilter = $("input[name*=newsModes]:checked").val();
        $('.ui-collapsible-content').slideUp(300);

        // Hide the travel news incidents
        $('.travelNews ul').css('display', 'none');
        $('div.travelNewsUnavailable').css('display', 'none');
        //$('.travelNews').css('min-height', '150px');
        $('.undergroundStatus ul').css('display', 'none');
        //$('.undergroundStatus').css('min-height', '150px');
        $('div.undergroundStatusUnavailable').css('display', 'none');
        $('div.waitContainer').css('display', 'block');

        if (newsfilter == "Venue") {
            $("div#venuespage").css('display', 'block');
        } else {

            // This isn't a venue so clear the selected venue label
            $('span[id*=selectedVenue]').text("");
            $('span[id*=selectedVenue]').css('display', 'none');

            // Update the selected filter heading
            var newsfiltertext = $("input[name*=newsModes]:checked").next().text();
            $('.tnFilter h2').text(newsfiltertext);

            try {
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                __doPostBack($('div[id*=updatePanel]').attr('id'), '');
            } catch (err) {
            }
        }

    });
        
    $(document).on("change", "input[name*=locationSelector]:radio", function () {
        var venueid = $(this).attr('id').substring(2);
        $('input[id*=venueNaptans]').val(venueid);
        $('span[id*=selectedVenue]').text("Travel news for " + $(this).val());
        $('span[id*=selectedVenue]').css('display', 'block');
        $('div#venuespage').css('display', 'none');

        // Update the selected filter heading
        var newsfiltertext = $("input[name*=newsModes]:checked").next().text();
        $('.tnFilter h2').text(newsfiltertext);

        try {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            __doPostBack($('div[id*=updatePanel]').attr('id'), '');
        } catch (err) {
        }
    });

    $(document).on("click", "a[id*=allVenuesButton]", function () {
        $('input[id*=venueNaptans]').val('');
        $('span[id*=selectedVenue]').text("Travel news for " + $(this).text());
        $('span[id*=selectedVenue]').css('display', 'block');
        $('div#venuespage').css('display', 'none');

        // Update the selected filter heading
        var newsfiltertext = $("input[name*=newsModes]:checked").next().text();
        $('.tnFilter h2').text(newsfiltertext);

        try {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            __doPostBack($('div[id*=updatePanel]').attr('id'), '');
        } catch (err) {
        }
        return false;
    });

    $(document).on("click", "a[id*=closevenues]", function () {
    
        $('div#venuespage').css('display', 'none');

        // Display the travel news incidents
        $('.travelNews ul').css('display', 'block');
        //$('.travelNews').css('min-height', 'auto');
        $('div.travelNewsUnavailable').css('display', 'block');
        $('.undergroundStatus ul').css('display', 'block');
        //$('.undergroundStatus').css('min-height', 'auto');
        $('div.undergroundStatusUnavailable').css('display', 'block');
        $('div.waitContainer').css('display', 'none');

        return false;
    });
}
