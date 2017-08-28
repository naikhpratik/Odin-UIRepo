var UserManagementController = function () {
    var table;

    var init = function (container) {
        table = $(container);
        console.log(table);
        table.DataTable({
            ajax: {
                url: "/api/users",
                dataSrc: ""
            },
            columns: [
                {
                    data: "username",
                    render: function(data, type, user) {
                        return "<a href='#'>" + data.username + "</a>";
                    }
                },
                {
                    data: "type"
                },
                {
                    data: "phone"
                }
            ]
        });
    };

    return {
        init: init
    }
}();