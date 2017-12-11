﻿var TransfereeHousingController = function (TransfereeHousingProperty) {

    var init = function () {
        console.log("Loading Housing");

        $('#propertyForm').submit(function (event) {
            if ($(this).valid()) {
                $.ajax({
                    url: this.action,
                    type: "POST",
                    method: "POST",
                    data: new FormData(this),
                    contentType: false,
                    processData: false,
                    success: function (result) {
                        reloadPropertiesPartial();
                        $(':input', '#propertyForm')
                            .not(':button, :submit, :reset, :hidden')
                            .val('')
                            .removeAttr('checked')
                            .removeAttr('selected');
                        $('#addPropertyModal').modal('hide');
                    },
                    error: function () {
                        toast("An unknown error has occurred.Please try again later.", "danger");
                    }
                });
            }

            event.preventDefault();
            return false;
        });
    };

    /*** Setup Methods ***/
    var setupPropertiesList = function () {
        $('.propertyItem input, .input-group.date-picker').click(function (event) {
            event.stopPropagation();
        });

        setupDatePickers();

        $('.propertyItem').click(function (event) {
            var propertyId = $(event.delegateTarget).data("property-id");
            var propertyModalUrl = '/homefindingproperties/propertypartial/' + propertyId;
            $('#propertyModalContent').load(propertyModalUrl, function (response, status, xhr) {
                if (status === "success") {
                    $('#propertyDetailsModal').modal('show');
                }
            });
        });

        setupLikeDislikeControls();
    };

    var setupLikeDislikeControls = function () {
        $('.likeDislike > .like').unbind('click');
        $('.likeDislike > .like').click(function (e) {
            e.preventDefault();
            e.stopPropagation();

            var controlWrappers = controllerWrappersForLikeDislikeButton($(this));
            controlWrappers.toggleClass("like");
            controlWrappers.removeClass("dislike");

            updateLikedStatusForControl(controlWrappers[0]);
        });

        $('.likeDislike > .dislike').unbind('click');
        $('.likeDislike > .dislike').click(function (e) {
            e.preventDefault();
            e.stopPropagation();

            var controlWrappers = controllerWrappersForLikeDislikeButton($(this));
            controlWrappers.toggleClass("dislike");
            controlWrappers.removeClass("like");

            updateLikedStatusForControl(controlWrappers[0]);
        });
    };

    var setupDatePickers = function () {
        $('.date-picker').datetimepicker({
            format: "MM/DD/YYYY",
            useCurrent: false,
            keepOpen: false
        }).on("dp.change", function (e) {
            console.log(e);
        });
    };


    /*** Private Helpers ***/
    // FIXME: this toas function is in 4 other spots. I'm copy/pasting here for quickness, but we should refactor
    var toast = function (message, type) {
        $.notify({
            message: message
        }, {
                type: type,
                placement: {
                    from: "bottom",
                    align: "center"
                },
                animate: {
                    enter: 'animated fadeInDown',
                    exit: 'animated fadeOutUp'
                }
            });
    };

    var reloadPropertiesPartial = function () {
        $('#propertiesContainer').load('/orders/propertiesPartial/' + currentOrderId);
    };

    var controllerWrappersForLikeDislikeButton = function (likeDislikeButton) {
        var propertyId = $(likeDislikeButton).data('property-id');
        var selectorString = '.likeDislike[data-property-id="' + propertyId + '"]';
        return $(selectorString);
    };


    /*** Update Methods ***/
    var updateLikedStatusForControl = function (controlElement) {
        var classList = controlElement.classList;

        var likedValue = null;
        if (classList.contains('like')) {
            likedValue = true;
        } else if (classList.contains('dislike')) {
            likedValue = false;
        }

        var propertyId = $(controlElement).closest("[data-property-id]").attr('data-property-id');
        var data = {
            id: propertyId,
            liked: likedValue
        };

        var success = function (result) {
            var message = "Your change was saved";

            if (likedValue !== null) {
                var messageVerb = likedValue ? "liked" : "disliked";
                message = "You " + messageVerb + " a property";
            }

            toast(message, "success");
        };

        updateProperty(data, success);
    };

    var updateProperty = function (data, success) {
        $.ajax({
            url: '/HomeFindingProperties/Update/',
            type: 'PUT',
            data: data,
            success: success,
            error: function () {
                toast("An unknown error has occurred.Please try again later.", "danger");
            }
        });
    };


    var deleteProperty = function (propertyId) {
        var confirmed = confirm("Are you sure you want to remove this property?");

        if (confirmed) {
            $.ajax({
                url: '/HomeFindingProperties/Delete/' + propertyId,
                type: 'DELETE',
                success: function (result) {
                    reloadPropertiesPartial();
                    $('#propertyDetailsModal').modal('hide');
                },
                error: function () {
                    toast("An unknown error has occurred.Please try again later.", "danger");
                }
            });
        }
    };

    var export2PDF = function (choice) {
        //window.location.href = "/Orders/PropertiesPartialPDF/" + currentOrderId + "?listChoice=" + choice;
        $.ajax({
            url: "/Orders/PropertiesPartialPDF/" + currentOrderId + "?listChoice=" + choice,
            type: 'GET',
            success: function (result) {
                window.location.href = "/Orders/PropertiesPartialPDF/" + currentOrderId + "?listChoice=" + choice;
            },
            error: function () {
                toast("No properties found that satisfy the selected option.", "warning");
            }
        });
    };

    // FIXME: this toas function is in 4 other spots. I'm copy/pasting here for quickness, but we should refactor
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
                    enter: 'animated fadeInDown',
                    exit: 'animated fadeOutUp'
                }
            });
    };

    return {
        init: init,
        deleteProperty: deleteProperty,
        setupPropertiesList: setupPropertiesList,
        setupLikeDislikeControls: setupLikeDislikeControls,
        setupDatePickers: setupDatePickers,
        export2PDF: export2PDF
    };

}();
