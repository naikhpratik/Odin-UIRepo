var OrdersPageController = function () {
     
    var init = function () {

        $('div.col-md-10 > div > div').each(function () { $(this).css("display", "none"); });

        $('#intake').css("display", "block");

        $('.item').click(function () {            
            $('.item.selected').removeClass('selected');            
            $(this).addClass('selected');
            var actionName = $(this).attr('data-panel');
            $('#orderContainer').load('/orders/' + actionName + 'Partial/' + currentOrderId);
        });

        // ensure icon spacing is adequate on mobile
        window.addEventListener("resize", resizeFn);
        resizeFn();
    };

    var resizeFn = function () {
        if (window.innerWidth <= 991) {
            $('li.item').each(function () {
                $(this).css("padding-right", (window.innerWidth / 6 - 52).toString() + "px");
            });
        }
        else {
            $('li.item').each(function () {
                $(this).css("padding-right", "0px");
            });
        }
    };

    return {
        TransfereeInit: init
    };
}();

