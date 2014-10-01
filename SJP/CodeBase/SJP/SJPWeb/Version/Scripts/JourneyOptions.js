var resultLinkClicked = false;

$(document).ready(function () {
    setupResults();
    setupResultRadios();
    setupRetailers();
    initPrinterFriendly();
    setupPrinterFriendly();
    setupDialogClose();
    setupInformationTips();
    setupAdditionalNotesDialogs($('tr.expanded'));
    setupDropDowns();

    // Following lines are to rebind the dropdowns after ajax update of the update panel
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function () {
        setupResults();
        setupResultRadios();
        setupRetailers();
        initPrinterFriendly();
        setupPrinterFriendly();
        setupDialogClose();
        setupInformationTips();
        setupAdditionalNotesDialogs($('tr.expanded'));
        setupDropDowns();
    });


});

// Sets up result to work on client side rather than posting back to show different journeys
// Also sets up retailers and book travel button
function setupResults() {
    $('tr.summaryRow td.transport div.arrow_btn').addClass('js_arrow_btn').find('input').hide();
    if ($('tr.summaryRow td.transport div.arrow_btn').length > 0) {
        try{
            $('tr.summaryRow td.transport div.arrow_btn').html('<a class="js_arrow_btn_link" href="#">&nbsp;</a>').attr('role', 'select').attr('title', $('tr.summaryRow td.transport div.arrow_btn').find('input:first').attr('title'));
        }
        catch (err) {
            //Handle errors here
        }
    }
        
    $('tr.summaryRow td.transport').die();
    // bind client side events and stop it to propagate so updatepanel don't fire
    $('tr.summaryRow td.transport').live('click', function (event) {

        var summaryTable = $(this).parents('table.legSummary');
        var detailRow = $(this).parent().next('tr.detailsRow');
        resultLinkClicked = true;
        $(this).nextUntil('td.select input:radio').find(':radio').click();

        var currentlyExpanded = detailRow.hasClass('expanded');

        // collapse the currently expanded one
        $(summaryTable).find('tr.expanded').find('div.rowDetail').hide('blind', {}, 2000);
        $.scrollTo($(summaryTable).prev(), 3000);
        $(summaryTable).find('tr.expanded').removeClass('expanded').addClass('collapsed');
        $(summaryTable).find('div.js_arrow_btn').css('background-position', '0 0');

        // if the currently expanded one is not the clicked one expand it
        if (!currentlyExpanded) {
            detailRow.find('div.rowDetail').show('blind', {}, 2000, function () {
                detailRow.find('div.row2').each(function (index) {
                    equalHeight($(this).children('div'), $(this).find('div.lineImage'));
                });
            

            });
            setupAdditionalNotesDialogs(detailRow);
            detailRow.addClass('expanded').removeClass('collapsed');


            $(this).find('div.js_arrow_btn').css('background-position', '0 -46px');
            //setupRetailers();
            setupPrinterFriendly();
        }
        else {
            //setupRetailers();
            setupPrinterFriendly();
        }


        event.preventDefault();

    });

    $('tr.detailsRow div.close input.linkButton').die();
    $('tr.detailsRow div.close input.linkButton').live('click', function (event) {
        var detailRow = $(this).parents('tr.detailsRow');
        detailRow.find('div.rowDetail').slideUp(1600);
        $.scrollTo($(detailRow).prev(), 1000);
        detailRow.removeClass('expanded').addClass('collapsed');
        $(detailRow).prev().find('div.js_arrow_btn').css('background-position', '0 0');
        //setupRetailers();
        setupPrinterFriendly();
        event.preventDefault();
    });

    $('tr.detailsRow div.rowDetail div.leg div.columnDetail div.arrow_btn').addClass('js_arrow_btn').find('input').hide();
    if ($('tr.detailsRow div.rowDetail div.leg div.columnDetail div.arrow_btn').length > 0) {
        try{
            $('tr.detailsRow div.rowDetail div.leg div.columnDetail div.arrow_btn').attr('role', 'select').attr('title', $('tr.detailsRow div.rowDetail div.leg div.columnDetail div.arrow_btn').find('input:first').attr('title'));
        }
        catch (err) {
            //Handle errors here
        }
    }
    
     $('tr.detailsRow div.rowDetail div.leg div.columnDetail input.linkButton').die();
     // bind client side events and stop it to propagate so updatepanel don't fire
     $('tr.detailsRow div.rowDetail div.leg div.columnDetail input.linkButton').live('click', CycleOrCarLegClickHandler);

     $('tr.detailsRow div.rowDetail div.leg div.columnDetail div.arrow_btn').die();
     // bind client side events and stop it to propagate so updatepanel don't fire
     $('tr.detailsRow div.rowDetail div.leg div.columnDetail div.arrow_btn').live('click', CycleOrCarLegClickHandler);


 }

 function CycleOrCarLegClickHandler(event) {
     var legInstructionsDetailsId = getJSHdnSettingFieldValue('legInstructionBeforeId', $(this).parent().parent());
     var legInstructionsDetailElem = document.getElementById(legInstructionsDetailsId);
     if (legInstructionsDetailElem) {
         if ($(legInstructionsDetailElem).hasClass('collapsed')) {
             $(legInstructionsDetailElem).slideDown(700);
             $(legInstructionsDetailElem).removeClass('collapsed').addClass('expanded');
             $(event.target).parent().find('div.js_arrow_btn').css('background-position', '0 -42px');
         }
         else {
             $(legInstructionsDetailElem).slideUp(700);
             $(legInstructionsDetailElem).removeClass('expanded').addClass('collapsed');
             $(event.target).parent().find('div.js_arrow_btn').css('background-position', '0 -1px');
         }
         setupPrinterFriendly();
     }
     event.preventDefault();
 }

// Sets up bookTravelButton actions based on the unique retailers of outward and return journey
function setupRetailers() {
    var journeyKeys = getJSHdnSettingFieldValue('journeyKeys').split(',');
    var journeyHash = getJSHdnSettingFieldValue('journeyHash');
    
    var handOffUrl = getJSHdnSettingFieldValue('handoff');
    var retailerUrl = getJSHdnSettingFieldValue('retailer');

    var journeyId_out = null;
    var journeyRetailers_out = null;

    var journeyId_return = null;
    var journeyRetailers_return = null;

    var retailerCount = 0;
    var returnRequired = $('div.summary table.legSummary').length == 2;
    var bookTravelButton = getAspElement('btnTickets','input.submit');

    if (bookTravelButton) {

        if ($("div.summary table.legSummary tr.summaryRow td.select input:radio[checked='true']").length > 0) {
            $("div.summary table.legSummary tr.summaryRow td.select input:radio[checked='true']").each(function (index) {
                var returnJourney = getJSHdnSettingFieldValue('returnJourney', $(this).parentsUntil('div.summary').parent().first());

                // get the values from corresponding detail row
                if (returnJourney.toLowerCase() == "true") {
                    journeyId_return = getJSHdnSettingFieldValue('journeyId', $(this).parent().parent().next() /* get the detail row */);
                    journeyRetailers_return = getJSHdnSettingFieldValue('journeyRetailers', $(this).parent().parent().next() /* get the detail row */);
                }
                else {
                    journeyId_out = getJSHdnSettingFieldValue('journeyId', $(this).parent().parent().next() /* get the detail row */);
                    journeyRetailers_out = getJSHdnSettingFieldValue('journeyRetailers', $(this).parent().parent().next() /* get the detail row */);
                }

            });
        }

        // Possible case of single outward and/or return journeys returned  and no radio buttons got displayed 
        if(journeyId_out == null || journeyId_return == null)
        {
            $("div.summary table.legSummary tr.expanded").each(function (index) {
                var returnJourney = getJSHdnSettingFieldValue('returnJourney', $(this).parentsUntil('div.summary').parent().first());

                // get the values from corresponding detail row
                if (returnJourney.toLowerCase() == "true") {
                    if (journeyId_return == null) {
                        journeyId_return = getJSHdnSettingFieldValue('journeyId', $(this)/* get the detail row */);
                        journeyRetailers_return = getJSHdnSettingFieldValue('journeyRetailers', $(this) /* get the detail row */);
                    }
                }
                else {
                    if (journeyId_out == null) {
                        journeyId_out = getJSHdnSettingFieldValue('journeyId', $(this) /* get the detail row */);
                        journeyRetailers_out = getJSHdnSettingFieldValue('journeyRetailers', $(this) /* get the detail row */);
                    }
                }

            });
        }

        retailers = getRetailers(journeyRetailers_out, journeyRetailers_return);

//        var out_return_Checked = ($("div.summary table.legSummary tr.summaryRow td.select input:radio[checked='true']").length == 2);

//        if($("div.summary table.legSummary tr.summaryRow td.select input:radio[checked='true']").length == 1 && returnRequired)
//        {
//            $("div.summary table.legSummary").each(function(i,elem){
//                if($(elem).find("tr.summaryRow").length == 1)
//                {
//                out_return_Checked = true;
//                }
//            })

//        }


//        if (returnRequired && !out_return_Checked) {
//            //$(bookTravelButton).fadeOut('slow');
//            $(bookTravelButton).attr('disabled', 'disabled');
//            return;
//        }

        if (retailers.length > 0) {
            // $(bookTravelButton).fadeIn('slow');

            if(retailers.length == 1)
            {
                $('img.retailerInNewWindow').show();
            }
            else
            {
                $('img.retailerInNewWindow').hide();
            }
            $(bookTravelButton).removeAttr('disabled');
            $(bookTravelButton).die().removeAttr('onclick');
            $(bookTravelButton).live('click', function (event) {
                
                event.preventDefault();
                urlbase = retailers.length > 1 ? retailerUrl : handOffUrl;
                url = urlbase + '?' + journeyKeys[0] + '=' + journeyHash;
                if (journeyId_out)
                    url += '&' + journeyKeys[1] + '=' + journeyId_out;
                if (journeyId_return)
                    url += '&' + journeyKeys[2] + '=' + journeyId_return;

                if (retailers.length == 1) {
                    url += '&' + journeyKeys[3] + '=' + retailers[0];
                    openWindow(url);

                }

                if (retailers.length > 1) {
                    location.href = url;
                }





            });
        }
        else {
            //$(bookTravelButton).fadeOut('slow');
            $(bookTravelButton).attr('disabled', 'disabled');
        }
    }

   

}

// Gets unique retailers for outward and return journey
function getRetailers(outputJourneyRetailers, returnJourneyRetailers) {
    var outArr = [];
    var retArr = [];
    // output retailers
    if (outputJourneyRetailers) {
        if (outputJourneyRetailers.length > 0)
            outArr = outputJourneyRetailers.split(',');
    }

    // return retailers
    if (returnJourneyRetailers) {
        if (returnJourneyRetailers.length > 0)
            retArr = returnJourneyRetailers.split(',');
    }

    return $.unique($.merge(outArr, retArr));

}

// Displays the printer friendly button
function initPrinterFriendly() {

    $('div.sjpContent div.printerFriendly').find('input').show();
    
}

// Sets up the printer friendly button based on the selected outward and return journey
function setupPrinterFriendly() {

    var journeyKeys = getJSHdnSettingFieldValue('journeyKeys').split(',');
    var journeyHash = getJSHdnSettingFieldValue('journeyHash');

    var printerFriendlyUrl = getJSHdnSettingFieldValue('printerFriendlyUrl');

    var journeyId_out = null;
    var journeyId_return = null;

    var journeyDetail_Out_Expanded = null;
    var journeyDetail_Return_Expanded = null;

    // Get selected journey id
    $('div.summary table.legSummary').find('tr.expanded').each(function (index) {
        var returnJourney = getJSHdnSettingFieldValue('returnJourney', $(this).parentsUntil('div.summary').parent().first());

        if (returnJourney.toLowerCase() == "true") {
            journeyId_return = getJSHdnSettingFieldValue('journeyId', $(this));
            journeyDetail_Return_Expanded = $(this).find('div.legsDetail div.leg div.expanded').length;
        }
        else {
            journeyId_out = getJSHdnSettingFieldValue('journeyId', $(this));
            journeyDetail_Out_Expanded = $(this).find('div.legsDetail div.leg div.expanded').length;
        }


    });

    // Update the click event for the link
    $('div.printerFriendly input.linkButton').die();
    $('div.printerFriendly input.linkButton').die().removeAttr('onclick');
    $('div.printerFriendly input.linkButton').live('click', function (event) {

        event.preventDefault();

        urlbase = printerFriendlyUrl;
        url = urlbase + '?' + journeyKeys[0] + '=' + journeyHash;
        if (journeyId_out) {
            url += '&' + journeyKeys[1] + '=' + journeyId_out;

            // Add expanded leg detail flag
            if (journeyDetail_Out_Expanded > 0) {
                url += '&' + journeyKeys[4] + '=1' ;
            }
        }
        if (journeyId_return) {
            url += '&' + journeyKeys[2] + '=' + journeyId_return;

            if (journeyDetail_Return_Expanded > 0) {
                url += '&' + journeyKeys[5] + '=1';
            }
        }


        openWindow(url);
    });

}

// Sets up the dialog boxes for journey web notes
function setupAdditionalNotesDialogs(container) {
    $(container).find('div.columnDetail div.detailNotes').each(function () {
        var refElem = $(this);

        // add css to get the height of the element
        $(this).css({ visibility: "hidden", display: "block" });

        var h = $(refElem).height();

        // reset the css added to find the height of the element
        $(this).css({ visibility: "", display: "" });
        $(this).hide();
        $(this).parent().prev().find('div.lineImage img').height($(this).parent().height() + 40); // Set line image height
        $(this).parent().find('a.travelNotesLink').show().bind('click', function (event) {
            event.preventDefault();
            $(refElem).dialog({
                title: $(this).find('span.linkText').html(),
                modal: true,
                resizable: true
            });


        });

        // Adjust line image height again in case there are gaps, 
        // also done above so that the original img height is reset to be "smaller"
        equalHeight($(this).parentsUntil('div.row2').parent().children('div'), $(this).find('div.lineImage'));

    });
}

// Sets up closure of dialogs when user clicks outside of the dialog
function setupDialogClose() {
    $('.ui-widget-overlay').die();
    $('.ui-widget-overlay').live('click', function () {
        $("div.detailNotes").each(function ()
        { $(this).dialog("close"); });
    });


}

function setupResultRadios() {
    $('tr.summaryRow td.select input:radio').die();
    $('tr.summaryRow td.select input:radio').live('click', function (event) {
        $(this).parents('table.legSummary').find('tr.selected').removeClass('selected');
        $(this).attr('checked', 'checked').parent().parent().addClass('selected');
        if (!$(this).parent().parent().next().hasClass('expanded')) {
            if ($(this).parents('table.legSummary').find('tr.expanded').length > 0 && !resultLinkClicked) {
               // $(this).parents('table.legSummary').find('tr.expanded').find('div.rowDetail').hide('blind', {}, 2000);
               // $.scrollTo($(this).parents('table.legSummary').prev(), 3000);
            }
        }
        setupRetailers();

        resultLinkClicked = false;
    });

    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(cancelResultRadioPostbacks);


}

function cancelResultRadioPostbacks(sender, args)
{

    var postbackelem = args.get_postBackElement();
    if (postbackelem.type == "radio" && $(postbackelem).is('tr.summaryRow td.select input:radio')) {
        args.set_cancel(true);
    }
    
}


// Sets up information tips
function setupInformationTips() {

    $('a.tooltip_information').qtip({
        position: {
            my: 'top center',
            at: 'bottom center',
            container: $('#MainContent'),
            adjust: {
                x: -20,
                y: -5,
                mouse: true,
                resize: true
            }
        },
        style: {
            classes: 'ui-tooltip-sjp'
        },
        show: {
            event: 'click mouseover mouseenter'
        },
        hide: {
            delay: 200,
            event: 'click mouseleave'
        }
    }).find('img:first').attr('title','');
}

function equalHeight(group, elem) {
    var tallest = 0;
    group.each(function () {
        var thisHeight = $(this).height();
        if (thisHeight > tallest) {
            tallest = thisHeight;
        }
    });

    if (elem) {
        elem.height(tallest);
        $(elem).find('img').height(tallest);
    }
    else {
        group.height(tallest);
    }
}

// Sets up the drop downs
function setupDropDowns() {
    $('div.selectJourneyDrop select').selectmenu({ width: 310, maxHeight: 200, positionOptions: { collision: "none"} });
}