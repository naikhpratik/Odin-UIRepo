﻿var TransfereeHousingController = function (TransfereeHousingProperty) {

    var init = function () {
        console.log("Loading Housing");

        setupLikeDislikeControls();

        setupPropertiesList();

        $('#propertyForm').submit(function (event) {
            if ($(this).valid()) {
                $.ajax({
                    url: this.action,
                    type: "POST",
                    method: "POST",
                    data: new FormData(this),
                    contentType: false,
                    processData: false,
                    success: function(result) {
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

        initMap();

        setupFilterButtons();
    };

    /*** Setup Methods ***/
    var initMap = function() {
        L.mapquest.key = '1tJblQYiEARJGDxuF9gfQVniw3jsi6Ll';

        var mapDiv = $("#map");

        var centLat = mapDiv.attr("data-lat");
        var centLng = mapDiv.attr("data-lng");

        // 'map' refers to a <div> element with the ID map
        var map = L.mapquest.map('map', {
            center: [37.7749, -122.4194],
            layers: L.mapquest.tileLayer('map'),
            zoom: 12
        });


        $("#propertiesList > .propertyItem").each(function (index) {

            var lat = $(this).attr("data-lat");
            var lng = $(this).attr("data-lng");

            if (lat !== "" && lng !== "") {
                if (centLat === "" || centLng === "") {
                    centLat = lat;
                    centLng = lng;
                }
                var marker = L.marker([lat, lng]).addTo(map);

                var propertyAddress = $(this).find(".propertyAddress");
                if (propertyAddress.length > 0) {
                    marker.bindPopup(propertyAddress.html());
                }

                marker.on("mouseover",
                    function(e) {
                        this.openPopup();
                    });

                marker.on("mouseout",
                    function (e) {
                        this.closePopup();
                    });

                var propertyId = $(this).attr("data-property-id");
                marker.on("click",
                    function() {
                        var propertyModalUrl = '/homefindingproperties/propertypartial/' + propertyId;
                        $('#propertyModalContent').load(propertyModalUrl, function (response, status, xhr) {
                            if (status == "success") {
                                $('#propertyDetailsModal').modal('show');
                            }
                        });

                    });
            }
        });

        if (centLat !== 0 && centLng !== 0) {
            map.setView(new L.LatLng(centLat, centLng), 12);
        } else {
            map.setView(new L.LatLng(37.7749, -122.4194), 12);
        }

        //For some reason need to set height then call invalidateSize to get map displaying correctly.
        //Hacky, works now, look for better solution.
        mapDiv.height(300);
        map.invalidateSize(false);
    }
    var setupPropertiesList = function () {
        setupDatePickers();

        $(document).off('click', '.propertyItem');
        $(document).on('click', '.propertyItem', function (event) {

            if (!$(event.target).is("input") &&
                !$(event.target).is("span")) { // prevents the date picker from triggering the modal

                var propertyId = $(this).data("property-id");
                var propertyModalUrl = '/homefindingproperties/propertypartial/' + propertyId;
                $('#propertyModalContent').load(propertyModalUrl, function (response, status, xhr) {
                    if (status === "success") {
                        $('#propertyDetailsModal').modal('show');
                    }
                });
            }
        });
    };
    var setupFilterButtons = function () {
        $('input[type=radio][name=Filter]').change(function () {
            $('#propertiesList').attr('data-filter', this.value);
        });
    };

    var triggerLikeStatus = function (propertyId, newLikedStatus) {

        var likeDislikeElements = likeDislikeElementsForPropertyId(propertyId);
        likeDislikeElements.each(function () {
            var currentLikedStatus = $(this).attr("data-liked");
            $(this).attr('data-liked', currentLikedStatus === newLikedStatus ? "" : newLikedStatus);
        });

        var firstWrapper = likeDislikeElements[0];
        var propertyId = $(firstWrapper).attr('data-property-id');
        var likedData = $(firstWrapper).attr('data-liked');

        var likedValue = null;
        if (likedData == "True") {
            likedValue = true;
        } else if (likedData == "False") {
            likedValue = false;
        }

        updatePropertyLiked(propertyId, likedValue);
    };

    var setupLikeDislikeControls = function () {
        var likeSelector = '.likeDislike > .like';
        $(document).off('click', likeSelector);
        $(document).on('click', likeSelector, function (e) {
            e.preventDefault();
            e.stopPropagation();

            var propertyId = $(this).data('property-id');
            triggerLikeStatus(propertyId, "True");
        });

        var dislikeSelector = '.likeDislike > .dislike';
        $(document).off('click', dislikeSelector);
        $(document).on('click', dislikeSelector, function (e) {
            e.preventDefault();
            e.stopPropagation();

            var propertyId = $(this).data('property-id');
            triggerLikeStatus(propertyId, "False");
        });
    };

    var setupDatePickers = function () {
        $('.date').datetimepicker({
            format: "DD-MMM-YYYY h:mm A",
            useCurrent: false,
            keepOpen: true,
            showClose: true,
            toolbarPlacement: 'bottom',
            icons: { close: 'custom-icon-check' }
        }).on("dp.hide", function (e) {
            var success = function (result) {
                toast("You scheduled to view the property at " + e.date.format("MM/DD/YYYY h:mm A"), "success");
            };

            var propertyId = $(this).closest("[data-property-id]").attr('data-property-id');
            var data = {
                id: propertyId,
                viewingDate: e.date.format("MM/DD/YYYY h:mm A")
            };

            updateProperty("viewingdate", data, success);
        });
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

    /**
     * Return all DOM elements that track the liked value for the property matching propertyId
     * @param {any} propertyId
     */
    var likeDislikeElementsForPropertyId = function (propertyId) {
        var selectorString = '[data-property-id="' + propertyId + '"][data-liked]';
        return $(selectorString);
    };


    /*** Update Methods ***/
    var updatePropertyLiked = function (propertyId, likedValue) {
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

        updateProperty("liked", data, success);
    };

    var updateProperty = function (action, data, success) {
        $.ajax({
            url: '/HomeFindingProperties/Update' + action,
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

    return {
        init: init,
        deleteProperty: deleteProperty,
        setupDatePickers: setupDatePickers,
        export2PDF: export2PDF
    };

}();
