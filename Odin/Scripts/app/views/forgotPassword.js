var ForgotPasswordController = function () {

    var init = function () {
        var message = $("#hdnMessage").val()
        if ( message === "error") {
            toast('Uh oh! The email failed to send. Please try again.','danger')
        }
        else if (message === "sent") {
            var txtEmail = $("#Email");
            toast('A reset email has been sent to '+txtEmail.val()+'!', 'info');
            txtEmail.val('');
        }
    }

    var toast = function (message, type) {
        $.notify({
            message: message,
        }, {
                type: type,
                placement: {
                    from: "top",
                    align: "center"
                },
                animate: {
                    enter: 'animated fadeInDown',
                    exit: 'animated fadeOutUp'
                }
            });
    }

    return {
        init: init
    };

}();