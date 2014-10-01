
// Run straight away rather than waiting for page to be ready
setupDeviceStylesheet();

// COMMON PAGE READY FUNCTIONS
$(document).ready(function () {
    displayJavascriptControls();
    setupMenuNav();
    setMainContentMinHeight();
    maps();

    // Following lines are to rebind the jquery plugins or js events after ajax update of the update panel
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function () {
        displayJavascriptControls();        
        setupJQueryMobileStyling();
        maps();
    });
});

// Displays any controls which only work with javascript by removing the js css class
function displayJavascriptControls() {
    $('div').removeClass('jshide');
    $('a').removeClass('jshide');
    $('input').removeClass('jshide');
    $('div').removeClass('jsshow');
    $('input').removeClass('jsinput');
}

// Rebinds the jquery mobile styling the elements on the page. 
// Assumes there is only one "sjpForm"
function setupJQueryMobileStyling() {
    $('.sjpForm').trigger('create');
}

// Detects the device and requests updates to use the device appropriate stylesheet
function setupDeviceStylesheet() {
    //  css file based on the device
    var controlCss;
    //  get the device agent and conver to lover case
    var deviceAgent = navigator.userAgent.toLowerCase();

    // Android
    if (deviceAgent.match(/android/i)) {

        controlCss = "./version/styles/device/android-320.css"; // default

        if (deviceAgent.match(/htc_wildfires/i)) { // htc wildfire s
            controlCss = "./version/styles/device/android-320.css";
        }
        else if (deviceAgent.match(/htc magic/i)) { // htc magic
            controlCss = "./version/styles/device/android-320.css";
        }
        else if (deviceAgent.match(/shw-m110s/i)) { // samsung galaxy s
            controlCss = "./version/styles/device/android-320.css";
        }
        else if (deviceAgent.match(/s5830/i)) { // samsung galaxy ace s5830
            controlCss = "./version/styles/device/android-320.css";
        }
        else if (deviceAgent.match(/e10i/i)) { // sonyericsson x10 mini/e10i
            controlCss = "./version/styles/device/android-240.css";
        }
    }
    // Iphone
    else if (deviceAgent.match(/iphone/i)) {
        controlCss = "./version/styles/device/iphone.css";
    }
    // Blackberry
    else if (deviceAgent.match(/blackberry/i)) {

        controlCss = "./version/styles/device/android-320.css"; // default

        if (deviceAgent.match(/9300/i)) { // blackberry curve 9300
            controlCss = "./version/styles/device/android-320.css";
        }
        else if (deviceAgent.match(/8520/i)) { // blackberry curve 8520
            controlCss = "./version/styles/device/android-320.css";
        }
    }
    // Windows phone
    else if (deviceAgent.match(/windows phone/i)) {

        controlCss = "./version/styles/device/windows-phone.css"; // default

        if (deviceAgent.match(/lumia 800/i)) { // nokia lumia 800
            controlCss = "./version/styles/device/windows-phone.css";
        }
        else if (deviceAgent.match(/lg-e900/i)) { // lg e900
            controlCss = "./version/styles/device/windows-phone.css";
        }
    }

    if (controlCss) {
        document.getElementById("cssLink").setAttribute("href", controlCss);
    }
}

// Setup the top menu by cloning the footer menu into a holder, 
// and attaching a click event to the header menu link
function setupMenuNav() {
    
    $('#nav').clone().appendTo('#navHolder').addClass('topNav');
    $('a#menu').toggle(
		function () {
		    $('.topNav').fadeIn(250);
		    $('.menuWrap').addClass("menuWrapUp");
		},
		function () {
		    $('.topNav').fadeOut(250);
		    $('.menuWrap').removeClass("menuWrapUp");
		    return false;
		});
}

// COMMON FUNCTIONS

// Open page and return "true" if open was unsuccessful
function openWindow(url) {
    var x = window.open(url);
    return (x == null);
}

// Gets the html element with matching serverId
// This method may return multiple elements. To restrict result pass the closest parent container
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

//Sets the minimum height of the MainContent Section so that the footer is always at the bottom of the screen
function setMainContentMinHeight() {
    $('div#MainContent').css('min-height', $(window).height() - $('div[id*=pnlHeader]').outerHeight() - $('div[id*=pnlFooter]').outerHeight() - 20);
}

// Functionality used in Venue maps and cycle park maps
function maps() {
    /* This function makes a div scrollable with android and iphone */
    $(document).ready(function () {

        function isTouchDevice() {
            /* Added Android 3.0 honeycomb detection because touchscroll.js breaks
            the built in div scrolling of android 3.0 mobile safari browser */
            if ((navigator.userAgent.match(/android 3/i)) ||
				(navigator.userAgent.match(/honeycomb/i)))
                return false;
            try {
                document.createEvent("TouchEvent");
                return true;
            } catch (e) {
                return false;
            }
        }

        function touchScroll(className) {
            if (isTouchDevice()) { //if touch events exist...
                $(className).each(function (index, el) {
                    var scrollStartPosY = 0;
                    var scrollStartPosX = 0;

                    if (el) {
                        el.addEventListener("touchstart", function (event) {
                            scrollStartPosY = this.scrollTop + event.touches[0].pageY;
                            scrollStartPosX = this.scrollLeft + event.touches[0].pageX;
                            //event.preventDefault(); // Keep this remarked so you can click on buttons and links in the div
                        }, false);

                        el.addEventListener("touchmove", function (event) {
                            // These if statements allow the full page to scroll (not just the div) if they are
                            // at the top of the div scroll or the bottom of the div scroll
                            // The -5 and +5 below are in case they are trying to scroll the page sideways
                            // but their finger moves a few pixels down or up.  The event.preventDefault() function
                            // will not be called in that case so that the whole page can scroll.
                            if ((this.scrollTop < this.scrollHeight - this.offsetHeight &&
						    this.scrollTop + event.touches[0].pageY < scrollStartPosY - 5) ||
						    (this.scrollTop != 0 && this.scrollTop + event.touches[0].pageY > scrollStartPosY + 5))
                                event.preventDefault();
                            if ((this.scrollLeft < this.scrollWidth - this.offsetWidth &&
						    this.scrollLeft + event.touches[0].pageX < scrollStartPosX - 5) ||
						    (this.scrollLeft != 0 && this.scrollLeft + event.touches[0].pageX > scrollStartPosX + 5))
                                event.preventDefault();
                            this.scrollTop = scrollStartPosY - event.touches[0].pageY;
                            this.scrollLeft = scrollStartPosX - event.touches[0].pageX;
                        }, false);
                    }
                });
            }
        }

        touchScroll(".wrapper");

        // May be more than one image to zoom
        $(".anchorzoomout").each(function (index, element) {
            $(element).css({ opacity: 0.4 });

            $(element).click(function () {
                $(".wrapper").each(function (i, ele) {
                    $(ele).removeClass("zoomIn");
                });

                $(".anchorzoomout").each(function (i, ele) {
                    $(ele).css({ opacity: 0.4 });
                });

                $(".anchorzoomin").each(function (i, ele) {
                    $(ele).css({ opacity: 1 });
                });
                window.scrollTo(0, 0);
            });
        });

        $(".anchorzoomin").each(function (index, element) {
            $(element).click(function () {
                $(".wrapper").each(function (i, ele) {
                    $(ele).addClass("zoomIn");
                });

                $(".anchorzoomin").each(function (i, ele) {
                    $(ele).css({ opacity: 0.4 });
                });

                $(".anchorzoomout").each(function (i, ele) {
                    $(ele).css({ opacity: 1 });
                });
            });
        });
    });
}
