
$(document).ready(function () {
    loadBorrowsTable();
    fillUserDropdown();
    fillBookDropdown();
});

/*-----------------Load functions--------------*/
function fillBookDropdown() {
    $('#bookSelect').select2({
        dropdownParent: $('#borrowModal'),
        placeholder: "Search book...",
        width: '100%',
        allowClear: true,
        ajax: {
            url: '/api/book/search',
            type: 'GET',
            dataType: 'json',
            data: function (params) {
                return { term: params.term };
            },
            processResults: function (data) {
                return {
                    results: data.map(function (book) {
                        return {
                            id: book.id,
                            text: book.text
                        };
                    })
                };
            },
            cache: true
        }
    });
}
function fillUserDropdown() {
    $('#userSelect').select2({
        dropdownParent: $('#borrowModal'),
        placeholder: "Search user...",
        width: '100%',
        allowClear: true,
        ajax: {
            url: '/api/users/search',
            type: 'GET',
            dataType: 'json',
            data: function (params) {
                return { term: params.term };
            },
            processResults: function (data) {
                return {
                    results: data.map(function (user) {
                        return {
                            id: user.id,
                            text: user.name
                        };
                    })
                };
            },
            cache: true
        }
    });
}
function loadBorrowsTable() {
    $('#borrowsTable').DataTable({
        "ajax": {
            "url": "/api/borrows",
            "type": "GET",
            "dataSrc": "",

        },
        "columns": [
            /* { "data": "id", visible: false },*/
            { "data": "userName" },
            { "data": "bookTitle" },
            {
                "data": "borrowDate",
                "render": function (data) { return data ? data.split('T')[0] : ''; }
            },
            {
                "data": "dueDate",
                "render": function (data) {
                    if (!data || data.startsWith("0001-01-01")) {
                        return '<span class="text-muted">-</span>';
                    }
                    return data.split('T')[0];
                }
            },
            {
                "data": null,
                "render": function (data, type, row) {
                    if (row.returnDate) {
                        return '<span class="badge bg-secondary">Returned</span>';
                    }

                    const isOverdue = new Date() > new Date(row.dueDate);
                    if (isOverdue || row.status === "Overdue") {
                        return '<span class="badge bg-danger">Overdue</span>';
                    }

                    return '<span class="badge bg-success">Active</span>';
                }
            },
            {
                "data": "returnDate",
                "render": function (data) { return data ? data.split('T')[0] : 'In Progress'; }
            },
            {
                "data": null,
                "render": function (data, type, row) {
                    let buttons = `
                        <button class="btn btn-sm btn-outline-primary me-1" onclick="openEditBorrowModal(${row.id})">Edit</button>
                        <button class="btn btn-sm btn-outline-danger me-1" onclick="deleteBorrow(${row.id})">Delete</button>
                    `;

                    // Butonul Return apare doar dacă nu este deja returnat
                    if (!row.returnDate) {
                        buttons += `<button class="btn btn-sm btn-outline-success" onclick="returnBorrow(${row.id})">Return</button>`;
                    }

                    return buttons;
                
                   }
                   
                }
            

        ],
        language: {
            search: "Search Borrows:",
            lengthMenu: "All _MENU_ Borrows"
        },
        rowId: 'id'
    });
}

/*-----------------Load functions end --------------*/


/*-------------------CRUD functions ---------------*/
function openEditBorrowModal(id) {
    var labelText = id ? 'Edit Borrow' : 'Add Borrow';

    $('#borrowModalTitle').text(labelText);

    clearModal();

    if (id) {
        loadEditData(id)
    }

    $('#borrowModal').modal('show');
}

function loadEditData(id) {
    $.ajax({
        url: `/api/borrows/${id}`,
        type: "GET",
        contentType: "application/json",
        success: function (data) {            
            $('#borrowId').val(data.id);

            var userOption = new Option(data.userName, data.userId, true, true);
            $('#userSelect').append(userOption).trigger('change');

            var bookOption = new Option(data.bookTitle, data.bookId, true, true);
            $('#bookSelect').append(bookOption).trigger('change');

            $('#borrowDate').val(data.borrowDate.split('T')[0]);
            $('#returnDate').val(data.returnDate ? data.returnDate.split('T')[0] : '');
        }
    });
}

function clearModal() {
    $('#userSelect').val(null).trigger('change');
    $('#bookSelect').val(null).trigger('change');
    $('#borrowDate').val('');
    $('#returnDate').val('');
   
}


function saveBorrow() {
    var id = $('#borrowId').val();
    var userId = $('#userSelect').val();
    var bookId = $('#bookSelect').val();

    if (!userId || !bookId) {
        alert("Please select a valid user from the list!");
        return;
    }

    const borrowData = {
        UserId: parseInt(userId),
        BookId: parseInt(bookId),
        BorrowDate: $('#borrowDate').val(),
        ReturnDate: $('#returnDate').val() || null
    };

    const isEdit = id && id !== "0";

    $.ajax({
        url: isEdit ? `/api/borrows/${id}` : '/api/borrows',
        type: isEdit ? 'PUT' : 'POST',
        contentType: 'application/json',
        data: JSON.stringify(borrowData),
        success: function () {
            alert("Saved succesfully!");
            $('#borrowModal').modal('hide');
            $('#borrowsTable').DataTable().ajax.reload();
            //location.reload();
        },
        error: function (xhr) {
            alert("Error:" + xhr.responseText);
        }
    });
}

function returnBorrow(id) {
    if (!confirm("Ești sigur că vrei să marchezi acest împrumut ca returnat?")) {
        return;
    }

    $.ajax({
        url: `/api/borrows/return/${id}`,
        type: 'POST',
        success: function (response) {
            $('#borrowsTable').DataTable().ajax.reload(null, false);
            alert("Cartea a fost returnată cu succes!");
        },
        error: function (xhr) {
            alert("Eroare la procesarea returnării: " + (xhr.responseText || "Ceva nu a funcționat."));
        }
    });
}


function deleteBorrow(id) {
    if (confirm("Delete borrow?")) {
        $.ajax({
            url: `/api/borrows/${id}`,
            type: "DELETE",
            success: function () {
                $('#borrowsTable').DataTable().ajax.reload();
                alert("The borrow has been deleted.");
            },
            error: function () {
                alert("Error deleting the borrow.");
            }
        });
    }
}
/*-------------------CRUD functions end---------------*/





























//function saveBorrow() {

//    const id = $('#borrowId').val();

//    //const userId = parseInt($('#userId').val());
//    //const bookId = parseInt($('#bookSelect').val());
//    //const borrowDate = $('#borrowDate').val();
//    //const returnDate = $('#returnDate').val() || null;

//    const borrowData = {
//        id: id ? parseInt(id) : 0,
//        UserName: $('#userId').val(),
//        BookTitle: $('#bookSelect option:selected').text(),
//        BorrowDate: $('#borrowDate').val(),
//        ReturnDate: $('#returnDate').val() || null
//    };


//    //if (!borrowData.userId || !borrowData.bookId || !borrowData.borrowDate) {
//    //    alert("campurile sunt obligatorii");
//    //    return;
//    //}


//    const isEdit = id && id !== "0";
//    //const url = isEdit ? `/api/borrows/${id}` : '/api/borrows';
//    //const method = isEdit ? 'PUT' : 'POST';

//    $.ajax({
//        url: isEdit ? `/api/borrows/${id}` : '/api/borrows' ,
//        type: isEdit ? 'PUT' : 'POST',
//        contentType: 'application/json',
//        data: JSON.stringify(borrowData),
//        success: function () {
//            alert(isEdit ? "Modificările au fost salvate!" : "Împrumut adăugat cu succes!");

//            if ($.fn.DataTable.isDataTable("#borrowTable")) {
//                $('#borrowTable').DataTable().ajax.reload();
//            } else {
//                location.reload();
//            }

//            // Închidem modalul folosind Bootstrap
//            const modalElement = document.getElementById('borrowModal');
//            bootstrap.Modal.getInstance(modalElement).hide();

//        },
//        error: function (xhr) {
//            alert("Eroare la salvare: " + xhr.responseText);
//        }
//    });
//}

























//function openBorrowModal() {
   
//    editId = id;
//    const modalElement = document.getElementById('borrowModal');
//    let bootstrapModal = bootstrap.Modal.getInstance(modalElement);

//    if (!bootstrapModal) {
//        bootstrapModal = new bootstrap.Modal.getInstance(modalElement);
//    }

//    if (id) {
//        //mod edit
//        $('borrowModalTitle').text("Edit Borrow");

//        $.ajax({
//            url: `/api/borrows/${id}`,
//            type: 'GET',
//            success: function (data) {
//                $('#userSelect').val(data.userId);
//                $('#bookSelect').val(data.bookId);

//                if (data.borrowDate) {
//                    $('#borrowDate').val(data.borrowDate.split('T')[0]);
//                }
//                if (data.returmDate) {
//                    $('#returnDate').val(data.returnDate.split('T')[0]);
//                } else {
//                    $('#returnDate').val('');
//                }
//                bootstrapModal.show();
//            },
//            error: function (xhr) {
//                alert("Error:" + xhr.responseText);
//            }

//        });
//    } else {
//        $('#borrowModalTitle').text("AddNewBorrow");
//        $('#userSelect').val('');
//        $('#bookSelect').val('');
//        $('borrowDate').val(new Date().toISOString().split('T')[0]);
//        $('returnDate').val('');

//        bootstrapModal.show();
//    }
//}















