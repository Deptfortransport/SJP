var currentVenueMap = null;
var currentPagePosition = null;
var loaded = false;

// Initialise javascript stuff
$(document).ready(function () {
    setupVenueMap();
    setupAdditionalNotesDialogs($('tr.expanded'));
});


$(function () {
    $(window).load(function () {
        $(window).resize();
    });

    $(window).resize(function () {
        $("img[id$='legLineImage']").each(function () {
            //need great grandparent 
            $(this).height(10);
        });
        $("img[id$='legLineImage']").each(function () {
            //need great grandparent 
            var ggp = $(this).parent().parent().parent();
            $(this).height(ggp.height());
        });
    });
});

// Setup click event handling for the venue map button
function setupVenueMap() {
    // attaches the the click event of the view map button in the last leg
    $(document).on("click", "a[id*=endLocationMapLink]", function () {
        
        var documentHeight = $(document).height();

        $('div[id*=destinationVenueMapControl]').css({ display: 'block', height: documentHeight });
        $(document).scrollTop(0);
        return false;
    });

    // attaches the the click event of the view map button in the first leg
    $(document).on("click", "a[id*=locationMapLink]", function () {

        var documentHeight = $(document).height();

        $('div[id*=originVenueMapControl]').css({ display: 'block', height: documentHeight });
        $(document).scrollTop(0);
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

// Sets up the dialog boxes for journey web notes
function setupAdditionalNotesDialogs(container) {

    $('div[id*=travelNotesLinkDiv]').css({display: "block"});

    $(document).on("click", "a.travelNotesLink", function () {

        var title = $(this).text();
        var note = $(this.parentNode.parentNode).find('div.detailNote').html()

        $('div#additionaInfoDialog h2[id*=additionalInfoHeader]').html(title);
        $('div#additionaInfoDialog div#additionalInfoNotes').html(note);

        var documentHeight = $(document).height();
        var windowHeight = $(window).height() / 2;

        $('div#additionalInfoPage').css({ display: 'block', height: documentHeight });
        $('div#additionaInfoDialog').css({ top: windowHeight+'px'});
        currentPagePosition = window.pageYOffset;
        $(document).scrollTop(0);

        return false;
    });

    $(document).on("click", "a[id*=closeinfodialog]", function () {
        $('div#additionalInfoPage').css('display', 'none');
        $(document).scrollTop(currentPagePosition);
        return false;
    });
}
