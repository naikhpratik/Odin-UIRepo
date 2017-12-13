var OrdersPageController = function () {
     
    var init = function () {

        //$('div.col-md-10 > div > div').each(function () { $(this).css("display", "none"); });

        //$('#intake').css("display", "block");

        initPanels();

        $(window).on('hashchange', function () {
            initPanels();
        });

        $('.item').click(function () {     
            $('.item.selected').removeClass('selected');            
            $(this).addClass('selected');
            
            if ($('.navbar-toggle').css('display') != 'none')
            {
                $('.navbar-toggle').click();
            }
        });
    };

    var initPanels = function() {
        var actionName = "intake";
        var urlArray = window.location.href.split("#");
        if (urlArray.length == 2) {
            actionName = urlArray[1];
        }
        loadPanel(actionName);
    }

    var loadPanel = function(actionName) {
        $('.item.selected').removeClass('selected');
        $("[data-panel=" + actionName + "]").addClass('selected');
        $('#orderContainer').load('/orders/' + actionName + 'Partial/' + currentOrderId);
    }


    return {
        TransfereeInit: init,
        loadPanel: loadPanel
    };
}();

