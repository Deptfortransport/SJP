$(document).ready(function () {
    setupPaging();
});


function setupPaging() {
    $('div.travelNewsWidget div.tnHeadlinesContainer ul.PagerContainer li.pageItem').css('display', 'block');

    var paging_div = $('div.travelNewsWidget div.tnHeadlinesContainer');
    var prevButton = $('div.travelNewsWidget div.navigation input.prev');
    var nextButton = $('div.travelNewsWidget div.navigation input.next');

    prevButton.die();
    nextButton.die();

    prevButton.live('click', function (event) { event.preventDefault(); });
    nextButton.live('click', function (event) { event.preventDefault(); });

    $(paging_div).serialScroll({
        items: 'li.pageItem', // Selector to the items ( relative to the matched elements, '#sections' in this case )
        prev: prevButton, // Selector to the 'prev' button (absolute!, meaning it's relative to the document)
        next: nextButton, // Selector to the 'next' button (absolute too)
        duration: 700, // Length of the animation (if you scroll 2 axes and use queue, then each axis take half this time)
        force: true, // Force a scroll to the element specified by 'start' (some browsers don't reset on refreshes)
        cycle: false,
        onBefore: function (e, elem, $pane, $items, pos) {
            /**
            * 'this' is the triggered element
            * e is the event object
            * elem is the element we'll be scrolling to
            * $pane is the element being scrolled
            * $items is the items collection at this moment
            * pos is the position of elem in the collection
            * if it returns false, the event will be ignored
            */
            //those arguments with a $ are jqueryfied, elem isn't.
            e.preventDefault();
            $('li.pageItem', paging_div).removeClass('active');
            $(elem).addClass('active');

            // set the current index
            $('div.travelNewsWidget div.navigation span.current').html((pos+1));

            prev = $('div.travelNewsWidget div.navigation input.prev');
            next = $('div.travelNewsWidget div.navigation input.next');

            if (pos == $items.length - 1)
                next.hide();
            else
                next.show();

            if (pos == 0)
                prev.hide();
            else
                prev.show();
        },
        onAfter: function (elem) {
            //'this' is the element being scrolled ($pane) not jqueryfied



        }
    }); 

}