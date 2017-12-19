var DWLoader = function () {

    var showLoaderAfterDelay = function (delay = 250) {
        return window.setTimeout(function () {
            $(".spinnerOverlay").fadeIn('slow');
        }, delay);
    };

    var hideLoaderWithId = function (loaderId) {
        window.clearTimeout(loaderId);
        $(".spinnerOverlay").fadeOut('fast');
    };

    return {
        showLoaderAfterDelay: showLoaderAfterDelay,
        hideLoaderWithId: hideLoaderWithId
    };
}();