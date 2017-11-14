var TransfereeItineraryService = function() {
    var route = "/api/orders/transferee/";

    var updateItineraryBlock = function (block, data, success, fail) {
        var url = route + "/itinerary/" + block;
        $.post(url, data).done(success).fail(fail);
    }
       
    return{
    
    }

}();

var TransfereeItineraryController = function (transfereeItineraryService) {

    var itineraryBlocks;
    var orderId;
       
    var init = function () {

        var pnlItinerary = $("div#itinerary");

        //Init Variables
        itineraryBlocks = pnlItinerary.find(".event-item");
        orderId = pnlItinerary.attr("data-order-id");
        clickable = itineraryBlocks.find(".clickable");
        //Init Dates
        itineraryBlocks.find(".itinerary-date").datetimepicker({
            format: "MM/DD/YYYY",
            useCurrent: true,
            keepOpen: false
        });
        //Bind Events        
        itineraryBlocks.on("click", clickable, toggleCollapse);        
    };
       

    var toggleCollapse = function (e) {
        if ($(e.target).get(0).tagName != 'IMG')
            return;
        var itineraryBlock = $(e.target).parents(".event-item");
        var colImg = itineraryBlock.find(".itinerary-collapse-img");
        var expImg = itineraryBlock.find(".itinerary-expand-img");
        var actionItem = itineraryBlock.find("#more_text");

        if (actionItem.is(":visible")) {            
            actionItem.hide();
            expImg.css("display", "inline");
            colImg.css("display", "none");
        } else {
            actionItem.show();
            expImg.css("display", "none");
            colImg.css("display", "inline");
        }
        
    }


    //Utilities

    var hasAttr = function (obj, attrName)
    {
        var attr = obj.attr(attrName);
        return typeof attr !== typeof undefined && attr !== false && attr !== "" && attr !== null;
    }

    var contains = function(value, searchFor)
    {
        return (value || '').indexOf(searchFor) > -1;
    };
     
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
}(TransfereeItineraryService);
