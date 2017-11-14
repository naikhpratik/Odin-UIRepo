var TransfereeDetailsService = function (){
     var route = "/api/orders/transferee/";

var updateDetailsBlock = function (block, data, success, fail) {
    var url = route + "/details/" + block;
    $.post(url, data).done(success).fail(fail);
     }
return {
    updateDetailsBlock: updateDetailsBlock    
}

}();
var TransfereeDetailsController = function (transfereeDetailseService) {
    
    var init = function () {

        var pnlDetails = $("div#details");
        var detailsBlocks = pnlDetails.find("#servicesBlock");

        detailsBlocks.find('.date').datetimepicker({    
            format: "MM/DD/YYYY",
            useCurrent:true,
            keepOpen: false
        }).on("dp.change", function (e) {
            saveBlock(e);
        });
        detailsBlocks.find('.time').datetimepicker({
            format: 'LT',
            showClose: true,
            toolbarPlacement: 'bottom',
            icons: { close: 'custom-icon-check'}
        });
        

        pnlDetails.find(".details-services").find("span").on("click",
            function () {
                var cols = $(this).parents("[data-entity-collection = 'services']").find(".service-col");                                                                                           
                cols.find("span").css("display", "block");                  
            });

        //Init Variables
        detailsBlocks = pnlDetails.find(".details-services");
        orderId = pnlDetails.attr("data-order-id");

        //Save Event for Services
        detailsBlocks.on("click", ".sectionSave", saveBlock);
    };

    var saveBlock = function (e) {

        var detailsBlock = $(e.target).parents(".details-services");

        var block = detailsBlock.attr("data-block");
        var rows = detailsBlock.find(".details-row[data-entity-id]");
        var err = false;
       
        var saveSuccess = function () {
            toast('changes to service dates are successful', 'success');
        }
        var saveFail = function () {
            toast('changes to service dates failed', 'danger');
        }

        var data = { "Id": orderId };
        
        rows.each(function () {
            if (err)
                return;
            var row = $(this);
            var rowInputs = row.find(":input").not("input[type='hidden']");
            if (hasAttr(row, 'data-entity-collection')) {
                if (!isCollectionRowEmpty(rowInputs)) {
                    var collectionKey = row.attr("data-entity-collection");
                    var collectionData = { "Id": row.attr("data-entity-id") };
                    
                    var ret = fillPostData(collectionData, rowInputs);
                    if (ret == -1) {
                        err = true;
                        return;
                    }
                    if (!(collectionKey in data)) {
                        data[collectionKey] = [];
                    }
                    data[collectionKey].push(collectionData);
                }
            } else {
                fillPostData(data, rowInputs);
            }            
        });
         if (!err)            
        TransfereeDetailsService.updateDetailsBlock(block, data, saveSuccess, saveFail);
    }

    var fillPostData = function (data, inputs) {
        var sd = '';
        var st = '';
        var cd = '';
        var timeTag;
        inputs.each(function () {
            var input = $(this);
            sd = input.attr("name") == "ScheduledDate" ? input.val() : sd;
            if (input.attr("name") == "ScheduledTime")
            { 
                st = input.val();
                timeTag = input.parent().find('.glyphicon-time');                
            }
            cd = input.attr("name") == "CompletedDate" ? input.val() : cd;
        });
        if (sd.length > 0 && st == '') {
            toast('schedule time is required', 'danger');
            timeTag.click();
            err = -1;
            return err;            
        }
        
        data["ScheduledDate"] = sd + ' ' + st;        
        data["CompletedDate"] = cd;
        
    }

    var isCollectionRowEmpty = function (rowInputs) {
        return rowInputs.filter(function () { return $.trim($(this).val()) !== ""; }).length === 0;
    }

    var hasAttr = function (obj, attrName) {
        var attr = obj.attr(attrName);
        return typeof attr !== typeof undefined && attr !== false && attr !== "" && attr !== null;
    }

    var contains = function(value, searchFor)
    {
        return (value || '').indexOf(searchFor) > -1;
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
}(TransfereeDetailsService);
