var oldVenueSelected = null;

// Initialise javascript stuff
$(document).ready(function () {
    setJSHdnSettingFieldValue('jsEnabled', $('div.locationControl'), 'true');
    setupDropDowns();
    setupMobilityNeeds();

    initTabs();
    // Order is importand here as setupReturnDateTimeBoxes() always execute after checkBox plugin execute
    setupReturnDateTimeBoxes();
    initCalendar();
    setupRefreshPanels();
    setupInformationTips();
    // Following lines are to rebind the jquery plugins or js events after ajax update of the update panel
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function () {
        setupInformationTips();
        setupReturnDateTimeBoxes();
        initCalendar();
        setupRefreshPanels();
        setJSHdnSettingFieldValue('jsEnabled', $('div.locationControl'), 'true');
    });
});

// Sets location suggest drop downs
function setupDropDowns() {
    $('div.sjpContent div.locationControl select.locationDrop').selectmenu({ width: 270, maxHeight: 300, positionOptions: { collision: "none"} });
    $('div.sjpContent div.eventDate select').selectmenu({ width: 56, maxHeight: 300, positionOptions: { collision: "none"} });
    $('div.selectJourneyDrop select').selectmenu({ width: 245, maxHeight: 200, positionOptions: { collision: "none"} });

    // Following lines are to rebind the dropdowns after ajax update of the update panel
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function () {
        $('div.sjpContent div.locationControl select.locationDrop').selectmenu({ width: 270, maxHeight: 300, positionOptions: { collision: "none"} });
        $('div.sjpContent div.eventDate select').selectmenu({ width: 56, maxHeight: 300, positionOptions: { collision: "none"} }); 
    });
}

// Init to make journey options tabs work only with javascript without postback
function initTabs() {

    $('div.sjp_jo_tab div.sjp_tab_headers ul').find('input').css('display', 'none');

    $('div.sjp_jo_tab div.sjp_tab_headers ul').find('input').each(function (elem) {

        // Get self containing the hidden active tab input
        var self = $(this);
        var headerlink = $('<a href="#" class="small" id="' + elem + '" title="' + $(this).attr('value') + '" >' + $(this).attr('value') + '</a>').click(
        function (event) {

            // Set the active tab class style
            $('div.sjp_jo_tab div.sjp_tab_headers ul li').removeClass('active');
            $(event.target).parent().addClass('active');

            // Set the highlight tab class style to be the class of this tab
            $('div.sjp_jo_tab div.sjp_tab_headers div.sjp_tab_highlight div').attr('class', $(event.target).parent().attr('class'))

            $('div.sjp_jo_tab div.content_wrapper div.tab').each(function (celem) {
                if (celem == $(event.target).attr('id')) {
                    $(this).show().addClass('active');

                    // Update hidden active tab input to the selected tab
                    var tabValue = $(self).val();
                    var hdnId = getAspElement("hdnActiveTab", "input", $('div.sjp_jo_tab'));
                    $(hdnId).val(tabValue);
                }
                else {
                    $(this).hide().removeClass('active');
                }

            });

            setTimeout('updateOptionTab()', 100);
            event.preventDefault();
        });

        $(this).after(headerlink);
        if ($(this).parent().hasClass('active')) { $(headerlink).parent().addClass('active'); }
    });


}


// Sets up refreshing of journey option tabs indiviual panels
// Updated to add the onblur handler as if the drop down changed by keyboard the panels were not refreshing
function setupRefreshPanels() {

    var toVenueLocationDrop = $('div#toLocation select.locationDrop');
    var fromVenueLocationDrop = $('div#fromLocation select.locationDrop');

    // If to location has venue drop down, attach to allow update of journey option tabs content
    if (toVenueLocationDrop.length > 0) {
        $('div#toLocation select.locationDrop').die();
        $('div#toLocation select.locationDrop').live('change', function (event) {
            if (oldVenueSelected != $('div#toLocation select.locationDrop').val()) {
                updateOptionTab();
                oldVenueSelected = $('div#toLocation select.locationDrop').val();
            }
            event.preventDefault();
            return false;
        }).live('blur', function () { $(this).change(); });
    }
    // If from location has venue drop down, attach to allow update of journey option tabs content
    else if (fromVenueLocationDrop.length > 0) {
        $('div#fromLocation select.locationDrop').die();
        $('div#fromLocation select.locationDrop').live('change', function (event) {
            if (oldVenueSelected != $('div#fromLocation select.locationDrop').val()) {
                updateOptionTab();
                oldVenueSelected = $('div#fromLocation select.locationDrop').val();
            }
            event.preventDefault();
            return false;
        }).live('blur', function () { $(this).change(); });
    }
   
}

function updateOptionTab() {
    var updateContainer = $('div.sjp_jo_tab div.content_wrapper div.active div.updateContent');

    var updatePanel = getAspElement('updateContent', 'div', updateContainer);
    if (updatePanel.length > 0) {
        try
        {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
             __doPostBack($(updatePanel).attr('id'), '');
           
        } catch (err)
        {
        }
    }
    
}

// Initialise the calendar
function initCalendar() {

    var startDateElemVal = getJSHdnSettingFieldValue('calendarStartDate',$('div.eventDate'));
    var endDateElemVal = getJSHdnSettingFieldValue('calendarEndDate', $('div.eventDate'));

    var startDate = new Date();
    var endDate = new Date().setMonth(new Date().getMonth()+4,1);

    if (startDateElemVal && startDateElemVal.length>0) {
        startDate = $.datepicker.parseDate('dd/mm/yy', startDateElemVal);
    }
    if (endDateElemVal && endDateElemVal.length > 0) {
        endDate = $.datepicker.parseDate('dd/mm/yy', endDateElemVal);
    }

    var buttonText = getJSHdnSettingFieldValue('calendar_ButtonText');
    var prevText = getJSHdnSettingFieldValue('calendar_PrevText');
    var nextText = getJSHdnSettingFieldValue('calendar_NextText');
    var monthNames = getJSHdnSettingFieldValue('calendar_MonthNames');
    var dayNames = getJSHdnSettingFieldValue('calendar_DayNames'); 

    $("input.dateEntry").dateEntry({
        dateFormat: 'dmy/',
        minDate: startDate,
        maxDate: endDate,
        spinnerImage:''
    }).datepicker({
        dateFormat: 'dd/mm/yy',
        showOn: "button",
        buttonImage: "./../version/images/presentation/datepicker.png",
        buttonImageOnly: true,
        minDate: startDate,
        maxDate: endDate,
        buttonText: buttonText,
        nextText: nextText,
        prevText: prevText,
        dayNamesMin: dayNames.split(','),
        monthNames: monthNames.split(',')
    });
}

// Sets additional mobility needs section expanding/collapsing
function setupMobilityNeeds() {
    var nonJsBtn = $('div.pjOptionsHead div.arrow_btn').addClass('js_arrow_btn').find('input');

    var dummyBtn = $('<input type="button" class="linkButton js_accessiblity_btn" value="' + $('div.pjOptionsHead span').html() + '" title="' + $(nonJsBtn).attr('title') + '" ></input>');

    $('div.pjOptionsHead div.arrow_btn').hide();

    $('div.pjOptionsHead span').hide().after(dummyBtn);

    // if mobility options selected expand the options by default
    if ($('div.pjOptions :radio[checked]').length > 0 && !($('div.pjOptions :radio:last').attr('checked'))) {
        $('div.pjOptions').show('blind', 300).switchClass('collapsed', 'expanded');
        $('div.pjOptionsHead').find('input.js_accessiblity_btn').css('background-position', '0 -49px');
    }
        

    $('div.pjOptionsHead').die();
    $('div.pjOptionsHead').bind('click', function (event) {
        if ($(this).next().hasClass('collapsed')) {
            $(this).next().show('blind', 300).switchClass('collapsed', 'expanded');
            $(this).find('input.js_accessiblity_btn').css('background-position', '0 -49px');
        }
        else {
            $(this).next().hide('blind', 300).switchClass('expanded', 'collapsed');
            $(this).find('input.js_accessiblity_btn').css('background-position', '0 -4px')
        }

        event.preventDefault();
    });

    // Set up the fewer changes box enable/disable
    if ($('div.pjOptions .noMobilityNeedsRadio input').attr('checked')) {
        $('div.pjOptions .fewerInterchangeCheck input').attr('disabled', 'disabled').addClass('disabled');
    }
    $('div.pjOptions .accessibleOption').bind('click', function (event) {
        if ($('div.pjOptions .noMobilityNeedsRadio input').attr('checked')) {
            $('div.pjOptions .fewerInterchangeCheck input').attr('disabled', 'disabled').addClass('disabled');
        }
        else {
            $('div.pjOptions .fewerInterchangeCheck input').removeAttr('disabled').removeClass('disabled');
        }
    });
}

// Set up return date time boxes enable/disable
function setupReturnDateTimeBoxes() {

    var isReturnOnly = getJSHdnSettingFieldValue('returnOnly', $('div.eventDate'));

    if (isReturnOnly != 'True') {
        // If return journey check box available and not checked, disable the input
        if (!$('.isReturn input').attr('checked')) {
            $('div.return').find('div.dateSelect input.dateEntry').attr('disabled', 'disabled').addClass('disabled');
            $('div.return').find('div.timePicker select').attr('disabled', 'disabled');
            $('div.return').find('div.timePicker select').selectmenu("disable");
        }
        $('.isReturn input').bind('click', function (event) {
            if ($('.isReturn input').attr('checked')) {
                $('div.return').find('div.dateSelect input.dateEntry').removeAttr('disabled').removeClass('disabled');
                $('div.return').find('div.timePicker select').removeAttr('disabled');
                $('div.return').find('div.timePicker select').selectmenu("enable");

                // Set the selected return date to be the outward date
                var sOutwardDate = $('div.outward').find('div.dateSelect input.dateEntry').val();
                var sReturnDate = $('div.return').find('div.dateSelect input.dateEntry').val();

                var outwardDate = new Date();
                var returnDate = new Date();

                outwardDate = $.datepicker.parseDate('dd/mm/yy', sOutwardDate);
                returnDate = $.datepicker.parseDate('dd/mm/yy', sReturnDate);

                $('div.return').find('div.dateSelect input.dateEntry').val(sOutwardDate);
            }
            else {
                $('div.return').find('div.dateSelect input.dateEntry').attr('disabled', 'disabled').addClass('disabled');
                $('div.return').find('div.timePicker select').attr('disabled', 'disabled');
                $('div.return').find('div.timePicker select').selectmenu("disable");

            }

        });
    }

    // On form submit remove the disabled attribute as this causes the original asp.net dropdown to loose the selected index
    // Selected index will reset to 0 if the dropdown is disabled and postback
    $("form").submit(function () {
        $('div.return').find('div.timePicker select').removeAttr('disabled'); 
    });

}

// Sets up information tips
function setupInformationTips() {

    // From and To Locations
    $('div.locationRow a.tooltip_information').qtip({
        position: {
            my: 'bottom center',
            at: 'top center',
            container: $('#MainContent'),
            adjust: {
                x: -20,
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
            event: 'click mouseleave',
            fixed: true
        }

    }).find('img:first').attr('title', '');

    // Mobility options
    $('div.pjOptions a.tooltip_information1').qtip({
        position: {
            my: 'bottom center',
            at: 'top center',
            container: $('#MainContent'),
            adjust: {
                x: -20,
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
            event: 'click mouseleave',
            fixed: true
        }

    }).find('img:first').attr('title', '');
    
}


