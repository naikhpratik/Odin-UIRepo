var TransfereeSelectedPropertyService = function () {
    var route = "/api/orders/transferee/";

    var updateIntakeBlock = function (block, data, success, fail) {
        var url = route + "/housing/update" ;
        $.post(url, data).done(success).fail(fail);
    }
    
    return {
        updateIntakeBlock: updateIntakeBlock
    }

}();

var TransfereeSelectedPropertyController = function (TransfereeSelectedPropertyService) {
    
    var init = function () {

        var housingPropertyBlock = $("div#housingPropertyModalBody");
        //console.dir(housingPropertyBlock);
        //Init Variables
        //intakeBlocks = pnlIntake.find(".intake-block");
        propertyId = housingPropertyBlock.find("div.container-fluid").attr("data-property-id");
        //console.dir(housingPropertyBlock.find("div.container-fluid"));
        //Bind Events
        housingPropertyBlock.on("click", "#editProperty", editSaveProperty);
        //intakeBlocks.on("click", ".intake-cancel", cancelEditBlock);
        //intakeBlocks.on("click", ".intake-add", addRowToBlock);
        //intakeBlocks.on("click", ".intake-collapse", toggleCollapse);
        //intakeBlocks.on("click", ".intake-del", deleteRowFromBlock);
        //intakeBlocks.on("click", ".intake-save", saveBlock);
        //intakeBlocks.on("change", "[type=checkbox]", saveChecked);
    };
    
    var editSaveProperty = function (e) {

        //alert("Hola");
        ////Local Variables
        var spnEditSave = $(e.target);
        //console.dir(spnEditSave);
        var propBlock = spnEditSave.parents(".housingProperty-block");
        //console.dir(propBlock);
        var rows = propBlock.find(".hpm_edit");
        //console.dir(rows);
        var spanp = rows.find(".prop-span");
        var inputs = rows.find(".intake-input");
        //console.dir(inputs);
        //inputs.css("display", "none");      
        
        ////Local Functions
        var edit = function () {
            //toggle text
             spnEditSave[0].innerHTML= "SAVE";
            
            //bind hidden values to inputs
            inputs.css("display", "block").each(function () {
                var hidden = $(this).next(".housingDetails-hidden");
                $(this).find("input").val(hidden.val());
            });

            ////bind hidden values to inputs
            //dates.css("display", "block").each(function () {
            //    var hidden = $(this).next(".intake-hidden");
            //    $(this).find("input").val(hidden.val());
            //});

            spanp.css("display", "none");
            //addspans.css("display", "none");
            //delspans.css("display", "none");

        }

        var save = function () {
            //alert("Hola in save")
            spnEditSave[0].innerHTML = "EDIT";
        //    //Prepare post url and data
            var block = "housing";
            var data = { "Id": propertyId };

            rows.each(function () {
                var row = $(this);
                var rowInputs = row.find(".intake-input");

                //If a collection that can have added values
                
                    fillPostData(data, rowInputs);
               
            });

            //Do save
            TransfereeSelectedPropertyService.updateIntakeBlock(block, data, saveSuccess, saveFail);
            //saveSuccess();
        }

        var saveSuccess = function (data) {
            //Revert Text
            spnEditSave[0].innerHTML = "EDIT";
            
            ////Update spans to new values and toggle.
            inputs.each(function () {
                console.dir($(this));
                var input = $(this);
                var span = input.prev(".prop-span");
                var hidden = input.next(".housingDetails-hidden");
                if (input.is('select')) {
                    if (input.val() === null) {
                        input.find("option:selected").removeAttr("selected");
                        span.text("");
                        hidden.val("");
                    } else {
                        var selText = input.find("option:selected").text();
                        span.text(selText);
                        hidden.val(input.val());
                    }
                } else {
                    input.prev("p").text($(this).val());
                    input.next(".housingDetails-hidden").val($(this).val());
                }
            });
            
            inputs.css("display", "none");
            spanp.css("display", "block");
           
            toast("Save Successful!", "success");
        }

        var saveFail = function () {
            toast("Uh oh!  Something went wrong!", "danger");
        }

        ////Function Execution
        //expand(intakeBlock);
        var contains = function (value, searchFor) {
            return (value || '').indexOf(searchFor) > -1;
        };
        var fillPostData = function (data, inputs) {
            inputs.each(function () {
                var elt = $(this);
                if (elt.hasClass("intake-date")) {
                    var innerInput = elt.find("input");
                    data[innerInput.attr("name")] = innerInput.val();
                } else if (elt.is(":checkbox")) {
                    data[elt.attr("name")] = elt.prop('checked');
                } else {
                    data[elt.attr("name")] = elt.val();
                }
            });
        }
       
        if (contains(spnEditSave[0].innerText, "EDIT")) {
            edit();
        } else {
            save();
        }
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
}(TransfereeSelectedPropertyService);