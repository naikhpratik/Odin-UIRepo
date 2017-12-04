var HeaderService = function () {

    var getNotifications = function (success, fail) {
        $.get("/UserNotification/GetUserNotifications/", success).fail(fail);
    }

    var markAsRead = function (success, fail) {
        $.post("/Api/UserNotification/MarkAsRead", success).fail(fail);

    }

    var markAsReadByNotificationID = function (success, fail, NotificationId) {
        $.post("/Api/UserNotification/NotificationMarkAsRead/" + NotificationId,  success).fail(fail);
    }

    return {
        getNotifications: getNotifications,
        markAsRead: markAsRead,
        markAsReadByNotificationID: markAsReadByNotificationID
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
        headerService.getNotifications(getNotificationsSuccess, getNotificationsFail);

        var primaryNav = $("#primaryNav");

        primaryNav.on("click", ".notification[data-isread='False']", function () {
            var notificationId = $(this).attr("data-id");
            headerService.markAsReadByNotificationID(markAsReadSuccess, markAsReadFail, notificationId);
        });    
    }

    var markAsReadSuccess = function (id) {
        //$("#spanNotificationCount").css("display", "none");
        //notificationCount = 0;

        var clickedNotification = $(".notification[data-id='" + id + "']");
        clickedNotification.attr("data-isread", "True");
        clickedNotification.removeClass("notifications_unread");
        clickedNotification.addClass("notifications_read");

        var clickedNotificationLabel = clickedNotification.find(".notification-label");
        clickedNotificationLabel.removeClass("notification-close-unread");
        clickedNotificationLabel.addClass("notification-close-read");

        notificationCount--;
        $("#hdnNotificationsCount").val(notificationCount);      

        if (notificationCount > 0) {
            var strCount = notificationCount + "";
            $("#spanNotificationCount").html(strCount + "+")
        }
        else {
            $("#spanNotificationCount").css("display", "none");
        }       
    }

    var markAsReadFail = function () {
        alert("Failed");
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
