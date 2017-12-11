javascript: (function () {
    var bmDomain = "#{BOOKMARKLET_DOMAIN}#";
    var bmUrl = bmDomain.toLowerCase().indexOf("bookmarklet_domain") >= 0
        ? "https://localhost:44303/Scripts/bookmarklet/app.min.js"
        : bmDomain + "/Scripts/bookmarklet/bookmarklet.js";
    var scriptTag = document.createElement('script');
    scriptTag.setAttribute('src', bmUrl);
    document.body.appendChild(scriptTag);
})();