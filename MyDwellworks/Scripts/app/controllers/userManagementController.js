var UserManagementController = function () {
    var table;

    var init = function (container) {
        table = $(container);

        table.DataTable({
            ajax: {
                url: "/api/users",
                dataSrc: ""
            },
            columns: [
                {
                    data: "userName",
                    render: function(data, type, user) {
                        return "<a href='#'>" + user.userName + "</a>";
                    }
                },
                {
                    data: "userRoles"
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