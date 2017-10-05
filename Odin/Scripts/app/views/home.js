var HomePageController = function () {
    var $currSort = null;

    //TODO: fill $Orders via an /api call
    var $Orders = {
        "Transferees": [
            {
                "FirstName": "Victoria",
                "LastName": "Sporer",

                "Rmc": "Cartus",
                "Company": "Amazon.com INC",

                "NumberOfScheduledServices": 2,
                "NumberOfServices": 1,
                "NumberOfCompletedServices": 0,

                "LastContacted": "09/22/17",
                "Manager": "Jane Doese",
                "MamagerPhone": "321-23432",
                "PreTrip": "10/15/17",
                "FinalArrival": "2/10/18",
                "NumberOfAlerts": 5,

                "Services": [
                    {
                        "Name": "Pick up",
                        "Completed": "10/15/17"
                    },
                    {
                        "Name": "Visit 1st Home",
                        "Completed": "10/15/17"
                    }
                ]
            },
            {
                "FirstName": "Lila",
                "LastName": "Smith",

                "Rmc": "Cartus",
                "Company": "Tesla, Inc",

                "NumberOfScheduledServices": 2,
                "NumberOfServices": 1,
                "NumberOfCompletedServices": 0,

                "LastContacted": "09/09/17",
                "Manager": "Austin Emser",
                "MamagerPhone": "321-23432",
                "PreTrip": "10/15/17",
                "FinalArrival": "2/10/18",
                "NumberOfAlerts": 12,

                "Services": [
                    {
                        "Name": "Pick up",
                        "Completed": "10/15/17"
                    },
                    {
                        "Name": "Visit 1st Home",
                        "Completed": "10/15/17"
                    }
                ]
            },
            {
                "FirstName": "Joseph",
                "LastName": "Jackson",

                "Rmc": "Cartus",
                "Company": "Amazon INC",

                "NumberOfScheduledServices": 2,
                "NumberOfServices": 1,
                "NumberOfCompletedServices": 0,

                "LastContacted": "08/12/17",
                "Manager": "Alexander Kovanen",
                "MamagerPhone": "321-23432",
                "PreTrip": "10/15/17",
                "FinalArrival": "2/10/18",
                "NumberOfAlerts": 3,

                "Services": [
                    {
                        "Name": "Pick up",
                        "Completed": "10/15/17"
                    },
                    {
                        "Name": "Visit 1st Home",
                        "Completed": "10/15/17"
                    }
                ]
            },
            {
                "FirstName": "Helen",
                "LastName": "Gutierrez",

                "Rmc": "Cartus",
                "Company": "Tesla, Inc",

                "NumberOfScheduledServices": 2,
                "NumberOfServices": 1,
                "NumberOfCompletedServices": 0,

                "LastContacted": "08/02/17",
                "Manager": "Joseph Jones",
                "MamagerPhone": "321-23432",
                "PreTrip": "10/15/17",
                "FinalArrival": "2/10/18",
                "NumberOfAlerts": 1,

                "Services": [
                    {
                        "Name": "Pick up",
                        "Completed": "10/15/17"
                    },
                    {
                        "Name": "Visit 1st Home",
                        "Completed": "10/15/17"
                    }
                ]
            },
            {
                "FirstName": "Ricky",
                "LastName": "Stanley",

                "Rmc": "Cartus",
                "Company": "Amazon INC",

                "NumberOfScheduledServices": 2,
                "NumberOfServices": 1,
                "NumberOfCompletedServices": 0,

                "LastContacted": "07/22/17",
                "Manager": "Katy Smith",
                "MamagerPhone": "321-23432",
                "PreTrip": "10/15/17",
                "FinalArrival": "2/10/18",
                "NumberOfAlerts": 9,

                "Services": [
                    {
                        "Name": "Pick up",
                        "Completed": "10/15/17"
                    },
                    {
                        "Name": "Visit 1st Home",
                        "Completed": "10/15/17"
                    }
                ]
            },
            {
                "FirstName": "John",
                "LastName": "Fludd",

                "Rmc": "Cartus",
                "Company": "Amazon INC",

                "NumberOfScheduledServices": 2,
                "NumberOfServices": 1,
                "NumberOfCompletedServices": 0,

                "LastContacted": "07/20/17",
                "Manager": "Zander Gilley",
                "MamagerPhone": "321-23432",
                "PreTrip": "10/15/17",
                "FinalArrival": "2/10/18",
                "NumberOfAlerts": 17,

                "Services": [
                    {
                        "Name": "Pick up",
                        "Completed": "10/15/17"
                    },
                    {
                        "Name": "Visit 1st Home",
                        "Completed": "10/15/17"
                    }
                ]
            },
            {
                "FirstName": "Margaret",
                "LastName": "Cooley",

                "Rmc": "Cartus",
                "Company": "Tesla, Inc",

                "NumberOfScheduledServices": 2,
                "NumberOfServices": 1,
                "NumberOfCompletedServices": 0,

                "LastContacted": "07/15/17",
                "Manager": "Patricia Applesmith",
                "MamagerPhone": "321-23432",
                "PreTrip": "10/15/17",
                "FinalArrival": "2/10/18",
                "NumberOfAlerts": 4,

                "Services": [
                    {
                        "Name": "Pick up",
                        "Completed": "10/15/17"
                    },
                    {
                        "Name": "Visit 1st Home",
                        "Completed": "10/15/17"
                    }
                ]
            }
        ]
    };

    var $SortedOrders = $Orders; 
    var $SearchedOrders = $SortedOrders;

    var init = function () { 

        var desktop_table = $('table.desktop tbody');
        var tablet_table = $('table.tablet tbody');
        var mobile_table = $('table.mobile tbody');

 
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
                $SearchedOrders = { "Transferees": [] };

                for (var i = 0; i < $Orders.Transferees.length; i++) {
                    if ($Orders.Transferees[i].FirstName.toLowerCase().includes(srchIndx) ||
                        $Orders.Transferees[i].LastName.toLowerCase().includes(srchIndx)) {
                        $SearchedOrders.Transferees.push($Orders.Transferees[i]);
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

            for (var i = 0; i < $SearchedOrders.Transferees.length; i++) {

                // name
                var name = "<p>" + $SearchedOrders.Transferees[i].FirstName + " " + $SearchedOrders.Transferees[i].LastName + "</p>" +
                    "<p class=\"Callout\">" + $SearchedOrders.Transferees[i].Rmc + "</p><p>" + $SearchedOrders.Transferees[i].Company + "</p>";

                var name_tablet = name + "<div><span class=\"tablet-column-label\">PreTrip:</span><span>" + $SearchedOrders.Transferees[i].PreTrip + "</span><br />" +
                    "<span class=\"tablet-column-label\">Final Arrival:</span><span>" + $SearchedOrders.Transferees[i].FinalArrival + "</span></div>";

                var name_mobile = name_tablet + "<div><span class=\"tablet-column-label\">Manager:</span><span class=\"Callout\">" + $SearchedOrders.Transferees[i].Manager + "</span><p class=\"mobile-phone\">" + $SearchedOrders.Transferees[i].MamagerPhone + "</p></div>";


                // services
                var services = "<div class=\"actlist-item-hdr\"><img class=\"expand\" src=\"/Content/Images/expand.png\" /><img class=\"collapse noinit\" src=\"/Content/Images/collapse.png\" />" +
                    "</div><div class=\"actlist-item-hdr\"><p>SS</p><a href=\"#\">" + $SearchedOrders.Transferees[i].NumberOfScheduledServices + "</a></div><div class=\"actlist-item-hdr\"><p>S</p><a href=\"#\">" + $SearchedOrders.Transferees[i].NumberOfServices + "</a></div>" +
                    "<div class=\"actlist-item-hdr\"><p>C</p><a href=\"#\">" + $SearchedOrders.Transferees[i].NumberOfCompletedServices + "</a></div>";

                var services_list = "";
                for (var j = 0; j < $SearchedOrders.Transferees[i].Services.length; j++) {
                    services_list += "<div class=\"actlist-item-blk\"><p class=\"actlist-item\">" + $SearchedOrders.Transferees[i].Services[j].Name + "</p><p>" + $SearchedOrders.Transferees[i].Services[j].Completed + "</p></div>";
                }
                services += services_list;

                var services_tablet = "<div class=\"actlist-item-hdr\"><img class=\"expand\" src=\"/Content/Images/expand.png\" /><img class=\"collapse noinit\" src=\"/Content/Images/collapse.png\" />" +
                    "</div><div class=\"actlist-item-hdr\"><p>SS</p><a href=\"#\">" + $SearchedOrders.Transferees[i].NumberOfScheduledServices + "</a></div><div class=\"actlist-item-hdr\"><p>S</p><a href=\"#\">" + $SearchedOrders.Transferees[i].NumberOfServices + "</a></div><div class=\"actlist-item-hdr\">" +
                    "<p>C</p><a href=\"#\">" + $SearchedOrders.Transferees[i].NumberOfCompletedServices + "</a></div>";
                services_tablet += services_list;

                // last contacted
                var lastcontacted = "<p>Last Contacted</p><p>" + $SearchedOrders.Transferees[i].LastContacted + "</p>";

                // manager
                var manager = "<td><p>Manager</p><p class=\"Callout\">" + $SearchedOrders.Transferees[i].Manager + "</p><p>" + $SearchedOrders.Transferees[i].MamagerPhone + "</p></td>";

                // pretrip
                var pretrip = "<td><p>PreTrip</p><p>" + $SearchedOrders.Transferees[i].PreTrip + "</p></td>";

                // final arrival
                var finalarrival = "<td><p>Final Arrival</p><p>" + $SearchedOrders.Transferees[i].FinalArrival + "</p></td>";

                // notification
                var notification = "<td><img src=\"/Content/Images/icn_note_1.png\" /><div class=\"notify-count\"><span>" + $SearchedOrders.Transferees[i].NumberOfAlerts + "</span></div></td>";

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

        populateTables();
    };

    return {
        HomePageInit: init
    };
}();


