var sjpMonthNamesShort = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

// Initialise javascript stuff
$(document).ready(function () {
    setupDropDowns();
    initCalendar();
    setupApplyFilter();
    setupResults();

    // Following lines are to rebind the dropdowns after ajax update of the update panel
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function () {
        initCalendar();
        setupResults();
        setupDropDowns();
    });

});

// Sets drop downs
function setupDropDowns() {
    $('div.tnOptions div.tnfilter select').selectmenu({ width: 285, maxHeight: 300, positionOptions: { collision: "none"} }); 

}

// Sets up calendar to work client side
function initCalendar() {
    var startDateElemVal = getJSHdnSettingFieldValue('calendarStartDate', $('div.tnOptions'));
    var endDateElemVal = getJSHdnSettingFieldValue('calendarEndDate', $('div.tnOptions'));

    var startDate = new Date();
    var endDate = new Date().setMonth(new Date().getMonth() + 4, 1);

    if (startDateElemVal && startDateElemVal.length > 0) {
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
        spinnerImage: ''
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

// Displays the please wait image when apply filter selected, 
// used to provide immediate feed back
function setupApplyFilter() {

    if ($('div[id*=newsProgress]').hasClass('hide')) {
        $('div[id*=newsProgress]').css('display', 'none');
    }

    $('input[id*=filterNews]').die();
    $('input[id*=filterNews]').live('click', function () {
        $('div.mainSection div.travelnews').css('display', 'none');
        $('div[id*=newsProgress]').css('display', 'block');
    });
}

// Sets result client side sliding open/close effects
function setupResults() {

    // Set up the expanding (and collapsing) olympic severity news section
    $('div[id*=tnOlympicHeading] div.arrow_btn').addClass('js_arrow_btn');
    $('div[id*=tnOlympicHeading]').die();
    // bind client side events and stop it to propagate so updatepanel don't fire
    $('div[id*=tnOlympicHeading]').live('click', function (event) {
        var summaryDiv = $(this).parent();
        var detailDiv = $('div[id*=olympicImpactContainer]');

        if (detailDiv.hasClass('expanded')) {
            detailDiv.slideUp(700);
            detailDiv.removeClass('expanded').addClass('collapsed');
            //$('div.travelnews div.impact div.impactHeading').removeClass('active');
            $(this).removeClass('active');
        } else {
            detailDiv.slideDown(700);
            detailDiv.addClass('expanded').removeClass('collapsed');
            $(this).addClass('active');
        }
        event.preventDefault();
    });

    // Set up the expanding (and collapsing!) other travel news section
    $('div[id*=tnOtherHeading] div.arrow_btn').addClass('js_arrow_btn');
    $('div[id*=tnOtherHeading]').die();
    // bind client side events and stop it to propagate so updatepanel don't fire
    $('div[id*=tnOtherHeading]').live('click', function (event) {
        var summaryDiv = $(this).parent();
        var detailDiv = $('div[id*=otherImpactContainer]');

        if (detailDiv.hasClass('expanded')) {
            detailDiv.slideUp(700);
            detailDiv.removeClass('expanded').addClass('collapsed');
            //$('div.travelnews div.impact div.impactHeading').removeClass('active');
            $(this).removeClass('active');
        } else {
            detailDiv.slideDown(700);
            detailDiv.addClass('expanded').removeClass('collapsed');
            $(this).addClass('active');
        }
        event.preventDefault();
    });

    // Set up the expanding (and collapsing!) other travel news sub-sections
    $('div.travelnews div.severity div.severityHeading div.arrow_btn').addClass('js_arrow_btn');
    $('div.travelnews div.severity div.severityHeading').die();
    // bind client side events and stop it to propagate so updatepanel don't fire
    $('div.travelnews div.severity div.severityHeading').live('click', function (event) {
        var summaryDiv = $('div.travelnews div.severity');
        var detailDiv = $(this).next('div.tnContainer');

        var currentlyExpanded = detailDiv.hasClass('expanded');

        // collapse the currently expanded one
        $(summaryDiv).find('div.expanded').slideUp(700);
        $(summaryDiv).find('div.expanded').removeClass('expanded').addClass('collapsed');
        $('div.travelnews div.severity div.severityHeading').removeClass('active');

        // if the currently expanded one is not the clicked one expand it
        if (!currentlyExpanded) {
            detailDiv.slideDown(700);
            detailDiv.addClass('expanded').removeClass('collapsed');
            $(this).addClass('active');
        }

        event.preventDefault();
    });

}

