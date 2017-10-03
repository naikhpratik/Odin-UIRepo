var HomePageController = function () {
    var $currSort = null;

    //TODO: fill $Orders via an /api call
    var $Orders = {
        "Transferees": [
            {
                "FirstName": "Victoria",
                "Middle": "A",
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
                "Middle": "C",
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
                "Middle": "S",
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
                "Middle": "A",
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
                "Middle": "L",
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
                "Middle": "D",
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
                "Middle": "A",
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

    var init = function () { 

        // populate tables
        var desktop_table = $('table.desktop tbody');
        var tablet_table = $('table.tablet tbody');
        var mobile_table = $('table.mobile tbody');

        for (var i = 0; i < $Orders.Transferees.length; i++) {

            // name
            var name = "<p>" + $Orders.Transferees[i].FirstName + " " + $Orders.Transferees[i].Middle + " " + $Orders.Transferees[i].LastName + "</p>" +
                "<p class=\"Callout\">" + $Orders.Transferees[i].Rmc + "</p><p>" + $Orders.Transferees[i].Company + "</p>";

            var name_tablet = name + "<div><span class=\"tablet-column-label\">PreTrip:</span><span>" + $Orders.Transferees[i].PreTrip + "</span><br />" +
                "<span class=\"tablet-column-label\">Final Arrival:</span><span>" + $Orders.Transferees[i].FinalArrival + "</span></div>";

            var name_mobile = name_tablet + "<div><span class=\"tablet-column-label\">Manager:</span><span class=\"Callout\">" + $Orders.Transferees[i].Manager + "</span><p class=\"mobile-phone\">" + $Orders.Transferees[i].MamagerPhone + "</p></div>";


            // services
            var services = "<div class=\"actlist-item-hdr\"><img class=\"expand\" src=\"/Content/Images/expand.png\" /><img class=\"collapse noinit\" src=\"/Content/Images/collapse.png\" />" +
                "</div><div class=\"actlist-item-hdr\"><p>SS</p><a href=\"#\">" + $Orders.Transferees[i].NumberOfScheduledServices + "</a></div><div class=\"actlist-item-hdr\"><p>S</p><a href=\"#\">" + $Orders.Transferees[i].NumberOfServices + "</a></div>" +
                "<div class=\"actlist-item-hdr\"><p>C</p><a href=\"#\">" + $Orders.Transferees[i].NumberOfCompletedServices + "</a></div>";

            var services_list = "";
            for (var j = 0; j < $Orders.Transferees[i].Services.length; j++) {
                services_list += "<div class=\"actlist-item-blk\"><p class=\"actlist-item\">" + $Orders.Transferees[i].Services[j].Name + "</p><p>" + $Orders.Transferees[i].Services[j].Completed + "</p></div>";
            }
            services += services_list;

            var services_tablet = "<div class=\"actlist-item-hdr\"><img class=\"expand\" src=\"/Content/Images/expand.png\" /><img class=\"collapse noinit\" src=\"/Content/Images/collapse.png\" />" +
                "</div><div class=\"actlist-item-hdr\"><p>SS</p><a href=\"#\">" + $Orders.Transferees[i].NumberOfScheduledServices + "</a></div><div class=\"actlist-item-hdr\"><p>S</p><a href=\"#\">" + $Orders.Transferees[i].NumberOfServices + "</a></div><div class=\"actlist-item-hdr\">" +
                "<p>C</p><a href=\"#\">" + $Orders.Transferees[i].NumberOfCompletedServices + "</a></div>";
            services_tablet += services_list;

            // last contacted
            var lastcontacted = "<p>Last Contacted</p><p>" + $Orders.Transferees[i].LastContacted + "</p>";

            // manager
            var manager = "<td><p>Manager</p><p class=\"Callout\">" + $Orders.Transferees[i].Manager + "</p><p>" + $Orders.Transferees[i].MamagerPhone + "</p></td>";

            // pretrip
            var pretrip = "<td><p>PreTrip</p><p>" + $Orders.Transferees[i].PreTrip + "</p></td>";

            // final arrival
            var finalarrival = "<td><p>Final Arrival</p><p>" + $Orders.Transferees[i].FinalArrival + "</p></td>";

            // notification
            var notification = "<td><img src=\"/Content/Images/icn_note_1.png\" /><div class=\"notify-count\"><span>" + $Orders.Transferees[i].NumberOfAlerts + "</span></div></td>";

            desktop_table.append("<tr>" + "<td>" + name + "</td>" + "<td>" + services + "</td>" + "<td>" + lastcontacted + "</td>" + manager + pretrip + finalarrival + notification + "</tr>");
            tablet_table.append("<tr>" + "<td>" + name_tablet + "</td>" + manager + "<td>" + lastcontacted + "</td>" + "<td>" + services_tablet + "</td>" + notification + "</tr>");
            mobile_table.append("<tr>" + "<td>" + name_mobile + "</td>" + "<td>" + lastcontacted + services + "</td>" + notification + "</tr>");
        }

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
    };

    return {
        HomePageInit: init
    };
}();


