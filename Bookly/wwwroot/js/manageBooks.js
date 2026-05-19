

$(document).ready(function () {
    var editId;
    loadBookTable();
   

    $('.select2-setup').select2({
        // theme: "bootstrap-5", 
        dropdownParent: $('#bookModal'),
        placeholder: "Type to search...",
        allowClear: true,
        width: '100%',
        ajax: {
            url: function () {
                return $(this).attr('id') === 'authorSelect' ? '/api/authors/search' : '/api/categories/search';
            },
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return { term: params.term};
            },
            processResults: function (data) {
                return {
                    results: data.map(item => ({
                        id: item.id,
                        text: item.name 
                    }))
                };
            }
        }
    });


});


function loadBookTable() {
    $('#bookTable').DataTable({
        autoWidth: false,
        "ajax": {
            "url": "/api/book",
            "type": "GET",
            "dataSrc": "",

        },
        "columns": [
            /* { "data": "id", visible: false },*/
            { "data": "title" },
            { "data": "authorName" },
            { "data": "categoryName" },
            { "data": "quantity" },
            {
                "data": "isBorrowed",
                "render": function (data) {
                    return data ? "Borrowed" : "Available";
                }
            },

            {
                "data": "summary",
                "render": function (data) {
                    if (!data) return '<span class="text-muted">Nici un rezumat</span>';
                    const limit = 50;
                    if (data.length > limit) {
                        return data.substring(0, limit) + "...";
                    }
                    return data;
                }
            },
            {
                "data": "imageUrl",
                "render": function (data) {
                    if (data) {
                        return `<img src="${data}" style="width: 50px; height: auto; border-radius: 4px; border: 1px solid #ddd;" />`;
                    } else {
                        return `<img src="${data}" alt="Book Cover" style="width:50px; height:auto;" onerror="this.onerror=null; this.src='https://placehold.co/150/e0e0e0/616161?text=No+Image';">`;
                    }
                }
            },


            {
                "data": null,
                "render": function (data, type, row) {
                    return `
                    <button class="btn btn-action-edit" onclick="openEditBookModal(${row.id})">Edit</button>
                    <button class="btn btn-action-delete" onclick="deleteBook(${row.id})">Delete</button>
                    `;
                }
            }

        ],
        language: {
            search: "Search Books:",
            lengthMenu: "All _MENU_ books"
        },
        rowId: 'id'
    });
}



function openEditBookModal(id) {
    var labelText = id ? 'Edit Book' : 'Add Book';

    $('#bookModalTitle').text(labelText);

    clearModal();

    if (id) {
        loadEditData(id)
    }

    $('#bookModal').modal('show');
}

function loadEditData(id) {
    $.ajax({
        url: `/api/book/${id}`,
        type: "GET",
        contentType: "application/json",
        success: function (data) {
            $('#bookId').val(data.id);
            $('#titleInput').val(data.title);

            if (data.authorId && data.authorName) {
                var authorOption = new Option(data.authorName, data.authorId, true, true);
                $('#authorSelect').append(authorOption).trigger('change');
            }

            if (data.categoryId && data.categoryName) {
                var categoryOption = new Option(data.categoryName, data.categoryId, true, true);
                $('#categorySelect').append(categoryOption).trigger('change');
            }
         
            $('#quantityInput').val(data.quantity);
            $('#isBorrowedInput').val(data.isBorrowed.toString());
            $('#summaryInput').val(data.summary);
            $('#imageUrl').attr("src",data.imageUrl);
        }
    });
}

function clearModal() {
    $('#bookId').val('0');
    $('#titleInput').val('');
    $('#authorSelect').val(null).trigger('change');
    $('#categorySelect').val(null).trigger('change');
    $('#quantityInput').val(''); 
    $('#summaryInput').val('');
 

}

function saveBook() {
    const fileInput = $('#bookImageInput')[0];
    const currentImageUrl = $('#imageUrl').attr("src");

    if (fileInput.files.length > 0) {
        const formData = new FormData();
        formData.append('file', fileInput.files[0]);

        $.ajax({
            url: '/api/book/upload-book-image',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                executeSaveBook(response.url);
            }
        });
      
    } else {
        executeSaveBook(currentImageUrl);
    }

    function executeSaveBook(imageUrl) {
        debugger;
        var id = $('#bookId').val();
        var titleInput = $('#titleInput').val();
        var authorId = $('#authorSelect').val();
        var categoryId = $('#categorySelect').val();
        var quantityInput = $('#quantityInput').val();
        var isBorrowedInput = $('#isBorrowedInput').val();
        var summaryInput = $('#summaryInput').val();

        if (!titleInput || !categoryId ||!quantityInput || !isBorrowedInput) {
                alert("Please select a valid user from the list!");
                return;
        }


        const bookData = {
            Id: parseInt(id),
            Title: titleInput,
            AuthorId: parseInt(authorId),
            CategoryId: parseInt(categoryId),
            Quantity: parseInt(quantityInput),
            IsBorrowed: isBorrowedInput === "true",
            Summary: summaryInput,
            /*ImageUrl: $('#bookImageInput'),*/
            ImageUrl: imageUrl
        };

        const isEdit = id && id !== "0";

        $.ajax({
            url: isEdit ? `/api/book/${id}` : '/api/book',
            type: isEdit ? 'PUT' : 'POST',
            contentType: 'application/json',
            data: JSON.stringify(bookData),
            success: function () {
                alert("Saved succesfully!");
                location.reload();
            },
            error: function (xhr) {
                alert("Error:" + xhr.responseText);
            }
        });
    }
}


function deleteBook(id) {
    if (confirm("Delete the book?")) {
        $.ajax({
            url: `/api/book/${id}`,
            type: "DELETE",
            success: function () {
                $('#bookTable').DataTable().ajax.reload();
                alert("The book has been deleted.");
            },
            error: function () {
                alert("Error deleting the book.");
            }
        });
    }
}



