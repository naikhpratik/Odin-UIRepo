var UserManagementController = function() {

    var init = function(container) {
        container.DataTable({
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