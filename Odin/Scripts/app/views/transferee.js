var OrdersPageController = function () {
     
    var init = function () { 
        $('div.col > div').each(function () { $(this).css("display", "none"); })
        $('div.col > div#intake').css("display", "block");
        $('.item').click(function () {
            $('.item').removeClass('selected');
            $(this).addClass('selected');

            $('div.col > div').each(function () { $(this).css("display", "none"); })
            $('div.col > div#' + $(this).attr('data-panel')).css("display", "block");
        });

        // ensure icon spacing is adequate on mobile
        window.addEventListener("resize", resizeFn);
        resizeFn();
    };

    var resizeFn = function () {
        if (window.innerWidth <= 991) {
            $('li.item').each(function () {
                $(this).css("padding-right", ((window.innerWidth / 6) - 48).toString() + "px");
            });
        }
        else {
            $('li.item').each(function () {
                $(this).css("padding-right", "0px");
            });
        }
    }

    return {
        TransfereeInit: init
    };
}();

