var OrdersPageController = function () {
        
    var init = function () { 
        $('.item').click(function () {
            $('.item').removeClass('selected');
            $(this).addClass('selected');

            $('div.col div').each(function () { $(this).css("display", "none"); })
            $('div.col div#' + $(this).attr('data-panel')).css("display", "block");
        });
    };

    return {
        TransfereeInit: init
    };
}();

