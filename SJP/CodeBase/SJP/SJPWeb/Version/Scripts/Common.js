// Open page and return "true" if open was unsuccessful
function openWindow(url) {
    var x = window.open(url);
    return (x == null);
}

// Gets the html element with matching serverId
// This method may return multiple elements. To restrict result pass the most closest parent container
function getAspElement(elemServerId, selector, container) {
    var elem = null;

    if (container) {
        elem  = $(container).find(selector + '[name$="' + elemServerId + '"]');
    }
    else
        elem = $(selector + '[name$="' + elemServerId + '"]');

    if ($(elem).length <= 0) {
        if (container) {
            elem = $(container).find(selector + '[id$="' + elemServerId + '"]');
        }
        else
            elem = $(selector + '[id$="' + elemServerId + '"]');
    }

    return $(elem);

}

// Gets the  hidden elements inside div with jssettings class
function getJSHdnSettingFieldValue(elemServerId, container) {

    var hdnElem = getAspElement(elemServerId, 'div.jssettings input:hidden', container);

    if (hdnElem)
        return hdnElem.val();

    return null;
}

// Gets the  hidden elements inside div with jssettings class
function setJSHdnSettingFieldValue(elemServerId, container, value) {

    var hdnElem = getAspElement(elemServerId, 'div.jssettings input:hidden', container);

    if (hdnElem)
        return hdnElem.val(value);

    return null;
}

// Make text fader for controls i.e. tool tips
function makeSJPTextFader(containerSelector, contentSelector, displayType) {
    $(containerSelector + ' ' + contentSelector + ':first').css('display', displayType);

    // $(containerSelector + ' a.next, ' + containerSelector + ' a.prev').click(function(){
    $(containerSelector + ' a.next').click(function () {

        $(containerSelector + ' ' + contentSelector).each(function () {
            if ($(this).css('display') == displayType) {
                $(this).fadeOut('medium', function () {
                    if ($(this).next(contentSelector).length) {
                        $(this).next(contentSelector).fadeIn('medium');
                    }
                    else {
                        $(containerSelector + ' ' + contentSelector + ':first').fadeIn('medium');
                    }
                });
            }
        });
        return false;
    });
    $(containerSelector + ' a.prev').click(function () {

        $(containerSelector + ' ' + contentSelector).each(function (intIndex) {
            if ($(this).css('display') == displayType) {
                $(this).fadeOut('medium', function () {
                    if ($(this).prev(contentSelector).length) {
                        $(this).prev(contentSelector).fadeIn('medium');
                    } else {
                        $(containerSelector + ' ' + contentSelector + ':last').fadeIn('medium');
                    }
                });
            }
        });
        return false;
    });
}

$(document).ready(function () {

    $("#top-tips div.navigation .navButton").hide();
    $("#top-tips div.navigation a.prev").show();
    $("#top-tips div.navigation a.next").show();
    setUpSJPSnippetNavigation('div#Content div.snippet div.snippet-header');
    makeSJPTextFader("#top-tips", "div.content", "block");

});

// Function to write in the navigation for each snippet on a page
function setUpSJPSnippetNavigation(navigationContainer) {
  
    //for each of the navigations, work out the correct numbers
    $(navigationContainer).each(function () {

        //get id of the navigation parent
        var sliderSelector = '#' + $(this).parent().attr('id');

        //if it is did-you-know, then it will be text content, else img content
        //if(sliderSelector == '#did-you-know')
        var totalNumber = $(sliderSelector + ' div.content').size();
        //else
        //var totalNumber = $(sliderSelector + ' div.slider img').size();  

        // Grab current page element
        var elCurrent = $(sliderSelector + ' span.current');

        // Correctly set the total number of images in the slider 
        $(sliderSelector + ' span.total').html(totalNumber);

        // On click of 'previous' button, work out what the current page number should be
        $(sliderSelector + ' a.prev').click(function () {
            var currentNumber = parseInt($(sliderSelector + ' span.current').html());
            if (currentNumber == 1) {
                elCurrent.html(totalNumber)
            } else {
                elCurrent.html(currentNumber - 1)
            }
        })

        // On click of 'next' button, work out what the current page number should be
        $(sliderSelector + ' a.next').click(function () {
            var currentNumber = parseInt($(sliderSelector + ' span.current').html());
            if (currentNumber == totalNumber) {
                elCurrent.html(1)
            } else {
                elCurrent.html(currentNumber + 1)
            }
        })
    })

}


  
