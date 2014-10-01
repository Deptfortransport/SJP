(function ($) {

    var methods = {
        // init check box
        init: function () {

            return this.each(function () {
                var checkBox = $(this);
                var checkBoxType = $(this).attr('type');
                var checkBoxLabel = $(this).next('label'); // To do : get the label using attr

                var checkBoxSpan = $('<span class="' + checkBoxType + '"><span class="box"><div><span>&nbsp;</span></div></span><span class="text">' + checkBoxLabel.html() + '</span></span>');

                if ($.browser.msie && checkBox.is(':disabled')) {
                    $(checkBox).parent(':disabled').before(checkBoxSpan).addClass('has_sb');
                }
                else {
                    $(checkBox).before(checkBoxSpan).addClass('has_sb');
                }
                $(checkBoxLabel).hide();

                if (checkBox.attr('checked')) { $(checkBoxSpan).find('span.box').addClass('checked'); }

                if (checkBox.is(':disabled')) {
                    $(checkBoxSpan).find('span.box div').addClass('disabled')/*.attr('disabled', 'disabled')*/;
                    $(checkBoxSpan).find('span.text').addClass('disabled');
                }


                $(checkBoxSpan).bind('click.checkBox', function (event) {

                    if ($(this).find('span.box div').hasClass('disabled')) {
                        event.preventDefault();
                        return;
                    }

                    var raw_data = $(this).data("data");
                    var orig_item = raw_data.elem;


                    $(this).find('span.box').toggleClass('checked');

                    //$(orig_item).click();

                    if ($(this).find('span.box').hasClass('checked'))
                        orig_item.attr('checked', 'checked');
                    else
                        orig_item.removeAttr('checked');


                })/*.mouseover(function (event) {
//                $(this).find('span.box div').addClass('hover');
//            }).mouseout(function (event) {
//                $(this).find('span.box div').removeClass('hover');
//            })*/.data("data", {
                elem: checkBox 
            });



            });

        },
        // Destroy the check box;
        destroy: function () {

            return this.each(function () {
                $(window).unbind('.checkBox');
            })

        },
        // Enables the check box
        enable: function () {
            $(this).find('span.box div').removeClass('disabled');
            $(this).find('span.text').removeClass('disabled');
        },
        // Disables the check box
        disable: function () {
            $(this).find('span.box div').addClass('disabled');
            $(this).find('span.text').addClass('disabled');
        },
        // Checks if the check box is checked
        checked: function () {
            var raw_data = $(this).data("data");
            var orig_item = raw_data.elem;
            return orig_item.attr('checked');
        },
        // uncheck the check box
        uncheck: function () {
            $(this).find('span.box').removeClass('checked');
            var raw_data = $(this).data("data");
            var orig_item = raw_data.elem;
            //            if ($(orig_item).is(':checked'))
            //                $(orig_item).click();
            $(this).data("data").elem.removeAttr('checked');

        }

    };

    $.fn.checkBox = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.tooltip');
        }

    };

})(jQuery);

