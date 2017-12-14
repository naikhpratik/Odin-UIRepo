var OrdersPageController = function () {

    var _data;
    var _sortAsc = true;
    var _sortCurr = "";

    var init = function () {
        _data = getData();
        initSearch();
        initSort();
    };

    var initSearch = function() {
        // constructs the suggestion engine
        var engine = new Bloodhound({
            datumTokenizer: function (datum) {
                return Bloodhound.tokenizers.whitespace(datum.name);
            },
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            local: _data,
            identify: function (obj) { return obj.id }
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
    }

    var initSort = function() {
        $("#eeNameSort").click(function () {
            sortCol($(this), "name", function(a) { return a.toUpperCase() });
        });

        $("#pmNameSort").click(function () {
            sortCol($(this), "pmName", function (a) { return a.toUpperCase() });
        });

        $("#preTripDateSort").click(function () {
            sortCol($(this), "preTripDate", function (a) { return new Date(a) });
        });

        $("#estimatedArrivalDateSort").click(function () {
            sortCol($(this), "estimatedArrivalDate", function (a) { return new Date(a) });
        });

        $("#notificationsSort").click(function () {
            sortCol($(this), "notifications", function (a) { return parseInt(a.replace("*","")) });
        });
    }

    var sortCol = function(headerElt, dataName, sortFunc) {
        resetSortHeaders();
        headerElt.css("font-weight", "Bold").css("color", "black");
       
        var sortId = headerElt.attr('id');
        if (_sortCurr === sortId) {
            _sortAsc = !_sortAsc;
        } else {
            _sortCurr = sortId;
            _sortAsc = true;
        }

        if (_sortAsc) {
            headerElt.find(".sortPlus").css("color", "#e53c2e");
            headerElt.find(".sortMinus").css("color", "black");
        } else {
            headerElt.find(".sortMinus").css("color", "#e53c2e");
            headerElt.find(".sortPlus").css("color", "black");
        }
        
        _data.sort(sortBy(dataName, _sortAsc, sortFunc));
        bindData();
    }

    var resetSortHeaders = function () {
        $(".sortLabel, .sortLabel .sortPlus, .sortLabel .sortMinus").css("font-weight", "normal").css("color", "#858585");
       
    }

    var bindData = function()
    {
        var rows = $(".orderRow");
        rows.each(function(i, elt) {
            $(this).attr("data-order-id",_data[i]["id"]);
            $(this).find(".eeName").text(_data[i]["name"]);
            $(this).find(".rmcName").text(_data[i]["rmcName"]);
            $(this).find(".clientName").text(_data[i]["clientName"]);
            $(this).find(".pmName").text(_data[i]["pmName"]);
            $(this).find(".pmPhone").text(_data[i]["pmPhone"]);
            $(this).find(".preTripDate").text(_data[i]["preTripDate"]);
            $(this).find(".estimatedArrivalDate").text(_data[i]["estimatedArrivalDate"]);
            $(this).find(".notifications").text(_data[i]["notifications"]);

            var pb = $(this).find(".progressBar");
            pb.data("comp-percent", _data[i]["compPercent"]);
            pb.data("auth-percent", _data[i]["authPercent"]);
            pb.data("sched-percent", _data[i]["schedPercent"]);
            loadProgressBar(pb);
        });
    }

    var getData = function() {
        var rows = $(".orderRow");
        var data = new Array();

        rows.each(function () {
            var id = $(this).attr("data-order-id");
            var name = $(this).find(".eeName").text();
            var rmcName = $(this).find(".rmcName").text();
            var clientName = $(this).find(".clientName").text();
            var pmName = $(this).find(".pmName").text();
            var pmPhone = $(this).find(".pmPhone").text();
            var preTripDate = $(this).find(".preTripDate").text();
            var estimatedArrivalDate = $(this).find(".estimatedArrivalDate").text();
            var notifications = $(this).find(".notifications").text();

            var pb = $(this).find(".progressBar");
            var authPercent = pb.data("auth-percent");
            var schedPercent = pb.data("sched-percent");
            var compPercent = pb.data("comp-percent");

            data.push({
                id: id,
                name: name,
                rmcName: rmcName,
                clientName: clientName,
                pmName: pmName,
                pmPhone:pmPhone,
                preTripDate: preTripDate,
                estimatedArrivalDate: estimatedArrivalDate,
                notifications: notifications,
                authPercent: authPercent,
                schedPercent: schedPercent,
                compPercent: compPercent
            });
        });

        return data;
    }

    //https://stackoverflow.com/questions/979256/sorting-an-array-of-javascript-objects
    var sortBy = function (field, asc, primer) {
        var key = primer ?
            function (x) { return primer(x[field]) } :
            function (x) { return x[field] };

        var reverse = asc ? 1 : -1;

        return function (a, b) {
            return a = key(a), b = key(b), reverse * ((a > b) - (b > a));
        } 
    }

    var goToOrder = function(id) {
        window.location.href = "/Orders/Transferee/" + id;
    }

    var loadProgressBar = function(pbElt) {
        pbElt.find(".progressBar__auth").width(pbElt.data("auth-percent"));
        pbElt.find(".progressBar__sched").width(pbElt.data("sched-percent"));
        pbElt.find(".progressBar__comp").width(pbElt.data("comp-percent"));
    }

    return { 
        init: init
    };
}();

