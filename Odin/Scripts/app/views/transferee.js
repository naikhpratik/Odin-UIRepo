var OrdersPageController = function () {
     
    var init = function () {

        //$('div.col-md-10 > div > div').each(function () { $(this).css("display", "none"); });

        //$('#intake').css("display", "block");
        sizePage();
        initPanels();

        $(window).on('hashchange', function () {
            initPanels();
        });

        $(window).resize(function () {
            sizePage();
        });

        $('.item').click(function () {     
            $('.item.selected').removeClass('selected');            
            $(this).addClass('selected');
            
            if ($('.navbar-toggle').css('display') !== 'none')
            {
                $('.navbar-toggle').click();
            }
        });
    };

    var initPanels = function() {
        var actionName = "intake";
        var urlArray = window.location.href.split("#");
        if (urlArray.length === 2) {
            actionName = urlArray[1];
        }
        loadPanel(actionName);
    }

    var loadPanel = function(actionName) {
        $('.item.selected').removeClass('selected');
        $("[data-panel=" + actionName + "]").addClass('selected');
        $('#orderContainer').load('/orders/' + actionName + 'Partial/' + currentOrderId);
    }

    var sizePage = function () {
        var marginalWidth = 0;
        marginalWidth = (window.innerWidth - 1440) / 2;

        if (window.innerWidth > 1440) {
            $('#transfereeSideNav').css('left', marginalWidth);
            $('#primaryNav').css('left', marginalWidth);
            $('#orderContainer').css('margin-left', marginalWidth + $('#transfereeSideNav').outerWidth());
        } else if (window.innerWidth >= 768) {
            $('#transfereeSideNav').css('left', 0);
            $('#primaryNav').css('left', 0);
            $('#orderContainer').css('margin-left', $('#transfereeSideNav').outerWidth());
        } else {
            $('#orderContainer').css('margin-left', 0);
        }
    }
    return {
        TransfereeInit: init,
        loadPanel: loadPanel
    };
}();

