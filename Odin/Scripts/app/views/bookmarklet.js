var BookMarkletService = function () {
    var route = "/api/bookmarklet/add/";

    var addProperty = function (data, success, fail) {
        $.post(route, data).done(success).fail(fail);
    }

    return {
        addProperty: addProperty
    }

}();

var BookMarkletController = function (bookMarkletService) {

    var init = function () {

        $("#btnAdd").on("click",
        function() {
            var data = $("form").serialize();
            bookMarkletService.addProperty(data, addSuccess, addFail);
        });
    };

    var addSuccess = function() {
        toast("Property added!", "success");
    }

    var addFail = function() {
        toast("Uh oh! Something went wrong!", "danger");
    }

    var toast = function (message, type) {
        $.notify({
            message: message
        }, {
                delay: 2000,
                type: type,
                placement: {
                    from: "bottom",
                    align: "center"
                },
                animate: {
                    enter: 'animated fadeInUp',
                    exit: 'animated fadeOutDown'
                }
            });
    }

    return {
        init: init
    };
}(BookMarkletService);
