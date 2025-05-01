$(document).ready(function () {
    $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAllProducts",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "title" },
            { "data": "author" },
            { "data": "price" },
            { "data": "category" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="/Admin/Product/UpSert/${data}" class="btn btn-primary mx-1 rounded-1">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a href="/Admin/Product/Delete/${data}" " class="btn btn-danger mx-1 rounded-1">
                                <i class="bi bi-trash"></i> Delete
                            </a>
                        </div>`;
                }
            }
        ]
    });
});


