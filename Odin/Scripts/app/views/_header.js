var HeaderService = function () {

    var getNotifications = function (success, fail) {
        $.get("/UserNotification/GetUserNotifications/", success).fail(fail);
    }

    var markAsRead = function (success, fail) {
        $.post("/Api/UserNotification/MarkAsRead", success).fail(fail);

    }

    var markAsReadByNotificationID = function (success, fail, UserNotificationId) {
        $.post("/Api/UserNotification/NotificationMarkAsRead/" + UserNotificationId,  success).fail(fail);
    }

    var markAsRemovedByNotificationID = function (success, fail, NotificationId) {
        $.post("/Api/UserNotification/NotificationMarkAsRemoved/" + NotificationId, success).fail(fail);
    }

    return {
        getNotifications: getNotifications,
        markAsRead: markAsRead,
        markAsReadByNotificationID: markAsReadByNotificationID,
        markAsRemovedByNotificationID: markAsRemovedByNotificationID
    }




}();

var HeaderController = function (headerService) {

    var notificationCount = 0;
    var notificationClicked;

    var hideNotification = function () {
        //$("#ulNotifications").remove();
        $("#spanNotificationCount").css("display", "none");
    }

   

    var init = function () {

        //this is to keep the drop down from disappearing on the click.
        $(function () {
            $('.dropdown').on({
                "click": function (event) {
                    if ($(event.target).closest('.dropdown-toggle').length) {
                        $(this).data('closable', true);
                    } else {
                        $(this).data('closable', false);
                    }
                },
                "hide.bs.dropdown": function (event) {
                    hide = $(this).data('closable');
                    $(this).data('closable', true);
                    return hide;
                }
            });
        });


        headerService.getNotifications(getNotificationsSuccess, getNotificationsFail);

        var primaryNav = $("#primaryNav");

        primaryNav.on("click", ".notification[data-isread='False']", function () {
            var UserNotificationId = $(this).attr("data-id");
            //alert(UserNotificationId);
            headerService.markAsReadByNotificationID(markAsReadSuccess, markAsReadFail, UserNotificationId);
        });    


        primaryNav.on("click", ".notification[data-isremoved='False'] .notification-label", function () {
            //alert("from isremoved");
            var notificationId = $(this).parent(".notification").attr("data-id");
            headerService.markAsRemovedByNotificationID(markAsRemovedSuccess, markAsReadFail, notificationId);

        });
    }

    var markAsReadSuccess = function (id) {
        //$("#spanNotificationCount").css("display", "none");
        //notificationCount = 0;
        //alert("1st");
        var clickedNotification = $(".notification[data-id='" + id + "']");
        clickedNotification.attr("data-isread", "True");
        clickedNotification.removeClass("notifications_unread");
        clickedNotification.addClass("notifications_read");

        //alert("2nd");
        var clickedNotificationLabel = clickedNotification.find(".notification-label");
        clickedNotificationLabel.removeClass("notification-close-unread");
        clickedNotificationLabel.addClass("notification-close-read");

        //alert(notificationCount);
        notificationCount--;
        $("#hdnNotificationsCount").val(notificationCount);      

        if (notificationCount > 0) {
            var strCount = notificationCount + "";
            $("#spanNotificationCount").html(strCount + "+")
            //alert("4th");
        }
        else {
            //alert("5th");
            $("#spanNotificationCount").css("display", "none");
        }       
    }

    var markAsReadFail = function () {
        alert("Failed");
    }

    var markAsRemovedSuccess = function (id)
    {
        var clickedNotification = $(".notification[data-id='" + id + "']");
        clickedNotification.attr("data-isremoved", "True");
        clickedNotification.css("display", "none");
        
    }

    var getNotificationsSuccess = function (data) {
        $("#ulNotifications").html(data);

        notificationCount = parseInt($("#hdnNotificationsCount").val());

        if (notificationCount === 0) {
            hideNotification();
        }
        else {
            var strCount = $("#hdnNotificationsCount").val() + "";
            $("#spanNotificationCount").html(strCount + "+")
        }
    }

    var getNotificationsFail = function () {
        alert("Failed");
    }

    return {
        init: init
    };
}(HeaderService);
