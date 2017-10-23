var TransfereeIntakeService = function() {
    var route = "/api/orders/transferee/";

    var updateIntakeBlock = function (block, data, success, fail) {
        var url = route + "/intake/" + block;
        $.post(url, data).done(success).fail(fail);
    }

    var insertIntakeEntity = function(addType, orderId, success, fail) {
        var url = route + addType + "/" + orderId;
        var data = {"orderId":orderId}
        $.post(url, orderId).done(success).fail(fail);
    }

    var deleteIntakeEntity = function(delType, entityId, success, fail) {
        var url = route + delType + "/" + entityId;
        $.ajax({
            url: url,
            type: 'DELETE'
        }).done(success).fail(fail);
    }

    var insertIntakeEntityWithType = function(addType, orderId, typeId, success, fail) {
        var url = route + addType + "/" + orderId + "/" + typeId;
        $.post(url).done(success).fail(fail);
    }

    return{
        updateIntakeBlock: updateIntakeBlock,
        insertIntakeEntity: insertIntakeEntity,
        deleteIntakeEntity: deleteIntakeEntity,
        insertIntakeEntityWithType: insertIntakeEntityWithType
    }

}();

var TransfereeIntakeController = function (transfereeIntakeService) {

    var intakeBlocks;
    var orderId;

    var childTemplate =
        '<div class="row intake-row collapse in" data-entity-collection="children" data-entity-id="#GUID#"> <div class="col-sm-3 intake-col"> <label>Child:</label> <span></span> <input type="text" class="form-control" name="Name"/> <input type="hidden" name="Name" /> </div> <div class="col-sm-3 intake-col"> <label>Age:</label> <span></span> <input type="text" class="form-control" name="Age"/> <input type="hidden" name="Age" /> </div> <div class="col-sm-3 intake-col"> <label>Grade:</label> <span></span> <input type="text" class="form-control" name="Grade"/> <input type="hidden" name="Grade" /> </div> <div class="col-sm-3 intake-col"> <span class="intake-del"> x Delete </span> </div> </div>';

    var init = function () {

        var pnlIntake = $("div#intake");

        //Init Variables
        intakeBlocks = pnlIntake.find(".intake-block");
        orderId = pnlIntake.attr("data-order-id");

        //Bind Events
        intakeBlocks.on("click", ".intake-edit", editSaveBlock);
        intakeBlocks.on("click", ".intake-cancel", cancelEditBlock);
        intakeBlocks.on("click", ".intake-add", addRowToBlock);
        intakeBlocks.on("click", ".intake-collapse", toggleCollapse);
        intakeBlocks.on("click", ".intake-del", deleteRowFromBlock);
        intakeBlocks.on("click", ".intake-save", saveBlock);
        intakeBlocks.on("change", "[type=checkbox]", saveChecked);
    };

    //Event Callbacks

    var editSaveBlock = function (e) {

        //Local Variables
        var spnEditSave = $(e.target);
        var intakeBlock = spnEditSave.parents(".intake-block");
        var rows = intakeBlock.find(".intake-row");
        var spans = rows.find("span");
        var inputs = rows.find(":input").not("input[type='hidden']");

        //Local Functions
        var edit = function () {
            //Toggle Text
            spnEditSave.text("- Save");
            spnEditSave.next(".intake-cancel").css("display", "inline");

            //Bind hidden values to inputs
            inputs.css("display", "block").each(function() {
                var hidden = $(this).next("input[type='hidden']");
                $(this).val(hidden.val());
            });
            spans.css("display", "none");
        }

        var save = function() {
            //Prepare post url and data
            var block = intakeBlock.attr("data-block");
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

            //Do save
            transfereeIntakeService.updateIntakeBlock(block, data, saveSuccess, saveFail);
        }

        var saveSuccess = function (data) {
            //Revert Text
            spnEditSave.text("+ Edit");
            spnEditSave.next(".intake-cancel").css("display", "none");

            //Update spans to new values and toggle.
            inputs.each(function () {
                $(this).prev("span").text($(this).val());
                $(this).next("input[type='hidden']").val($(this).val());
            });
            inputs.css("display", "none");
            spans.css("display", "block");
        }

        var saveFail = function() {
            alert('fail');
        }

        //Function Execution
        expand(intakeBlock);
        if (contains(spnEditSave.text(), "Edit")) {
            edit();
        } else {
            save();
        }
    }

    var cancelEditBlock = function (e) {
        var intakeBlock = $(e.target).parents(".intake-block");
        cancel(intakeBlock);
    }

    var addRowToBlock = function (e) {

        var spnAdd = $(e.target);
        var addType = spnAdd.attr("data-entity-add");
        var template = '';
        switch(addType) {
            case 'children':
                template = childTemplate;
                break;
            default:
                break;
        }

        var insertSuccess = function (guid) {
            var row = spnAdd.parents(".intake-row");
            template = template.replace("#GUID#", guid);
            row.after(template);
            row.parents(".intake-block").find(".intake-edit").click();
        }

        var insertFail = function() {
            alert('failed');
        }

        //Function Execution
        TransfereeIntakeService.insertIntakeEntity(addType, orderId, insertSuccess, insertFail);
    }

    var deleteRowFromBlock = function (e) {
        var spnDel = $(e.target);
        var row = spnDel.parents(".intake-row");
        var delType = row.attr("data-entity-collection");
        var entityId = row.attr("data-entity-id");

        var deleteSuccess = function() {
            row.remove();
        }

        var deleteFail = function() {
            alert('failed');
        }

        transfereeIntakeService.deleteIntakeEntity(delType, entityId, deleteSuccess, deleteFail);
    }

    var toggleCollapse = function (e) {
        var intakeBlock = $(e.target).parents(".intake-block");
        var img = intakeBlock.find(".intake-collapse-img");
        var src = img.attr("src");

        if (contains(src,"collapse")) {
            collapse(intakeBlock);
            if (intakeBlock.find(".intake-cancel").length > 0) {
                cancel(intakeBlock);
            }
        } else {
            expand(intakeBlock);
        }
        
    }

    var saveBlock = function(e) {
        var intakeBlock = $(e.target).parents(".intake-block");
        var block = intakeBlock.attr("data-block");
        var rows = intakeBlock.find(".intake-row[data-entity-id]");

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

        transfereeIntakeService.updateIntakeBlock(block, data, saveSuccess, saveFail);
    }

    var saveChecked = function (e) {

        var checkbox = $(e.target);
        var row = checkbox.parents(".intake-row");
        var rowInputs = row.find(":input").not("input[type='hidden']");

        var saveSuccess = function(data) {
            row.attr("data-entity-id", data);
        }

        var saveFail = function () {
            rowInputs.not("[type=checkbox]").prop("disabled", true);
            alert("failed");
        }

        if (checkbox.prop('checked')) {
            rowInputs.prop("disabled", false);
            if (!hasAttr(row, "data-entity-id")) {

                var addType = row.attr("data-entity-collection");
                var typeId = row.attr("data-entity-type-id");
                
                //create new service
                transfereeIntakeService.insertIntakeEntityWithType(addType, orderId, typeId, saveSuccess, saveFail);
                
            }
        } else {
            rowInputs.not("[type=checkbox]").prop("disabled", true);
        }
    }

    //Utilities

    var fillPostData = function(data, inputs){
        inputs.each(function () {
            var input = $(this);
            if (input.is(":checkbox")) {
                data[input.attr("name")] = input.prop('checked');
            } else {
                data[input.attr("name")] = input.val();
            }
        });
    }

    var isCollectionRowEmpty = function(rowInputs) {
        return rowInputs.filter(function() { return $.trim($(this).val()) !== ""; }).length === 0;
    }

    var hasAttr = function (obj, attrName)
    {
        var attr = obj.attr(attrName);
        return typeof attr !== typeof undefined && attr !== false && attr !== "" && attr !== null;
    }

    var contains = function(value, searchFor)
    {
        return (value || '').indexOf(searchFor) > -1;
    }

    var expand = function (intakeBlock) {
        var img = intakeBlock.find(".intake-collapse-img");
        var src = img.attr("src");
        var intakeRows = intakeBlock.find(".intake-row");
        if (!intakeRows.hasClass("in")) {
            intakeBlock.find(".intake-row").collapse("show");
            img.attr("src", src.replace("expand", "collapse"));
        }
    }

    var collapse = function(intakeBlock) {
        var img = intakeBlock.find(".intake-collapse-img");
        var src = img.attr("src");
        var intakeRows = intakeBlock.find(".intake-row");
        if (intakeRows.hasClass("in")) {
            intakeBlock.find(".intake-row").collapse("hide");
            img.attr("src", src.replace("collapse", "expand"));
        }
    }

    var cancel = function (intakeBlock) {
        spnCancel = intakeBlock.find(".intake-cancel");
        var cols = intakeBlock.find(".intake-row > .intake-col");
        spnCancel.css("display", "none");
        spnCancel.prev(".intake-edit").text("+ Edit");
        cols.find(":input").not("input[type='hidden']").css("display", "none").val("");
        cols.find("span").css("display", "block");
    }

    return {
        init: init
    };
}(TransfereeIntakeService);
