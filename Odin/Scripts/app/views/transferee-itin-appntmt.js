var TransfereeItineraryAppointment = function (){
     var route = "/api/orders/transferee/";

    var updateAppointment = function (block, data, success, fail) {
        var url = route + "/itinerary/" + block + "/";
    $.post(url, data).done(success).fail(fail);
     }

    var deleteAppointment = function (deleteType, apptmtId, success, fail) {
        var url = route  + "/itinerary/" + deleteType + "/" + apptmtId;        
    $.ajax({
        url: url,
        type: 'DELETE'
    }).done(success).fail(fail);
    }
    return {
    updateAppointment: updateAppointment,
    deleteAppointment: deleteAppointment
    }
}();
var TransfereeAppointmentController = function (transfereeItineraryAppointment) {
    
    var init = function () { 
        var appointmentModal = $("div#appointmentModal");        
        appointmentModal.find('.date').datetimepicker({    
            format: "DD-MMM-YYYY HH:MM A",
            showClose: true,
            toolbarPlacement: 'bottom',
            icons: { close: 'custom-icon-check' },
            useCurrent:true,
            keepOpen: false
        });

        var modalParent = appointmentModal.parent().parent();
        //New Event for Appointment
        modalParent.on("click", ".new", newAppointment);
       
        //Save Event for Appointment
        modalParent.on("click", ".btn-primary", saveAppointment);

        //Delete Event for Appointment
        modalParent.on("click", ".delete", deleteAppointment);
    };

    var newAppointment = function (e) {
        var appointment = $(e.target).parent().parent().parent().find("div#appointmentModal");        
        appointment.attr("data-appointment-id", '');
        var dt = appointment.children().find("input[name=ScheduledDate]");
        dt.val('');
        var ds = appointment.find("#Description");
        ds.attr('value', '');
    }

    var deleteAppointment = function (e) {        
        if (confirm("Are you sure you want to delete this appointment?")) {
            var appointment = $(e.target).parent().parent().parent().find("div#appointmentModal");     
            var appointmentId = appointment.attr("data-appointment-id");
            var orderId = $('div#itinerary').attr('data-order-id');
            var err = false;

            var deleteSuccess = function () {
                toast('the appointment was deleted successfully', 'success');
                $('body').removeClass('modal-open');
                OrdersPageController.loadPanel("itinerary");
            }
            var deleteFail = function () {
                toast('the appointment deletion failed', 'danger');                
            }

            if (orderId === undefined || appointmentId === undefined)
                err=true;           

            //var data = { "Id": orderId, "AppointmentId": appointmentId };

            if (!err)
                TransfereeItineraryAppointment.deleteAppointment("appointment", appointmentId, deleteSuccess, deleteFail);
        }
    }
    var saveAppointment = function (e) {
        
        var appointment = $(e.target).parent().parent().parent().find("div#appointmentModal");
        var orderId = $('div#itinerary').attr('data-order-id');
        var appointmentId = appointment.attr("data-appointment-id");
        var dt = appointment.children().find("input[name=ScheduledDate]");
        var ds = appointment.find("#Description");

        var err = false;
       
        var saveSuccess = function () {
            toast('changes to the appointment are successful', 'success');
            $('body').removeClass('modal-open');
            OrdersPageController.loadPanel("itinerary");           
        }
        var saveFail = function () {
            err = true;
            toast('changes to the appointment failed', 'danger');
        }       
       
        if (!err) {
            
            var data = { "Id": appointmentId, "OrderId": orderId, "ScheduledDate": dt.val(), "Description": ds.val() };
            TransfereeItineraryAppointment.updateAppointment("appointment", data, saveSuccess, saveFail);
        }
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
}(TransfereeItineraryAppointment);
