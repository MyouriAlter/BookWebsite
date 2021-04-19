let dataProductTable;

$(document).ready(function (){
    loadProductDataTable();
});

function loadProductDataTable() {
    dataProductTable = $('#tblProductData').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "title", "width": "15%"},
            { "data": "isbn", "width": "15%"},
            { "data": "price", "width": "15%%"},
            { "data": "author", "width": "15%%"},
            { "data": "category.Name", "width": "15%%"},
            {
                "data": "id",
                "render": function (data){
                    return `
                        <div class="text-center">
                            <a href="/Admin/Product/Upsert/${data}" 
                            class="btn btn-success text-white" style="cursor: pointer">
                                <i class="fas f a-edit"></i>
                            </a>
                            <a onclick=DeleteProduct("/Admin/Product/Delete/${data}") 
                            class="btn btn-danger text-white" style="cursor: pointer">
                                <i class="fas fa-trash-alt"></i>
                            </a>
                        </div>
                    `;
                } , "width": "40%"
            }
        ]
    });
}

function DeleteProduct(url){
    swal({
        title: "Are you sure you want to delete this entry?",
        text: "You will not be able to revert this change",
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
                        dataCoverTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    });
}