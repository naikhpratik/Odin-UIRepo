var TransfereeDetailsService = function (){
     var route = "/api/orders/transferee/";

var updateServicesBlock = function (block, data, success, fail) {
    var url = route + "/details/" + block;
    $.post(url, data).done(success).fail(fail);
     }
return {
    updateServicesBlock: updateServicesBlock    
}

}();
var TransfereeDetailsController = function (transfereeDetailseService) {
    
    var init = function () {

        var pnlDetails = $("div#details");
        var servicesBlocks = pnlDetails.find("#servicesBlock");

        servicesBlocks.find('.date').datetimepicker({
            format: "DD-MMM-YYYY",
            useCurrent: false,
            keepOpen: false
        }).on("dp.change", function (e) { saveServices(e); });

        servicesBlocks.find('.time').datetimepicker({
            format: 'LT',
            showClose: true,
            useCurrent: false,
            toolbarPlacement: 'bottom',
            keepOpen: false,
            icons: { close: 'custom-icon-check'}
        }).on("dp.hide", function (e) { saveServices(e); });


        //Init Variables
        servicesBlocks = pnlDetails.find(".details-services");
        orderId = pnlDetails.attr("data-order-id");

        //Save Event for Services
        servicesBlocks.on("click", ".sectionSave", saveServices);
    };

    var saveServices = function (e) {;
        var detailsBlock = $(e.target).parents(".details-services");
        var who = $(e.target).attr("class");
        var err = false;
        var block = detailsBlock.attr("data-block");
        var rows = detailsBlock.find(".details-row[data-entity-id]");        

        var saveSuccess = function () {
            toast('changes to service dates are successful', 'success');
        }
        var saveFail = function () {
            toast('changes to service dates failed', 'danger');
        }

        var data = { "Id": orderId };
        
        rows.each(function () {
           
            var row = $(this);

            if (hasAttr(row, 'data-entity-collection')) {
                var rowInputs = row.find(":input").not("input[type='hidden']");
                var collectionKey = row.attr("data-entity-collection");
                var collectionData = { "Id": row.attr("data-entity-id") };
                var ret = fillPostData(collectionData, rowInputs, who);                    
                if (ret == -1) {
                    err = true;
                    return;
                }
                if (!(collectionKey in data)) {
                    data[collectionKey] = [];
                }
                data[collectionKey].push(collectionData);
            }
            else {
                fillPostData(data, rowInputs);
            }            
        });
         if (!err)            
             TransfereeDetailsService.updateServicesBlock(block, data, saveSuccess, saveFail);
    }

    var fillPostData = function (data, inputs, who) {
        var sd = '';
        var st = '';
        var cd = '';
        var timeTag;
        var dateTag;

        inputs.each(function () {
            var input = $(this);
            if (input.attr("name") == "ScheduledDate") {
                sd = input.val();
                dateTag = input.parent().find('.glyphicon-calendar');
            }
            if (input.attr("name") == "ScheduledTime")
            { 
                st = input.val();
                timeTag = input.parent().find('.glyphicon-time');                
            }
            cd = input.attr("name") == "CompletedDate" ? input.val() : cd;                   
        });
        
        if (sd.length == 0 && st.length > 0) {    
            if (who == 'sectionSave') 
                toast('schedule date is required', 'danger');
            else
                toast('schedule date is required', 'warning');
            dateTag.click();
            err = -1;
            return err;
        }
        if (sd.length > 0 && st == '') { 

            if (who == 'sectionSave') 
                toast('schedule time is required', 'danger');
            else
                toast('schedule time is required', 'warning');
            timeTag.click();
            err = -1;
            return err;
        }
        if (sd.length == 0 || sd == undefined) {
            data["ScheduledDate"] = '';            
        }
        else {
            data["ScheduledDate"] = sd + ' ' + st;
        }
        data["CompletedDate"] = cd;
        return 0;
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
