﻿var TransfereeItineraryService = function() {
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

        //Init Dates
        itineraryBlocks.find(".itinerary-date").datetimepicker({
            format: "MM/DD/YYYY",
            useCurrent: true,
            keepOpen: false,
            showClose: true,
            toolbarPlacement: 'bottom',
            icons: { close: 'custom-icon-check' }
        });

        //Bind Events
        $('#cmdPDF').click(function () {
            window.location.href = "/Orders/GeneratePDF/" + $('#itinerary').attr("data-order-id");
        });

        $('#cmdEmail').click(function () {
            var url = "/Email/EmailForm/" + $('#itinerary').attr("data-order-id");
            var app = $("#modalForm");
            app.find('.modal-title').text("Email Message");
            app.find(".modal-footer").css("display", "none");
            $.get(url, function (data) {
                app.find('.modal-body').html();
                app.find('.modal-body').html(data);
            });
        });

        $('.showAppointment').click(function () {
            var app = $('#modalForm');
            app.find('.modal-title').text("Appointment");
            var url = '/Appointment/appointmentPartial/' + $(this).attr('data-appointment-id');
            app.find(".modal-footer").css("display", "block");
            app.find(".delete").css("display", "block");
            $.get(url, function (data) {
                app.find('.modal-body').html();
                app.find('.modal-body').html(data);
            });
        });
        $('#cmdNew').click(function () {
            var app = $('#modalForm');
            app.find('.modal-title').text("New Appointment");
            app.find(".modal-footer").css("display", "block");
            app.find(".delete").css("display", "none");
            var url = '/Appointment/appointmentPartial/';

            app.find('.modal-body').load(url, function (response, status, xhr) {
                if (status === "success") {
                    app.modal('show');
                }
            });
        });

        $(".new").click(function (e) {
            e.stopPropagation();
            return false;
        })
    }
   
    //formaction = '@Url.Action("Index","email")'

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
