// *** Update History ****
//-----------------------------------------------------------------------------------------------------------
// @ 13-07-2011 12:34 AP - Commented out the '#footer-share-this' elements click event in shareLinks function
//                         This is to make Share this page mailto link working
//-----------------------------------------------------------------------------------------------------------

// Variable to work out if the browser is IE7 or below
var lteIE7 = false /*@cc_on || @_jscript_version <= 5.7 @*/;
$(document).ready(function() {
    $(document).pngFix();    
    
    // hoverState("div.venue a img", 0.6);
    // hoverState("div.landing-child a img", 0.6);
    // hoverState("div.homepage-2012-content-container a img", 0.6);
    
    // scrolling filmstrip on the making_it_happen and other level 1 landing pages
    $("div#scrollable-content").smoothDivScroll({scrollingSpeed: 2, mouseDownSpeedBooster: 1, autoScroll: "onstart", autoScrollDirection: "left", autoScrollSpeed: 1, visibleHotSpots: "always", hotSpotsVisibleTime: 9, startAtElementId: "startAtMe"});        
    //hides the scrollbar that exists for non javascript users
    $("div#scrollable-content div.scrollWrapper").css('overflow','hidden');
    
    // scrolling filmstrip on the image_gallery page
    $("div#scrollable-content-small").smoothDivScroll({scrollingSpeed: 2, mouseDownSpeedBooster: 1, autoScroll: "onstart", autoScrollDirection: "left", autoScrollSpeed: 1, visibleHotSpots: "always", hotSpotsVisibleTime: 9, startAtElementId: "startAtMe", scrollWrapper: "div.scrollWrapperSmall", scrollableArea:    "div.scrollableAreaSmall"});        
    $("div#scrollable-content-small div.scrollWrapperSmall").css('overflow','hidden');
    
    // scrolling filmstrip on the homepage for the webcams
    $("div.scrollable-content-tiny").smoothDivScroll({scrollingSpeed: 2, mouseDownSpeedBooster: 1, autoScroll: "onstart", autoScrollDirection: "left", autoScrollSpeed: 1, visibleHotSpots: "always", hotSpotsVisibleTime: 9, startAtElementId: "startAtMe", scrollWrapper: "div.scroll-wrapper-tiny", scrollableArea:    "div.scrollable-area-tiny"});        
    $("div.scrollable-content-tiny div.scroll-wrapper-tiny").css('overflow','hidden');
     
    //sliding down the second navigation    
    slideNavigationContent('a.header-dropdown');
    shareLinks();
    externalLinks();

    // show the venue hover states
    showVenues("div#venues");
    
    //swapCSS('a.font-css-swap', '#font-stylesheet');
    //swapCSS('a.main-css-swap', '#main-css');
    
    // Function that grabs all the tabbed content containers ('div.tabbed-content'),
    swapTabbedContent('div.normal-tabbed-behaviour', '.content');
    
    // Function that loads in the correct calendar month
    swapCalendarContent();
    
    // handles loading image gallery items from their scrolling controller
    // Liz Goulding: 10th Dec 2009 / commented out to stop scroller images replacing 'in page'
    // largeImageGalleryContent('div#scrollable-content-small', 'div.scrollableAreaSmall div.main-slider-content', 'div#image-gallery-container div.image-gallery-content', 'div.image-gallery-large-item');
    
    // handles loading web image gallery items on their home page
    imageGalleryContent('div#webcam-content', 'div.tiny-image', 'div.tiny-image-container');
    
    // handles loading video gallery items on their home page
    imageGalleryVideoContent('div#video-content', 'div.tiny-image', 'div.tiny-image-container');
    
    // reveals hidden content (.expandInfo), when an element is clicked
    // expandContent(".expandButton", ".expandInfo"); 
    toggleContent(".expandButton", ".expandInfo");

    //sliding down general content
    slideContent('a.advanced-search-link', 'div.events-advanced-search-container');

    $('.homepage-2012-content-container .tabbed-content .content .tab-content-item a').focus(function() {
        if($(this).parents('.current').length != 0) {return;}
        var tab = $(this).parents('.tabbed-content').find('.tabbed-headers li:not(.current) a');
        tab.click();
    });

    // *********** APRIL UPDATE ******************************
      // Create microsite links list drop down
      createDropDown('form.drop-down-form');
  
      // Get the links from the homepage feature. On click load in the content from an include file based on the links ID.
      loadContent($("div#load-content-links ul li a.onclickJs"), $("div#load-content-area"), "php");
    
      // Lars added the .onclickJs class to diff between js links and normal links...
      addClassOnClick($("div#load-content-links ul li a.onclickJs"), false, "selected", true);
      addClassOnClick($("div#load-content-links ul li a.onclickJs"), $("img.background"), "cursor-pointer");

      removeClassOnClick($("img.background"), $("div#load-content-links ul li a"), "selected");
      removeClassOnClick($("div#load-content-links ul li a"), $("img.background"), "selected");
    
      fadeElementOnHover($("div#load-content-links ul"), $("img.background"), 0.5, 300);
  
      onClickHideElement($("img.background"), 300, $("div#load-content-area"));
  
      // Remove white background to reveal semi opaque background (and avoid flicker)
      setTimeout('$("div#explore-sidenav div").css("background", "url(homepage-feature/images/presentation/alpha-bg.png) repeat")', 500);
  // ********** /APRIL UPDATE ******************************

});

function externalLinks() {
    $('a.link-external').removeClass('link-external').addClass('link-external-js');

    $('#MainContent a, #sidebar-right-2012 a').each(function() {
        var h = $(this).attr('href');
        if($(this).parents('#ticketPromo, #ticketPromo2, #moduleContentFormats').length != 0) {return;}
        if(h && h.match(/http/) && !h.match(/http.*\/\/(www.|)london2012.com/)) {$(this).addClass('link-external-js');}
    });
       
    $('a.link-external-js').each(function() {
        if($(this).html() === $(this).text()) {
            $(this).attr('target','_blank').attr('title', ($(this).attr('title') ? $(this).attr('title')+' - ' : '') + 'External link opens in a new window');
            $(this).prepend('<span></span>');
        }
    });
}

function shareLinks() {
    var slt = (document.title + ' | ').split(' | ')[0];
    var sll = (document.location + '?').split('?')[0];
    $('#share_message').val($('#share_message').val() + "\n" + sll);

    $('#share-link-twitter').attr('href','http://twitter.com/home?status='+encodeURIComponent(slt)+'%20-%20'+encodeURIComponent(sll));
    $('#share-link-facebook').attr('href','http://www.facebook.com/sharer.php?t=' + encodeURIComponent(slt)+'&u='+encodeURIComponent(sll));
    $('#share-link-digg').attr('href','http://digg.com/submit?url=' + encodeURIComponent(sll) + '&title=' + encodeURIComponent(slt) + '&bodytext=&media=news&topic=olympics');
    $('#share-link-delicious').attr('href','http://delicious.com/save?v=5&noui&jump=close&url='+encodeURIComponent(sll)+'&title='+encodeURIComponent(slt));
    $('#share-link-stumbleupon').attr('href','http://stumbleupon.com/submit?url='+sll);
    $('#share-link-reddit').attr('href','http://www.reddit.com/submit?url=' + encodeURIComponent(sll)+'&title=' + encodeURIComponent(slt));

    // following commented to make 'Share this page' link working
//    $('#footer-share-this').click(function(event) {
//        event.preventDefault();
//        if($('#share-box:visible').length != 0) {
//            $('#share-box').animate({'height': '0px', 'top': '0px'}, 100, function() {$(this).hide()});
//            $('#Content').animate({'opacity': 1}, 100);
//        } else {
//            $('#Content').animate({'opacity': .3}, 300);
//            $('#share-box').css({'height': '0px', 'top': '0px'}).show().animate({'height': '110px', 'top': '-110px'},300);
//        }
//    });
    $('#share-link-close').click(function(event) {
          event.preventDefault();
            $('#share-box').animate({'height': '0px', 'top': '0px'}, 100, function() {$(this).hide()});
            $('#Content').animate({'opacity': 1}, 100);
    });
    $(window).keydown(function(event) {
      if($('#share-box:visible').length != 0 && event.keyCode == 27) {
          event.preventDefault();
          $('#share-box').animate({'height': '0px', 'top': '0px'}, 100, function() {$(this).hide()});
          $('#Content').animate({'opacity': 1}, 100);
      }
    });
    $('#form_share').submit(function(event) {
        event.preventDefault();
        $('#submit_form_share').attr('disabled','disabled');
        $('#share-error').removeClass('share-error-success').removeClass('share-error-error').html('Working...');
        $.ajax({
            type: 'POST',
            url: $('#form_share').attr('action'),
            data: {
                "sender": $('#form_share input[name=sender]').val(),
                "recipient": $('#form_share input[name=recipient]').val(),
                "message": $('#share_message').val()
            },
            success: function(data) {
              if(data.success) {
                  $('#share-error').addClass('share-error-success').html(data.reason);
              } else {
                  $('#share-error').addClass('share-error-error').html(data.reason);
              }
              $('#submit_form_share').attr('disabled','');
            },
            dataType: 'json'
        });
    });
}

function expandContent(expandEvent, expandInfo) {
    $(expandEvent).click(function() {
        var infoButton = $(this);
        var infoSibling = $(this).siblings(expandInfo);

        if (infoSibling.css("display") == "block") {
            $(this).siblings(expandInfo).slideUp("medium");
        } else {
            $(this).siblings(expandInfo).slideDown("medium", function() {
                infoButton.slideUp("medium");
            });
        }
    });
    return false;
}

function toggleContent(expandEvent, expandInfo) {
    // new function - june2010 - lars

    // expandEvent = .expandButton
    // expandInfo = .expandInfo    
        
    var openState = $(expandEvent).html();
    var closedState = 'close';

    $(expandEvent).click(function() {
        var infoButton = $(this); 
        var infoSibling = $(this).siblings(expandInfo); // points to hidden content

        if (infoSibling.css("display") == "none") {
            $(this).siblings(expandInfo).slideDown("medium");
            infoButton.html(closedState) // change button
        } else {
            $(this).siblings(expandInfo).slideUp("medium");
            infoButton.html(openState)
        }
    });
    return false;
}

// Function that fades in the events being hosted at a venue
// NOTE: Also changes the width of the events container if there is only 1 or 2
function showVenues(venueContainer) {
    //find all the divs containing the on hover images and hide the div
    $(venueContainer).find('div.venue div').css('display', 'none');
    //$(venueContainer).find('div.venue div').css('z-index', '500');
    $(venueContainer).find('div.venue').css('margin-left', '8px');
        
        
    $(venueContainer).find('div.venue').each(function(intIndex) {
    
        // If the amount of events is 3 or less, lower the width of the container
        if ($(this).find("div img").length == 1) {
            $(this).find("div").css("width", "51px");
        } else if ($(this).find("div img").length == 2) {
            $(this).find("div").css("width", "102px");
        } else if ($(this).find("div img").length == 3) {
            $(this).find("div").css("width", "153px");
        }
        
        var venue = $(this);
        
        // On mouse over hide all events, then show the events for that venue
        venue.find('a img').mouseover(function(){
            $(venueContainer).find("div.venue div").css('display', 'none');
            $(venueContainer).find("div.venue").css('margin-left','8px');
            venue.css('z-index','500');
            venue.find("div").fadeIn();
            venue.css('margin-left','8px');
        });
        
        venue.mouseout(function(){
            venue.css('z-index','1');
        });

        // On mouse out of entire venues section hide all event rollovers.
        $(venueContainer).mouseout(function() {
            venue.find('a img').css('opacity','1');
            $(venueContainer).find("div.venue div").css('display', 'none');
            
        })

        // On mouse out hide all events
        venue.find("div").mouseout(function(){
            $(venueContainer).find("div.venue div").css('display', 'none');
            $(venueContainer).find("div.venue").css('z-index','1');
            venue.find("div").css('display', 'none');
            
        });
    });
}

function swapTabbedContent(linkSelector, contentTab) {
    
    $(linkSelector).each(function() {
        // The relevant divTabbedContent we are working in
        var divTabbed = $(this);
        $(this).find('ul.tabbed-headers li').each(function(intIndex) {
            // The li we are currently in
            var tab = $(this);
            // The links that are in the ul
            var tabLink = $(this).find('a');
            
            tabLink.click(function() {
                // Remove 'current' class from all tabs
                tab.siblings().attr('class', '');
                tab.attr('class','current');
                
                // Hide all tabbed content
                $(divTabbed).find(contentTab).removeClass("current")
                
                // Display the content related to the tab clicked
                $(divTabbed).find(contentTab + ':eq(' + intIndex + ')').addClass("current")
                return false;
            });
        });
    });
    
}

function swapCalendarContent() {
    
    //display the content related to the current li tab first
    
    $('#calendar-tabs ul.tabbed-headers').each(function() {
        
        if($(this).css('display') == 'block') {
            
            $(this).find('li').removeClass('current');
            $(this).find('li:first').addClass('current');
            
            $(this).find('li a').each(function() {
                
                if($(this).parent().attr('class') == 'current') {
                    var currentId = $(this).attr("id");
                    $('#calendar-tabs div#BlogCalendar').find('div').css('display','none');
                    $('#calendar-tabs div#BlogCalendar').find('div.'+currentId).css('display','block');
                }
                
                $(this).click(function() {
                    var id = $(this).attr("id");
                    
                    $(this).parent().siblings().removeClass('current');
                    $(this).parent().addClass('current');
                    
                    $('#calendar-tabs div#BlogCalendar').find('div').css('display','none');
                    $('#calendar-tabs div#BlogCalendar').find('div.'+id).css('display','block');
                    
                    return false;
                });
            });
        }
    });
}

// Function to swap out the css relevant to the overall font size or accessibility style sheet.
function swapCSS(linkSelector, cssSelector) {
    $(linkSelector).click(function() {

        //get the id of the link clicked which represents the style sheet wanted
        var id = $(this).attr('id');
        
        if (id == "main") {
            $("link[href*='colours.css']").each(function() { this.disabled = false; });
            $("link[href*='corecontent.css']").each(function() { this.disabled = false; });
            $("link#dyslexia-css").each(function() { this.disabled = true; });
            $("link#highvis-css").each(function() { this.disabled = true; });
            $("link#main-css").each(function() { this.disabled = false; });

            //add the new href to the relevant css link in the header
            $(cssSelector).attr('href', "css/"+id+".css");
        } else if (id == "dyslexia") {
            $("link[href*='colours.css']").each(function() { this.disabled = false; });
            $("link[href*='corecontent.css']").each(function() { this.disabled = false; });
            $("link#dyslexia-css").attr("href", "css/dyslexia.css");
            $("link#dyslexia-css").each(function() { this.disabled = false; });
            $("link#highvis-css").each(function() { this.disabled = true; });
            $("link#main-css").each(function() { this.disabled = false; });
        } else if (id == "high-vis") {
            $("link[href*='colours.css']").each(function() { this.disabled = true; });
            $("link[href*='corecontent.css']").each(function() { this.disabled = true; });
            $("link#dyslexia-css").each(function() { this.disabled = true; });
            $("link#highvis-css").attr("href", "css/high-vis.css");
            $("link#highvis-css").each(function() { this.disabled = false; });
            $("link#main-css").each(function() { this.disabled = true; });
        } else {
            //add the new href to the relevant css link in the header
            $(cssSelector).attr('href', "css/"+id+".css");
        }
        return false;
    });
    
}

// Function that takes an ID selector,
// and upon click slides down any elements with the corresponding class
function slideNavigationContent(dropDownSelector) {
    //the link that has been clicked
    $(dropDownSelector).click(function(){
        //get its id to be used as the class for the sub navigation div
        id = $(this).attr('id');
        link = $(this).parent().find('a:last');
        
        //get the div element using the id above as its class
        var container = $('.' + id);
        //get the border style to put back on later
        var border = link.parent().css('border-bottom');
        
        //if the container is already displayed then hide it, else display it
        if(container.css('display') == 'block'){
            //container.siblings('div.nav-secondary-container').css('display','none');
            container.slideUp('medium', function() {
                link.attr('class','header-dropdown');
                link.parent().css('border-bottom',border);
            });
        } else {
            var visibleDropDown = false;
            var dropDownList;
            // Currently not using drop down highlight
            // link.parent().css('border-bottom','5px solid #FFFFFF');
            container.siblings('.nav-secondary-container').each(function(intIndex) {
                if ($(this).css('display') == 'block'){
                    dropDownList = $(this);
                    visibleDropDown = true;
                }
            });
            
            if (visibleDropDown) {
                dropDownList.slideUp('medium', function() {
                    container.slideDown('medium', function() {
                        $('ul#nav-primary li a.header-closeup').attr('class','header-dropdown');
                        link.attr('class','header-closeup');
                    });
                });
            } else {
                container.slideDown('medium', function() {
                    $('ul#nav-primary li a.header-closeup').attr('class','header-dropdown');
                    link.attr('class','header-closeup');
                });
            }
        }
        
        return false;
    })
}


// Function that takes content from the slider and puts it in the gallery above it
function imageGalleryContent(parentContainer, controllerContentSelector, galleryContentSelector) {
    var galleryContentSelector = $(parentContainer).find(galleryContentSelector);
    var controllerContentSelector = $(parentContainer).find(controllerContentSelector);
    
    // removes the class first which is used in non javascript solution,
    // but still makes sure it is displayed block
    galleryContentSelector.each(function() {
        if ($(this).css('display') == 'block') {
            $(this).removeClass('first');
            $(this).css('display','block');
        }
    });
    
    // iterate through each of the small images in the slider
    controllerContentSelector.each(function(intIndex) {
        //get the index of the image clicked
        var intClickedOn = intIndex;
        
        $(this).find('a').click(function() {

            // when the link is clicked, get the image source
            var smallSource = $(this).parent().find('a img').attr('src');
            var smallLink = $(this).parent().find('a').attr('href');

            //var bigSource = smallSource.replace('small','large');
            //var bigSource = smallSource.replace('sc90x63','m700'); // need a better way to do this !!!
            bigSource = smallSource.replace('sc61x44','sc209x160'); // need a better way to do this !!!

            galleryContentSelector.fadeOut('fast', function() {
                // write the html out
                galleryContentSelector.html('<a href="'+smallLink+'"><img src="'+bigSource+'" alt="image gallery item" /></a>');
                galleryContentSelector.fadeIn('fast');
            });
            return false;
        });
    });
    
}

//Function that takes content from the slider and puts the related VIDEO in the gallery above it
function imageGalleryVideoContent(parentContainer, controllerContentSelector, galleryContentSelector) {
    var galleryContentSelector = $(parentContainer).find(galleryContentSelector);
    var controllerContentSelector = $(parentContainer).find(controllerContentSelector);
    
    // removes the class first which is used in non javascript solution,
    // but still makes sure it is displayed block
    galleryContentSelector.each(function() {
        if ($(this).css('display') == 'block') {
            $(this).removeClass('first');
            $(this).css('display','block');
        }
    });

    // iterate through each of the small images in the slider
    controllerContentSelector.each(function(intIndex) {
        //get the index of the image clicked
        var intClickedOn = intIndex;
        
        $(this).find('a').click(function() {

            var swfSource = $(this).parent().find('a img').attr('link');

            galleryContentSelector.fadeOut('fast', function() {
              
                galleryContentSelector.html('').flash({
                    swf: swfSource,
                    width: 209,
                    height: 144
                })
              
                // hide all popUpButtons
                $('#videoLightbox a').hide();
                              
                $('.tiny-image-container').bind("mouseenter",function(event){
                    $('.tiny-image-container-overlay').fadeIn();
                    event.stopPropagation();
                })

                galleryContentSelector.fadeIn('fast');

                // show currten popUpButton
                $('#videoLightbox a:eq('+(intIndex+1)+')').fadeIn();
            });
            return false;
        });
    });
    
}

// Function that takes content from the slider and puts it in the gallery above it
function largeImageGalleryContent(parentContainer, controllerContentSelector, galleryContentSelector, imageContainer) {
    var galleryContentSelector = $(galleryContentSelector);
    var controllerContentSelector = $(parentContainer).find(controllerContentSelector);
    var imageContainer = $(galleryContentSelector).find(imageContainer);
    // removes the class first which is used in non javascript solution,
    // but still makes sure it is displayed block
    galleryContentSelector.each(function() {
        if ($(this).css('display') == 'block') {
            $(this).removeClass('first');
            $(this).css('display','block');
        }
    });
    
    // iterate through each of the small images in the slider
    controllerContentSelector.each(function(intIndex) {
        //get the index of the image clicked
        var intClickedOn = intIndex;
        
        $(this).find('a').click(function() {
            
            // when the link is clicked, get the image source
            var smallSource = $(this).find('img').attr('src');
            var bigSource = $(this).find('img').attr('link');

            // grab link content, use to make page title
            if ($(this).find('img')) {
                $("div#Content h1").text($(this).find('img').attr('alt'));   
            } else {
                $("div#Content h1").text($(this).find('a').text());   
            }

            // if the large gallery is there
            // iterate through the large gallery content
            galleryContentSelector.each(function(galleryIndex) {
                
                var galleryItem = $(this);
                if(intClickedOn == galleryIndex) {
                    
                    // if the large gallery content equals the clicked on small content, then hide the other large gallery items
                    galleryItem.siblings().hide();
                    galleryItem.fadeIn('fast');
                    
                    if(bigSource.indexOf('swf') !== -1) {
                        //alert(imageContainer.eq(intClickedOn).html());
                        //imageContainer.eq(intClickedOn).find('object').attr('data', bigSource);
                        imageContainer.eq(intClickedOn).html('').flash({
                            swf: bigSource,
                            width: 460,
                            height: 420
                        });
                        //swfobject.embedSWF(bigSource, "video", 460, 420, "8.0.0", false, {}, {'bgcolor' : '#333333', 'wmode' : 'opaque'});
                    } else {
                        imageContainer.eq(intClickedOn).html('<img src="' + bigSource + '" />');
                    }
                    
                     //following agcy code commented out
                     //galleryItem.siblings().fadeOut('slow', function() {
                        //change the image source to point to the larger image directory
                        //bigSource = smallSource.replace('small','large');
                        //bigSource = smallSource.replace('sc90x63','m700x'); // need a better way to do this !!!
                        //bigSource = galleryItem.find('img').attr('link');

                        // write the html out
                        //imageContainer.eq(intClickedOn).html('<img src="' + bigSource + '" />');
                        // fade in the new image

                        //galleryItem.fadeIn('slow');
                    //});
                    
                    return false; //we break out of the loop
                }
            });

            return false;
        });
    });
    
}

function slideContent(linkClicked, contentSelector) {
    $(linkClicked).click(function() {
    
        if($(contentSelector).css('display') == 'block') {
            $(contentSelector).slideUp('medium', function() {
                $(linkClicked).css('background',"url('../images/icons/header-drop-down-icon.gif') no-repeat right center");
            });
        } else {
            $(contentSelector).slideDown('medium', function() {
                $(linkClicked).css('background',"url('../images/icons/header-arrow-up-icon.gif') no-repeat right center");
            });
        }
        
        return false;
    });
}

// Function to take in a css selector, and change the opacity when you hover that element
function hoverState(cssSelector, opacity) {    
    if (!opacity) var opacity = 0.75;
    
    $(cssSelector).mouseover(function() {
        $(this).css({
            'opacity' : opacity,
            'zoom' : 1,
            'filter' : 'alpha(opacity = 75)'
            });
    });
        
    $(cssSelector).mouseout(function() {
        $(this).css({
            'opacity' : 1,
            'zoom' : 1,
            'filter' : 'alpha(opacity = 100)'
        });
    });
}

// *********** APRIL UPDATE ******************************

    function onClickHideElement(elActivation, speed, elToHide) {
      elActivation.click(function() {
        if ($(this).attr("opacity") != 1) {
          $(this).animate({
            opacity: 1
          }, speed, function() {});
      
          elToHide.fadeOut(speed, function() {
        
          });
        }
      });
    }

    // On hover of an element (elActivation), make another element (elToFade) semi opaque);
    function fadeElementOnHover(elActivation, elToFade, opacity, speed) {
      var booSelected = false;
      elActivation.hover(
        function() {
          elToFade.animate({
            opacity: opacity
          }, speed, function() {});
        }, function() {
          elActivation.find("a").each(function() {
            if ($(this).attr("class") == "selected") {
              booSelected = true;
            }
          });
          if (!booSelected) {
            elToFade.animate({
              opacity: 1
            }, speed, function() {});
          }
        }
      );
    }

    // Function that removes a class from an element (elToRemoveClass),
    // when another element is clicked (elToClick)
    function removeClassOnClick(elToClick, elToRemoveClass, className) {
      elToClick.each(function(intIndex) {
        $(this).click(function() {
          var elID = $(this).attr("id");
          if (elToRemoveClass) {
            elToRemoveClass.removeClass(className);
          } else {
            $(this).removeClass(className);
          }
        });
      });
    }

    // Function that adds a class to an element (elToAddClass),
    // when another element is clicked (elToClick)
    // OPTIONAL: 'booUnique' sets whether only one element in 'elToClick' can have the class 'className'
    function addClassOnClick(elToClick, elToAddClass, className, booUnique) {
      elToClick.each(function(intIndex) {
        $(this).click(function() {
          if (booUnique) {
            elToClick.each(function(intIndex) { $(this).attr("class", ""); });
          }
          var elID = $(this).attr("id");
          if (elToAddClass) {
            elToAddClass.addClass(className);
          } else {
            $(this).addClass(className);
          }
        });
      });
    }

    // Function that takes a collection of html elements (elActivation),
    // Then on click loads in content with a dynamic filename (includeName + [0-9] + includeExtension),
    // Into the content area (loadContent)
    function loadContent(elActivation, loadContent, includeExtension) {
      elActivation.each(function(intIndex) {
        $(this).click(function() {
          var elID = $(this).attr("id");
          loadContent.fadeOut("normal", function() {
            loadContent.load("homepage-feature/includes/" + elID + "." + includeExtension, function() {
              loadContent.fadeIn();
            });
          });
      
          return false;
        });
      });
    }

    // Function that switches out a standard drop down menu,
    // and replaces it with one that can be styled fully
    function createDropDown(elForm) {
      $(elForm).each(function() {
        // Grab the necessary elements
        var dropDownForm = $(this);
        var dropDownSubmit = dropDownForm.find("input[type='submit']");
        var elSelect = dropDownForm.find("select:first")
        var elOptions = elSelect.find("option");
    
        // Hide drop down button
        dropDownSubmit.each(function() { $(this).css("display", "none");});

        // Lars addin for new DropDownButton
        $('.dropDownSubmit').css("display", "none");

        // Build up new drop down
        var dropDownHTML = '<div class="drop-down">';
    
        // Grab all the drop down options to make the drop down links
        elOptions.each(function(intIndex) {
          if (intIndex == 0) {
            dropDownHTML += '<div class="selected"><a href="' + $(this).val() + '">' + $(this).text() + '</a></div>';
            dropDownHTML += '<ul>';
          } else {
            dropDownHTML += '<li><a href="' + $(this).val() + '">' + $(this).text() + '</a></li>';
          }
      
      

          if ((elOptions.length - 1) == intIndex) {
            dropDownHTML += '</ul></div>';
          }
        });

    
        // Replace drop down options with drop down links
        elSelect.replaceWith(dropDownHTML);

        // Store drop down information
        var dropDownList = dropDownForm.find("ul");
        var dropDownSelected = dropDownForm.find("div.selected");
    
        // Slide up and down functionality
        dropDownSelected.click(function(e) {
          e.preventDefault();
          $(this).toggleClass("open");
          dropDownList.slideToggle("fast");
        });
    
    
        // If the forms submit button is pressed then go to the first link of the javascrip drop down
        dropDownSubmit.click(function() {
          window.location.href = dropDownList.find("a:first").attr("href");
          return false;
        });

      });
    }
    
// *********** /APRIL UPDATE ******************************