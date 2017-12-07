javascript: (function () {
    var bmDomain = "#{BOOKMARKLET_DOMAIN}#";
    var bmUrl = bmDomain.toLowerCase() === "#{bookmarklet_domain}#"
        ? "https://localhost:44303/Scripts/bookmarklet/bookmarklet.min.js"
        : bmDomain + "/Scripts/bookmarklet/bookmarklet.js";
    var scriptTag = document.createElement('script');
    scriptTag.setAttribute('src', bmUrl);
    document.body.appendChild(scriptTag);
})();