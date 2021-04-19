let dataCategoryTable;

$(document).ready(function () {
    loadCategoryDataTable();
});

function loadCategoryDataTable() {
    dataCategoryTable = $('#tblCategoryData').DataTable({
        "ajax": {
            "url": "/Admin/Category/GetAll"
        },
        "columns": [
            { "data": "name", "width": "60%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/Category/Upsert/${data}"
                            class="btn btn-success text-white" style="cursor:pointer">
                                <i class="fas fa-edit"></i>
                            </a>
                             <a onclick=DeleteCategory("/Admin/Category/Delete/${data}")
                                class="btn btn-danger text-white" style="cursor:pointer">
                                <i class="fas fa-trash-alt"></i>
                            </a>
                        </div>
                    `;
                }, "width": "40%"
            }
        ]
    });
}

function DeleteCategory(url) {
    swal({
        title: "Are you sure you want to delete this?",
        text: "You will not be able to reverse this change",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataCategoryTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    });
}