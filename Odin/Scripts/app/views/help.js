var HelpIndexController = function() {

    var init = function() {
        sizePage();
        $(window).resize(function() {
            sizePage();
        });
    }

    var sizePage = function() {

        var marginalWidth = 0;
        marginalWidth = ($(window).innerWidth() - 1440) / 2;

        if (window.innerWidth > 1440) {
            $('#primaryNav').css('left', marginalWidth);

        } else if (window.innerWidth >= 768) {
            $('#primaryNav').css('left', 0);

        }
    };

    return {
        init: init
    };
}();