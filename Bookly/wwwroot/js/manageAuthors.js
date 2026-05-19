


$(document).ready(function () {
    var editId;
    loadAuthorsTable();
});

function loadAuthorsTable() {
    $('#authorsTable').DataTable({
        "ajax": {
            "url": "/api/authors",
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
                    <button class="btn btn-action-edit" onclick="openEditAuthorModal(${row.id})">Edit</button>
                    <button class="btn btn-action-delete" onclick="deleteAuthor(${row.id})">Delete</button>
                    `;
                }
            }

        ],
        language: {
            search: "Search Authors:",
            lengthMenu: "All _MENU_ authors"
        },
        rowId: 'id'
    });

}


function openEditAuthorModal(id) {
    var labelText = id ? 'Edit Author' : 'Add Author';

    $('#authorModalTitle').text(labelText);

    clearModal();

    if (id) {
        loadEditData(id)
    }

    $('#authorModal').modal('show');
}

function loadEditData(id) {
    $.ajax({
        url: `/api/authors/${id}`,
        type: "GET",
        contentType: "application/json",
        success: function (data) {
            $('#authorId').val(data.id);
            $('#authorInput').val(data.name);
        }
    });
}

function clearModal() {
    $('#authorInput').val('');

}

function saveAuthor() {
    const id = $('#authorId').val();
    const name = $('#authorInput').val();
   
    //if (!name) {
    //    alert("Please select a valid user from the list!");
    //    return;
    //}

    //const authorData = {
    //    Name: $("authorInput").val()
        
    //};

    const isEdit = id && id !== "0";

    $.ajax({
        url: isEdit ? `/api/authors/${id}` : '/api/authors',
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
  

    function deleteAuthor(id) {
        if (confirm("Delete the author?")) {
            $.ajax({
                url: `/api/authors/${id}`,
                type: "DELETE",
                success: function () {
                    $('#authorsTable').DataTable().ajax.reload();
                    alert("The author has been deleted.");
                },
                error: function () {
                    alert("Error deleting the author.");
                }
            });
        }
    }


