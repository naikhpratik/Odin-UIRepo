var OrdersPageController = function () {
     
    var init = function () {

        $('div.col-md-10 > div > div').each(function () { $(this).css("display", "none"); });

        $('#intake').css("display", "block");

        $('.item').click(function () {     
            $('.item.selected').removeClass('selected');            
            $(this).addClass('selected');
            var actionName = $(this).attr('data-panel');
            $('#orderContainer').load('/orders/' + actionName + 'Partial/' + currentOrderId);

            if ($('.navbar-toggle').css('display') != 'none')
            {
                $('.navbar-toggle').click();
            }
        });
    };

    return {
        TransfereeInit: init
    };
}();

