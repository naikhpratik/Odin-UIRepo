var TransfereeIntakeController = function () {

    var init = function () {
        $(".intake-header").find("span").on("click",
            function () {
                var cols = $(this).parents(".intake-block").find(".intake-row > .intake-col");
                if (contains($(this).text(), "Edit")) {
                    $(this).text("- Save");
                    
                    cols.find("input").css("display", "block");
                    cols.find("span").css("display", "none");
                } else {
                    $(this).text("+ Edit");
                    cols.find("input").css("display", "none");
                    cols.find("span").css("display", "block");
                }
            });
    };

    var contains = function(value, searchFor)
    {
        return (value || '').indexOf(searchFor) > -1;
    }

    return {
        init: init
    };
}();
