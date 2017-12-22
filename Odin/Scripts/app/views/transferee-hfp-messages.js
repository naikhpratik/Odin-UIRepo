var transfereeHousingMessages = function (){
     var route = "/api/orders/transferee/";

    var updateMessage = function (block, data, success, fail) {
        var url = route + "/housing/messages/";
    $.post(url, data).done(success).fail(fail);
     }    
    return {
        updateMessage: updateMessage
    }
}();
var TransfereeMessagesController = function (transfereeHousingMessages) {
    
    var init = function () {
        var messagesModal = $("div#messagesModal");
        var modalParent = messagesModal.parent().parent();

        //Save Event for message
        modalParent.on("click", ".btn-primary.send", saveMessage);
        modalParent.on("click", ".btn-secondary", readMessages);
    };
    
    var saveMessage = function (e) {

        var orderId = $('div#housing').attr('data-order-id');
        var message = $('div#messagesModal').find('#message').val();        
        if (message === 'Enter your comment here...' || message.length === 0) {
            alert('Please type a message to send.');
            return;
        }
        var propertyId = $('div#messagesModal').attr('data-property-id');
        var currDate = moment().format('YYYY-MM-DD HH:mm');
        var err = false;
        var saveSuccess = function () {      
            var app = $('#modalNotification');            
            var url = '/Message/MessagePartial/' + propertyId;
            $.get(url, function (data) {
                app.find('.modal-content').html();
                app.find('.modal-content').html(data);
            });
        }
        var saveFail = function () {
            err = true;
            toast('The message was not sent', 'danger');
        }       

        if (!err) {
            
            var data = { "HomeFindingPropertyId": propertyId, "MessageDate": currDate, "MessageText": message, "OrderId":orderId };
            transfereeHousingMessages.updateMessage("messages", data, saveSuccess, saveFail);
        }
    }    

    var readMessages = function (e) {
        var propertyId = $('#messagesModal').attr('data-property-id');
        var Url = "/api/orders/transferee/housing/markRead/" + propertyId;

        $.ajax({
            url: Url,
            type: 'POST',
            success: function (result) {
                $('body').removeClass('modal-open');
                OrdersPageController.loadPanel("housing");
            },
            error: function () {
                //toast("No messageses have been marked as read.", "warning");
            }
        });
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
}(transfereeHousingMessages);
