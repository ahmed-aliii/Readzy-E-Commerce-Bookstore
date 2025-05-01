$(document).ready(function () {
    $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetAllOrders",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id" },
            { "data": "name" },
            { "data": "applicationUser.email" },
            { "data": "orderStatus" },
            { "data": "orderTotal" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="/Admin/Product/UpSert/${data}" class="btn btn-primary mx-1 rounded-1">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                        </div>`;
                }
            }
        ]
    });
});


