var OrdersPageController = function () {

    var $currSort = null;

    var $Orders = null;
    var $SortedOrders = null;
    var $SearchedOrders = null;

    var init = function () { 

        var tablet_table = $('table.tablet tbody');
        var mobile_table = $('table.mobile tbody');
        var desktop_table = $('table.desktop tbody');

        $.getJSON("/api/orders",
            function (orders) {
                $Orders = orders;
                for (var i = 0; i < $Orders.transferees.length; i++) {
                    $Orders.transferees[i].index = i;
                }

                $SortedOrders = $Orders; 
                $SearchedOrders = $SortedOrders;

                populateTables();
            }
        );

        // sort handlers
        $('.arrow.up').click(function () {

            if ($currSort !== null) {
                $currSort.next().css("display", "none");
                $currSort.next().next().css("display", "none");

                $currSort.css("display", "block");
            }

            $currSort = $(this);
            $currSort.css("display", "none");
            $currSort.next().css("display", "block");

            if ($currSort.hasClass('name')) {
                nameSort(true);
            }

            if ($currSort.hasClass('date')) {
                dateSort($(this).attr('data-field'), true);
            }

            if ($currSort.hasClass('number')) {
                numberSort(true);
            }

            $SearchedOrders = $SortedOrders;

            populateTables();
        });

        $('.arrow.down').click(function () {

            $(this).css("display", "none");
            $(this).next().css("display", "block");

            if ($currSort.hasClass('name')) {
                nameSort(false);
            }

            if ($currSort.hasClass('date')) {
                dateSort($(this).attr('data-field'), false);
            }

            if ($currSort.hasClass('number')) {
                numberSort(false);
            }

            $SearchedOrders = $SortedOrders;

            populateTables();
        });

        $('.arrow.upsel').click(function () {

            $(this).css("display", "none");
            $(this).prev().css("display", "block");

            if ($currSort.hasClass('name')) {
                nameSort(true);
            }

            if ($currSort.hasClass('date')) {
                dateSort($(this).attr('data-field'), true);
            }

            if ($currSort.hasClass('number')) {
                numberSort(true);
            }

            $SearchedOrders = $SortedOrders;

            populateTables();
        });

        // search box
        $('#searchbox').on("keyup input", function () {
            var srchIndx = $(this).val().toLowerCase();

            if (srchIndx.length > 1) {
                $SearchedOrders = { "transferees": [] };

                for (var i = 0; i < $SortedOrders.transferees.length; i++) {
                    var firstMatch = $SortedOrders.transferees[i].firstName.split(srchIndx);
                    var lastMatch = $SortedOrders.transferees[i].lastName.split(srchIndx);

                    if (firstMatch.length > 1 || lastMatch.length > 1) {
                        var transferee = {};

                        Object.assign(transferee, $SortedOrders.transferees[i]);

                        // indicate search on first name
                        if (firstMatch.length > 1) {
                            var firstName = firstMatch[0];

                            firstName += "<span class=\"srchIndx\">";
                            for (var j = 0; j < srchIndx.length; j++) {
                                firstName += transferee.firstName[firstMatch[0].length + j];
                            }
                            firstName += "</span>" + firstMatch[1];

                            transferee.firstName = firstName;
                        }

                        // indicate search on last name
                        if (lastMatch.length > 1) {
                            var lastName = lastMatch[0];

                            lastName += "<span class=\"srchIndx\">";
                            for (var k = 0; k < srchIndx.length; k++) {
                                lastName += transferee.lastName[lastMatch[0].length + k];
                            }
                            lastName += "</span>" + lastMatch[1];

                            transferee.lastName = lastName;
                        }

                        $SearchedOrders.transferees.push(transferee);
                    }
                }
            }
            else $SearchedOrders = $SortedOrders;

            populateTables();  
        });

        // sorting routines
        var nameSort = function (asc) {
            $SortedOrders.transferees.sort(function (a, b) {
                if (asc)
                    return a.lastName.localeCompare(b.lastName);
                else
                    return a.lastName.localeCompare(b.lastName) * -1;
            });
        };

        var dateSort = function (field, asc) {
            $SortedOrders.transferees.sort(function (a, b) {
                // trap nulls
                if (a[field] === null && b[field] === null)
                    return 0;

                if (a[field] === null && b[field] !== null)
                    if (asc)
                        return -1;
                    else
                        return 1;

                if (a[field] !== null && b[field] === null)
                    if (asc)
                        return 1;
                    else
                        return -1;

                var dateA = new Date(a[field]);
                var dateB = new Date(b[field]);

                if (dateA > dateB)
                    if (asc)
                        return -1;
                    else
                        return 1;
                else if (dateA === dateB)
                    return 0;
                else
                    if (asc)
                        return 1;
                    else
                        return -1;
            });
        };

        var numberSort = function (asc) {
            $SortedOrders.transferees.sort(function (a, b) {
                if (a.numberOfAlerts > b.numberOfAlerts)
                    if (asc)
                        return -1;
                    else
                        return 1;
                else if (a.numberOfAlerts === b.numberOfAlerts)
                    return 0;
                else
                    if (asc)
                        return 1;
                    else
                        return -1;
            });
        };

        // tables builder
        var populateTables = function () {

            desktop_table.empty();
            tablet_table.empty();
            mobile_table.empty();

            if ($SearchedOrders.transferees === null) return;

            for (var i = 0; i < $SearchedOrders.transferees.length; i++) {

                // name
                var name = "<p class=\"transferee\">" + $SearchedOrders.transferees[i].firstName + " " + $SearchedOrders.transferees[i].lastName + "</p>";
                if ($SearchedOrders.transferees[i].rmc !== null) 
                    name += "<p class=\"Callout\">" + $SearchedOrders.transferees[i].rmc + "</p>";                        
                
                if ($SearchedOrders.transferees[i].company !== null) 
                    name += "<p class=\"company\">" + $SearchedOrders.transferees[i].company + "</p>";   

                if ($SearchedOrders.transferees[i].rush !== undefined && $SearchedOrders.transferees[i].rush === true)
                    name += "<p class=\"urgent\">Rush</p>";

                var name_tablet = name + "<div><span class=\"tablet-column-label\">PreTrip:</span>";
                if ($SearchedOrders.transferees[i].preTrip !== null)
                    name_tablet += "<span>" + $SearchedOrders.transferees[i].preTrip.split("T")[0] + "</span><br />";
                else name_tablet += "<br />";

                name_tablet += "<span class=\"tablet-column-label\">Final Arrival:</span>";
                if ($SearchedOrders.transferees[i].finalArrival !== null)
                    name_tablet += "<span> " + $SearchedOrders.transferees[i].finalArrival.split("T")[0] + "</span></div>";
                else name_tablet += "</div>";

                var name_mobile = name_tablet + "<div><span class=\"tablet-column-label\">Manager:</span>";
                if ($SearchedOrders.transferees[i].manager !== null)
                    name_mobile += "<span class=\"Callout\">" + $SearchedOrders.transferees[i].manager + "</span>";

                if ($SearchedOrders.transferees[i].managerPhone !== null)
                    name_mobile += "<p class=\"mobile-phone\">" + $SearchedOrders.transferees[i].managerPhone + "</p></div>";
                else name_mobile += "</div>";

                // services
                var services = "<div class=\"actlist-item-hdr\"><img class=\"expand\" src=\"/Content/Images/expand.png\" /><img class=\"collapse noinit\" src=\"/Content/Images/collapse.png\" />" +
                    "</div><div class=\"actlist-item-hdr\"><p>AT</p><a href=\"#\">" + ("0" + $SearchedOrders.transferees[i].numberOfScheduledServices).slice(-2) + "</a></div><div class=\"actlist-item-hdr\"><p>S</p><a href=\"#\">" +
                    ("0" + $SearchedOrders.transferees[i].numberOfServices).slice(-2) + "</a></div>" + "<div class=\"actlist-item-hdr\"><p>C</p><a href=\"#\">" + ("0" + $SearchedOrders.transferees[i].numberOfCompletedServices).slice(-2) + "</a></div>";

                var services_list = "";
                if ($SearchedOrders.transferees[i].services !== null) {
                    for (var j = 0; j < $SearchedOrders.transferees[i].services.length; j++) {
                        services_list += "<div class=\"actlist-item-blk\"><p class=\"actlist-item\">" + $SearchedOrders.transferees[i].services[j].name + "</p>";
                        if ($SearchedOrders.transferees[i].services[j].completedDate !== null)
                            services_list += "<p>" + $SearchedOrders.transferees[i].services[j].completedDate.split("T")[0] + "</p></div>";
                        else services_list += "</div>";
                    }
                    services += services_list;
                }

                var services_tablet = "<div class=\"actlist-item-hdr\"><img class=\"expand\" src=\"/Content/Images/expand.png\" /><img class=\"collapse noinit\" src=\"/Content/Images/collapse.png\" />" +
                    "</div><div class=\"actlist-item-hdr\"><p>AT</p><a href=\"#\">" + ("0" + $SearchedOrders.transferees[i].numberOfScheduledServices).slice(-2) + "</a></div><div class=\"actlist-item-hdr\"><p>S</p><a href=\"#\">" +
                    ("0" + $SearchedOrders.transferees[i].numberOfServices).slice(-2) + "</a></div><div class=\"actlist-item-hdr\">" + "<p>C</p><a href=\"#\">" + ("0"+$SearchedOrders.transferees[i].numberOfCompletedServices).slice(-2) + "</a></div>";
                services_tablet += services_list;

                // last contacted
                var lastcontacted = "";
                if ($SearchedOrders.transferees[i].lastContacted !== null)
                    lastcontacted += "<p class=\"lastcontacted\">" + $SearchedOrders.transferees[i].lastContacted.split("T")[0] + "</p>";

                // manager
                var manager = "<td>";
                if ($SearchedOrders.transferees[i].manager !== null)
                    manager += "<p class=\"Callout\">" + $SearchedOrders.transferees[i].manager + "</p>";
                if ($SearchedOrders.transferees[i].managerPhone !== null)
                    manager += "<p class=\"managerphone\">" + $SearchedOrders.transferees[i].managerPhone + "</p></td>";
                else manager += "</td>";

                // pretrip
                var pretrip = "<td>";
                if ($SearchedOrders.transferees[i].preTrip !== null)
                    pretrip += "<p class=\"pretrip\">" + $SearchedOrders.transferees[i].preTrip.split("T")[0] + "</p></td>";
                else pretrip += "</td>";

                // final arrival
                var finalarrival = "<td>";
                if ($SearchedOrders.transferees[i].finalArrival !== null)
                    finalarrival += "<p class=\"finalarrival\">" + $SearchedOrders.transferees[i].finalArrival.split("T")[0] + "</p></td>";
                else finalarrival += "</td>";

                // notification
                var notification = "<td><img src=\"/Content/Images/icn_alert_B1.png\" />";
                if ($SearchedOrders.transferees[i].numberOfAlerts > 0)
                    notification += "<div class=\"notify-count\"><img src=\"/Content/Images/icn_alert_circle.png\"/><span>" + ("0"+$SearchedOrders.transferees[i].numberOfAlerts).slice(-2) + "</span></div></td>";

                desktop_table.append("<tr data-order-id=\""+$SearchedOrders.transferees[i].id+"\" data-index=\"" + $SearchedOrders.transferees[i].index + "\">" + "<td>" + name + "</td>" + "<td>" + services + "</td>" + "<td>" + lastcontacted + "</td>" + manager + pretrip + finalarrival + notification + "</tr>");
                tablet_table.append("<tr data-order-id=\"" + $SearchedOrders.transferees[i].id +"\" data-index=\"" + $SearchedOrders.transferees[i].index + "\">" + "<td>" + name_tablet + "</td>" + manager + "<td>" + lastcontacted + "</td>" + "<td>" + services_tablet + "</td>" + notification + "</tr>");
                mobile_table.append("<tr data-order-id=\"" + $SearchedOrders.transferees[i].id +"\" data-index=\"" + $SearchedOrders.transferees[i].index + "\">" + "<td>" + name_mobile + "</td>" + "<td>" + lastcontacted + services + "</td>" + notification + "</tr>");

                // service expansion handlers
                $('.expand').click(function (event) {
                    event.stopPropagation();

                    $(this).css("display", "none");
                    $(this).next().css("display", "block");

                    $(this).parent().parent().children('.actlist-item-blk').each(function () {
                        $(this).css("display", "block");
                    });
                });

                $('.collapse').click(function (event) {
                    event.stopPropagation();

                    $(this).css("display", "none");
                    $(this).prev().css("display", "block");

                    $(this).parent().parent().children('.actlist-item-blk').each(function () {
                        $(this).css("display", "none");
                    });
                });

                // <a> handler until functionality decided
                $('a').click(function (event) {
                    event.stopPropagation();
                });

                // row selection handler
                $('tr').not(':first').click(function () {
                    window.location.href = "/Orders/Transferee/" + $(this).attr("data-order-id");
                });
            }
        };
    };

    return {
        OrdersPageInit: init
    };
}();

