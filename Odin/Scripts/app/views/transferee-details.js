var TransfereeDetailsController = function () {
    
    var pnlIntake;

    var init = function () {
       
        $('.date').datetimepicker({    
            format: "MM/DD/YYYY",
            useCurrent:true,
            keepOpen: false
        });
        $('.time').datetimepicker({
            format: 'LT'
        });
        pnlDetails = $("div#details");

        pnlDetails.find(".details-header").find("span").on("click",
            function () {
                var cols = $(this).parents(".details-block").find(".details-row > .details-col");                                                                                           
                    cols.find("span").css("display", "block");                
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
