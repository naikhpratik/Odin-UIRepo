var OrdersPageController = function () {

    var init = function () {

        var data = [];
        $(".orderRow").each(function () {
            var name = $(this).find(".eeName").text();
            var id = $(this).attr("data-order-id");
            data.push({ id: id, name: name });
        });

        // constructs the suggestion engine
        var engine = new Bloodhound({
            datumTokenizer: function(datum) {
                return Bloodhound.tokenizers.whitespace(datum.name);
            },
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            local: data,
            identify: function(obj) {return obj.id}
        });

        $('#searchbox').typeahead({
                hint: true,
                highlight: true,
                minLength: 1
            },
            {
                name: 'data',
                source: engine,
                displayKey: 'name'
            }
        ).on('typeahead:selected',
            function (e, suggestion, name) {
                goToOrder(suggestion.id);
            }
        );
        
        // row selection handler
        $('.clickable').click(function () {
            goToOrder($(this).attr("data-order-id"));
        });
    };

    var goToOrder = function(id) {
        window.location.href = "/Orders/Transferee/" + id;
    }

    return {
        init: init
    };
}();

