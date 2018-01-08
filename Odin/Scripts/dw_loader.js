var DWLoader = function () {

    /**
     * 
     * @param {any} delay Delay in milliseconds. Default 250 (.25s)
     * @returns {any} Id of the the loader timeout. Pass this value to the hideLoaderWithId funciton
     *                to properly cancel the delay of the loader showing
     */
    var showLoaderAfterDelay = function (delay) {

        delay = (typeof delay !== 'undefined') ? delay : 1;

        return window.setTimeout(function () {
            $(".loaderOverlay").fadeIn('slow');
        }, delay);
    };

    var hideLoaderWithId = function (loaderId) {
        window.clearTimeout(loaderId);
        $(".loaderOverlay").fadeOut('fast');
    };

    return {
        showLoaderAfterDelay: showLoaderAfterDelay,
        hideLoaderWithId: hideLoaderWithId
    };
}();