﻿var OrdersPageController = function () {
     
    var init = function () {

        //$('div.col-md-10 > div > div').each(function () { $(this).css("display", "none"); });

        //$('#intake').css("display", "block");

        initPanels();

        $(window).on('hashchange', function () {
            initPanels();
        });

        $('.item').click(function () {
            var actionName = $(this).attr('data-panel');
            loadPanel(actionName);
        });

        // ensure icon spacing is adequate on mobile
        window.addEventListener("resize", resizeFn);
        resizeFn();
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

