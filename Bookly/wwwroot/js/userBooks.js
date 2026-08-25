
let allBooks = [];
let clickedBook =null;

$(document).ready(function () {
    loadUserBooks();
    loadCategories();
    

});


function displayBooks(booksToRender) {
    const grid = $('#userBooksGrid');
    grid.empty();

    if (!booksToRender || booksToRender.length === 0) {
        grid.html('<div class="col-12 text-center text-muted my-5">No books found.</div>');
        return;
    }

    booksToRender.forEach(book => {
        const bookId = book.id || book.Id || 0;
        const title = book.title || book.Title || 'Uncknown title';
        const imageUrl = book.imageUrl || book.ImageUrl || '';
        const authorName = book.authorName || book.AuthorName || 'Unknown author';
        const isBorrowed = book.isBorrowed || book.IsBorrowed || false;

        const isAvailable = isBorrowed === false || isBorrowed === "false";
        const statusBadge = isAvailable
            ? '<span class="badge bg-success">Available</span>'
            : '<span class="badge bg-danger">Borrowed</span>';

                        const cardHtml = `
                          <div class="col-12 col-sm-6 col-md-4 col-lg-3 mb-4">
                           <div class="card h-100 shadow-sm border-0 book-card">
                              <div class="book-image-wrapper">
                                  <img src="${book.imageUrl || '/images/no-book.png'}" class="card-img-top" alt="${book.title}">
                              </div>

                             <div class="card-body d-flex flex-column text-start p-3">
                              <h6 class="card-title fw-bold">${book.title}</h6>
                              <p class="text-muted small">${book.authorName}</p>

                              <p class="book-summary">
                                 ${book.summary || 'There is no available summary for this book.'}
                              </p>

                            <div class="mt-auto">
                            <span class="badge ${book.isBorrowed ? 'bg-danger' : 'bg-success'} mb-2">
                                ${book.isBorrowed ? 'Borrowed' : 'Available'}
                            </span>
                            <button class="btn btn-outline-primary btn-sm w-100 mt-2 view-details-btn" data-id="${bookId}">View Details</button>
                        </div>
                    </div>
                </div>
            </div>`;
      
        grid.append(cardHtml);
    });
}

function loadUserBooks() {
    $.ajax({
        url: '/api/book',
        type: 'GET',
        success: function (books) {
            allBooks = books;        
            displayBooks(allBooks);  
        },
        error: function (err) {
            console.error("Error loading the book:", err);
        }
    });
}

function displayCategories(categories) {
    const container = $('#categoriesContainer');
    container.empty();

    container.append(`
        <button class="btn btn-sm text-start category-btn fw-bold active"
                data-category="all"
                style="background-color: #ffe8e8; color: #dc3545; border-radius: 5px; padding:6px 12px;">
            All Books
        </button>
    `);

    categories.forEach(category => {
        const categoryName = category.name || category.Name || '';

        if (categoryName) {
            container.append(`
                <button class="btn btn-sm text-start category-btn text-secondary"
                        data-category="${categoryName}"
                        style="background-color: transparent; border: none; padding: 6px 12px; transition: all 0.2s;">
                     ${categoryName}
                </button>
            `);
        }
    });
}

function loadCategories() {
    $.ajax({
        url: '/api/categories',
        type: 'GET',
        success: function (data) {
            displayCategories(data);
        },
        error: function (err) {
            console.error("Eroare la imcarcarea categoriilor:", err);
        }

    });
}

$(document).on('click', '.category-btn', function () {
        $('.category-btn').removeClass('fw-bold active').addClass('text-secondary').css({ 'background-color': 'transparent', 'color': '' });
        $(this).addClass('fw-bold active').removeClass('text-secondary').css({ 'background-color': '#ffe8e8', 'color': '#dc3545' });

        const selectedCategory = $(this).attr('data-category');

        if (selectedCategory === 'all') {
            $('#collectionTitle').text('All Books');
            displayBooks(allBooks); 
        } else {
            $('#collectionTitle').text(selectedCategory);

            const filteredByResult = allBooks.filter(book => {
                const bookCategory = book.categoryName || book.CategoryName || '';
                return bookCategory.toString().toLowerCase() === selectedCategory.toString().toLowerCase();
            });

            displayBooks(filteredByResult);
        }
});

    
    $(document).on('click', '.view-details-btn', function () {
        const idSelected = $(this).attr('data-id');




            const clickedBook = allBooks.find(b => (b.id || b.Id).toString() === idSelected.toString());

        if (clickedBook) {
                currentBookId = clickedBook.id || clickedBook.Id;
                const bookId = clickedBook.id || clickedBook.Id;
                const title = clickedBook.title || clickedBook.Title || 'Unknown Title';
                const imageUrl = clickedBook.imageUrl || clickedBook.ImageUrl || '';
                const authorName = clickedBook.authorName || clickedBook.AuthorName || 'Unknown Author';
                //const isFav = clickedBook.isFavorite === true || clickedBook.IsFavorite === true;
                //const isFav = clickedBook.isFavorite || clickedBook.IsFavorite || true;
                //const isFav = Boolean(clickedBook.isFavorite || clickedBook.IsFavorite);
                const genre = clickedBook.categoryName || clickedBook.CategoryName || clickedBook.genre || clickedBook.Genre || 'General';
                const description = clickedBook.summary || clickedBook.Summary || clickedBook.description || clickedBook.Description || 'Nu există o descriere disponibilă pentru această carte.';
                const isBorrowed = clickedBook.isBorrowed || clickedBook.IsBorrowed || false;
                const isAvailable = isBorrowed === false || isBorrowed === "false";
                const statusBadge = isAvailable
                    ? '<span class="badge bg-success-subtle text-success border border-success-subtle px-3 py-2 rounded-pill">Available in the library</span>'
                    : '<span class="badge bg-danger-subtle text-danger border border-danger-subtle px-3 py-2 rounded-pill">Currently borrowed</span>';


                const modalHtml = `
            <div class="col-md-5 text-center">
                <img src="${imageUrl}" class="img-fluid rounded shadow-sm" alt="${title}" style="max-height: 380px; object-fit: contain;">
            </div>
            <div class="col-md-7 d-flex flex-column justify-content-between">
                <div>
                    <span class="text-uppercase text-muted fw-bold tracking-wider small" style="font-size: 0.75rem; letter-spacing: 1px;">
                        ${genre}
                    </span>
                    <h3 class="fw-bold text-dark mt-1 mb-2" style="font-family: Georgia, serif;">${title}</h3>
                    
                    <p class="text-secondary mb-4" style="font-style: italic;">by <strong>${authorName}</strong></p>
                    
                    <h6 class="fw-bold text-dark mb-2">About book:</h6>
                    <p class="text-muted" style="font-size: 0.95rem; line-height: 1.6; text-align: justify;">
                        ${description}
                    </p>
                </div>
                <div class="modal-body">
               
                <div class="mt-4 pt-3 border-top d-flex align-items-center justify-content-between">
                    <div>
                        ${statusBadge}
                    </div>
                  
                    <div class="d-flex gap-2 align-items-center">
               <!-- Borrow Book -->
                  <button class="badge btn-pill-borrow px-3 py-2 rounded-pill fw-medium"
                  style="font-size: 0.88rem; cursor: pointer;"
                    onclick="reserveBook(${bookId})">
                        Borrow Book
                  </button>

               <!-- Close -->
                  <button type="button" 
                      class="badge btn-pill-close px-3 py-2 rounded-pill fw-medium" 
                      style="font-size: 0.88rem; cursor: pointer;" 
                      data-bs-dismiss="modal">
                    Close
                  </button>
                 </div>
                </div>
              </div>
             `;

                $('#modalBookContent').html(modalHtml);
                $('#bookDetailsModal').modal('show');

                //updateFavoriteHeart(isFav);
                updateFavoriteHeart(
                    clickedBook.isFavorite ?? clickedBook.IsFavorite
                );
                

            }
        
        
    });


async function reserveBook(bookCopyId) {
   

    try {
        
        const response = await fetch('/api/borrows/borrow', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            credentials: 'include',
            body: JSON.stringify({ bookCopyId: Number(bookCopyId) })
        });

        
        if (response.ok) {
            alert("Cartea a fost împrumutată cu succes!");
            window.location.href = '/Account/Profile';
        } else {
            
            const text = await response.text();
            console.error("Eroare detalii server:", text);

            try {
                const data = JSON.parse(text);
                alert(data.message || "Eroare la împrumutarea cărții.");
            } catch {
                alert(text || "Eroare la împrumutarea cărții.");
            }
        }
    } catch (err) {
        console.error("Eroare de rețea:", err);
    }
}

async function returnBook(borrowId) {
    if (!confirm('Ești sigur că vrei să returnezi această carte?')) return;

    const response = await fetch(`/api/borrows/${borrowId}/return`, {
        method: 'PATCH'
    });

    if (response.ok) {
        alert('Cartea a fost returnată cu succes!');
        location.reload();
    } else {
        alert('A apărut o eroare la returnarea cărții.');
    }
}



//$(document).on('click', '#btnFavoriteModal', function (e) {
//    e.preventDefault();
//    const bookId = $(this).attr('data-id');
//    console.log("BOOK ID:", bookId);
//    if (bookId) {
//        toggleFavoriteFromModal(bookId);
//    }
//});

$(document).on('click', '#btnFavoriteModal', function (e) {
    e.preventDefault();

    //const bookId = clickedBook.id || clickedBook.Id;
    const bookId = currentBookId;

    console.log("BOOK ID:", bookId);

    if (bookId) {
        toggleFavoriteFromModal(bookId);
    }
});


function updateFavoriteHeart(isFavorite) {
    const heartIcon = $('#favoriteHeartIcon');

    if (isFavorite) {
        heartIcon
            .removeClass('bi-heart')
            .addClass('bi-heart-fill text-danger');
    } else {
        heartIcon
            .removeClass('bi-heart-fill text-danger')
            .addClass('bi-heart');
    }

}

function toggleFavoriteFromModal(bookId) {
    $.ajax({
        url: '/api/favorites/toggle',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ BookId: parseInt(bookId) }),
        success: function (res) {
            console.log("API RESPONSE:", res);
            const isFav = res.isFavorite;

            updateFavoriteHeart(isFav);

            const targetBook = allBooks.find(b => (b.id || b.Id) == bookId);
            if (targetBook) {
                targetBook.isFavorite = isFav;
                targetBook.IsFavorite = isFav;
            }
          
       
        },
        error: function (xhr) {
            console.log("STATUS:", xhr.status);
            console.log("RESPONSE:", xhr.responseText);

            if (xhr.status === 401) {
                alert('Trebuie să fii autentificat!');
            }
        }
    });
}