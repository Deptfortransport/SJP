var sjpMonthNamesShort = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

$(document).ready(function () {
    initCalendar();
});

function initCalendar() {

    $('#sjp-calendar-tabs table.BlogCalendarTable td input').each(function () {
        var spElem = $('<span>' + $(this).val() + '</span>');
        $(this).hide().after(spElem);
    });

    $('input.monthTab').each(function () {
        $(this).css('display', 'none');
        $(this).after('<a href="#" class="small" id="' + $(this).attr('value') + '">' + $(this).attr('value') + '</a>');
    });

    $('#sjp-calendar-tabs table.BlogCalendarTable td.day').click(function () {
        var calendarSection = $(this).parentsUntil('div.calendarMonth').parent('div.calendarMonth');
        var month = $('#' + $(calendarSection).attr('id') + '_Month').val();
        var year = $('#' + $(calendarSection).attr('id') + '_Year').val();

        if (month && year) {
            if (month.length == 1)
                month = '0' + month;

            $(calendarSection).parentsUntil('#sjp-calendar-tabs').parent('#sjp-calendar-tabs').find('input:hidden:first').val($(this).find('span').html() + month + year);
        }
        $('#sjp-calendar-tabs table.BlogCalendarTable td.day').removeClass('selected');
        $(this).addClass('selected');
        var selectedDate = new Date(year, month - 1, $(this).find('span').html());
        if ($('div.calendarSection div.arrive').hasClass('active')) {
            $('div.journeyTime div.arrive .dateEntry').dateEntry('setDate', selectedDate);
        }
        else {
            $('div.journeyTime div.return .dateEntry').dateEntry('setDate', selectedDate);
        }
    });

    setupOutwardReturnCalendars();
    setupDateEntryInputBoxes();
    swapSJPCalendarContent();
}

function swapSJPCalendarContent() {

    //display the content related to the current li tab first

    $('#sjp-calendar-tabs ul.tabbed-headers').each(function () {

        if ($(this).css('display') == 'block') {

           // $(this).find('li').removeClass('current');
           // $(this).find('li:first').addClass('current');

            $(this).find('li a').each(function () {

                if ($(this).parent().attr('class') == 'current') {
                    var currentId = $(this).html();
                    $('#sjp-calendar-tabs div#BlogCalendar').find('div').css('display', 'none');
                    $('#sjp-calendar-tabs div#BlogCalendar').find('div.' + currentId).css('display', 'block');
                }

                $(this).click(function () {
                    var id = $(this).html();

                    $(this).parent().siblings().removeClass('current');
                    $(this).parent().addClass('current');

                    $('#sjp-calendar-tabs div#BlogCalendar').find('div').css('display', 'none');
                    $('#sjp-calendar-tabs div#BlogCalendar').find('div.' + id).css('display', 'block');

                    return false;
                });
            });
        }
    });
}

function setupOutwardReturnCalendars() {
    $('div.journeyTime div.toggle input.linkButton').bind('click', function (event) { event.preventDefault(); });
    $('div.calendarSection div.return').height($('div.journeyTime div.return').height()).css('top', $('div.journeyTime div.return').position().top).css('left', '250px');
    $('div.journeyTime div.arrive').bind('click', function () {
        if(!$(this).hasClass('active'))
        {
            $(this).addClass('active');
            $('div.journeyTime div.return').removeClass('active');
            $('div.calendarSection div.return').animate(
                        { height: $('div.journeyTime div.return').height(), top: $('div.journeyTime div.return').position().top }, 200
                ).animate({ width: 'toggle', left: '+=250' }, 300, function () {
                $('div.calendarSection div.arrive').animate(
                    { width: 'toggle', left: '0' }, 200
                ).animate(
                        { height: '100%' }, 300
                ).addClass('active');
            }).removeClass('active');  
        }  
    });

    $('div.journeyTime div.return').bind('click', function () {
        if(!$(this).hasClass('active'))
        {
            $(this).addClass('active');
            $('div.journeyTime div.arrive').removeClass('active');
            $('div.calendarSection div.arrive').animate(
                        { height: $('div.journeyTime div.arrive').height() }, 200
                ).animate({ width: 'toggle', left: '+=250' }, 300, function () {
                $('div.calendarSection div.return').animate(
                    { width: 'toggle', left: '0' }, 200
                ).animate(
                        { top: '0', height: '100%'  }, 300
                ).addClass('active');
            }).removeClass('active');
        }

    }); 
}

function setupDateEntryInputBoxes() {

    $('div.journeyTime div.arrive .dateEntry').dateEntry({ dateFormat: 'dmy/', minDate: getCalendarMinDate(false), maxDate: getCalendarMaxDate(false) }).change(function () {
        var selectedDate = $('div.journeyTime div.arrive .dateEntry').dateEntry('getDate');
        $('div.calendarSection div.arrive td.day').removeClass('selected');
        $('div.calendarSection div.arrive ul#monthHeader li').removeClass('current');
        $('div.calendarSection div.arrive ul#monthHeader li a[id="' + sjpMonthNamesShort[selectedDate.getMonth()] + '"]').parent().addClass('current');
        $('div.calendarSection div.arrive div.calendarMonth').removeClass('current').hide();
        $('div.calendarSection div.arrive div.' + sjpMonthNamesShort[selectedDate.getMonth()]).show().addClass('current');
        $('div.calendarSection div.arrive div.' + sjpMonthNamesShort[selectedDate.getMonth()] + ' td.day span').each(function () {
            var selectedDay = (selectedDate.getDate()).toString();
            if (selectedDay.length == 1) selectedDay = '0' + selectedDay;
            if ($(this).html() == selectedDay) {
                $(this).parent().addClass('selected');
                var month = (selectedDate.getMonth() + 1).toString();
                var year = selectedDate.getFullYear();
                if (month.length == 1) month = '0' + month;
                $(this).parentsUntil('#sjp-calendar-tabs').parent('#sjp-calendar-tabs').find('input:hidden:first').val(selectedDay + month + year.toString());
                return false;
            }
        });

    });

    $('div.journeyTime div.return .dateEntry').dateEntry({ dateFormat: 'dmy/', minDate: getCalendarMinDate(true), maxDate: getCalendarMaxDate(true) }).change(function () {
        var selectedDate = $('div.journeyTime div.return .dateEntry').dateEntry('getDate');
        $('div.calendarSection div.return td.day').removeClass('selected');
        $('div.calendarSection div.return ul#monthHeader li').removeClass('current');
        $('div.calendarSection div.return ul#monthHeader li a[id="' + sjpMonthNamesShort[selectedDate.getMonth()] + '"]').parent().addClass('current');
        $('div.calendarSection div.return div.calendarMonth').removeClass('current').hide();
        $('div.calendarSection div.return div.' + sjpMonthNamesShort[selectedDate.getMonth()]).show().addClass('current');
        $('div.calendarSection div.return div.' + sjpMonthNamesShort[selectedDate.getMonth()] + ' td.day span').each(function () {
            var selectedDay = (selectedDate.getDate()).toString();
            if (selectedDay.length == 1) selectedDay = '0' + selectedDay;
            if ($(this).html() == selectedDay) {
                $(this).parent().addClass('selected');
                var month = (selectedDate.getMonth() + 1).toString();
                var year = selectedDate.getFullYear();
                if (month.length == 1) month = '0' + month;
                $(this).parentsUntil('#sjp-calendar-tabs').parent('#sjp-calendar-tabs').find('input:hidden:first').val(selectedDay + month + year.toString());
                return false;
            }
        });
    });

    $('div.journeyTime span.dateEntry_control').hide();
}

function getCalendarMinDate(isReturn) {
    var firstDay = null;
    if (isReturn) {
        firstDay = $('div.calendarSection div.return div.calendarMonth td.day:first span');
    }
    else {
        firstDay = $('div.calendarSection div.arrive div.calendarMonth td.day:first span');
    }


    var calendarSection = $(firstDay).parentsUntil('div.calendarMonth').parent('div.calendarMonth');
    var month = $('#' + $(calendarSection).attr('id') + '_Month').val();
    var year = $('#' + $(calendarSection).attr('id') + '_Year').val();

    return new Date(year, month-1, firstDay.html());


}

function getCalendarMaxDate(isReturn) {
    var lastDay = null;
    if (isReturn) {
        lastDay = $('div.calendarSection div.return div.calendarMonth td.day:last span');
    }
    else {
        lastDay = $('div.calendarSection div.arrive div.calendarMonth td.day:last span');
    }


    var calendarSection = $(lastDay).parentsUntil('div.calendarMonth').parent('div.calendarMonth');
    var month = $('#' + $(calendarSection).attr('id') + '_Month').val();
    var year = $('#' + $(calendarSection).attr('id') + '_Year').val();

    return new Date(year, month-1, lastDay.html());
}

