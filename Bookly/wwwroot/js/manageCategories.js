
$(document).ready(function () {
    var editId;
    loadCategoriesTable();
});

function loadCategoriesTable() {
    $('#categoriesTable').DataTable({
        "ajax": {
            "url": "/api/categories",
            "type": "GET",
            "dataSrc": "",

        },
        "columns": [
            /*{ "data": "id", visible: false },*/
            { "data": "name" },
            {
                "data": null,
                "render": function (data, type, row) {
                    return `
                    <button class="btn btn-action-edit" onclick="openEditCategoryModal(${row.id})">Edit</button>
                    <button class="btn btn-action-delete" onclick="deleteCategory(${row.id})">Delete</button>
                    `;
                }
            }

        ],
        language: {
            search: "Search Categories:",
            lengthMenu: "All _MENU_ Categories"
        },
        rowId: 'id'
    });
}

function addNewCategory() {
    const newCategory = {
        Name: $('#addCategoryName').val()
    };

    $.ajax({
        url: '/api/categories',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(newCategory),
        success: function () {
            const modal = bootstrap.Modal.getInstance(document.getElementById('addCategoryModal'));
            modal.hide();

            $('#addCategoryName').val('');

            $('#categoriesTable').DataTable().ajax.reload();
            alert("Category added successfully!");
        },
        error: function (xhr) {
            alert("Error:" + xhr.responseText);
        }
    });
}

function openEditCategoryModal(id) {
    var labelText = id ? 'Edit Category' : 'Add Category';

    $('#categoryModalTitle').text(labelText);

    clearModal();

    if (id) {
        loadEditData(id)
    }

    $('#categoryModal').modal('show');
}

function loadEditData(id) {
    $.ajax({
        url: `/api/categories/${id}`,
        type: "GET",
        contentType: "application/json",
        success: function (data) {
            $('#categoryId').val(data.id);
            $('#categoryInput').val(data.name);
        }
    });
}

function clearModal() {
    $('#categoryInput').val('');

}

function saveCategory() {
    const id = $('#categoryId').val();
    const name = $('#categoryInput').val();

    //if (!name) {
    //    alert("Please select a valid user from the list!");
    //    return;
    //}

    //const categoryData = {
    //    Name: $("categoryInput").val()

    //};

    const isEdit = id && id !== "0";

    $.ajax({
        url: isEdit ? `/api/categories/${id}` : '/api/categories',
        type: isEdit ? 'PUT' : 'POST',
        contentType: 'application/json',
        data: JSON.stringify(name),
        success: function () {
            alert("Saved succesfully!");
            location.reload();
        },
        error: function (xhr) {
            alert("Error:" + xhr.responseText);
        }
    });
}


function deleteCategory(id) {
    if (confirm("Delete category?")) {
        $.ajax({
            url: `/api/categories/${id}`,
            type: "DELETE",
            success: function () {
                $('#categoriesTable').DataTable().ajax.reload();
                alert("The category has been deleted.");
            },
            error: function () {
                alert("Error deleting the category.");
            }
        });
    }
}