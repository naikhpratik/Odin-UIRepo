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
                        $('#propertiesContainer').load('/orders/propertiesPartial/' + currentOrderId);
                        $(':input', '#propertyForm')
                            .not(':button, :submit, :reset, :hidden')
                            .val('')
                            .removeAttr('checked')
                            .removeAttr('selected');
                        $('#addPropertyModal').modal('hide');
                    }
                });
            }

            event.preventDefault();
            return false;
        });
    };

    return {
        init: init
    };

}();






//var TransfereeHousingProperty = function (){
//     var route = "/api/orders/transferee/";

//var updatePropertyBlock = function (block, data, success, fail) {
//    var url = route + "/property/" + block;
//    $.post(url, data).done(success).fail(fail);
//     }
//return {
//    updatePropertyBlock: updatePropertyBlock    
//}

//}();
//var TransfereeHousingController = function (TransfereeHousingProperty) {
    
//    var init = function () {

//        var pnlHousing = $("div#housing");
//        var propertiesBlock = pnlHousing.find("#propertiesBlock");
       
//        propertiesBlock.find('.date').datetimepicker({                
//            useCurrent:true,
//            keepOpen: false
//        });
        
        

//        //pnlHousing.find(".details-header").find("span").on("click",
//        //    function () {
//        //        var cols = $(this).parents(".details-blocks").find(".details-row > .details-col");                                                                                           
//        //        cols.find("span").css("display", "block");                  
//        //    });

//        //Init Variables
//        housingBlocks = pnlHousing.find(".details-blocks");
//        orderId = pnlHousing.attr("data-order-id");

//        //Save Event for Services
//        //detailsProperty.on("click", ".details-save", saveBlock);
//    };

//    //var saveBlock = function (e) {

//    //    var detailsBlock = $(e.target).parents(".details-blocks");
//    //    var block = detailsBlock.attr("data-block");
//    //    var rows = detailsBlock.find(".details-row[data-entity-id]");
//    //    var err = false;
//    //    var saveSuccess = function () {
//    //        toast('changes to service dates are successful', 'success');
//    //    }
//    //    var saveFail = function () {
//    //        toast('changes to service dates failed', 'danger');
//    //    }

//    //    var data = { "Id": orderId };
       
//    //    rows.each(function () {
//    //        if (err)
//    //            return;
//    //        var row = $(this);
//    //        var rowInputs = row.find(":input").not("input[type='hidden']");
//    //        if (hasAttr(row, 'data-entity-collection')) {
//    //            if (!isCollectionRowEmpty(rowInputs)) {
//    //                var collectionKey = row.attr("data-entity-collection");
//    //                var collectionData = { "Id": row.attr("data-entity-id") };
                    
//    //                var ret = fillPostData(collectionData, rowInputs);
//    //                if (ret == -1) {
//    //                    err = true;
//    //                    return;
//    //                }
//    //                if (!(collectionKey in data)) {
//    //                    data[collectionKey] = [];
//    //                }
//    //                data[collectionKey].push(collectionData);
//    //            }
//    //        } else {
//    //            fillPostData(data, rowInputs);
//    //        }
            
//    //    });
//    //     if (!err)            
//    //    TransfereeDetailsService.updateDetailsBlock(block, data, saveSuccess, saveFail);
//    //}

//    //var fillPostData = function (data, inputs) {
//    //    var sd = '';
//    //    var st = '';
//    //    var cd = '';
//    //    inputs.each(function () {
//    //        var input = $(this);
//    //        sd = input.attr("name") == "ScheduledDate" ? input.val() : sd;
//    //        st = input.attr("name") == "ScheduledTime" ? input.val() : st;
//    //        cd = input.attr("name") == "CompletedDate" ? input.val() : cd;
//    //    });
//    //    if (sd.length > 0 && st == '') {            
//    //        $('.text-danger[data-entity-id="' + data['Id'] + '"]').show();
//    //        return -1;            
//    //    }
//    //    else           
//    //        $('.text-danger[data-entity-id="' + data['Id'] + '"]').hide();

//    //    data["ScheduledDate"] = sd + ' ' + st;        
//    //    data["CompletedDate"] = cd;
        
//    //}

//    //var isCollectionRowEmpty = function (rowInputs) {
//    //    return rowInputs.filter(function () { return $.trim($(this).val()) !== ""; }).length === 0;
//    //}

//    //var hasAttr = function (obj, attrName) {
//    //    var attr = obj.attr(attrName);
//    //    return typeof attr !== typeof undefined && attr !== false && attr !== "" && attr !== null;
//    //}

//    //var contains = function(value, searchFor)
//    //{
//    //    return (value || '').indexOf(searchFor) > -1;
//    //}

//    var toast = function (message, type) {
//        $.notify({
//            message: message
//        }, {
//            delay: 2000,
//            type: type,
//            placement: {
//                from: "bottom",
//                align: "center"
//            },
//            animate: {
//                enter: 'animated fadeInUp',
//                exit: 'animated fadeOutDown'
//            }
//        });
//    }


//    return {
//        init: init
//    };
//}(TransfereeHousingProperty);
