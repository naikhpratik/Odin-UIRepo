var OrdersPageController = function () {
     
    var init = function () {
            window.alert('hide all content');
        $('div.col-md-10 > div > div').each(function () { $(this).css("display", "none"); });

        window.alert('show intake content');
        $('#intake').css("display", "block");

        $('.item').click(function () {
            window.alert('remove selected from currently select item');
            $('.item').removeClass('selected');

            window.alert('add selected class to the item');
            
            $(this).addClass('selected');

            window.alert('Hide the other content');

            $('div.col-md-10 > div').each(function() { $(this).css("display", "none"); });      
            $('div.col > div.frame').each(function () { $(this).css("display", "none"); });

            window.alert('show the picked content');

            $('div.col > div#' + $(this).attr('data-panel')).css("display", "block");
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

