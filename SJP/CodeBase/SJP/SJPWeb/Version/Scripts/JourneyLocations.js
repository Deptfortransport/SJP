// Initialise javascript stuff
$(document).ready(function () {
    setupDropDowns();

    setupMapBullets();

    // Following lines are to rebind the dropdowns after ajax update of the update panel
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function () {
         setupDropDowns();

         setupMapBullets();
    });


});

function setupDropDowns() {
    $('#JourneyLocations select.options').selectmenu({ width: 448, maxHeight: 300, positionOptions: { collision: "none"} }); //.sb({ fixwidth: true, ddCtx: $('#MainContent'), useTie: true });

}    

// Sets the map overlay clickable links
function setupMapBullets() {
    $('div.JourneyLocations div.venueMap a.bullet').show().qtip({
        position: {
            my: 'bottom center',
            at: 'top center',
            container: $('#MainContent'),
            adjust: {
                x: -20,
                mouse: true,
                resize: true
            },
            viewport: $('div.JourneyLocations div.venueMap')
        },
        style: {
            classes: 'ui-tooltip-sjp'
        }
    }).bind('click', function (event) {
        var targetElem = getJSHdnSettingFieldValue('mapBulletTarget', $(this).parent());
        $('#' + targetElem).val($(this).attr('name'));
               

        event.preventDefault();
        $('#JourneyLocations select.options').sb("refresh");
    }).bind('mouseover', function (event) {
        $(this).parent().find('img.'+$(this).attr('name').split(':')[0]).show();
    }).bind('mouseout', function (event) {
        $(this).parent().find('img.' + $(this).attr('name').split(':')[0]).hide();
    });

}