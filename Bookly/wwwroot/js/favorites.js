
$(document).ready(function () {
    loadFavorites();
});

function loadFavorites() {
   

    $.ajax({
        url: '/api/favorites',
        type: 'GET',
        success: function (books) {
            let container = $('#favoritesContainer');
            container.empty();

            if (!books || books.length === 0) {
                container.html('<p class="text-muted">Aici vor apărea cărțile salvate la favorite.</p>');
                return;
            }

            let html = '<div class="row g-3">';
            books.forEach(book => {
                html += `
                    <div class="col-md-4 col-lg-3" id="favorite-card-${book.bookId}">
                        <div class="card h-100 shadow-sm border-0">
                            <img src="${book.coverImageUrl || '/images/default-book.png'}" class="card-img-top" style="height: 200px; object-fit: cover;" alt="${book.title}">
                            <div class="card-body d-flex flex-column justify-content-between">
                                <div>
                                    <h6 class="card-title fw-bold mb-1">${book.title}</h6>
                                    <p class="card-text text-muted small mb-2">${book.author}</p>
                                </div>
                                <button class="btn btn-sm btn-outline-danger w-100 mt-2" onclick="removeFavorite(${book.bookId})">
                                    Remove 
                                </button>
                            </div>
                        </div>
                    </div>
                `;
            });
            html += '</div>';

            container.html(html);
        },
        error: function () {
            $('#favoritesContainer').html('<p class="text-danger">A apărut o eroare la încărcarea cărților favorite.</p>');
        }
    });
}

function removeFavorite(bookId) {
    let currentUserId = 1;

    $.ajax({
        url: '/api/favorites/toggle',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ userId: currentUserId, bookId: bookId }),
        success: function (res) {
            if (!res.isFavorite) {
                $(`#favorite-card-${bookId}`).fadeOut(300, function () {
                    $(this).remove();
                    if ($('#favoritesContainer .col-md-4').length === 0) {
                        $('#favoritesContainer').html('<p class="text-muted">Aici vor apărea cărțile salvate la favorite.</p>');
                    }
                });
            }
        }
    });
}