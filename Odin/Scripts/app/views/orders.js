var OrdersPageController = function () {

    var $currSort = null;

    var $Orders = null;
    var $SortedOrders = null;
    var $SearchedOrders = null;
    
     
    var init = function () { 

        var desktop_table = $('table.desktop tbody');
        var tablet_table = $('table.tablet tbody');
        var mobile_table = $('table.mobile tbody');

        $.getJSON("/api/orders",
            function (orders) {
                $Orders = orders;
                $SortedOrders = $Orders; 
                $SearchedOrders = $SortedOrders;

                populateTables();
            }
        );
         
        // sort handlers
        $('.arrow.up').click(function () {

            if ($currSort !== null) {
                $currSort.next().css("display", "none");
                $currSort.css("display", "block");
            }

            $currSort = $(this);
            $currSort.css("display", "none");
            $currSort.next().css("display", "block");
        });

        $('.arrow.down').click(function () {
            $currSort = null;
            $(this).css("display", "none");
            $(this).prev().css("display", "block");
        });

        // search box
        $('#searchbox').on("keyup paste", function () {
            var srchIndx = $(this).val().toLowerCase();

            if (srchIndx.length > 1) {
                $SearchedOrders = { "transferees": [] };

                for (var i = 0; i < $Orders.transferees.length; i++) {
                    if ($Orders.transferees[i].firstName.toLowerCase().includes(srchIndx) ||
                        $Orders.transferees[i].fastName.toLowerCase().includes(srchIndx)) {
                        $SearchedOrders.transferees.push($Orders.transferees[i]);
                    }
                }
            }
            else $SearchedOrders = $SortedOrders;

            desktop_table.empty();
            tablet_table.empty();
            mobile_table.empty();

            populateTables();  
        });

        var populateTables = function () {

            if ($SearchedOrders.transferees == null) return;

            for (var i = 0; i < $SearchedOrders.transferees.length; i++) {

                // name
                var name = "<p>" + $SearchedOrders.transferees[i].firstName + " " + $SearchedOrders.transferees[i].lastName + "</p>";
                if ($SearchedOrders.transferees[i].rmc !== null) 
                    name += "<p class=\"Callout\">" + $SearchedOrders.transferees[i].rmc + "</p>";                        
                
                if ($SearchedOrders.transferees[i].company !== null) 
                    name += "<p>" + $SearchedOrders.transferees[i].company + "</p>";                                   

                var name_tablet = name + "<div><span class=\"tablet-column-label\">PreTrip:</span>";
                if ($SearchedOrders.transferees[i].preTrip !== null)
                    name_tablet += "<span>" + $SearchedOrders.transferees[i].preTrip + "</span><br />";
                else name_tablet += "<br />";
                name_tablet += "<span class=\"tablet-column-label\">Final Arrival:</span><span>" + $SearchedOrders.transferees[i].finalArrival.split("T")[0] + "</span></div>"

                var name_mobile = name_tablet + "<div><span class=\"tablet-column-label\">Manager:</span><span class=\"Callout\">" + $SearchedOrders.transferees[i].manager + "</span>";
                if ($SearchedOrders.transferees[i].managerPhone != null)
                    name_mobile += "<p class=\"mobile-phone\">" + $SearchedOrders.transferees[i].managerPhone + "</p></div>";
                else name_mobile += "</div>";

                // services
                var services = "<div class=\"actlist-item-hdr\"><img class=\"expand\" src=\"/Content/Images/expand.png\" /><img class=\"collapse noinit\" src=\"/Content/Images/collapse.png\" />" +
                    "</div><div class=\"actlist-item-hdr\"><p>SS</p><a href=\"#\">" + $SearchedOrders.transferees[i].numberOfScheduledServices + "</a></div><div class=\"actlist-item-hdr\"><p>S</p><a href=\"#\">" + $SearchedOrders.transferees[i].numberOfServices + "</a></div>" +
                    "<div class=\"actlist-item-hdr\"><p>C</p><a href=\"#\">" + $SearchedOrders.transferees[i].numberOfCompletedServices + "</a></div>";

                var services_list = "";
                if ($SearchedOrders.transferees[i].services !== null) {
                    for (var j = 0; j < $SearchedOrders.transferees[i].services.length; j++) {
                        services_list += "<div class=\"actlist-item-blk\"><p class=\"actlist-item\">" + $SearchedOrders.transferees[i].services[j].name + "</p><p>" + $SearchedOrders.transferees[i].services[j].completed + "</p></div>";
                    }
                    services += services_list;
                }

                var services_tablet = "<div class=\"actlist-item-hdr\"><img class=\"expand\" src=\"/Content/Images/expand.png\" /><img class=\"collapse noinit\" src=\"/Content/Images/collapse.png\" />" +
                    "</div><div class=\"actlist-item-hdr\"><p>SS</p><a href=\"#\">" + $SearchedOrders.transferees[i].numberOfScheduledServices + "</a></div><div class=\"actlist-item-hdr\"><p>S</p><a href=\"#\">" + $SearchedOrders.transferees[i].numberOfServices + "</a></div><div class=\"actlist-item-hdr\">" +
                    "<p>C</p><a href=\"#\">" + $SearchedOrders.transferees[i].numberOfCompletedServices + "</a></div>";
                services_tablet += services_list;

                // last contacted
                var lastcontacted = "<p> Last Contacted</p>";
                if ($SearchedOrders.transferees[i].lastContacted !== null) lastcontacted = "<p>Last Contacted</p><p>" + $SearchedOrders.transferees[i].lastContacted + "</p>";

                // manager
                var manager = "<td><p>Manager</p><p class=\"Callout\">" + $SearchedOrders.transferees[i].manager + "</p><p>" + $SearchedOrders.transferees[i].managerPhone + "</p></td>";

                // pretrip
                var pretrip = "<td><p>PreTrip</p><p>";
                if ($SearchedOrders.transferees[i].preTrip !== null) pretrip = "<td><p>PreTrip</p><p>" + $SearchedOrders.transferees[i].preTrip + "</p></td>";

                // final arrival
                var finalarrival = "<td><p>Final Arrival</p><p>" + $SearchedOrders.transferees[i].finalArrival.split("T")[0] + "</p></td>";

                // notification
                var notification = "<td><img src=\"/Content/Images/icn_note_1.png\" /><div class=\"notify-count\"><span>" + $SearchedOrders.transferees[i].numberOfAlerts + "</span></div></td>";

                desktop_table.append("<tr>" + "<td>" + name + "</td>" + "<td>" + services + "</td>" + "<td>" + lastcontacted + "</td>" + manager + pretrip + finalarrival + notification + "</tr>");
                tablet_table.append("<tr>" + "<td>" + name_tablet + "</td>" + manager + "<td>" + lastcontacted + "</td>" + "<td>" + services_tablet + "</td>" + notification + "</tr>");
                mobile_table.append("<tr>" + "<td>" + name_mobile + "</td>" + "<td>" + lastcontacted + services + "</td>" + notification + "</tr>");

                // row expansion handlers
                $('.expand').click(function () {
                    $(this).css("display", "none");
                    $(this).next().css("display", "block");

                    $(this).parent().parent().children('.actlist-item-blk').each(function () {
                        $(this).css("display", "block");
                    });
                });

                $('.collapse').click(function () {
                    $(this).css("display", "none");
                    $(this).prev().css("display", "block");

                    $(this).parent().parent().children('.actlist-item-blk').each(function () {
                        $(this).css("display", "none");
                    });
                });
            }
        };
    };

    return {
        OrdersPageInit: init
    };
}();

