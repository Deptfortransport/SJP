// Initialise javascript stuff
$(document).ready(function () {
    $('input.btnSmallerPink').hide();
    setJSHdnSettingFieldValue('jsEnabled', $('div.stopSelection'), 'true');

    setupDropDowns();


    // Following lines are to rebind the jquery plugins or js events after ajax update of the update panel
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function () {
        setupDropDowns();
    });
});


function setupDropDowns() {
    $('div.stopSelection select').selectmenu({ width: 270, maxHeight: 180, positionOptions: { collision: "none"} });
}