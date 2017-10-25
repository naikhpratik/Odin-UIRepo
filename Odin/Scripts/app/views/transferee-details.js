var TransfereeDetailsController = function () {
    
    
    var init = function () {
       
        $('.date').datetimepicker({    
            format: "MM/DD/YY",
            useCurrent:true,
            keepOpen: false
        });
        $('.time').datetimepicker({
            format: 'LT',
            showClose: true,
            toolbarPlacement: 'bottom',
            icons: { close: 'custom-icon-check'}
        });
        pnlDetails = $("div#details");

        pnlDetails.find(".details-header").find("span").on("click",
            function () {
                var cols = $(this).parents(".details-blocks").find(".details-row > .details-col");                                                                                           
                    cols.find("span").css("display", "block");                
            });

        //Init Variables
        detailBlocks = pnlDetails.find(".details-blocks");
        orderId = pnlDetails.attr("data-order-id");

        //Save Event for Services
        detailsBlocks.on("click", ".details-save", saveBlock);
    };

    var saveBlock = function (e) {
        var detailsBlock = $(e.target).parents(".details-blocks");
        var block = detailsBlock.attr("data-block");
        var rows = detailsBlock.find(".details-row[data-entity-id]");

        var saveSuccess = function () { }
        var saveFail = function () {
            alert("failed");
        }

        var data = { "Id": orderId };

        rows.each(function () {
            var row = $(this);
            var rowInputs = row.find(":input").not("input[type='hidden']");

            //If a collection that can have added values
            if (hasAttr(row, 'data-entity-collection')) {
                if (!isCollectionRowEmpty(rowInputs)) {
                    var collectionKey = row.attr("data-entity-collection");
                    var collectionData = { "Id": row.attr("data-entity-id") };

                    fillPostData(collectionData, rowInputs);

                    if (!(collectionKey in data)) {
                        data[collectionKey] = [];
                    }
                    data[collectionKey].push(collectionData);
                }
            } else {
                fillPostData(data, rowInputs);
            }
        });

        transfereedetailsService.updatedetailsBlock(block, data, saveSuccess, saveFail);
    }

    var fillPostData = function (data, inputs) {
        inputs.each(function () {
            var input = $(this);
            if (input.is(":checkbox")) {
                data[input.attr("name")] = input.prop('checked');
            } else {
                data[input.attr("name")] = input.val();
            }
        });
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
      

    return {
        init: init
    };
}();
