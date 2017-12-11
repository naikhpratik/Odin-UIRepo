var TransfereeHousingController = function (TransfereeHousingProperty) {

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
                        alert("An unknown error has occurred. Please try again later.");
                    }
                });
            }

            event.preventDefault();
            return false;
        });

        $('.likeDislike > .like').click(function (e) {
            e.preventDefault();
            e.stopPropagation();

            var parentElement = $(this.parentElement);
            parentElement.toggleClass("like");
            parentElement.removeClass("dislike");

            updateLikedStatusForControl(parentElement[0]);
        });

        $('.likeDislike > .dislike').click(function (e) {
            e.preventDefault();
            e.stopPropagation();

            var parentElement = $(this.parentElement);
            parentElement.toggleClass("dislike");
            parentElement.removeClass("like");

            updateLikedStatusForControl(parentElement[0]);
        });
    };

    var updateLikedStatusForControl = function (controlElement) {
        console.log(controlElement);
        console.log(controlElement.classList);
        var classList = controlElement.classList;

        var likedValue = null;
        if (classList.contains('like')) {
            likedValue = true;
        } else if (classList.contains('dislike')) {
            likedValue = false;
        }

        console.log(likedValue);

        var propertyId = $(controlElement).parents('li.propertyItem').attr('data-property-id')
        var postData = {
            id: propertyId,
            liked: likedValue
        };

        $.ajax({
            url: '/HomeFindingProperties/Update/',
            type: 'PUT',
            data: postData,
            success: function (result) {
                console.log("Weee");
                console.log(result);
            },
            error: function () {
                alert("An unknown error has occurred. Please try again later.");
            }
        });
    };

    var reloadPropertiesPartial = function () {
        $('#propertiesContainer').load('/orders/propertiesPartial/' + currentOrderId);
    };

    var deleteProperty = function (propertyId) {
        var confirmed = confirm("Are you sure you want to remove this property?");

        if (confirmed) {
            $.ajax({
                url: '/HomeFindingProperties/Delete/'+propertyId,
                type: 'DELETE',
                success: function (result) {
                    reloadPropertiesPartial();
                    $('#propertyDetailsModal').modal('hide');
                },
                error: function () {
                    alert("An unknown error has occurred. Please try again later.");
                }
            });
        }
    };

    return {
        init: init,
        deleteProperty: deleteProperty
    };

}();
