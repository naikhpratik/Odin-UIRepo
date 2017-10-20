var TransfereeIntakeService = function() {
    var route = "/api/orders/transferee/intake/";

    var updateIntakeBlock = function (block, data, success, fail) {
        var url = route + block;
        $.post(url, data).done(success).fail(fail);
    }

    return{
        updateIntakeBlock: updateIntakeBlock
    }

}();

var TransfereeIntakeController = function (transfereeIntakeService) {

    var intakeBlocks;
    var orderId;

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
                        var collectionData = hasAttr(row, "data-entity-id") ? { "Id": row.attr("data-entity-id") } : { "TempId": row.attr("data-entity-temp-id") }

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

            //Remove temp ids of newly created rows and replace with db ids
            for (key in data) {
                var newCollectionRow = intakeBlock.find(".intake-row[data-entity-temp-id='" + key + "']");
                newCollectionRow.removeAttr('data-entity-temp-id');
                newCollectionRow.attr("data-entity-id", data[key]);
            }
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
        //Duplicate current row
        var row = spnAdd.parents(".intake-row");
        var clone = row.clone();

        //Remove add button from current row
        spnAdd.remove();

        //Clear values in clone
        clone.find(":input").val(""); //All inputs including hidden fields
        clone.find("span").not(".intake-add").text("");
        clone.removeAttr("data-entity-id");
        clone.attr("data-entity-temp-id", getTempId());

        //Add duplicate row to dom, make cancel button visible, call click of edit so all fields editable
        row.after(clone);
        clone.parents(".intake-block").find(".intake-edit").click();
    }

    var toggleCollapse = function (e) {
        var intakeBlock = $(e.target).parents(".intake-block");
        var img = intakeBlock.find(".intake-collapse-img");
        var src = img.attr("src");

        if (contains(src,"collapse")) {
            collapse(intakeBlock);
            cancel(intakeBlock);
        } else {
            expand(intakeBlock);
        }
        
    }

    //Utilities

    var fillPostData = function(data, inputs){
        inputs.each(function () {
            data[$(this).attr("name")] = $(this).val();
        });
    }

    var getTempId = function() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        }) + "-Temp"+Date.now();
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
